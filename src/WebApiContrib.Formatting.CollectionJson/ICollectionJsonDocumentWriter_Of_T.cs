using System.Collections.Generic;

namespace WebApiContrib.Formatting.CollectionJson
{
    public interface ICollectionJsonDocumentWriter<TItem>
    {
        ReadDocument Write(IEnumerable<TItem> data);
    }
}