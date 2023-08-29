namespace Infrastructure.Services.FileSaveService;

public abstract class FilesSaveServiceBase
{
    public string WebRootDirectoryPath { get; init; } = null!;

    protected string GetSaveDirectoryPath(string saveFolder)
    {
        string savePath = Path.Combine(WebRootDirectoryPath, saveFolder);

        if (Directory.Exists(savePath) == false)
            Directory.CreateDirectory(savePath);

        return savePath;
    }

    protected string GenerateFileName(string sourceFileName)
    {
        string sourceExtension = Path.GetExtension(sourceFileName);
        string randomFileName = Path.GetRandomFileName();
        
        string fileName = Path.ChangeExtension(randomFileName, sourceExtension);
        
        return fileName;
    }

    protected string GenerateSavePath(string fullPathToSaveDirectory, string fileName)
    {
        string savePath = Path.Combine(fullPathToSaveDirectory, fileName);
        return savePath;
    }
}