namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.NetworkWorker;
    using Bars.Gkh.FormatDataExport.NetworkWorker.Responses;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис экспорта в формате 4.0.X
    /// </summary>
    public class FormatDataExportService : IFormatDataExportService
    {
        public IWindsorContainer Container { get;set; }

        public IFormatDataTransferService FormatDataTransferService { get; set; }
        public IEnumerable<IExportableEntityGroup> ExportableEntityGroup { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }
        public IFormatDataExportRoleService FormatDataExportRoleService { get; set; }

        /// <inheritdoc />
        public IDataResult ListAvailableSection(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var exportType = baseParams.Params.GetAs<FormatDataExportType>("exportType");

            return this.ExportableEntityGroup
                .GroupBy(g => g.Code)
                .Where(x => (exportType & x.First().ExportType) != 0)
                .Select(group =>
                {
                    var inheritedEntityCodeList = group.First().InheritedEntityCodeList.ToList();
                    var trigger = true;
                    
                    foreach (var data in group)
                    {
                        if (trigger)
                        {
                            trigger = !trigger;
                            continue;
                        }
                    
                        inheritedEntityCodeList.AddRange(data.InheritedEntityCodeList);
                    }

                    return new
                    {
                        Code = group.Key,
                        group.First().Description,
                        InheritedEntityCodeList = inheritedEntityCodeList
                    };
                })
                .ToListDataResult(loadParams, this.Container);
        }

        /// <inheritdoc />
        public IDataResult GetRemoteStatus(BaseParams baseParams)
        {
            var statusId = baseParams.Params.GetAsId();
            if (statusId == 0)
            {
                return BaseDataResult.Error("Отсутствует идентификатор загрузки");
            }

            return this.FormatDataTransferService.GetStatus(statusId);
        }

        /// <inheritdoc />
        public IDataResult StartRemoteImport(BaseParams baseParams)
        {
            var fileId = baseParams.Params.GetAsId();
            if (fileId == 0)
            {
                return BaseDataResult.Error("Отсутствует идентификатор загрузки");
            }

            return this.FormatDataTransferService.StartImport(fileId, CancellationToken.None);
        }

        /// <inheritdoc />
        public IDataResult GetRemoteFile(BaseParams baseParams)
        {
            var fileId = baseParams.Params.GetAsId();
            if (fileId == 0)
            {
                return BaseDataResult.Error("Отсутствует идентификатор удаленного файла");
            }

            return this.FormatDataTransferService.GetFile(fileId);
        }

        /// <inheritdoc />
        public IDataResult UpdateRemoteStatus(BaseParams baseParams)
        {
            var result = this.GetRemoteStatus(baseParams);
            var statusResult = result?.Data as StatusSuccess;
            if (statusResult != null)
            {
                var resultId = baseParams.Params.GetAsId("resultId");
                var remoteResultDomain = this.Container.ResolveDomain<FormatDataExportRemoteResult>();
                using (this.Container.Using(remoteResultDomain))
                {
                    var remoteResult = remoteResultDomain.GetAll()
                        .FirstOrDefault(x => x.Id == resultId);

                    if (remoteResult != null)
                    {
                        remoteResult.Status = statusResult.Status;
                        remoteResult.LogId = statusResult.LogId;
                    }

                    remoteResultDomain.Update(remoteResult);
                }
            }

            return result;
        }
    }
}