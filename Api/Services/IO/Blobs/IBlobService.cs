namespace Api.Services.IO.Blobs
{
    public interface IBlobService
    {
        Task UploadBlob(string container, string name, Stream content, bool overwrite);
        Task<Stream> GetBlob(string container, string name);
        Task<bool> Exists(string container, string name);
    }
}
