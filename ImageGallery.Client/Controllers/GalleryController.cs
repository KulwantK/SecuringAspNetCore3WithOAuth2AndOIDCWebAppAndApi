using ImageGallery.Client.ViewModels;
using ImageGallery.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    }
}
