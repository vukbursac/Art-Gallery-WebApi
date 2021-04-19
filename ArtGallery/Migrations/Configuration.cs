namespace ArtGallery.Migrations
{
    using ArtGallery.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ArtGallery.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ArtGallery.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Galleries.AddOrUpdate(
        new Gallery() { Id = 1, Name = "Ancient Cleverness Group", Year = 2005 },
        new Gallery() { Id = 2, Name = "The Cultural Center", Year = 2011, },
        new Gallery() { Id = 3, Name = "Exclusive Designs", Year = 1999, }

    );

            context.SaveChanges();

            context.Pictures.AddOrUpdate(
                    new Picture() { Id = 1, Name = "Golden Stiching", Author = "Ruth Gould", Price = 38000m, MadeYear = 1989, GalleryId = 2 },
                    new Picture() { Id = 2, Name = "Clay & Paper", Author = "Lani Roach", Price = 19900m, MadeYear = 2007, GalleryId = 3 },
                    new Picture() { Id = 3, Name = "Greeting Card", Author = "Chiara Gallagher", Price = 12000m, MadeYear = 2012, GalleryId = 1 },
                    new Picture() { Id = 4, Name = "Alexandria Revisited", Author = "Devonte Almond", Price = 5220m, MadeYear = 1983, GalleryId = 2 },
                    new Picture() { Id = 5, Name = "Alice’s First Puppy", Author = "Ishan O'Reilly", Price = 3400m, MadeYear = 1994, GalleryId = 1 }


                );

            context.SaveChanges();
        }
    }
}
