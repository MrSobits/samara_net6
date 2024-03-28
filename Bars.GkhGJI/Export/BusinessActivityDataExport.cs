namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Utils;
    using B4.Modules.DataExport.Domain;
    using Entities;
    using Gkh.Authentification;

    public class BusinessActivityDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var userManager = Container.Resolve<IGkhUserManager>();

            var contragentList = userManager.GetContragentIds();
            var municipalityList = userManager.GetMunicipalityIds();

            return Container.Resolve<IDomainService<ViewBusinessActivity>>().GetAll()
                .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.MunicipalityId))
                .WhereIf(contragentList.Count > 0, x => contragentList.Contains(x.ContragentId))
                .Select(x => new
                    {
                        x.Id,
                        x.ContragentName,
                        x.OrgFormName,
                        x.ContragentMailingAddress,
                        x.ContragentOgrn,
                        x.ContragentInn,
                        x.TypeKindActivity,
                        x.IncomingNotificationNum,
                        x.DateRegistration,
                        x.DateNotification,
                        x.RegNum,
                        x.IsOriginal,
                        HasFile = x.FileInfoId != null,
                        x.MunicipalityName,
                        x.State,
                        x.ServiceCount,
                        RegNumDateYear = string.Format("{0} от {1}", x.RegNum, x.DateRegistration.HasValue && x.DateRegistration.Value != DateTime.MinValue ? x.DateRegistration.Value.ToShortDateString() : "-")
                    })
                .Filter(loadParam, Container)
                .OrderIf(loadParam.Order.Length == 0, true, x => x.MunicipalityName)
                .Order(loadParam)
                .ToList();
        }
    }
}