using System.Linq;
using Bars.B4.DataAccess;

namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{

    using B4;
    using Entities;
    using Bars.Gkh.Entities;
    using System;
    using Bars.B4.Utils;

    public class ProtocolOSPRequestViewModel : BaseViewModel<ProtocolOSPRequest>
    {
        public override IDataResult List(IDomainService<ProtocolOSPRequest> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);
            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());


            var data = domainService.GetAll()
                .Where(x => x.Date >= dateStart2 && x.Date <= dateEnd2)
                .WhereIf(!showCloseAppeals, x => !x.State.FinalState)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.FIO,
                    Address = x.RealityObject != null ? x.RealityObject.Address : "",
                    Municipality = x.RealityObject != null ? x.RealityObject.Municipality.Name : "",
                    x.RealityObject,
                    x.RoFiasGuid,
                    x.UserEsiaGuid,
                    x.Date,
                    x.GjiId,
                    x.Approved,
                    x.Email,
                    x.State,
                    x.RequestNumber,
                    x.ApplicantType,
                    OwnerProtocolType = x.OwnerProtocolType != null? x.OwnerProtocolType.Name:"",
                    Inspector = x.Inspector != null? x.Inspector.Fio:"",
                    x.ProtocolFile,
                }).AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}