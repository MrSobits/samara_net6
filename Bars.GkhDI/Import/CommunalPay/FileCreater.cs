namespace Bars.GkhDi.Import
{
    using System.IO;
    using System.Linq;
    using System.Web;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.FileStorage;

    using Ionic.Zip;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public static class FileCreater
    {
        public static FileInfo Create(string zipName, string fileName, IDomainService<FileInfo> fileinfoService, IFileManager fileManager)
        {
            var data = GetData(zipName, fileName);

            if (data.Length == 0)
            {
                return null;
            }

            var fileInfo = fileManager.SaveFile(data, fileName);

            fileinfoService.Save(fileInfo);

            return fileInfo;
        }

        private static MemoryStream GetData(string zipName, string fileName)
        {
            var result = new MemoryStream();
            if (!string.IsNullOrEmpty(fileName))
            {
                var path = ApplicationContext.Current.MapPath(string.Format("~/Import/{0}", zipName));

                using (var zipFile = ZipFile.Read(path))
                {
                    var pdfZipEntry =
                        zipFile.FirstOrDefault(x => x.FileName == fileName);

                    if (pdfZipEntry != null)
                    {
                        pdfZipEntry.Extract(result);
                    }
                }


            }

            return result;
        }
    }
}
