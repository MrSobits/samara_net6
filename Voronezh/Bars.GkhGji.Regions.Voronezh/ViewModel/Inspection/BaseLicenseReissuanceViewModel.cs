namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GkhGji.Entities;
    using B4;
    using B4.Utils;
    using Entities;
    using GkhGji.Enums;
    using B4.DataAccess;

    public class BaseLicenseReissuanceViewModel : BaseViewModel<BaseLicenseReissuance>
    {
        public override IDataResult List(IDomainService<BaseLicenseReissuance> domainService, BaseParams baseParams)
        {
            var serviceInspRobject = Container.ResolveDomain<InspectionGjiRealityObject>();
            var serviceInspection = Container.ResolveDomain<InspectionGji>();
            var serviceView = Container.ResolveDomain<ViewBaseLicApplicants>();

            try
            {
                var loadParam = GetLoadParam(baseParams);

                var contragentId = baseParams.Params.GetAs<long>("contragentId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                if (showCloseInspections)
                {

                    var data = domainService.GetAll()
                        .Where(x => x.LicenseReissuance != null && x.TypeBase == TypeBase.LicenseReissuance)
                        .Select(x => new
                        {
                            x.Id,
                            x.InspectionNumber,
                            x.InspectionYear,
                            LicNumGJI = x.LicenseReissuance.ManOrgLicense.LicNumber,
                            x.TypeBase,
                            x.State,
                            ContragentName = x.LicenseReissuance.Contragent.Name,
                            ContragentInn = x.LicenseReissuance.Contragent.Inn,
                            x.LicenseReissuance.RegisterNum
                        }
                        ).Filter(loadParam, Container);

                    var totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
                }
                else
                {

                    var data = domainService.GetAll()
                        .Where(x => !x.State.FinalState)
                        .Where(x => x.LicenseReissuance != null && x.TypeBase == TypeBase.LicenseReissuance)
                        .Select(x => new
                        {
                            x.Id,
                            x.InspectionNumber,
                            x.InspectionYear,
                            LicNumGJI = x.LicenseReissuance.ManOrgLicense.LicNumber,
                            x.TypeBase,
                            x.State,
                            ContragentName = x.LicenseReissuance.Contragent.Name,
                            ContragentInn = x.LicenseReissuance.Contragent.Inn,
                            x.LicenseReissuance.RegisterNum
                        }
                        ).Filter(loadParam, Container);

                    var totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
                }
            }
            finally
            {
                Container.Release(serviceView);
                Container.Release(serviceInspRobject);
            }
        }

        public override IDataResult Get(IDomainService<BaseLicenseReissuance> domainService, BaseParams baseParams)
        {
            var serviceDiposal = Container.ResolveDomain<Disposal>();
            try
            {
                var id = baseParams.Params["id"].To<long>();
                var obj = domainService.Get(id);

                obj.Contragent = obj.LicenseReissuance.Contragent;

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