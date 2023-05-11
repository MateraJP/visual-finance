using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using VisualFinanceiro.Auth.Interfaces;

namespace VisualFinanceiro.Auth.Services
{
    internal class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient) => (_blobServiceClient) = (blobServiceClient);

        public async Task<Stream> GetAsync(string container, string filename)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = blobContainer.GetBlobClient(filename);

            if (!await blobClient.ExistsAsync())
                throw new ArgumentException($"Blob with name '{filename}' not found in '{container}'.", "filename");

            return await blobClient.OpenReadAsync();
        }

        public async IAsyncEnumerable<string> GetListAsync(string container, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container);
            await foreach (var blob in blobContainer.GetBlobsAsync(cancellationToken: cancellationToken))
                yield return blob.Name;
        }

        public async Task<string> SaveAsync(Stream file, string container, string filename, bool replaceifexists, CancellationToken cancellationToken = new CancellationToken())
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.BlobContainer, null, cancellationToken);

            var blobClient = blobContainer.GetBlobClient(filename);
            
            if (await blobClient.ExistsAsync(cancellationToken))
            {
                if (replaceifexists)
                {
                    await blobClient.DeleteAsync();
                }
                else
                {
                    throw new ArgumentException($"Blob with name '{filename}' already exists.", "filename");
                }
            }

            await blobClient.UploadAsync(file);
            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> DeleteAsync(string container, string filename, CancellationToken cancellationToken = new CancellationToken())
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container);
            return await blobContainer.DeleteBlobIfExistsAsync(filename, cancellationToken: cancellationToken);
        }
    }
}
