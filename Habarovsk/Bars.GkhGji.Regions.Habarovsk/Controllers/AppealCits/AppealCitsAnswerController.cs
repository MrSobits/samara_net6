namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Utils.ExternalSignatureContainer;
    //using Bars.GkhGji.Utils.StampStrategy;
    //using Bars.GkhGji.Utils.TextExtractionStrategy
        // TODO: Найти замену
    /*using CryptoPro.Sharpei;
    using iTextSharp.text;
    using iTextSharp.text.pdf.parser;
    using iTextSharp.text.pdf.security;*/
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using FileInfo = B4.Modules.FileStorage.FileInfo;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Habarovsk.Services.DataContracts.SyncAppealCitFromEDM;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;

    public class AppealCitsAnswerController : GkhGji.Controllers.AppealCitsAnswerController<AppealCitsAnswer>
    {
        public AppealCitsAnswerController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
            //blazon = Image.GetInstance(Properties.Resources.blazon_Voronezh);
        }

        private string subjectName;
        private string serialNumber;
        private DateTime validToDate;

        public override ActionResult GetPdfHash(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");
            var subjectName = baseParams.Params.GetAs<string>("SubjectName");
            var serialNumber = baseParams.Params.GetAs<string>("SerialNumber");
            this.serialNumber = serialNumber;
            var validFromDate = baseParams.Params.GetAs<DateTime>("ValidFromDate");
            var validToDate = baseParams.Params.GetAs<DateTime>("ValidToDate");
            this.validToDate = validToDate;
           // System.Security.Cryptography.HashAlgorithm hashAlgorithm = new Gost3411_2012_256CryptoServiceProvider();

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
            this.subjectName = subjName;

            if (!this.CheckPermissions(id))
            {
                return new JsonNetResult(new
                {
                    message = "Нет прав для подписи.",
                    success = false
                });
            }

            var sig = Resolve<ISignature<AppealCitsAnswer>>();

            if (id == 0 && sig == null)
            {
                return new JsonNetResult("Нет данных для отображения");
            }

            var pdf = DomainService.Get(id).File;

            if (pdf == null)
            {
                return new JsonNetResult("Не найден файл для подписания");
            }

          //  var extractStrategy = new SignLocationTextExtractionStrategy("Џ");

            var pdfStream = ConvertToPdfIfMsOffice(pdf);
            if (pdfStream != null)
            {
                // TODO: Найти замену ITextSharp
              /*  iTextSharp.text.Rectangle point = null;
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
                }

                var strategy = new BaseStampStrategy(point, page, blazon)
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
                }*/
            }
            else
            {
                return new JsonNetResult("Неподдерживаемое расширение файла");
            }

            return null;
        }

        public ActionResult SendToEdm(BaseParams baseParams)
        {
            var answerId = baseParams.Params.GetAs<long>("Id");
            var signature = baseParams.Params.GetAs<string>("Signature");
            var answer = DomainService.Get(answerId);
            var answerPdf = answer.File;

            try
            {
                Task<string> signContainerIdTask = Task.Run(async () => await SendFile(Convert.FromBase64String(signature), "Signature.sig"));
                var signContainerId = signContainerIdTask.Result;
                if (signContainerId == null || signContainerId == "")
                {
                    throw new Exception("Не удалось отправить сигнатуру файла");
                }

                Task<string> containerIdTask = Task.Run(async () => await SendFile(answer.SignedFile));
                var containerId = signContainerIdTask.Result;
                if (containerId == null || containerId == "")
                {
                    throw new Exception("Не удалось отправить файл");
                }

                var appealAnswerLongTextDomain = Container.ResolveDomain<AppealAnswerLongText>();
                var answerLongText = appealAnswerLongTextDomain.GetAll().FirstOrDefault(x => x.AppealCitsAnswer.Id == answer.Id);
                var answerText = Encoding.UTF8.GetString(answerLongText.Description);

                var request = new RegCardToEdmAdapterDto
                {
                    ComplaintId = new Guid(answer.AppealCits.ArchiveNumber),
                    AnswerKind = answer.TypeAppealAnswer.GetDisplayName(),
                    DocumentNumber = answer.DocumentNumber,
                    OrderNumber = answer.SerialNumber,
                    CreateDate = answer.DocumentDate.Value,
                    Correspondent = answer.Addressee.Name,
                    ExecutorName = answer.Executor.Fio,
                    SignatoryName = answer.Signer.Fio,
                    Content = answerText,
                    DocumentComment = answer.Description,
                };

                var attachments = new List<FileDto>
                {
                    new FileDto
                    {
                        File = new ObjectMinDto
                        {
                            Id = new Guid(containerId),
                            Name = answer.File.FullName
                        },
                        Signs = new List<BarsEdmSignDto>
                        {
                            new BarsEdmSignDto
                            {
                                Id = new Guid(signContainerId),
                                Name = "Signature.sig",
                                Owner = this.subjectName,
                                Serial = this.serialNumber,
                                ExpiryDate = this.validToDate.ToString(),
                                SignDate = DateTime.Now
                            }
                        }
                    }
                };

                request.Attachments = attachments;

                string jsonRequest = JsonConvert.SerializeObject(request);

                Task<bool> answerResult = Task.Run(async () => await SendAccepted(jsonRequest));

                if (answerResult.Result)
                {
                    answer.Sended = true;
                }

                var signedInfoDomain = Container.ResolveDomain<PdfSignInfo>();

                var signInfo = signedInfoDomain.GetAll()
                    .Where(x => x.OriginalPdf.Id == answerPdf.Id)
                    .FirstOrDefault();

                byte[] pdfArray;
                using (var pdfStream = _fileManager.GetFile(signInfo.StampedPdf))
                using (var pdfMemoryStream = new MemoryStream())
                {
                    pdfStream.CopyTo(pdfMemoryStream);
                    pdfArray = pdfMemoryStream.ToArray();
                }

                FileInfo signedPdf;
                // TODO: Заменить ITextSharp
               /* using (var reader = new iTextSharp.text.pdf.PdfReader(pdfArray))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        IExternalSignatureContainer external = new SimpleExternalSignatureContainer(Convert.FromBase64String(signature));
                        MakeSignature.SignDeferred(reader, signInfo.ContextGuid, stream, external);
                        signedPdf = _fileManager.SaveFile(stream, "signed_" + answerPdf.Name + ".pdf");
                    }
                }

                signInfo.SignedPdf = signedPdf;
                //answerAnnex.Signature = _fileManager.SaveFile("signature", "sig", Convert.FromBase64String(signature));
                answer.SignedFile = signedPdf;*/

                signedInfoDomain.Update(signInfo);
                DomainService.Update(answer);

                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        private async Task<bool> SendAccepted(string jsonRequest)
        {
            string url = "http://172.18.205.70/edm-bars-adapter/ws/send";
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                HttpContent responseContent = response.Content;
                string result = await responseContent.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<ErrorDto>(result) != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        

        private async Task<string> SendFile(FileInfo fileInfo)
        {
            string url = "http://172.18.205.54:4346/containers/create";
            var fm = Container.Resolve<IFileManager>();
            byte[] file_bytes = ReadFully(fm.GetFile(fileInfo));
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                MultipartFormDataContent form = new MultipartFormDataContent
                {
                    { new ByteArrayContent(file_bytes, 0, file_bytes.Length), "file", fileInfo.FullName }
                };
                HttpResponseMessage response = await client.PostAsync(url, form);
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<ErrorDto>(result) != null)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<string> SendFile(byte[] file_bytes, string fileFullName)
        {
            string url = "http://172.18.205.54:4346/containers/create";
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                MultipartFormDataContent form = new MultipartFormDataContent
                {
                    { new ByteArrayContent(file_bytes, 0, file_bytes.Length), "file", fileFullName }
                };
                HttpResponseMessage response = await client.PostAsync(url, form);
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();

                if (JsonConvert.DeserializeObject<ErrorDto>(result) != null)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private class ErrorDto
        {
            public int Status { get; set; }
            public string Message { get; set; }

        }
    }
}