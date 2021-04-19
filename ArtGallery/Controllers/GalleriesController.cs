using ArtGallery.Interfaces;
using ArtGallery.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ArtGallery.Controllers
{
    public class GalleriesController : ApiController
    {
        IGalleryRepository _repository { get; set; }

        public GalleriesController(IGalleryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ResponseType(typeof(IQueryable<GalleryDTO>))]
        public IQueryable<GalleryDTO> GetAll()
        {
            return _repository.GetAll().ProjectTo<GalleryDTO>();
        }

        [Authorize]
        [ResponseType(typeof(GalleryDTO))]
        public IHttpActionResult GetById(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            if (_repository.GetById(id) == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<GalleryDTO>(_repository.GetById(id)));
        }

        [Authorize]
        [ResponseType(typeof(GalleryDTO))]
        public IHttpActionResult Post(Gallery Gallery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(Gallery);
            return CreatedAtRoute("DefaultApi", new { id = Gallery.Id }, Mapper.Map<GalleryDTO>(Gallery));
        }

        [Authorize]
        [ResponseType(typeof(GalleryDTO))]
        public IHttpActionResult Put(int id, Gallery Gallery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Gallery.Id)
            {
                return BadRequest();
            }
            try
            {
                _repository.Update(Gallery);
            }
            catch (Exception)
            {

                return BadRequest();
            }

            return Ok(Mapper.Map<GalleryDTO>(Gallery));
        }

        [Authorize]
        [ResponseType(typeof(HttpStatusCode))]
        public IHttpActionResult Delete(int id)
        {
            Gallery Gallery = _repository.GetById(id);
            if (Gallery == null)
            {
                return NotFound();
            }

            _repository.Delete(Gallery);

            return StatusCode(HttpStatusCode.NoContent);
            //return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("api/tradition")]
        [ResponseType(typeof(IQueryable<GalleryDTO>))]
        public IHttpActionResult GetTradition()
        {
            var galeries = _repository.GetTradition();
            if (galeries == null)
            {
                return NotFound();

            }
            return Ok(galeries.ProjectTo<GalleryDTO>());
        }
        [HttpGet]
        [Authorize]
        [Route("api/number")]
        [ResponseType(typeof(IQueryable<GalleryPicturesDTO>))]
        public IHttpActionResult GetNumber()
        {
            var result = _repository.GetNumber();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
