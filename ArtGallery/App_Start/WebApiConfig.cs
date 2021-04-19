using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ArtGallery.Interfaces;
using ArtGallery.Models;
using ArtGallery.Repository;
using ArtGallery.Resolver;
using AutoMapper;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;

namespace ArtGallery
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.EnableSystemDiagnosticsTracing();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Gallery, GalleryDTO>();
                cfg.CreateMap<Picture, PictureDTO>()
                .ForMember(dest => dest.GalleryName, opt => opt.MapFrom(src => src.Gallery.Name))
                .ForMember(dest => dest.GalleryId, opt => opt.MapFrom(src => src.Gallery.Id));


            });

            var container = new UnityContainer();
            //container.RegisterType<IGalerysRepository, GalerysRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPictureRepository, PictureRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IGalleryRepository, GalleryRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}
