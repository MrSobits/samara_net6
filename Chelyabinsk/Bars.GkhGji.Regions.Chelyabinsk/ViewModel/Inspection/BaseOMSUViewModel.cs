namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GkhGji.Entities;
    using B4;
    using B4.Utils;
    using Entities;
    using GkhGji.Enums;


    public class BaseOMSUViewModel : BaseViewModel<BaseOMSU>
    {

        public override IDataResult List(IDomainService<BaseOMSU> domainService, BaseParams baseParams)
        {
            var serviceInspOMSU = Container.Resolve<IDomainService<BaseOMSU>>();
            var serviceInspGji = Container.Resolve<IDomainService<InspectionGji>>();
            var serviceInspInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var serviceInspZonalInspection = Container.Resolve<IDomainService<InspectionGjiZonalInspection>>();
            var serviceView = Container.Resolve<IDomainService<BaseOMSU>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            var loadParams = baseParams.GetLoadParam();

                /*
                 * dateStart - период с
                 * dateEnd - период по
                 * planId - идентификатор плана
                 * inspectorIds - идентификаторы инспекторов
                 * zonalIsnpectionIds - идентификаторы отделов
                 */
                var dateStart = baseParams.Params.ContainsKey("dateStart") ? baseParams.Params["dateStart"].ToDateTime() : DateTime.MinValue;
                var dateEnd = baseParams.Params.ContainsKey("dateEnd") ? baseParams.Params["dateEnd"].ToDateTime() : DateTime.MinValue;
                var planId = baseParams.Params.ContainsKey("planId") ? baseParams.Params["planId"].ToLong() : 0;
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);
                var inspectorIds = baseParams.Params.GetAs<long[]>("inspectorIds");
                var zonalIsnpectionIds = baseParams.Params.GetAs<long[]>("zonalIsnpectionIds");

#warning Здесь зачем тостояла эта проверка. Но при ней приходит = null и возвращается сразу пустой список. Незнаю кто и зачем так сделал но надо разобратся в дальнейшем
            /*
        if (inspectorIds == null)
        {
            return new ListDataResult(null,0);
        }
          */var baseInsp = serviceInspOMSU.GetAll()
                   .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DateStart <= dateEnd)
                .Select(x => x.Id).ToList();

            Dictionary<long, string> inspDict = new Dictionary<long, string>();
            var dataInspectors = serviceInspInspector.GetAll()
                  .Where(x=> baseInsp.Contains(x.Inspection.Id))
                    .ToList();
            if (dataInspectors.Count > 0)
            {
                foreach (InspectionGjiInspector inspinsp in dataInspectors)
                {
                    if (!inspDict.ContainsKey(inspinsp.Inspection.Id))
                    {
                        inspDict.Add(inspinsp.Inspection.Id, inspinsp.Inspector.Fio);
                    }
                    else
                    {
                        inspDict[inspinsp.Inspection.Id] += ", " + inspinsp.Inspector.Fio;
                    }
                }
            }

            var data = serviceView.GetAll()
                .WhereIf(inspectorIds != null && inspectorIds.Length > 0, y => serviceInspInspector.GetAll().Any(x => x.Inspection.Id == y.Id && inspectorIds.Contains(x.Inspector.Id)))
                .WhereIf(zonalIsnpectionIds != null && zonalIsnpectionIds.Length > 0, y => serviceInspZonalInspection.GetAll().Any(x => x.Inspection.Id == y.Id && zonalIsnpectionIds.Contains(x.ZonalInspection.Id)))
                .WhereIf(planId > 0, x => x.Plan.Id == planId)
                .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DateStart <= dateEnd)
                .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Contragent.Municipality.Name,
                    PlaceName = x.Contragent.JuridicalAddress,
                    Contragent = x.Contragent.Name,
                    Plan = x.Plan.Name,
                    x.InspectionNumber,
                    x.OmsuPerson,
                    x.DateStart,
                    x.CountDays,
                    InspectorNames = inspDict.ContainsKey(x.Id) ? inspDict[x.Id] : "",
                        x.TypeFact,
                        x.State
                    })
                    .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
           
          
        }

        public override IDataResult Get(IDomainService<BaseOMSU> domainService, BaseParams baseParams)
        {
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            try
            {
                var id = baseParams.Params["id"].To<long>();
                var obj = domainService.Get(id);

                // Получаем Распоряжение
                var disposal = serviceDisposal.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == id.ToLong() && x.TypeDisposal == TypeDisposalGji.Base);

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