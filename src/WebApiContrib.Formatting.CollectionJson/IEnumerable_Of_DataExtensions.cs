using System.Collections.Generic;
using System.Linq;

namespace WebApiContrib.Formatting.CollectionJson
{
    public static class IEnumerable_Of_DataExtensions
    {
        public static Data GetDataByName(this IEnumerable<Data> data, string name)
        {
            return data.Single(d => d.Name == name);
        }
    }
}