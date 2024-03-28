namespace Bars.GkhGji.Controllers
{

    using Bars.GkhGji.Utils;
    using FileInfo = B4.Modules.FileStorage.FileInfo;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.DocIoGenerator;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using PdfSharp.Drawing;
    using PdfSharp.Pdf;
    using PdfSharp.Pdf.IO;

    public class SpecialAccountReportController : SpecialAccountReportController<SpecialAccountReport>
    {
        public SpecialAccountReportController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
        }
    }
    public class SpecialAccountReportController<T> : FileStorageDataController<T>
         where T : SpecialAccountReport
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;
        protected Bitmap stamp = Properties.Resources.stamp2;

        public SpecialAccountReportController(IFileManager fileManager, IDomainService<FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public ActionResult GetXML(BaseParams baseParams)
        {
            var pdfId = baseParams.Params.GetAs<long>("xmlId");
            var fileInfo = DomainService.Get(pdfId).File;

            var fileId = DomainService.Get(pdfId).File.Id;

            byte[] data;
            if (fileId == 0)
            {
                data = new byte[0];
            }
            else
            {
                using (var file = _fileManager.GetFile(_fileDomain.Get(fileId)))
                {
                    using (var tmpStream = new MemoryStream())
                    {
                        if (string.Compare(fileInfo.Extention, "pdf", true) == 0)
                        {
                            file.CopyTo(tmpStream);
                            data = tmpStream.ToArray();
                        }
                        else
                        {
                            data = new byte[0];
                        }
                    }
                }
            }

            Response.Headers.Add("Content-Disposition", "inline; filename=report.pdf");
            Response.Headers.Add("Content-Length", data.Length.ToString());
            return File(data, "application/pdf");
        }

        public virtual bool CheckPermissions(long id)
        {
            return true;
        }

        public ActionResult GetData(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");
            if (!this.CheckPermissions(id))
            {
                return new JsonNetResult(new
                {
                    message = "Нет прав для подписи.",
                    success = false
                });
            }

            if (id == 0)
            {
                return new JsonNetResult("Нет данных для отображения");
            }

            var sig = Resolve<ISignature<T>>();

            if (sig == null)
            {
                return new JsonNetResult("Нет данных для отображения");
            }


            using (var xmlStream = sig.GetXmlStream(id))
            {
                var str = Convert.ToBase64String(xmlStream.ToArray());

                return new JsonNetResult(new { success = true, data = new { dataToSign = str, xmlId = id } });
            }

        }

        public ActionResult Sign(BaseParams baseParams)
        {
            var ds = Resolve<IDomainService<T>>();

            var id = baseParams.Params.GetAs<long>("Id");
            var xmlId = baseParams.Params.GetAs<long>("xmlId");
            var sign = baseParams.Params.GetAs<string>("sign");
            var certificate = baseParams.Params.GetAs<string>("certificate");

            var entity = ds.Get(id);

            var message = string.Empty;

            using (var tr = Resolve<IDataTransaction>())
            {
                var docGen = Resolve<IDocIo>();
                try
                {
                    entity.Signature = _fileManager.SaveFile("signature", "sig", Encoding.UTF8.GetBytes(sign));
                    entity.Certificate = _fileManager.SaveFile("certificate", "cer",
                        Encoding.UTF8.GetBytes("-----BEGIN CERTIFICATE-----" + certificate + "-----END CERTIFICATE-----"));


                    var bmp = Utils.GetFullStamp(stamp, certificate);

                    var fileName = DomainService.Get(xmlId).File.Name;


                    // если исходник - pdf
                    if (string.Compare(entity.File.Extention, "pdf", true) == 0)
                    {
                        Stream fileStream = _fileManager.GetFile(entity.File);
                        var signedStream = new MemoryStream();
                        fileStream.CopyTo(signedStream);
                        fileStream.Position = 0;
                        signedStream.Position = 0;
                        PdfDocument pdfDocument = PdfReader.Open(signedStream, 0);
                        PdfPage newPage = pdfDocument.Pages[0];
                        XPdfForm form = XPdfForm.FromStream(fileStream);
                        XGraphics gfx = XGraphics.FromPdfPage(newPage);

                        MemoryStream strm = new MemoryStream();
                        bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

                        gfx.DrawImage(XImage.FromStream(strm), 20, 20, 225, 99);
                        pdfDocument.Save(signedStream);
                        entity.SignedXMLFile = _fileManager.SaveFile(fileName + "_signed", "pdf", signedStream.ToArray());
                    }

                    ds.Update(entity);
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return message.IsEmpty() ? JsonNetResult.Success : JsonNetResult.Failure(message);
        }


        public ActionResult DeleteDocs(BaseParams baseParams)
        {
            //var xmlId = baseParams.Params.GetAs<long>("xmlId");
            //var pdfId = baseParams.Params.GetAs<long>("pdfId");

            //if (xmlId > 0)
            //{
            //    _fileDomain.Delete(xmlId);
            //}

            //if (pdfId > 0)
            //{
            //    _fileDomain.Delete(pdfId);
            //}

            return JsSuccess();
        }
    }
}