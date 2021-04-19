using ArtGallery.Interfaces;
using ArtGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ArtGallery.Repository
{
    public class GalleryRepository : IGalleryRepository, IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Add(Gallery Gallery)
        {
            db.Galleries.Add(Gallery);
            db.SaveChanges();
        }

        public void Delete(Gallery Gallery)
        {
            db.Galleries.Remove(Gallery);
            db.SaveChanges();
        }

        public IQueryable<Gallery> GetAll()
        {
            return db.Galleries.OrderBy(x => x.Name);
        }

        public Gallery GetById(int id)
        {
            return db.Galleries.Find(id);
        }

        public void Update(Gallery Gallery)
        {
            db.Entry(Gallery).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IQueryable<Gallery> GetTradition()
        {
            return db.Galleries.OrderBy(x => x.Year);

        }

        public IQueryable<GalleryPicturesDTO> GetNumber()
        {
            var galeries = GetAll().ToList();
            List<GalleryPicturesDTO> lista = new List<GalleryPicturesDTO>();
            foreach (Gallery g in galeries)
            {
                int pom = db.Pictures.Where(x => x.GalleryId == g.Id).Count();
                lista.Add(new GalleryPicturesDTO { Id = g.Id, Name = g.Name, PicturesSum = pom });
            }


            return lista.OrderByDescending(x => x.PicturesSum).AsQueryable();
        }

    }
}