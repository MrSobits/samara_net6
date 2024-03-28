namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Enums;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using System;
    using System.Net.Mail;
    using System.Drawing;
    using System.Drawing.Imaging;

    using PdfSharp.Drawing;
    using System.IO;
    using PdfSharp.Pdf.IO;
    using PdfSharp.Pdf;
    using Syncfusion.DocIO.DLS;
    using Syncfusion.DocIO;
    using Bars.Gkh.DocIoGenerator;
    using Bars.GkhGji.DomainService;
    using Bars.B4.DataAccess;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Habarovsk.DomainService;

    public class AppealCitsAdmonitionAnswerSignController : BaseController
    {
        public IDomainService<AppealCitsAdmonition> DomainService { get; set; }

        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;
        protected Bitmap stamp = Properties.Resources.stamp_Voronezh;

        public AppealCitsAdmonitionAnswerSignController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }


        public ActionResult GetXML(BaseParams baseParams)
        {
            var pdfId = baseParams.Params.GetAs<long>("xmlId");
            var fileInfo = DomainService.Get(pdfId).AnswerFile;

            var fileId = DomainService.Get(pdfId).AnswerFile.Id;

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

            var sig = Resolve<IAnswerSignature<AppealCitsAdmonition>>();

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
            var ds = Resolve<IDomainService<AppealCitsAdmonition>>();

            var id = baseParams.Params.GetAs<long>("Id");
            var xmlId = baseParams.Params.GetAs<long>("xmlId");
            var sign = baseParams.Params.GetAs<string>("sign");
            var certificate = baseParams.Params.GetAs<string>("certificate");

            var entity = ds.Get(id);

            var message = string.Empty;

            using (var tr = Resolve<IDataTransaction>())
            {
                try
                {
                    entity.AnswerSignature = _fileManager.SaveFile($"{entity.AnswerFile.Name}.{entity.AnswerFile.Extention}", "sig", Encoding.UTF8.GetBytes(sign));
                    entity.AnswerCertificate = _fileManager.SaveFile("certificate", "cer",
                        Encoding.UTF8.GetBytes("-----BEGIN CERTIFICATE-----" + certificate + "-----END CERTIFICATE-----"));


                    var bmp = Utils.Utils.GetFullStamp(stamp, certificate);

                    // если исходник - Word
                    if (string.Compare(entity.AnswerFile.Extention, "docx", true) == 0 || string.Compare(entity.AnswerFile.Extention, "doc", true) == 0)
                    {
                        Stream docStream = _fileManager.GetFile(entity.AnswerFile);


                        var wordDocument = new WordDocument(docStream, FormatType.Doc);
                        MemoryStream pdfMemSream = new MemoryStream();
                        Resolve<IDocIo>().ConvertToPdf(docStream).CopyTo(pdfMemSream);
                        var textSelection = wordDocument.Find("$_ЭП_$", false, false);
                        //если есть маркер
                        if (textSelection != null)
                        {
                            var paragraph = textSelection.GetAsOneRange().OwnerParagraph;
                            paragraph.Items.Clear();
                            var picture = (WPicture)paragraph.AppendPicture(ToStream(bmp, ImageFormat.Png)); 
                            picture.Width = 225;
                            picture.Height = 99;
                            MemoryStream signedDocStream = new MemoryStream();
                            wordDocument.Save(signedDocStream, FormatType.Word2010);
                            MemoryStream signedStream = new MemoryStream();
                            Resolve<IDocIo>().ConvertToPdf(signedDocStream).CopyTo(signedStream);
                            PdfDocument pdfDocument = PdfReader.Open(signedStream, 0);
                            pdfDocument.Save(signedStream);
                            entity.SignedAnswerFile = _fileManager.SaveFile(entity.AnswerFile.Name + "_signed", "pdf", signedStream.ToArray());

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
                            XPdfForm form = XPdfForm.FromStream(pdfMemSream);
                            XGraphics gfx = XGraphics.FromPdfPage(newPage); MemoryStream strm = new MemoryStream();
                            bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

                            gfx.DrawImage(XImage.FromStream(strm), 20, 20, 225, 99);
                            pdfDocument.Save(signedStream);
                            entity.SignedAnswerFile = _fileManager.SaveFile(entity.AnswerFile.Name + "_signed", "pdf", signedStream.ToArray());
                        }
                    }
                    // если исходник - pdf
                    else if (string.Compare(entity.AnswerFile.Extention, "pdf", true) == 0)
                    {
                        Stream fileStream = _fileManager.GetFile(entity.AnswerFile);
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
                        entity.SignedAnswerFile = _fileManager.SaveFile(entity.AnswerFile.Name + "_signed", "pdf", signedStream.ToArray());
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
        
        public Stream ToStream(Image image, ImageFormat format) {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
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
