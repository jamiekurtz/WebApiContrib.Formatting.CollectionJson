using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Moq;
using Should;
using Xunit;

namespace WebApiContrib.Formatting.CollectionJson.Tests
{
    public class CollectionJsonControllerTests
    {
        private readonly Mock<ICollectionJsonDocumentReader<string>> _reader;
        private readonly ReadDocument _testReadDocument;
        private readonly WriteDocument _testWriteDocument;
        private readonly Mock<ICollectionJsonDocumentWriter<string>> _writer;
        private TestController _controller;

        public CollectionJsonControllerTests()
        {
            _reader = new Mock<ICollectionJsonDocumentReader<string>>();
            _writer = new Mock<ICollectionJsonDocumentWriter<string>>();

            _testReadDocument = new ReadDocument();
            _testReadDocument.Collection.Href = new Uri("http://test.com");
            _testWriteDocument = new WriteDocument();

            _writer.Setup(w => w.Write(It.IsAny<IEnumerable<string>>())).Returns(_testReadDocument);
            _reader.Setup(r => r.Read(It.IsAny<WriteDocument>())).Returns("Test");
            Configure();
        }

        private void Configure()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test/");
            var config = new HttpConfiguration();
            config.Formatters.Add(new CollectionJsonFormatter());
            IHttpRoute route = config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}",
                                                          new {id = RouteParameter.Optional});
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary {{"controller", "test"}});

            _controller = new TestController(_writer.Object, _reader.Object);
            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(config, "test",
                                                                                             typeof (TestController));
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);
        }

        [Fact]
        public void WhenGettingAllShouldReturnDocument()
        {
            HttpResponseMessage response = _controller.Get();
            ReadDocument value = response.Content.ReadAsAsync<ReadDocument>().Result;
            value.Collection.Href.AbsoluteUri.ShouldEqual("http://test.com/");
        }

        [Fact]
        public void WhenGettingSingleShouldReturnDocument()
        {
            HttpResponseMessage response = _controller.Get(1);
            ReadDocument value = response.Content.ReadAsAsync<ReadDocument>().Result;
            value.Collection.Href.AbsoluteUri.ShouldEqual("http://test.com/");
        }

        [Fact]
        public void WhenPostingShouldSetLocationHeader()
        {
            _controller.Request.Method = HttpMethod.Post;
            HttpResponseMessage response = _controller.Post(_testWriteDocument);
            response.Headers.Location.AbsoluteUri.ShouldEqual("http://localhost/test/1");
        }

        [Fact]
        public void WhenPostingShouldSetStatusToCreated()
        {
            _controller.Request.Method = HttpMethod.Post;
            HttpResponseMessage response = _controller.Post(_testWriteDocument);
            response.StatusCode.ShouldEqual(HttpStatusCode.Created);
        }

        [Fact]
        public void WhenPostingShouldCallCreate()
        {
            _controller.Request.Method = HttpMethod.Post;
            _controller.Post(_testWriteDocument);
            _controller.CreateCalled.ShouldBeTrue();
        }

        [Fact]
        public void WhenPutShouldReturnDocument()
        {
            _controller.Request.Method = HttpMethod.Put;
            HttpResponseMessage response = _controller.Put(1, _testWriteDocument);
            ReadDocument value = response.Content.ReadAsAsync<ReadDocument>().Result;
            value.Collection.Href.AbsoluteUri.ShouldEqual("http://test.com/");
        }

        [Fact]
        public void WhenPutShouldCallUpdate()
        {
            _controller.Request.Method = HttpMethod.Put;
            _controller.Put(1, _testWriteDocument);
            _controller.UpdateCalled.ShouldBeTrue();
        }

        [Fact]
        public void WhenRemoveShouldCallDelete()
        {
            _controller.Request.Method = HttpMethod.Delete;
            _controller.Remove(1);
            _controller.DeleteCalled.ShouldBeTrue();
        }
    }

    //needed for overriding protected methods
    public class TestController : CollectionJsonController<string>
    {
        public const string TestValue = "Test";

        public bool CreateCalled;
        public bool DeleteCalled;
        public bool ReadAllCalled;
        public bool ReadSingleCalled;
        public bool UpdateCalled;

        public TestController(ICollectionJsonDocumentWriter<string> writer, ICollectionJsonDocumentReader<string> reader)
            : base(writer, reader)
        {
        }

        protected override int Create(string data, HttpResponseMessage response)
        {
            CreateCalled = true;
            return 1;
        }

        protected override string Read(int id, HttpResponseMessage response)
        {
            ReadSingleCalled = true;
            return TestValue;
        }

        protected override IEnumerable<string> Read(HttpResponseMessage response)
        {
            ReadAllCalled = true;
            return new[] {TestValue};
        }

        protected override string Update(int id, string data, HttpResponseMessage response)
        {
            UpdateCalled = true;
            return data;
        }

        protected override void Delete(int id, HttpResponseMessage response)
        {
            DeleteCalled = true;
        }
    }
}