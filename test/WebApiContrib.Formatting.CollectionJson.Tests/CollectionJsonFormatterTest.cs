using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Should;
using Xunit;

namespace WebApiContrib.Formatting.CollectionJson.Tests
{
    public class CollectionJsonFormatterTest
    {
        private readonly CollectionJsonFormatter _formatter = new CollectionJsonFormatter();

        [Fact]
        public void WhenTypeIsWriteDocumentShouldBeAbleToRead()
        {
            _formatter.CanReadType(typeof (WriteDocument)).ShouldBeTrue();
        }

        [Fact]
        public void WhenTypeIsWriteDocumentShouldBeAbleToWrite()
        {
            _formatter.CanWriteType(typeof (WriteDocument)).ShouldBeTrue();
        }

        [Fact]
        public void WhenTypeIsReadDocumentShouldBeAbleToRead()
        {
            _formatter.CanReadType(typeof (ReadDocument)).ShouldBeTrue();
        }

        [Fact]
        public void WhenTypeIsReadDocumentShouldBeAbleToWrite()
        {
            _formatter.CanWriteType(typeof (ReadDocument)).ShouldBeTrue();
        }

        [Fact]
        public void WhenTypeIsStringShouldNotBeAbleToRead()
        {
            _formatter.CanReadType(typeof (string)).ShouldBeFalse();
        }

        [Fact]
        public void WhenTypeIsStringShouldNotBeAbleToWrite()
        {
            _formatter.CanReadType(typeof (string)).ShouldBeFalse();
        }

        [Fact]
        public void WhenInitializedShouldSetSupportedMediaTpeToCollectionJson()
        {
            _formatter.SupportedMediaTypes.Count(m => m.MediaType == "application/vnd.collection+json").ShouldEqual(1);
        }

        [Fact]
        public void WhenInitializedShouldSetCamelCasePropertyResolver()
        {
            _formatter.SerializerSettings.ContractResolver.ShouldBeType<CamelCasePropertyNamesContractResolver>();
        }

        [Fact]
        public void WhenInitializedShouldSetIndentation()
        {
            _formatter.SerializerSettings.Formatting.ShouldEqual(Newtonsoft.Json.Formatting.Indented);
        }

        [Fact]
        public void WhenInitializedShouldIgnoreNulls()
        {
            _formatter.SerializerSettings.NullValueHandling.ShouldEqual(NullValueHandling.Ignore);
        }
    }
}