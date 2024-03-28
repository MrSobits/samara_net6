namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Enums;

    public class BaseLicenseApplicantsViewModel : BaseViewModel<BaseLicenseApplicants>
    {
        public override IDataResult List(IDomainService<BaseLicenseApplicants> domainService, BaseParams baseParams)
        {
            var serviceInspRobject = Container.ResolveDomain<InspectionGjiRealityObject>();
            var serviceView = Container.ResolveDomain<ViewBaseLicApplicants>();

            try
            {
                var loadParam = GetLoadParam(baseParams);

                var contragentId = baseParams.Params.GetAs<long>("contragentId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                var data = serviceView.GetAll()
                    .WhereIf(contragentId > 0, y => y.ContragentId == contragentId)
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.ReqNumber,
                        x.ContragentName,
                        x.PersonInspection,
                        x.InspectionNumber,
                        TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                        x.IsDisposal,
                        x.RealObjAddresses,
                        x.State
                    })
                    .Filter(loadParam, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(serviceView);
                Container.Release(serviceInspRobject);
            }
        }

        public override IDataResult Get(IDomainService<BaseLicenseApplicants> domainService, BaseParams baseParams)
        {
            var serviceDiposal = Container.ResolveDomain<Disposal>();
            try
            {
                var id = baseParams.Params["id"].To<long>();
                var obj = domainService.Get(id);

                obj.Contragent = obj.ManOrgLicenseRequest.Contragent;

                // Получаем Распоряжение
                var disposal = serviceDiposal.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == id && x.TypeDisposal == TypeDisposalGji.Base);

                if (disposal != null)
                    obj.DisposalId = disposal.Id;

                return new BaseDataResult(obj);
            }
            finally 
            {
                Container.Release(serviceDiposal);
            }
        }
    }
}