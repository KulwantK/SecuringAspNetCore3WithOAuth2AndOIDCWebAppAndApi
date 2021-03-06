using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Client.ViewModels
{
    public class EditImageViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
