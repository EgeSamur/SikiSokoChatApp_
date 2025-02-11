namespace SikiSokoChatApp.Shared.CrossCuttingConcerns.Logging.Serilog.Configurations;

/// <summary>
/// Represents the configuration settings for file logging.
/// </summary>
public class FileLogConfiguration
{
    public string FolderPath { get; set; }

    public FileLogConfiguration()
    {
        FolderPath = string.Empty;
    }

    public FileLogConfiguration(string folderPath)
    {
        FolderPath = folderPath;
    }
}