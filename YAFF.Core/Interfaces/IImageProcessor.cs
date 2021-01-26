using System.IO;
using System.Threading.Tasks;

namespace YAFF.Core.Interfaces
{
    /// <summary>
    /// Image processor
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Resizes image
        /// </summary>
        /// <param name="image"></param>
        /// <returns>Resized png image stored as a byte array</returns>
        Task<byte[]> ResizeImage(IFile image);

        /// <inheritdoc cref="ResizeImage(YAFF.Core.Interfaces.IFile)"/>
        Task<byte[]> ResizeImage(Stream image);
    }
}