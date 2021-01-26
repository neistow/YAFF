using System.IO;
using System.Threading.Tasks;

namespace YAFF.Core.Interfaces
{
    /// <summary>
    /// Represents a Photo Storage
    /// </summary>
    public interface IPhotoStorage
    {
        /// <summary>
        /// Stores a photo
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>Stored Photo FileName</returns>
        Task<string> StorePhotoAsync(IFile photo);

        /// <summary>
        /// Stores a photo
        /// </summary>
        /// <param name="photo">Photo as byte array</param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task<string> StorePhotoAsync(byte[] photo, string extension);

        /// <summary>
        /// Deletes photo with specified fileName
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>True if result was successful</returns>
        Task DeletePhotoAsync(string fileName);
    }
}