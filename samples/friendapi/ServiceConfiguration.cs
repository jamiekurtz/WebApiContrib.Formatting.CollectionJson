﻿using System.Web.Http;
using Autofac;
using WebApiContrib.Formatting.CollectionJson.Infrastructure;
using WebApiContrib.Formatting.CollectionJson.Models;
using WebApiContrib.IoC.Autofac;

namespace WebApiContrib.Formatting.CollectionJson
{
    public static class ServiceConfiguration
    {
        public static void Configure(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                "default", "{controller}/{id}",
                new {id = RouteParameter.Optional});

            var builder = new ContainerBuilder();
            builder.RegisterType<FakeFriendRepository>().As<IFriendRepository>();
            builder.RegisterType<FriendDocumentWriter>().As<ICollectionJsonDocumentWriter<Friend>>();
            builder.RegisterType<FriendDocumentReader>().As<ICollectionJsonDocumentReader<Friend>>();
            builder.RegisterApiControllers(typeof (ServiceConfiguration).Assembly);

            IContainer container = builder.Build();
            var resolver = new AutofacResolver(container);

            config.DependencyResolver = resolver;
        }
    }
}