using ImageGallery.Client.ViewModels;
using ImageGallery.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageGallery.Client.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IHttpClientFactory httpClientFacotry;

        public GalleryController(IHttpClientFactory httpClientFacotry)
        {
            this.httpClientFacotry = httpClientFacotry;
        }
        public async Task<IActionResult> Index()
        {
            var httpClient = httpClientFacotry.CreateClient("ApiClient");
            
            var request = new HttpRequestMessage
                (
                    HttpMethod.Get,
                    "https://localhost:44397/api/images"
                );

            var response = await httpClient.SendAsync(request,HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            return View(new GalleryIndexViewModel(await JsonSerializer.DeserializeAsync<List<Image>>(responseStream)));
        }
        public async Task<IActionResult>EditImage(Guid id)
        {
            var httpClient = httpClientFacotry.CreateClient("ApiClient");

            var request = new HttpRequestMessage
                (
                    HttpMethod.Get,
                    $"https://localhost:44397/api/images/{id}"
                );

            var response = await httpClient.SendAsync
                (
                    request
                    , HttpCompletionOption.ResponseHeadersRead
                ).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using var responseStream= await response.Content.ReadAsStreamAsync();

            var deserializedImage = await JsonSerializer.DeserializeAsync<Image>(responseStream);

            var editImageViewModel = new EditImageViewModel()
            {
                Id = deserializedImage.Id,
                Title = deserializedImage.Title
            };

            return View(editImageViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>EditImage(EditImageViewModel editImageViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var imageForUpdate = new ImageForUpdate
            {
                Title = editImageViewModel.Title
            };

            var serializedImageForUpdate = JsonSerializer.Serialize(imageForUpdate);

            var httpClient = httpClientFacotry.CreateClient("ApiClient");

            var request = new HttpRequestMessage
                (
                    HttpMethod.Put,
                    $"https://localhost:44397/api/images/{editImageViewModel.Id}"
                );
            request.Content = new StringContent
            (
                serializedImageForUpdate
                ,Encoding.Unicode
                , "application/json"
            );
            var response = await httpClient.SendAsync
                (
                    request
                    , HttpCompletionOption.ResponseHeadersRead
                ).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var httpClient = httpClientFacotry.CreateClient("ApiClient");

            var request = new HttpRequestMessage
                (
                    HttpMethod.Delete,
                    $"https://localhost:44397/api/images/{id}"
                );

            var response = await httpClient.SendAsync
                (
                    request
                    , HttpCompletionOption.ResponseHeadersRead
                ).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }
        public IActionResult AddImage()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>AddImage(AddImageViewModel addImageViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }


            // create an ImageForCreation instance
            var imageForCreation = new ImageForCreation()
            { Title = addImageViewModel.Title };

            // take the first (only) file in the Files list
            var imageFile = addImageViewModel.Files.First();

            if (imageFile.Length > 0)
            {
                using (var fileStream = imageFile.OpenReadStream())
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    imageForCreation.Bytes = ms.ToArray();
                }
            }

            // serialize it
            var serializedImageForCreation = JsonSerializer.Serialize(imageForCreation);

            var httpClient = httpClientFacotry.CreateClient("APIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://localhost:44397/api/images");

            request.Content = new StringContent(
                serializedImageForCreation,
                System.Text.Encoding.Unicode,
                "application/json");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }
    }
}
