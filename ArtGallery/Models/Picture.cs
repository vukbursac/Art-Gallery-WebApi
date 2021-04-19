using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGallery.Models
{
    public class Picture
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        [Required]
        [StringLength(70)]
        public string Author { get; set; }
        [Range(1520, 2019)]
        public int MadeYear { get; set; }
        [Required]
        [Range(100.00, 49999.99)]
        public decimal Price { get; set; }
        public virtual int GalleryId { get; set; }
        public virtual Gallery Gallery { get; set; }
    }
}