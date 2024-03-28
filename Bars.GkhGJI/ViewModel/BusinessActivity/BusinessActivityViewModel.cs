namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;

    public class BusinessActivityViewModel : BaseViewModel<BusinessActivity>
    {
        public override IDataResult List(IDomainService<BusinessActivity> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var userManager = this.Container.Resolve<IGkhUserManager>();

            var contragentList = userManager.GetContragentIds();
            var municipalityList = userManager.GetMunicipalityIds();

            var data = Container.Resolve<IDomainService<ViewBusinessActivity>>().GetAll()
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
                        x.Registered,
                        RegNumDateYear = string.Format("{0} от {1}", x.RegNum, x.DateRegistration.HasValue && x.DateRegistration.Value != DateTime.MinValue ? x.DateRegistration.Value.ToShortDateString() : "-")
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}