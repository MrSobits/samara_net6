namespace Bars.Gkh.Regions.Tatarstan.Services.Impl.Fssp.CourtOrderGku
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Gis.DataResult;
    using Bars.Gkh.Import;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;
    using Bars.Gkh.Regions.Tatarstan.Enums;
    using Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.Fssp.CourtOrderGku;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.Enum;

    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class CourtOrderInfoImportService : ICourtOrderInfoImportService
    {
        public IWindsorContainer Container { get; set; }

        public IBilConnectionService BilConnectionService { get; set; }

        public IDataResult Import(BaseParams baseParams)
        {
            IGkhImportService importService;
            IDomainService<FileInfo> fileInfoDomain;
            IFileManager fileManager;
            IGkhUserManager userManager;
            IRepository<UploadDownloadInfo> uploadDownloadInfoDomain;

            using (this.Container.Using(
                importService = this.Container.Resolve<IGkhImportService>(),
                fileInfoDomain = this.Container.ResolveDomain<FileInfo>(),
                fileManager = this.Container.Resolve<IFileManager>(),
                userManager = this.Container.Resolve<IGkhUserManager>(),
                uploadDownloadInfoDomain = this.Container.Resolve<IRepository<UploadDownloadInfo>>()))
            {
                try
                {
                    var id = baseParams.Params.GetAsId();

                    if (id == 0)
                    {
                        var file = baseParams.Files["FileImport"];

                        var fileInfo = fileManager.SaveFile(file);

                        var importInfo = new UploadDownloadInfo
                        {
                            DownloadFile = fileInfo,
                            User = userManager.GetActiveUser(),
                            DateDownloadFile = DateTime.Now,
                            Status = FsspFileState.InQueue
                        };

                        uploadDownloadInfoDomain.Save(importInfo);
                        baseParams.Params.Add("id", importInfo.Id);
                    }
                    else
                    {
                        var info = uploadDownloadInfoDomain.Get(id);
                        var logFileId = info.LogFile?.Id;

                        info.Status = FsspFileState.InQueue;
                        info.LogFile = null;
                        info.DateDownloadFile = DateTime.Now;

                        uploadDownloadInfoDomain.Update(info);

                        if (logFileId != null)
                        {
                            var logFile = fileInfoDomain.Get(logFileId);
                            fileManager.Delete(logFile);
                        }

                        baseParams.Params.Add("importId", "Bars.Gkh.Regions.Tatarstan.Import.Fssp.CourtOrderGku.CourtOrderGkuInfoImport");
                    }

                    baseParams.Params.Add("connPgmu", this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu));

                    importService.Import(baseParams);

                    return new ImportDataResult(true, null, new List<long>());
                }
                catch (Exception e)
                {
                    return new ImportDataResult(false, e.Message, new List<long>());
                }
            }
        }
    }
}