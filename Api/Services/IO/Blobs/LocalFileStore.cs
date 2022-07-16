using Api.Common;
using System.Text.RegularExpressions;

namespace Api.Services.IO.Blobs
{
    public class LocalFileStore : IBlobService
    {
        private readonly IConfiguration configuration;
        private static readonly Regex validName = new Regex(@"^[0-9a-zA-Z_\-]+$");
        private static readonly string invalidNameMessage = "Name contains invalid characters.";
        private readonly ReaderWriterLockSlim lck;

        public LocalFileStore(
            IConfiguration configuration)
        {
            this.configuration = configuration;
            this.lck = new ReaderWriterLockSlim();
        }

        public Task<Stream> GetBlob(string container, string name)
        {
            var (containerPath, path) = this.validateNames(container, name);

            return Task.FromResult<Stream>(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None));
        }

        public async Task UploadBlob(string container, string name, Stream content, bool overwrite)
        {
            var (containerPath, path) = this.validateNames(container, name);

            if (System.IO.Directory.Exists(containerPath) == false)
            {
                System.IO.Directory.CreateDirectory(containerPath);
            }

            using (content)
            {
                if (overwrite == true && System.IO.File.Exists(path) == true)
                {
                    throw new ArgumentException($"'{name}' already exists in container '{container}'.");
                }

                using (var writer = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    await content.CopyToAsync(writer);
                }
            }
        }

        private (string containerPath, string path) validateNames(string container, string name)
        {
            ArgumentNullException.ThrowIfNull(container);
            ArgumentNullException.ThrowIfNull(name);

            Exceptions.ThrowIf(container, c => validName.IsMatch(c?.ToString() ?? ""), invalidNameMessage);
            Exceptions.ThrowIf(name, c => validName.IsMatch(c?.ToString() ?? ""), invalidNameMessage);

            var containerPath = Path.Combine(this.configuration["connectionStrings:blobs"], container);

            var path = Path.Combine(containerPath, name);

            return (containerPath, path);
        }

        public Task<bool> Exists(string container, string name)
        {
            var (_, path) = this.validateNames(container, name);

            return Task.FromResult(System.IO.File.Exists(path));
        }
    }
}
