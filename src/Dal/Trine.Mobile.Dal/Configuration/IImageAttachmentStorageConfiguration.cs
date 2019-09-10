namespace Trine.Mobile.Dal.Configuration
{
    public interface IImageAttachmentStorageConfiguration
    {
        string ConnectionString { get; }
        string Container { get; }
        string MetadataKey { get; }
    }
}
