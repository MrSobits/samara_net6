namespace Bars.Gkh.Domain.Impl
{
    using System.IO;

    using Bars.B4.Config;
    using Bars.B4.Modules.FileStorage;

    using Castle.Windsor;

    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class FileService : IFileService
    {
        public IWindsorContainer Container { get; set; }
        public IFileManager FileManager { get; set; }

        public FileInfo ReCreateFile(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }

            var fileName = $"{fileInfo.Id}.{fileInfo.Extention}";
            var fileCreateDate = fileInfo.ObjectCreateDate;

            var pathDir = new DirectoryInfo(this.Container.Resolve<IConfigProvider>().GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"].GetAs("FileDirectory", string.Empty));
            var path = Path.Combine(pathDir.FullName, fileCreateDate.Year.ToString(), fileCreateDate.Month.ToString(), fileName);

            if (File.Exists(path))
            {
                using (var fileInfoStream = new MemoryStream(File.ReadAllBytes(path)))
                {
                    return this.FileManager.SaveFile(fileInfoStream, $"{fileInfo.Name}.{fileInfo.Extention}");
                }
            }

            return null;
        }
    }
}