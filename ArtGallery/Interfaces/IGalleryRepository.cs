using ArtGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Interfaces
{
    public interface IGalleryRepository
    {
        Gallery GetById(int id);
        void Add(Gallery Galery);
        IQueryable<Gallery> GetAll();
        IQueryable<Gallery> GetTradition();
        IQueryable<GalleryPicturesDTO> GetNumber();

        void Update(Gallery Gallery);
        void Delete(Gallery Gallery);
    }
}
