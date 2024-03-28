namespace Bars.Gkh.Domain
{
    using Bars.B4.Modules.FileStorage;

    public interface IFileService
    {
        FileInfo ReCreateFile(FileInfo fileInfo);
    }
}