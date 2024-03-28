using System.Linq;

namespace Bars.GkhGji.Regions.Tyumen
{

    using B4;
    using B4.Utils;
    using Entities;
    using System;
    

    public class LicenseNotificationViewModel : BaseViewModel<LicenseNotification>
    {

        public override IDataResult List(IDomainService<LicenseNotification> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("mcid", 0L);
            if (id != 0)
            {
                var data = domain.GetAll()
                     .WhereIf(id != 0, x => x.ManagingOrgRealityObject.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.Comment,
                        x.LicenseNotificationNumber,
                        LocalGovernment = x.LocalGovernment.Contragent.Name,
                        Contragent = x.Contragent.Name,
                        x.MoDateStart,
                        x.NoticeOMSSendDate,
                        x.NoticeResivedDate,
                        x.OMSNoticeResult,
                        x.OMSNoticeResultDate,
                        x.OMSNoticeResultNumber,
                        x.RegistredNumber,
                        ManagingOrgRealityObject = x.ManagingOrgRealityObject.Id,
                        RealityObject = x.ManagingOrgRealityObject.RealityObject.Address,
                        Municipality = x.ManagingOrgRealityObject.RealityObject.Municipality.Name
                    })
                    .AsQueryable()
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var data = domain.GetAll()
                   .Select(x => new
                   {
                       x.Id,
                       x.Comment,
                       x.LicenseNotificationNumber,
                       LocalGovernment = x.LocalGovernment.Contragent.Name,
                       Contragent = x.Contragent.Name,
                       x.MoDateStart,
                       x.NoticeOMSSendDate,
                       x.NoticeResivedDate,
                       x.OMSNoticeResult,
                       x.OMSNoticeResultDate,
                       x.OMSNoticeResultNumber,
                       x.RegistredNumber,
                       ManagingOrgRealityObject = x.ManagingOrgRealityObject.Id,
                       RealityObject = x.ManagingOrgRealityObject.RealityObject.Address,
                       Municipality = x.ManagingOrgRealityObject.RealityObject.Municipality.Name

                   })
                   .AsQueryable()
                   .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }

        public override IDataResult Get(IDomainService<LicenseNotification> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Comment,
                    x.LicenseNotificationNumber,
                    LocalGovernment = x.LocalGovernment.Contragent.Name,
                    Contragent = x.Contragent.Name,
                    x.MoDateStart,
                    x.NoticeOMSSendDate,
                    x.NoticeResivedDate,
                    x.OMSNoticeResult,
                    x.OMSNoticeResultDate,
                    x.OMSNoticeResultNumber,
                    x.RegistredNumber,
                    ManagingOrgRealityObject = x.ManagingOrgRealityObject.Id,
                    RealityObject = x.ManagingOrgRealityObject.RealityObject.Address,
                    Municipality = x.ManagingOrgRealityObject.RealityObject.Municipality.Name

                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}