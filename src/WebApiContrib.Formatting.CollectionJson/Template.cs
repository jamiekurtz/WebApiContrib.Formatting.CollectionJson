using System.Collections.Generic;

namespace WebApiContrib.Formatting.CollectionJson
{
    public class Template
    {
        public Template()
        {
            Data = new List<Data>();
        }

        public IList<Data> Data { get; set; }
    }
}