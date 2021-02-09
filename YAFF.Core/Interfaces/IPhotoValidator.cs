using YAFF.Core.Common;

namespace YAFF.Core.Interfaces
{
    public interface IPhotoValidator
    {
        Result<object> ValidatePhoto(IFile photo);
    }
}