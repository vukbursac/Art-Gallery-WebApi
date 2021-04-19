using ArtGallery.Controllers;
using ArtGallery.Interfaces;
using ArtGallery.Models;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace ArtGallery.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Picture, PictureDTO>()
                .ForMember(dest => dest.GalleryName, opt => opt.MapFrom(src => src.Gallery.Name))
                .ForMember(dest => dest.GalleryId, opt => opt.MapFrom(src => src.Gallery.Id));

            });

            var mockRepository = new Mock<IPictureRepository>();
            mockRepository.Setup(x => x.GetById(4)).Returns(new Picture() { Id = 4 });

            var controller = new PicturesController(mockRepository.Object);

            IHttpActionResult actionResult = controller.GetById(4);
            var contentResult = actionResult as OkNegotiatedContentResult<PictureDTO>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(4, contentResult.Content.Id);
        }
        [TestMethod]
        public void TestMethod2()
        {
            // Arrange
            var mockRepository = new Mock<IPictureRepository>();
            var controller = new PicturesController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(10, new Picture { Id = 9, Name = "Picture No.1" });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void TestMethod3()
        {
            List<Picture> lista = new List<Picture>();
            lista.Add(new Picture()
            {
                Id = 1,
                Name = "test1",
                Gallery = new Gallery() { Id = 1, Name = "Gallery No.1" }
            });
            lista.Add(new Picture()
            {
                Id = 2,
                Name = "test2",
                Gallery = new Gallery() { Id = 2, Name = "Gallery No.2" }
            });

            var mockRepository = new Mock<IPictureRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(lista.AsQueryable());
            var controller = new PicturesController(mockRepository.Object);

            IQueryable<PictureDTO> result = controller.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(lista.Count, result.ToList().Count);

            Assert.AreEqual(lista.ElementAt(0).Id, result.ToList().ElementAt(0).Id);
            Assert.AreEqual(lista.ElementAt(0).Name, result.ToList().ElementAt(0).Name);
            Assert.AreEqual(lista.ElementAt(0).Gallery.Name, result.ToList().ElementAt(0).GalleryName);

            Assert.AreEqual(lista.ElementAt(1).Id, result.ToList().ElementAt(1).Id);
            Assert.AreEqual(lista.ElementAt(1).Name, result.ToList().ElementAt(1).Name);
            Assert.AreEqual(lista.ElementAt(1).Gallery.Name, result.ToList().ElementAt(1).GalleryName);
        }

        [TestMethod]
        public void TestMethod4()
        {
            List<Picture> lista1 = new List<Picture>();
            lista1.Add(new Picture()
            {
                Id = 1,
                Name = "test1",
                Price = 120m,
                Gallery = new Gallery() { Id = 1, Name = "Galery No.1" }
            });
            lista1.Add(new Picture()
            {
                Id = 2,
                Name = "test2",
                Price = 130m,
                Gallery = new Gallery() { Id = 2, Name = "Galery No.2" }
            });

            var mockRepository = new Mock<IPictureRepository>();
            mockRepository.Setup(x => x.PostSearch(101m, 150m)).Returns(lista1.AsQueryable());
            var controller = new PicturesController(mockRepository.Object);

            IHttpActionResult result = controller.PostSearch(101m, 150m);
            var finalResult = result as OkNegotiatedContentResult<IQueryable<PictureDTO>>;

            Assert.IsNotNull(finalResult);
            Assert.AreEqual(lista1.Count, finalResult.Content.ToList().Count);

            Assert.AreEqual(lista1.ElementAt(0).Id, finalResult.Content.ToList().ElementAt(0).Id);
            Assert.AreEqual(lista1.ElementAt(0).Name, finalResult.Content.ToList().ElementAt(0).Name);
            Assert.AreEqual(lista1.ElementAt(0).Gallery.Name, finalResult.Content.ToList().ElementAt(0).GalleryName);

            Assert.AreEqual(lista1.ElementAt(1).Id, finalResult.Content.ToList().ElementAt(1).Id);
            Assert.AreEqual(lista1.ElementAt(1).Name, finalResult.Content.ToList().ElementAt(1).Name);
            Assert.AreEqual(lista1.ElementAt(1).Gallery.Name, finalResult.Content.ToList().ElementAt(1).GalleryName);
        }
    }
}
