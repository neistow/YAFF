using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace YAFF.Core.Interfaces
{
    public interface IFile
    {
        long Length { get; }
        string FileName { get; }
        Stream OpenReadStream();
        void CopyTo(Stream target);
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
    }
}