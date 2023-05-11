using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VisualFinanceiro.Auth.Interfaces
{
    public interface IBlobService
    {
        Task<Stream> GetAsync(string container, string filename);//GetBlobRequest request);

        IAsyncEnumerable<string> GetListAsync(string container, CancellationToken cancellationToken);

        Task<string> SaveAsync(Stream file, string container, string filename, bool replaceifexists = false, CancellationToken cancellationToken = new CancellationToken());

        Task<bool> DeleteAsync(string container, string filename, CancellationToken cancellationToken = new CancellationToken());
    }
}
