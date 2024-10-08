﻿namespace Bars.GkhGji.Controllers
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.DocIoGenerator;
    using Bars.Gkh.Report;
    using Bars.Gkh.StimulReport;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Utils;
    using Bars.GkhGji.Utils.ExternalSignatureContainer;
    using Bars.GkhGji.Utils.StampStrategy;
    using Bars.GkhGji.Utils.TextExtractionStrategy;
    // TODO: Замена 
    /*using CryptoPro.Sharpei;
    using iTextSharp.text.pdf.parser;
    using iTextSharp.text.pdf.security;
    using Org.BouncyCastle.Crypto.Tls;*/
    using PdfSharp.Drawing;
    using PdfSharp.Pdf;
    using PdfSharp.Pdf.IO;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using FileInfo = B4.Modules.FileStorage.FileInfo;
    using Path = System.IO.Path;
    //using Image = iTextSharp.text.Image;

    public class DecisionAnnexController : DecisionAnnexController<DecisionAnnex>
    {
        public DecisionAnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
        }
    }

    public class DecisionAnnexController<T> : FileStorageDataController<T>
        where T : DecisionAnnex
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;
        protected Bitmap stamp = Properties.Resources.stamp2;
        protected Image blazon;

        public DecisionAnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            
            _fileDomain = fileDomain;
        }

        public ActionResult GetXML(BaseParams baseParams)
        {
            try
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
            catch(Exception ex)
            {
                return new JsonNetResult(new
                {
                    message = $"{ex.Message} {ex.StackTrace}",
                    success = false
                });
            }
        }

        public virtual bool CheckPermissions(long id)
        {
            return true;
        }

        public ActionResult GetPdfHash(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");
            var subjectName = baseParams.Params.GetAs<string>("SubjectName");
            var serialNumber = baseParams.Params.GetAs<string>("SerialNumber");
            var validFromDate = baseParams.Params.GetAs<DateTime>("ValidFromDate");
            var validToDate = baseParams.Params.GetAs<DateTime>("ValidToDate");

            // TODO: Замена
            //System.Security.Cryptography.HashAlgorithm hashAlgorithm = new Gost3411_2012_256CryptoServiceProvider();

            //switch (hashAlgorithmName)
            //{
            //    case "3411":
            //        hashAlgorithm = new Gost3411CryptoServiceProvider();
            //        break;
            //    case "2012256":
            //        hashAlgorithm = new Gost3411_2012_256CryptoServiceProvider();
            //        break;
            //    case "2012512":
            //        hashAlgorithm = new Gost3411_2012_512CryptoServiceProvider();
            //        break;
            //    default:
            //        GetLogger().AddError("Неизвестный алгоритм хеширования", $"hashAlgorithmName: {hashAlgorithmName}");
            //        return HttpStatusCode.BadRequest;
            //}

            var subjName = GetCName(subjectName, "SN=");
            if (subjectName.Contains("G="))
            {
                subjName += " " + GetCName(subjectName, "G=");
            }

            if (!this.CheckPermissions(id))
            {
                return new JsonNetResult(new
                {
                    message = "Нет прав для подписи.",
                    success = false
                });
            }

            var sig = Resolve<ISignature<T>>();

            if (id == 0 && sig == null)
            {
                return new JsonNetResult("Нет данных для отображения");
            }

            var pdf = DomainService.Get(id).File;

            if (pdf == null)
            {
                return new JsonNetResult("Не найден файл для подписания");
            }

            //byte[] pdfArray;
            //using (var pdfStream = _fileManager.GetFile(pdf))
            //using (var pdfMemoryStream = new MemoryStream())
            //{
            //    pdfStream.CopyTo(pdfMemoryStream);
            //    pdfArray = pdfMemoryStream.ToArray();
            //}

            //  var extractStrategy = new SignLocationTextExtractionStrategy("Џ");

            var pdfStream = ConvertToPdfIfMsOffice(pdf);
            if (pdfStream != null)
            {
                /*iTextSharp.text.Rectangle point = null;
                int page = 1;
                using (var reader = new iTextSharp.text.pdf.PdfReader(pdfStream))
                {
                    page = reader.NumberOfPages;
                    PdfTextExtractor.GetTextFromPage(reader, page, extractStrategy);
                    point = extractStrategy.TextPoints.LastOrDefault();

                    if (point == null)
                    {
                        page = reader.NumberOfPages - 1;
                        if (page < 1)
                            page = 1;
                        PdfTextExtractor.GetTextFromPage(reader, page, extractStrategy);
                        point = extractStrategy.TextPoints.LastOrDefault();
                    }
                }*/

                /** var strategy = new BaseStampStrategy(point, page, blazon)
                 {
                     SerialNumber = serialNumber,
                     PersonName = subjName,
                     ValidFromDate = validFromDate,
                     ValidToDate = validToDate,
                     ContextGuid = Guid.NewGuid().ToString()
                 };

                 var signInfo = AddSignatureStamp(pdf, strategy);
                 if (signInfo != null)
                 {
                     var stampedFileStream = _fileManager.GetFile(signInfo.ForHashFile);
                     var hash = string.Join(string.Empty, hashAlgorithm.ComputeHash(stampedFileStream).Select(x => x.ToString("X2")));

                     return new JsonNetResult(new { success = true, data = new { dataToSign = hash, xmlId = id } });
                 }
                 else
                 {
                     return new JsonNetResult("Неподдерживаемое расширение файла");
                 }
             }
             else
             {
                 return new JsonNetResult("Неподдерживаемое расширение файла");
             }*/
       
            }
            return null;
        }

        public ActionResult PostSignature(BaseParams baseParams)
        {
            var ds = Resolve<IDomainService<T>>();
            var decisionId = baseParams.Params.GetAs<long>("Id");
            var signature = baseParams.Params.GetAs<string>("Signature");
            var decisionAnnex = DomainService.Get(decisionId);
            var pdf = decisionAnnex.File;

            var signedInfoDomain = Container.ResolveDomain<PdfSignInfo>();

            var signInfo = signedInfoDomain.GetAll()
                .Where(x => x.OriginalPdf.Id == pdf.Id)
                .FirstOrDefault();

            byte[] pdfArray;
            using (var pdfStream = _fileManager.GetFile(signInfo.StampedPdf))
            using (var pdfMemoryStream = new MemoryStream())
            {
                pdfStream.CopyTo(pdfMemoryStream);
                pdfArray = pdfMemoryStream.ToArray();
            }

            FileInfo signedPdf;
            // TODO : Заменить iTextSharp
           /* using (var reader = new iTextSharp.text.pdf.PdfReader(pdfArray))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    IExternalSignatureContainer external = new SimpleExternalSignatureContainer(Convert.FromBase64String(signature));
                    MakeSignature.SignDeferred(reader, signInfo.ContextGuid, stream, external);
                    signedPdf = _fileManager.SaveFile(stream, "signed_" + pdf.Name + ".pdf");
                }
            }

            signInfo.SignedPdf = signedPdf;
            decisionAnnex.Signature = _fileManager.SaveFile("signature", "sig", Convert.FromBase64String(signature));
            decisionAnnex.SignedFile = signedPdf;*/

            signedInfoDomain.Update(signInfo);
            DomainService.Update(decisionAnnex);

          
            if (decisionAnnex.TypeAnnex == TypeAnnex.Decision)
            {
                CreateNotification(decisionAnnex, ds);
            }

            return JsonNetResult.Success;
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
                        bmp.Save(picStrm, ImageFormat.Png);

                        if (docGen.TrySetPicture("Џ", picStrm, 225, 99))
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
                            bmp.Save(strm, ImageFormat.Png);

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
                        bmp.Save(strm, ImageFormat.Png);

                        gfx.DrawImage(XImage.FromStream(strm), 20, 20, 225, 99);
                        pdfDocument.Save(signedStream);
                        entity.SignedFile = _fileManager.SaveFile(fileName + "_signed", "pdf", signedStream.ToArray());
                    }
                    var anx = entity as DecisionAnnex;
                    if (anx.TypeAnnex == TypeAnnex.Decision)
                    {
                        CreateNotification(entity, ds);
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

        private PdfSignInfo AddSignatureStamp(FileInfo file)
        //private PdfSignInfo AddSignatureStamp(FileInfo file, BaseStampStrategy stampStrategy)
        {
            FileInfo stampedPdfInfo;
            FileInfo forHashFileInfo;
            var tempPdf = Path.GetRandomFileName();

         /*   var pdfStream = ConvertToPdfIfMsOffice(file);
            if (pdfStream != null)
            {
                using (iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdfStream))
                {
                    using (var output = System.IO.File.OpenWrite(tempPdf))
                    {
                        var appearance = stampStrategy.Stamp(reader, output);

                        forHashFileInfo = _fileManager.SaveFile(appearance.GetRangeStream(), "for_hash_" + file.FullName);
                    }
                }

                using (var stampedPdf = System.IO.File.OpenRead(tempPdf))
                {
                    stampedPdfInfo = _fileManager.SaveFile(stampedPdf, "stamped_" + file.FullName);
                }

                if (System.IO.File.Exists(tempPdf))
                {
                    System.IO.File.Delete(tempPdf);
                }

                var signInfo = new PdfSignInfo
                {
                    OriginalPdf = file,
                    ForHashFile = forHashFileInfo,
                    StampedPdf = stampedPdfInfo,
                    ContextGuid = stampStrategy.ContextGuid
                };

                var signDomain = Container.ResolveDomain<PdfSignInfo>();
                signDomain.Save(signInfo);

                return signInfo;
            }
            else
            {
                return null;
            }*/
         return null;
        }

        private static string GetCName(string subject, string splitter)
        {
            var rows = subject.Split(',');
            if (rows != null && rows.Length > 0)
            {
                foreach (string rec in rows.ToList())
                {
                    if (rec.Contains(splitter))
                    {
                        return rec.Replace(splitter, "");
                    }
                }
            }
            return "";
        }

        private Stream ConvertToPdfIfMsOffice(FileInfo fileInfo)
        {
            var docGen = Resolve<IDocIo>();

            Stream docStream = _fileManager.GetFile(fileInfo);

            if (string.Compare(fileInfo.Extention, "docx", true) == 0 || string.Compare(fileInfo.Extention, "doc", true) == 0)
            {
                try
                {
                    MemoryStream pdfMemSream = new MemoryStream();
                    docGen.ConvertToPdf(docStream).CopyTo(pdfMemSream);
                    pdfMemSream.Position = 0;

                    return pdfMemSream;
                }
                catch
                {
                    return null;
                }
            }
            else if (string.Compare(fileInfo.Extention, "pdf", true) == 0)
            {
                return docStream;
            }

            return null;
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

        private void CreateNotification(DecisionAnnex decisionAnnex, IDomainService<T> domain)
        {
            var decision = decisionAnnex.Decision;

            var notificationReport = CreateNortificationReport(decisionAnnex);

            if (notificationReport != null)
            {
                var notification = new DecisionAnnex()
                {
                    Decision = decision,
                    TypeAnnex = TypeAnnex.CorrespondentNotice,
                    DocumentDate = DateTime.Now,
                    Name = "Уведомление заявителя",
                    File = notificationReport
                };

                domain.Save(notification);
            }
        }

        private FileInfo CreateNortificationReport(DecisionAnnex decisionAnnex)
        {
            try
            {
                var reportDomain = Container.ResolveAll<IGkhBaseReport>();
                var reportProvider = Container.Resolve<IGkhReportProvider>();
                var fileManager = Container.Resolve<IFileManager>();
                MemoryStream stream;

                var postName = decisionAnnex.File.Name;
                var decisionId = decisionAnnex.Decision.Id;
                var userParam = new UserParamsValues();
                userParam.AddValue("DecisionId", decisionId);


                var report = reportDomain.FirstOrDefault(x => x.Id == "DecisionNortification");
                report.SetUserParams(userParam);

                if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                {
                    stream = (report as StimulReport).GetGeneratedReport();
                }
                else
                {
                    var reportParams = new ReportParams();
                    report.PrepareReport(reportParams);

                    // получаем Генератор отчета
                    var generatorName = report.GetReportGenerator();

                    stream = new MemoryStream();
                    var generator = Container.Resolve<IReportGenerator>(generatorName);
                    reportProvider.GenerateReport(report, stream, generator, reportParams);
                }

                var reportFile = fileManager.SaveFile(stream, $"Уведомление к \"{postName}\".pdf");

                return reportFile;
            }
            catch
            {
                return null;
            }
        }
    }
}