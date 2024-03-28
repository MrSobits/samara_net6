namespace Bars.Gkh.PrintForm.Services
{
    using System;
    using System.Threading.Tasks;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.PrintForm.Entities;
    using Bars.Gkh.PrintForm.Utils;

    using iTextSharp.text.pdf;

    /// <summary>
    /// Сервис для внесения изменений в PDF
    /// </summary>
    public interface IPdfTunerService
    {
        /// <summary>
        /// Добавить ЭП в pdf файл
        /// </summary>
        Task<FileInfo> EmbedSignatureAsync(FileInfo tmpFile, FileInfo signedPdf, byte[] signedBytes, string contextGuid);
        
        /// <summary>
        /// Добавить ЭП в pdf файл
        /// </summary>
        Task<FileInfo> EmbedSignatureAsync(FileInfo tmpFileInfo, FileInfo signedPdfInfo, string signature, string contextGuid);
        
        /// <summary>
        /// Добавить штамп в pdf файл
        /// </summary>
        Task<PdfSignInfo> AddSignatureStampAsync(FileInfo file, BaseStampStrategy stampStrategy);
    }
}