namespace Infrastructure
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> AddFileAsync(string folderName, IFormFile file)
        {
             var fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderName, fileName);

            using FileStream fileStream = new(filePath, FileMode.OpenOrCreate);
            await file.CopyToAsync(fileStream);
            return fileName;
        }

        public async Task<bool> DeleteFileAsync(string folderName, string fileName)
        {
            return await Task.Run(() =>
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderName, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            });
        }

        public async Task<string> UpdateFileAsync(string folderName, IFormFile newFile, string oldFileName)
        {
            await Task.Run(() =>
            {
                if (oldFileName != null)
                {
                    var lastFilePath = Path.Combine(_webHostEnvironment.WebRootPath, folderName, oldFileName);
                    if (File.Exists(lastFilePath))
                    {
                        File.Delete(lastFilePath);
                    }
                }
            });

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newFile.FileName);

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderName, fileName);

            using FileStream fileStream = new(filePath, FileMode.OpenOrCreate);
            await newFile.CopyToAsync(fileStream);
            return fileName;
        }
    }
}
