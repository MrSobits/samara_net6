namespace Bars.Gkh.FormatDataExport.NetworkWorker.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Cache;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.NetworkWorker.Responses;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Ionic.Zip;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    /// <summary>
    /// Сервис взаимодействия с API РИС ЖКХ
    /// </summary>
    public class FormatDataTransferService : IFormatDataTransferService
    {
        private string RemoteAddress { get; }

        private string AuthToken { get; }

        public ILogger LogManager { get; set; }

        private TimeSpan Timeout { get; set; }

        public FormatDataTransferService(IWindsorContainer container)
        {
            var config = container.GetGkhConfig<AdministrationConfig>()?
                .FormatDataExport
                .FormatDataExportGeneral;

            this.RemoteAddress = config.TransferServiceAddress ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(this.RemoteAddress) && !this.RemoteAddress.EndsWith("/"))
            {
                this.RemoteAddress += "/";
            }

            var token = config.TransferServiceToken ?? string.Empty;
            this.AuthToken = token.StartsWith("Token ")
                ? token
                : $"Token {token}";

            this.Timeout = TimeSpan.FromMinutes(config.Timeout);
        }

        /// <inheritdoc />
        public IDataResult GetStatus(long id)
        {
            using (var client = new WebClient())
            {
                client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                client.UseDefaultCredentials = false;
                client.Headers.Add("Authorization", this.AuthToken);

                try
                {
                    var response = client.DownloadString(new Uri(new Uri(this.RemoteAddress), $"import/status/{id}"));

                    return new BaseDataResult(JsonConvert.DeserializeObject<StatusSuccess>(response));
                }
                catch (Exception e)
                {
                    return this.ReturnError("Ошибка получения статуса", e);
                }
            }
        }

        /// <inheritdoc />
        public List<IDataResult> UploadFile(string filePath, CancellationToken cancellationToken, int numberOfSegments)
        {
            var uri = new Uri(this.RemoteAddress);

            var result = new List<IDataResult>();

            Func<int, string> getExtension = segment => segment >= 10 ? $"z{segment}" : $"z0{segment}";

            for (var segment = 0; segment < numberOfSegments; segment++)
            {
                var segmentFilePath = segment == 0 ? filePath : Path.ChangeExtension(filePath, getExtension(segment));
                var uploadResult = this.UploadFile(uri, segmentFilePath, cancellationToken);
                result.Add(uploadResult);
            }
            //в первую очередь экспортируем в РИС сегменты архива
            result.Reverse();
            return result;
        }

        private IDataResult UploadFile(Uri remoteAddress, string filePath, CancellationToken cancellationToken)
        {
            this.LogManager.LogDebug($"Передача файла '{filePath}' на сервер {remoteAddress.Host}:{remoteAddress.Port}");

            var fileName = Path.GetFileName(filePath);

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            using (var fileStreamContent = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
            using (var name = new StringContent(fileName, Encoding.UTF8))
            using (var checksum = new StringContent(string.Empty))
            {
                fileStreamContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=\"file\"; filename=\"{fileName}\"");
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                name.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data; name=\"name\"");

                checksum.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data; name=\"checksum\"");

                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(this.AuthToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
                client.Timeout = this.Timeout;

                formData.Add(name);
                formData.Add(checksum);
                formData.Add(fileStreamContent);

                HttpResponseMessage response = null;
                try
                {
                    response = client.PostAsync(new Uri(remoteAddress, "storage/upload/"), formData, cancellationToken).Result;
                }
                catch (Exception e)
                {
                    return this.ReturnError("Ошибка загрузки файла", e);
                }

                if (response != null)
                {
                    var responseValue = string.Empty;
                    var responseStream = response.Content.ReadAsStreamAsync().Result;
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseValue = reader.ReadToEnd();
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        this.LogManager.LogDebug("Передача файла завершена успешно");
                        if (!responseValue.IsEmpty())
                        {
                            this.LogManager.LogDebug(responseValue);
                        }

                        return new BaseDataResult(JsonConvert.DeserializeObject<UploadSuccess>(responseValue));
                    }
                    else
                    {
                        this.LogManager.LogDebug($"Передача файла завершена c ошибкой. {response}");
                        if (!responseValue.IsEmpty())
                        {
                            this.LogManager.LogDebug(responseValue);
                        }

                        return new BaseDataResult { Success = false, Data = JsonConvert.DeserializeObject<Error>(responseValue) };
                    }
                }
            }

            return BaseDataResult.Error("Нет ответа от сервера");
        }

        /// <inheritdoc />
        public IDataResult StartImport(long id, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(this.AuthToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.ParseAdd("*/*");

                HttpResponseMessage response = null;
                try
                {
                    response = client.PostAsync(new Uri(new Uri(this.RemoteAddress), $"import/data/{id}"), formData, cancellationToken).Result;
                }
                catch (Exception e)
                {
                    return this.ReturnError("Ошибка запуска импорта", e);
                }

                if (response != null)
                {
                    var responseValue = string.Empty;
                    var responseStream = response.Content.ReadAsStreamAsync().Result;
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseValue = reader.ReadToEnd();
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        this.LogManager.LogDebug("Удаленный импорт успешно запущен");
                        if (!responseValue.IsEmpty())
                        {
                            this.LogManager.LogDebug(responseValue);
                        }

                        return new BaseDataResult(JsonConvert.DeserializeObject<DataSuccess>(responseValue));
                    }
                    else
                    {
                        this.LogManager.LogDebug($"Ошибка при постановке задачи удаленного импорта. {response}");
                        if (!responseValue.IsEmpty())
                        {
                            this.LogManager.LogDebug(responseValue);
                        }

                        return new BaseDataResult { Success = false, Data = JsonConvert.DeserializeObject<Error>(responseValue) };
                    }
                }
            }

            return BaseDataResult.Error("Нет ответа от сервера");
        }

        /// <inheritdoc />
        public IDataResult GetFile(long fileId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(this.AuthToken);

                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(new Uri(new Uri(this.RemoteAddress), $"storage/download/{fileId}")).Result;
                }
                catch (Exception e)
                {
                    return this.ReturnError("Ошибка получения файла", e);
                }

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var fileName = response.Content.Headers?.ContentDisposition?.FileName.Trim('"');
                        fileName = string.IsNullOrEmpty(fileName)
                            ? Path.GetTempFileName()
                            : Path.Combine(Path.GetTempPath(), fileName);

                        using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                        {
                            response.Content.CopyToAsync(fs).Wait();
                        }

                        return new BaseDataResult(fileName);
                    }
                    else
                    {
                        var responseValue = string.Empty;
                        var responseStream = response.Content.ReadAsStreamAsync().Result;
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }

                        return new BaseDataResult { Success = false, Data = JsonConvert.DeserializeObject<Error>(responseValue) };
                    }
                }
            }

            return BaseDataResult.Error("Нет ответа от сервера");
        }

        private IDataResult ReturnError<T>(string message, T exception)
            where T: Exception
        {
            var webException = exception as WebException;
            if (webException?.Response != null)
            {
                using (var responseStream = webException.Response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    var errorResponse = reader.ReadToEnd();
                    var error = JsonConvert.DeserializeObject<BaseDataResult>(errorResponse);

                    error.Message = $"{message}|{error.Message}";

                    return error;
                }
            }

            var innerException = this.GetInnerException(exception);
            var errorMessage = $"{message}|{innerException.Message}";
            this.LogManager.LogError(exception, errorMessage);
            return new BaseDataResult
            {
                Success = false,
                Message = errorMessage
            };
        }

        private Exception GetInnerException(Exception exception, int level = 0)
        {
            if (exception.InnerException != null && level < 10)
            {
                return this.GetInnerException(exception.InnerException, ++level);
            }
            else
            {
                return exception;
            }
        }
    }
}