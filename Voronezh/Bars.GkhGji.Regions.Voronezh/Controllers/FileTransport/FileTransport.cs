namespace Bars.GkhGji.Regions.Voronezh.Controllers.FileTransport
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Microsoft.AspNetCore.Mvc;
    using Castle.Windsor;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils.Web;
    using Bars.GkhGji.Regions.Voronezh.Entities;

    public class FileTransportController : Controller
    {
        public IWindsorContainer Container { get; set; }

        public ActionResult GetFile(long id)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            if (fileManager.CheckFile(id).Success == false) return new JsonNetResult("file does not exists");
            return fileManager.LoadFile(id);
        }

        public ActionResult GetFileFromServer(long id, string server = null)
        {
            if (server == null)
            {
                var appSettings = ApplicationContext.Current.Configuration.AppSettings;
                server = appSettings.GetAs<string>("ServerAddresForGetFiles");
            }

            var file = Path.Combine(server ?? "", @"action/FileTransport/GetFile?id=" + id);

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(new Uri(file)).Result;
                }
                catch (Exception e)
                {
                    return new DownloadResult { ResultCode = ResultCode.OtherError, FileDownloadName = e.Message };
                }

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var fileName = response.Content.Headers?.ContentDisposition?.FileName.Trim('"');
                        var filePath = string.IsNullOrEmpty(fileName)
                            ? Path.GetTempFileName()
                            : Path.Combine(Path.GetTempPath(), fileName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            response.Content.CopyToAsync(fs).Wait();
                        }

                        return new DownloadResult
                        {
                            ResultCode = ResultCode.Success,
                            FileDownloadName = fileName,
                            Path = filePath
                        };
                    }
                }
            }

            return new DownloadResult { ResultCode = ResultCode.FileNotFound };
        }

        public ActionResult GetFileFromPrivateServer(long id, string server = null)
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var min = appSettings.GetAs<bool>("MinimalApp");
            if (min)
            {
                return GetFileFromServer(id, server);
            }

            var fileManager = this.Container.Resolve<IFileManager>();
            if (fileManager.CheckFile(id).Success == false) return new JsonNetResult("file does not exists");
            return fileManager.LoadFile(id);
            
        }
        
        public ActionResult GetFileFromPublicServer(long id, string server = null)
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var min = appSettings.GetAs<bool>("MinimalApp");
            if (!min)
            {
                return GetFileFromServer(id, server);
            }

            var fileManager = this.Container.Resolve<IFileManager>();
            if (fileManager.CheckFile(id).Success == false) return new JsonNetResult("file does not exists");
            return fileManager.LoadFile(id);
            
        }
    }
}