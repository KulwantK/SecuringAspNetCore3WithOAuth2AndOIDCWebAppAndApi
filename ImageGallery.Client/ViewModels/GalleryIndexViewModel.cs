using ImageGallery.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Client.ViewModels
{
    public class GalleryIndexViewModel
    {
        public IEnumerable<Image> Images { get; private set; }
            = new List<Image>();

        public GalleryIndexViewModel(IEnumerable<Image> images)
        {
            Images = images;
        }
    }
}
