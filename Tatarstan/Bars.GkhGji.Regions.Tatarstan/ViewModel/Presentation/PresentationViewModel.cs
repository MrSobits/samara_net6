namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Presentation
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class PresentationViewModel : GkhGji.ViewModel.PresentationViewModel
    {
        public override IDataResult List(IDomainService<Presentation> domainService, BaseParams baseParams)
        {
            var serviceInspectionRobject = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var servPresentationService = this.Container.Resolve<IPresentationService>();
            var servPresentation = this.Container.Resolve<IDomainService<Presentation>>();
            var protocolMvdRobjetcDomain = this.Container.ResolveDomain<ProtocolMvdRealityObject>();

            try
            {
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
                var isExport = baseParams.Params.GetAs<bool>("isExport");

                var query = servPresentationService.GetFilteredByOperator(servPresentation)
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
                        x.TypeDocumentGji,
                    });
                
                var protocolMvdAddresses = protocolMvdRobjetcDomain.GetAll()
                    .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                    .Where(x => query.Select(y => y.InspectionId).Any(y => y == x.ProtocolMvd.Inspection.Id))
                    .Select(x => new
                    {
                        InspectionId = x.ProtocolMvd.Inspection.Id,
                        x.RealityObject.Address
                    })
                    .AsEnumerable();

                var addressDict = serviceInspectionRobject
                    .GetAll()
                    .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                    .Where(x => query.Select(y => y.InspectionId).Any(y => y == x.Inspection.Id))
                    .Select(x => new
                    {
                        InspectionId = x.Inspection.Id,
                        x.RealityObject.Address
                    })
                    .AsEnumerable()
                    .Union(protocolMvdAddresses)
                    .GroupBy(x => x.InspectionId)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.Address).AggregateWithSeparator(", "));

                return query
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.ContragentName,
                        x.MunicipalityName,
                        x.MoSettlement,
                        x.PlaceName,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.DocumentDate,
                        x.TypeInitiativeOrg,
                        x.TypeBase,
                        x.PhysicalPerson,
                        x.InspectionId,
                        x.TypeDocumentGji,
                        PersonInspectionAddress = addressDict.ContainsKey(x.InspectionId)
                            ? addressDict[x.InspectionId]
                            : ""
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container, true, !isExport);
            }
            finally 
            {
                this.Container.Release(serviceInspectionRobject);
                this.Container.Release(servPresentationService);
                this.Container.Release(servPresentation);
                this.Container.Release(protocolMvdRobjetcDomain);
            }
        }
    }
}