namespace Bars.Gkh.PrintForm.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.PrintForm.Entities;
    using Bars.Gkh.PrintForm.Utils;
    using Castle.Windsor;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.security;
    using Microsoft.Office.Interop.Excel;
    using Microsoft.Office.Interop.Word;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    /// <inheritdoc />
    public class PdfTunerService : IPdfTunerService
    {
        private const string forHashPrefix = "for_hash_";
        private const string stampedPrefix = "stamped_";

        #region Dependency Injection
        private readonly IFileManager _fileManager;
        private readonly IWindsorContainer _container;

        public PdfTunerService(IFileManager fileManager, IWindsorContainer container)
        {
            this._fileManager = fileManager;
            this._container = container;
        }
        #endregion

        /// <inheritdoc />
        public async Task<FileInfo> EmbedSignatureAsync(FileInfo tmpFile, FileInfo signedPdf, byte[] signedBytes, string contextGuid) =>
            await EmbedSignatureAsyncInternal(tmpFile, signedPdf, signedBytes, contextGuid);

        /// <inheritdoc />
        public async Task<FileInfo> EmbedSignatureAsync(FileInfo tmpFile, FileInfo signedPdf, string signature, string contextGuid) =>
            await EmbedSignatureAsyncInternal(tmpFile, signedPdf, Convert.FromBase64String(signature), contextGuid);

        /// <inheritdoc />
        public async Task<PdfSignInfo> AddSignatureStampAsync(FileInfo file, BaseStampStrategy stampStrategy)
        {
            using (var reader = new PdfReader(this.ConvertToPdfIfMsOffice(file)))
            {
                FileInfo stampedPdfInfo;
                FileInfo forHashFileInfo;
                var tempPdf = Path.GetRandomFileName();
                var pdfFileName = $"{file.Name}.pdf";

                using (var os = File.OpenWrite(tempPdf))
                using(var ms = new MemoryStream())
                {
                    var appearance = stampStrategy.Stamp(reader, os);

                    await appearance.GetRangeStream().CopyToAsync(ms);

                    forHashFileInfo = this._fileManager.SaveFile(appearance.GetRangeStream(), PdfTunerService.forHashPrefix + pdfFileName);
                }

                using (var stampedPdf = File.OpenRead(tempPdf))
                {
                    stampedPdfInfo = this._fileManager.SaveFile(stampedPdf, PdfTunerService.stampedPrefix + pdfFileName);
                }

                if (File.Exists(tempPdf))
                {
                    File.Delete(tempPdf);
                }

                return new PdfSignInfo { OriginalFile = file, ForHasFile = forHashFileInfo, StampedPdf = stampedPdfInfo };
            }
        }

        /// <summary>
        /// Добавить ЭП в файл
        /// </summary>
        private async Task<FileInfo> EmbedSignatureAsyncInternal(FileInfo tmpFile, FileInfo signedPdf, byte[] signedBytes, string contextGuid)
        {
            using (var reader = new PdfReader(this._fileManager.GetFile(tmpFile)))
            using (var sourceMs = this._fileManager.GetFile(signedPdf))
            using (var os = new MemoryStream())
            {
                await sourceMs.CopyToAsync(os);
                await os.FlushAsync();
                os.Seek(0, SeekOrigin.Begin);

                var external = new SimpleExternalSignatureContainer(signedBytes);
                MakeSignature.SignDeferred(reader, contextGuid, os, external);

                return this._fileManager.SaveFile(os, $"{signedPdf.Name}.pdf");
            }
        }

        /// <summary>
        /// Сконвертировать файл в PDF, если документ имеет формат Microsof Office
        /// </summary>
        /// <param name="fileInfo">Информация о файле</param>
        /// <returns>Результат конвертации</returns>
        private Stream ConvertToPdfIfMsOffice(FileInfo fileInfo)
        {
            var fileExtension = fileInfo.Extention;

            if (fileExtension == "pdf")
            {
                return this._fileManager.GetFile(fileInfo);
            }

            if (!new[] { "doc", "docx", "xls", "xlsx" }.Contains(fileExtension))
            {
                throw new Exception("Недопустимый формат файла");
            }
            
            var storagePath = this._container.Resolve<IConfigProvider>()
                .GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"]
                .GetAs("FileDirectory", string.Empty);

            var filesDirectory = Path.IsPathRooted(storagePath)
                ? storagePath
                : ApplicationContext.Current.MapPath("~/" + storagePath.TrimStart('~', '/'));

            var createDate = fileInfo.ObjectCreateDate;
            var fileSource = Path.Combine(filesDirectory, createDate.Year.ToString(), createDate.Month.ToString(), $"{fileInfo.Id}.{fileExtension}");
            var tmpFile = Path.Combine(Path.GetTempPath(), $"{fileInfo.Name}{DateTime.Now.Ticks}.pdf");

            switch (fileInfo.Extention)
            {
                case "doc":
                case "docx":
                    var wordApp = new Microsoft.Office.Interop.Word.Application();
                    var wordDocument = wordApp.Documents.Open(fileSource);
                    wordDocument.ExportAsFixedFormat(tmpFile, WdExportFormat.wdExportFormatPDF);
                    wordDocument.Close(WdSaveOptions.wdDoNotSaveChanges, WdOriginalFormat.wdOriginalDocumentFormat, false);
                    wordApp.Quit();
                    break;
                case "xls":
                case "xlsx":
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    var excelDocument = excelApp.Workbooks.Open(fileSource);
                    excelDocument.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, tmpFile);
                    excelDocument.Close(WdSaveOptions.wdDoNotSaveChanges, WdOriginalFormat.wdOriginalDocumentFormat, false);
                    excelApp.Quit();
                    break;
            }

            var tmpFileStream = new MemoryStream();
            using (var fstream = new FileStream(tmpFile, FileMode.Open))
            {
                fstream.CopyTo(tmpFileStream);
            }
            File.Delete(tmpFile);
            tmpFileStream.Seek(0, SeekOrigin.Begin);

            return tmpFileStream;
        }

        /// <summary>
        /// Контейнер для подписи
        /// </summary>
        private class SimpleExternalSignatureContainer : IExternalSignatureContainer
        {
            private readonly byte[] _signedBytes;

            public SimpleExternalSignatureContainer(byte[] signedBytes)
            {
                this._signedBytes = signedBytes;
            }

            public void ModifySigningDictionary(PdfDictionary signDic)
            {
            }

            public byte[] Sign(Stream data)
            {
                return _signedBytes;
            }
        }
    }
}