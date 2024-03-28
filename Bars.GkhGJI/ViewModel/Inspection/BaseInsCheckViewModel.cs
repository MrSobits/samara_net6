namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Enums;

    public class BaseInsCheckViewModel: BaseInsCheckViewModel<BaseInsCheck>
    {
    }

    public class BaseInsCheckViewModel<T> : BaseViewModel<T>
        where T: BaseInsCheck
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceInspInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var serviceView = Container.Resolve<IDomainService<ViewBaseInsCheck>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                /**
                 * dateStart - период с
                 * dateEnd - период по
                 * planId - идентификатор плана
                 * inspectorIds - идентификаторы инспекторов
                 */
                var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
                var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
                var planId = baseParams.Params.GetAs<long>("planId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);
                var inspectorIds = baseParams.Params.GetAs<long[]>("inspectorIds");

                var data = serviceView.GetAll()
                    .WhereIf(planId > 0, x => x.PlanId == planId)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.InsCheckDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.InsCheckDate <= dateEnd)
                    .WhereIf(inspectorIds.Length > 0, y => serviceInspInspector.GetAll().Any(x => x.Inspection.Id == y.Id && inspectorIds.Contains(x.Inspector.Id)))
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        x.InspectionNumber,
                        Municipality = x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        Address = x.RealityObjectAddress,
                        Plan = x.PlanName,
                        x.ContragentName,
                        x.InsCheckDate,
                        x.DisposalNumber,
                        x.InspectorNames,
                        x.TypeFact,
                        x.State
                    })
                    .Filter(loadParam, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(serviceInspInspector);
                Container.Release(serviceView);
            }
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();
            try
            {
                var id = baseParams.Params["id"].To<long>();
                var obj = domainService.Get(id);

                // Получаем Распоряжение
                var disposal = serviceDisposal.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == id && x.TypeDisposal == TypeDisposalGji.Base);

                if (disposal != null)
                {
                    obj.DisposalId = disposal.Id;
                }

                return new BaseDataResult(obj);
            }
            finally 
            {
                Container.Release(serviceDisposal);
            }
        }
    }
}