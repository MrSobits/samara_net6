﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
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
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using System.Linq;

    public class FileTransportController : BaseController
    {
        public IWindsorContainer Container { get; set; }      
        public ActionResult GetFile(long id)
        {
            var UserManager = this.Container.Resolve<IGkhUserManager>();
            var SMEVEGRNFileDomain = this.Container.Resolve<IDomainService<SMEVEGRNFile>>();
            var SMEVEGRNLogDomain = this.Container.Resolve<IDomainService<SMEVEGRNLog>>();
            try
            {
                Operator thisOperator = UserManager.GetActiveOperator();               
                var egrn = SMEVEGRNFileDomain.GetAll().FirstOrDefault(x => x.FileInfo.Id == id)?.SMEVEGRN;
                if (thisOperator != null && egrn != null)
                {
                    SMEVEGRNLog log = new SMEVEGRNLog
                    {
                        Login = thisOperator.User.Login,
                        UserName = thisOperator.User.Name,
                        SMEVEGRN = new SMEVEGRN { Id = egrn.Id },
                        OperationType = "Скачивание файла",
                        FileInfo = new B4.Modules.FileStorage.FileInfo {Id = id}
                    };
                    SMEVEGRNLogDomain.Save(log);
                }
            }
            finally
            {
                Container.Release(UserManager);
                Container.Release(SMEVEGRNFileDomain);
                Container.Release(SMEVEGRNLogDomain);
            }
            var fileManager = this.Container.Resolve<IFileManager>();
            if (fileManager.CheckFile(id).Success == false) return new JsonNetResult("file does not exists");
            return fileManager.LoadFile(id);
        }
    }
}