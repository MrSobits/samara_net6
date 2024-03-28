namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActRemoval
{
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
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using PdfSharp.Drawing;
    using PdfSharp.Pdf;
    using PdfSharp.Pdf.IO;
    using FileInfo = B4.Modules.FileStorage.FileInfo;
    using Bars.GkhGji.Utils;
    using Bars.B4.Modules.DataExport.Domain;

    public class Protocol197AnnexController : Protocol197AnnexController<Protocol197Annex>
    {
        public Protocol197AnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
        }
    }

    public class Protocol197AnnexController<T> : B4.Alt.DataController<T>
        where T : Protocol197Annex
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;
        protected Bitmap stamp = Properties.Resources.stamp2;

        public Protocol197AnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain)
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
                        else if (string.Compare(fileInfo.Extention, "docx", true) == 0 || string.Compare(fileInfo.Extention, "doc", true) == 0)
                        {
                            file.CopyTo(tmpStream);
                            MemoryStream pdfMemSream = new MemoryStream();
                            Resolve<IDocIo>().ConvertToPdf(tmpStream).CopyTo(pdfMemSream);
                            data = pdfMemSream.ToArray();
                        }
                        else
                        {
                            data = new byte[0];
                        }
                    }
                }
            }

            Response.Headers.Add("Content-Disposition", "inline; filename=passport.pdf");
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

                    // если исходник - Word
                    if (string.Compare(entity.File.Extention, "docx", true) == 0 || string.Compare(entity.File.Extention, "doc", true) == 0)
                    {
                        Stream docStream = _fileManager.GetFile(entity.File);

                        if (string.Compare(entity.File.Extention, "docx", true) == 0 || string.Compare(entity.File.Extention, "doc", true) == 0)
                        {
                            Stream docMemSream = new MemoryStream();
                            docGen.ConvertToDoc(docStream).CopyTo(docMemSream);
                            docStream = new MemoryStream();
                            docMemSream.Position = 0;
                            docMemSream.CopyTo(docStream);
                        }

                        docGen.OpenTemplate(docStream);
                        MemoryStream pdfMemSream = new MemoryStream();
                        docGen.ConvertToPdf(docStream).CopyTo(pdfMemSream);

                        MemoryStream picStrm = new MemoryStream();
                        bmp.Save(picStrm, System.Drawing.Imaging.ImageFormat.Png);

                        if (docGen.TrySetPicture("Џ", picStrm, 225, 99)) /*225, 99)*/
                        {
                            MemoryStream signedStream = new MemoryStream();
                            MemoryStream sigDocStrm = new MemoryStream();
                            docStream = new MemoryStream();
                            docGen.SaveDocument(docStream);
                            docGen.ConvertToPdf(docStream).CopyTo(signedStream);
                            PdfDocument pdfDocument = PdfReader.Open(signedStream, 0);
                            pdfDocument.Save(signedStream);
                            entity.SignedFile = _fileManager.SaveFile(fileName + "_signed", "pdf", signedStream.ToArray());
                        }
                        //если нет маркера
                        else
                        {
                            pdfMemSream.Position = 0;
                            var signedStream = new MemoryStream();
                            pdfMemSream.CopyTo(signedStream);
                            signedStream.Position = 0;
                            pdfMemSream.Position = 0;
                            PdfDocument pdfDocument = PdfReader.Open(signedStream, 0);
                            PdfPage newPage = pdfDocument.Pages[0];
                            XGraphics gfx = XGraphics.FromPdfPage(newPage);
                            MemoryStream strm = new MemoryStream();
                            bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

                            gfx.DrawImage(XImage.FromStream(strm), 20, 20, 225, 99);
                            pdfDocument.Save(signedStream);
                            entity.SignedFile = _fileManager.SaveFile(fileName + "_signed", "pdf", signedStream.ToArray());
                        }
                    }
                    // если исходник - pdf
                    else if (string.Compare(entity.File.Extention, "pdf", true) == 0)
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
                        entity.SignedFile = _fileManager.SaveFile(fileName + "_signed", "pdf", signedStream.ToArray());
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