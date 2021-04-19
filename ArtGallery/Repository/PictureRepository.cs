using ArtGallery.Interfaces;
using ArtGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ArtGallery.Repository
{
    public class PictureRepository : IPictureRepository, IDisposable
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

        public void Add(Picture Picture)
        {
            db.Pictures.Add(Picture);
            db.SaveChanges();
        }

        public void Delete(Picture Picture)
        {
            db.Pictures.Remove(Picture);
            db.SaveChanges();
        }

        public IQueryable<Picture> GetAll()
        {
            return db.Pictures.OrderBy(x => x.Name);
        }

        public Picture GetById(int id)
        {
            return db.Pictures.Find(id);
        }

        public void Update(Picture Picture)
        {
            db.Entry(Picture).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IQueryable<Picture> GetByYear(int godina)
        {
            return db.Pictures.Where(x => x.MadeYear > godina).OrderBy(x => x.MadeYear);
        }

        public IQueryable<Picture> PostSearch(decimal min, decimal max)
        {
            return db.Pictures.Where(x => x.Price > min && x.Price < max).OrderByDescending(x => x.Price);
        }

    }
}