namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;

    using Entities;

    public class PresentationViewModel: PresentationViewModel<Presentation>
    {
    }

    public class PresentationViewModel<T> : BaseViewModel<T>
        where T: Presentation
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceInspectionRobject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var servPresentationService = Container.Resolve<IPresentationService>();
            var servPresentation = Container.Resolve<IDomainService<Presentation>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                /*
                 * В качестве фильтров приходят следующие параметры
                 * dateStart - Необходимо получить документы больше даты начала
                 * dateEnd - Необходимо получить документы меньше даты окончания
                 * realityObjectId - Необходимо получить документы по дому 
                 * stageId - необходимо получить документы по этапу проверки
                 */

                var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
                var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
                var realityObjectId = baseParams.Params.GetAsId("realityObjectId");
                var stageId = baseParams.Params.GetAsId("stageId");

                var data = servPresentationService.GetFilteredByOperator(servPresentation)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, y => serviceInspectionRobject.GetAll()
                        .Any(x => x.RealityObject.Id == realityObjectId
                            && x.Inspection.Id == y.Inspection.Id))
                    .WhereIf(stageId > 0, x => x.Stage.Id == stageId)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        ContragentName = x.Contragent.Name,
                        MunicipalityName = x.Contragent.Municipality.Name,
                        MoSettlement = x.Contragent.MoSettlement.Name,
                        x.Contragent.FiasJuridicalAddress.PlaceName,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.DocumentDate,
                        x.TypeInitiativeOrg,
                        x.Inspection.TypeBase,
                        x.PhysicalPerson,
                        InspectionId = x.Inspection.Id,
                        x.TypeDocumentGji
                    })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(serviceInspectionRobject);
                Container.Release(servPresentationService);
                Container.Release(servPresentation);
            }
        }
    }
}