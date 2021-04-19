using ArtGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Interfaces
{
    public interface IPictureRepository
    {
        Picture GetById(int id);
        IQueryable<Picture> GetByYear(int godina);
        void Add(Picture Picture);
        IQueryable<Picture> GetAll();
        void Update(Picture Picture);
        void Delete(Picture Picture);
        IQueryable<Picture> PostSearch(decimal min, decimal max);
    }
}
