using CloudinaryDotNet.Actions;

namespace API.Data.Interfaces;

public interface IPhotoService
{
  Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
  Task<DeletionResult> DeletePhotoAsync(string publicId);
}
