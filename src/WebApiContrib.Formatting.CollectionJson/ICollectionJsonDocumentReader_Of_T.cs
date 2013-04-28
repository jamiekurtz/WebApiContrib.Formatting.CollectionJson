namespace WebApiContrib.Formatting.CollectionJson
{
    public interface ICollectionJsonDocumentReader<TItem>
    {
        TItem Read(WriteDocument document);
    }
}