using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;

namespace Bars.GkhGji.Regions.Chelyabinsk.Services.Impl
{
    /// <summary>
    /// Утилита для работы с FTP
    /// </summary>
    public class FtpUtility
    {
        /// <summary>
        /// Метод поиска файла на Ftp сервере
        /// </summary>
        [Flags]
        public enum ExistsMethod
        {
            /// <summary>
            /// По размеру файла
            /// </summary>
            FileSize,

            /// <summary>
            /// По списку файлов в каталоге
            /// </summary>
            List
        }

        /// <summary>
        /// Домен пользователя
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string User { get; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Прокси для подключения
        /// </summary>
        public WebProxy Proxy { get; private set; }

        /// <summary>
        /// Uri сервера
        /// </summary>
        public Uri ServerUri { get; }

        /// <summary>
        /// Создает подключение к серверу и авторизуется заданным пользователем
        /// </summary>
        /// <param name="serverIp">Строка следующего вида: "user:password@IP_Address:Port_number"</param>
        public FtpUtility(Uri serverIp)
        {
            this.ServerUri = new Uri($"ftp://{serverIp.Host}:{serverIp.Port}/");
            this.User = serverIp.UserInfo.Split(':').First();
            this.Password = serverIp.UserInfo.Split(':').Last();
            this.Domain = string.Empty;
            this.Proxy = null;
        }

        /// <summary>
        /// Создает подключение к серверу и авторизуется как анонимный пользователь
        /// </summary>
        /// <param name="serverIp">Строка следующего вида: "IP_Address[:Port_number]"</param>
        public FtpUtility(string serverIp)
            :
            this(serverIp, "anonymous", string.Empty, string.Empty)
        {
            
        }

        /// <summary>
        /// Создает подключение к серверу и авторизуется заданным пользователем
        /// </summary>
        /// <param name="serverIp">Строка следующего вида: "IP_Address[:Port_number]"</param>
        /// <param name="user">Имя пользователя для входа на удаленный FTP сервер.</param>
        /// <param name="password">Пароль пользователя для входа на удаленный FTP сервер.</param>
        public FtpUtility(string serverIp, string user, string password)
            :
            this(serverIp, user, password, string.Empty)
        {
            
        }

        /// <summary>
        /// Создает подключение к серверу и авторизуется заданным пользователем
        /// </summary>
        /// <param name="serverIp">Строка следующего вида: "IP_Address[:Port_number]"</param>
        /// <param name="user">Имя пользователя для входа на удаленный FTP сервер.</param>
        /// <param name="password">Пароль пользователя для входа на удаленный FTP сервер.</param>
        /// <param name="domain">Домен пользователя для входа на удаленный FTP сервер.</param>
        public FtpUtility(string serverIp, string user, string password, string domain)
        {
            if (string.IsNullOrEmpty(serverIp)) throw new ArgumentException("Строка подключения не может быть пустой.\n");
            FtpWebRequest.DefaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            this.ServerUri = new Uri($"ftp://{serverIp}/");
            this.User = user;
            this.Password = password;
            this.Domain = domain;
            this.Proxy = null;
        }

        /// <summary>
        /// Задает настройки Proxy-сервера с авторизацией пользователя.
        /// </summary>
        /// <param name="proxyAddress">Адрес Proxy-сервера вида [http[s]://]ProxyAddres[:PortNumber].</param>
        /// <param name="proxyUser">Имя пользователя Proxy-сервера.</param>
        /// <param name="proxyPassword">Пароль пользователя Proxy-сервера.</param>
        /// <param name="domain">Домен пользователя Proxy-сервера.</param>
        public void SetProxy(string proxyAddress, string proxyUser, string proxyPassword, string domain)
        {
            var wp = new WebProxy(new Uri(proxyAddress));
            if (!string.IsNullOrEmpty(proxyUser) && !string.IsNullOrEmpty(proxyPassword))
                wp.Credentials = new NetworkCredential(proxyUser, proxyPassword, domain);
            this.Proxy = wp;
        }

        /// <summary>
        /// Удаляет ранее заданный Proxy-сервер
        /// </summary>
        public void ClearProxy() { this.Proxy = null; }

        /// <summary>
        /// Создает новый запрос к FTP-серверу
        /// </summary>
        /// <param name="subUri">Ысылка</param>
        /// <param name="method">Метод</param>
        /// <returns>Запрос</returns>
        private FtpWebRequest CreateRequest(string subUri, string method)
        {
            var request = (FtpWebRequest)WebRequest.Create(new Uri(this.ServerUri, subUri));
            request.Credentials = new NetworkCredential(this.User, this.Password, this.Domain);
            request.Method = method;
            request.KeepAlive = false;
            request.UseBinary = true;
            request.UsePassive = true;
            request.Proxy = this.Proxy;
            return request;
        }

        /// <summary>
        /// Декорирует директорию
        /// </summary>
        /// <param name="path">Сиходный путь</param>
        /// <returns>Декорированный путь</returns>
        private string MasquaradePath(string path)
        {
            return path;
            //var regexp = new Regex(@"^(\\?|/?)\d{4,4}-\d{2,2}(\\|/)");
            //return (regexp.IsMatch(path) ? path :
            //    $"{DateTime.UtcNow:yyyy-MM}/{path}").Replace('\\', '/');
        }

        /// <summary>
        /// Функция проверяет существует ли файл на FTP сервере
        /// </summary>
        /// <param name="ftpPath">Путь к файлу на удаленном сервере вида "Folder\SubFolder\File.bin"</param>
        /// <param name="method">Метод поиска файла на FTP сервере</param>
        /// <returns>Возвращает значение True если файл найден и имеет не нулевой размер, False если файл отсутствует или его размер равен 0.</returns>
        public bool FileExists(string ftpPath, ExistsMethod method = ExistsMethod.FileSize)
        {
            ftpPath = this.MasquaradePath(ftpPath);
            bool boolExistFile;

            var fileName = string.Empty;
            var directory = string.Empty;
            switch (method)
            {
                case ExistsMethod.List:
                    foreach (var split in ftpPath.Split('/'))
                    {
                        directory += $"{fileName}/";
                        fileName = split;
                    }
                    break;
            }

            FtpWebRequest request;
            switch (method)
            {
                case ExistsMethod.FileSize:
                    request = this.CreateRequest(ftpPath, WebRequestMethods.Ftp.GetFileSize);
                    break;

                case ExistsMethod.List:
                    request = this.CreateRequest(directory, WebRequestMethods.Ftp.ListDirectory);
                    break;

                default: throw new InvalidOperationException("Unknown search method.");
            }

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                {
                    if(responseStream == null) 
                        throw new InvalidOperationException("Получен пустой ответ на запрос");

                    switch (method)
                    {
                        case ExistsMethod.List:
                            using (var reader = new StreamReader(responseStream))
                            {
                                var files = reader.ReadToEnd().Replace("\r\n", "\n");
                                boolExistFile = files.Split('\n').Select(x => x.TrimEnd().ToLower()).Contains(fileName.ToLower());
                            }
                            break;
                        case ExistsMethod.FileSize:
                            boolExistFile = true;
                            break;
                        default: throw new InvalidOperationException("Unknown search method.");
                    }
                    responseStream.Close();
                    response.Close();
                }
            }
            catch (WebException ex)
            {
                using (var response = (FtpWebResponse)ex.Response)
                {
                    switch (method)
                    {
                        case ExistsMethod.FileSize:
                            if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable) boolExistFile = false;
                            else throw;
                            break;

                        default:
                            throw;
                    }
                    response.Close();
                }
            }

            return boolExistFile;
        }

        /// <summary>
        /// Создает рекурсивно папки на удаленном FTP сервере.
        /// </summary>
        /// <param name="ftpDirectory">Путь для создания на улаленном FTP сервере вида "Folder\SubFolder".</param>
        /// <returns>Возвращает True в случае успеха или False при ошибке.</returns>
        public bool MakeDirectory(string ftpDirectory)
        {
            ftpDirectory = this.MasquaradePath(ftpDirectory);
            var path = string.Empty;
            foreach (var directory in ftpDirectory.Split('/'))
            {
                if (string.IsNullOrWhiteSpace(directory)) continue;
                path = $"{path}/{directory}";
                var request = this.CreateRequest(path, WebRequestMethods.Ftp.MakeDirectory);
                try
                {
                    using (var response = request.GetResponse())
                    {
                        var responseStream = response.GetResponseStream();
                        if (responseStream == null)
                            throw new InvalidOperationException("Получен пустой ответ на запрос");

                        responseStream.Close();
                        response.Close();
                    }
                }
                catch (WebException ex)
                {
                    using (var response = (FtpWebResponse) ex.Response)
                    {
                        if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                        {
                            response.Close();
                            continue;
                        }

                        response.Close();
                        throw;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Удаляет файл с FTP сервера
        /// </summary>
        /// <param name="ftpPath">Путь к файлу на удаленном сервере вида "Folder\SubFolder\File.bin"</param>
        /// <returns>Возвращает True при успешном удалении файла или False при возникновении ошибки</returns>
        public bool DeleteFile(string ftpPath)
        {
            ftpPath = this.MasquaradePath(ftpPath);
            var boolDeleteFile = false;
            var request = this.CreateRequest(ftpPath, WebRequestMethods.Ftp.DeleteFile);
            try
            {
                using (var response = request.GetResponse())
                    response.Close();
                boolDeleteFile = true;
            }
            catch (WebException ex)
            {
                using (var response = (FtpWebResponse)ex.Response)
                {
                    if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                        throw;

                    response.Close();
                }
            }

            return boolDeleteFile;
        }

        /// <summary>
        /// Выгружает файл на удаленный FTP сервер.
        /// </summary>
        /// <param name="filePath">Путь к локальному файлу.</param>
        /// <param name="ftpPath">Путь к файлу на удаленном сервере вида "Folder\SubFolder\File.bin"</param>
        /// <returns>Путь к файлу на FTP сервере</returns>
        public string UploadFile(string filePath, string ftpPath)
        {
            string result;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                result = this.UploadFile(stream, ftpPath);
                stream.Close();
            }
            return result;
        }

        /// <summary>
        /// Выгружает файл на удаленный FTP сервер.
        /// </summary>
        /// <param name="stream">Поток для отправки на сервер</param>
        /// <param name="ftpPath">Путь к локальному файлу.</param>
        /// <param name="disposeStreamAfterUpload">Уничтожить поток после работы</param>
        /// <param name="createDirectories">Создать директории при их отсутствии</param>
        /// <returns>Путь к файлу на FTP сервере</returns>
        public string UploadFile(Stream stream, string ftpPath, bool disposeStreamAfterUpload = false, bool createDirectories = true)
        {
            ftpPath = this.MasquaradePath(ftpPath);
            var request = this.CreateRequest(ftpPath, WebRequestMethods.Ftp.UploadFile);
            try
            {
                var requestStream = request.GetRequestStream();
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(requestStream);
                requestStream.Close();
                using (var response = (FtpWebResponse)request.GetResponse())
                    response.Close();
            }
            catch (WebException ex)
            {
                using (var response = (FtpWebResponse)ex.Response)
                {
                    if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable && response.StatusCode != FtpStatusCode.ActionNotTakenFilenameNotAllowed)
                        throw;

                    response.Close();
                }
                if (createDirectories)
                {
                    var directory = string.Empty;
                    var fileName = string.Empty;
                    foreach (var split in ftpPath.Split('/'))
                    {
                        directory += $"{fileName}/";
                        fileName = split;
                    }

                    if (this.MakeDirectory(directory))
                        return this.UploadFile(stream, ftpPath, disposeStreamAfterUpload, false);
                }

                throw;
            }
            finally
            {
                if (disposeStreamAfterUpload && stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
            return ftpPath;
        }

        /// <summary>
        /// Функция скачивает удаленный файл с FTP сервера на локальный компьютер.
        /// </summary>
        /// <param name="ftpPath">Путь к файлу на удаленном сервере вида "Folder\SubFolder\File.bin".</param>
        /// <returns>Поток скаченного файла</returns>
        public Stream DownloadFile(string ftpPath)
        {
            var request = this.CreateRequest(ftpPath, WebRequestMethods.Ftp.DownloadFile);
            request.KeepAlive = true;
            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                        throw new InvalidOperationException("Получен пустой ответ на запрос");

                    var resultStream = new MemoryStream();
                    try
                    {
                        responseStream.CopyTo(resultStream);
                        resultStream.Seek(0, SeekOrigin.Begin);
                    }
                    catch (OutOfMemoryException)
                    {
                        resultStream.Flush();
                        resultStream.Close();
                        resultStream.Dispose();
                        resultStream = null;
                    }
                    responseStream.Close();
                    response.Close();
                    return resultStream;
                }
            }
            catch (WebException ex)
            {
                using (var response = (FtpWebResponse)ex.Response)
                {
                    if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                        throw;

                    response.Close();
                }
                if (ftpPath != this.MasquaradePath(ftpPath))
                    return this.DownloadFile(this.MasquaradePath(ftpPath));

                throw;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Не удалось найти файл \"{ftpPath}\"", ftpPath, ex);
            }
        }
    }
}
