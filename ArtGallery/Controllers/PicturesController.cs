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
    public class PicturesController : ApiController
    {
        IPictureRepository _repository { get; set; }

        public PicturesController(IPictureRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ResponseType(typeof(IQueryable<PictureDTO>))]
        public IQueryable<PictureDTO> GetAll()
        {
            return _repository.GetAll().ProjectTo<PictureDTO>();
        }

        [Authorize]
        [ResponseType(typeof(PictureDTO))]
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
            return Ok(Mapper.Map<PictureDTO>(_repository.GetById(id)));
        }

        [Authorize]
        [ResponseType(typeof(PictureDTO))]
        public IHttpActionResult Post(Picture Picture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(Picture);
            return CreatedAtRoute("DefaultApi", new { id = Picture.Id }, Mapper.Map<PictureDTO>(Picture));
        }

        [Authorize]
        [ResponseType(typeof(PictureDTO))]
        public IHttpActionResult Put(int id, Picture Picture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Picture.Id)
            {
                return BadRequest();
            }
            try
            {
                _repository.Update(Picture);
            }
            catch (Exception)
            {

                return BadRequest();
            }

            return Ok(Mapper.Map<PictureDTO>(Picture));
        }

        [Authorize]
        [ResponseType(typeof(HttpStatusCode))]
        public IHttpActionResult Delete(int id)
        {
            Picture Picture = _repository.GetById(id);
            if (Picture == null)
            {
                return NotFound();
            }

            _repository.Delete(Picture);

            return StatusCode(HttpStatusCode.NoContent);
            //return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/search")]
        [ResponseType(typeof(IQueryable<PictureDTO>))]
        public IHttpActionResult PostSearch(decimal min, decimal max)
        {
            if (min < 100m || max < 100m || min > max)
            {
                return BadRequest();
            }
            var pictures = _repository.PostSearch(min, max);
            if (pictures == null)
            {
                return NotFound();
            }
            return Ok(pictures.ProjectTo<PictureDTO>());
        }

        [HttpGet]
        //[Authorize]
        [ResponseType(typeof(IQueryable<PictureDTO>))]
        public IHttpActionResult GetByYear(int made)
        {
            if (made < 1520 || made > 2019)
            {
                return BadRequest();
            }
            var pictures = _repository.GetByYear(made);
            if (pictures == null)
            {
                return NotFound();
            }
            return Ok(pictures.ProjectTo<PictureDTO>());
        }
    }
}
