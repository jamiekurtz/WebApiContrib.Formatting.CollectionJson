using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiContrib.Formatting.CollectionJson
{
    public abstract class CollectionJsonController<TData> : CollectionJsonController<TData, int>
    {
        protected CollectionJsonController(ICollectionJsonDocumentWriter<TData> writer,
                                           ICollectionJsonDocumentReader<TData> reader, string routeName = "DefaultApi")
            :
                base(writer, reader, routeName)
        {
        }
    }

    public abstract class CollectionJsonController<TData, TId> : ApiController
    {
        private readonly CollectionJsonFormatter _formatter = new CollectionJsonFormatter();
        private readonly ICollectionJsonDocumentReader<TData> _reader;
        private readonly string _routeName;
        private readonly ICollectionJsonDocumentWriter<TData> _writer;

        public CollectionJsonController(ICollectionJsonDocumentWriter<TData> writer,
                                        ICollectionJsonDocumentReader<TData> reader, string routeName = "DefaultApi")
        {
            _routeName = routeName;
            _writer = writer;
            _reader = reader;
        }


        private string ControllerName
        {
            get { return ControllerContext.ControllerDescriptor.ControllerName; }
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            controllerContext.Configuration.Formatters.Add(_formatter);
            base.Initialize(controllerContext);
        }

        private ObjectContent GetDocumentContent(ReadDocument document)
        {
            return new ObjectContent<ReadDocument>(document, _formatter, "application/vnd.collection+json");
        }

        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();
            IEnumerable<TData> data = Read(response);
            response.Content = GetDocumentContent(_writer.Write(data));
            return response;
        }

        public HttpResponseMessage Get(TId id)
        {
            var response = new HttpResponseMessage();
            TData data = Read(id, response);
            response.Content = GetDocumentContent(_writer.Write(new[] {data}));
            return response;
        }

        public HttpResponseMessage Post(WriteDocument document)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            int id = Create(_reader.Read(document), response);
            response.Headers.Location = new Uri(Url.Link(_routeName, new {controller = ControllerName, id}));
            return response;
        }

        public HttpResponseMessage Put(TId id, WriteDocument document)
        {
            var response = new HttpResponseMessage();
            TData data = Update(id, _reader.Read(document), response);
            response.Content = GetDocumentContent(_writer.Write(new[] {data}));
            return response;
        }

        [AcceptVerbs("DELETE")]
        public HttpResponseMessage Remove(TId id)
        {
            var response = new HttpResponseMessage();
            Delete(id, response);
            return response;
        }

        protected virtual int Create(TData data, HttpResponseMessage response)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        protected virtual TData Read(TId id, HttpResponseMessage response)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        protected virtual IEnumerable<TData> Read(HttpResponseMessage response)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        protected virtual TData Update(TId id, TData data, HttpResponseMessage response)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        protected virtual void Delete(TId id, HttpResponseMessage response)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }
    }
}