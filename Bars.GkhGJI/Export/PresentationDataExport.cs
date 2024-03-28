namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using Entities;
    using DomainService;

    public class PresentationDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
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
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var stageId = baseParams.Params.GetAs<long>("stageId");

            var serviceInspectionRobject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var domainService = Container.Resolve<IDomainService<Presentation>>();

            return Container.Resolve<IPresentationService>().GetFilteredByOperator(domainService)
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
                                    x.DocumentNumber,
                                    x.DocumentNum,
                                    x.DocumentDate,
                                    x.TypeInitiativeOrg,
                                    x.Inspection.TypeBase,
                                    x.PhysicalPerson,
                                    InspectionId = x.Inspection.Id
                                })
                            .Filter(loadParam, Container)
                            .Order(loadParam)
                            .ToList();
        }
    }
}