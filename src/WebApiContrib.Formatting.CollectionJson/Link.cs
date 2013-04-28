using System;

namespace WebApiContrib.Formatting.CollectionJson
{
    public class Link
    {
        public string Rel { get; set; }
        public Uri Href { get; set; }
        public string Prompt { get; set; }
        public string Render { get; set; }
    }
}