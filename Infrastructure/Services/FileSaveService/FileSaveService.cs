using Application.Common.Contracts;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileSaveService;

public class FileSaveService : FilesSaveServiceBase, IFileSaveService
{
    public async Task<string> SaveAsync(IFormFile formFile, string saveFolder)
    {
        if(formFile is null)
            throw new NullReferenceException("Нет целевого файла на сохранение");
        if (string.IsNullOrEmpty(saveFolder) == true)
            throw new NullReferenceException("Название папки для сохранения не указана");
        
        string saveDirectory = GetSaveDirectoryPath(saveFolder);

        string fileName = GenerateFileName(formFile.FileName);
        string savePath = GenerateSavePath(saveDirectory, fileName);
        
        await using Stream stream = new FileStream(savePath, FileMode.Create);
        await formFile.CopyToAsync(stream);

        return Path.Combine(saveFolder, fileName);
    }
}