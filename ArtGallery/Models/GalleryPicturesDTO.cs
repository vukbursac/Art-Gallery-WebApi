using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtGallery.Models
{
    public class GalleryPicturesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PicturesSum { get; set; }
    }
}