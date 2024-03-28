namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System;
    using B4.Utils;
    using System.Linq;
    using B4;
    using Gkh.Utils;
    using GkhGji.Entities;
    using Castle.Windsor;
    using Entities;

    public class ActVisualService : IActVisualService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var inspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                .Where(x => x.DocumentGji.Id == documentId)
                .Select(x => new
                {
                    x.Inspector.Id,
                    x.Inspector.Fio
                })
                .ToArray();

            var inspectorFio = inspectors.AggregateWithSeparator(x => x.Fio, ", ");
            var inspectorIds = inspectors.AggregateWithSeparator(x => x.Id, ",");

            return new BaseDataResult(new {inspectorFio, inspectorIds});
        }

        public IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var dictDocGjiInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                               .GroupBy(x => x.DocumentGji.Id)
                                               .ToDictionary(x => x.Key,
                                                             y => string.Join(", ", y.Select(x => x.Inspector.Fio)));

            var data = Container.Resolve<IDomainService<ActVisual>>().GetAll()
                .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    InspectorNames = dictDocGjiInspector.ContainsKey(x.Id) ? dictDocGjiInspector[x.Id] : "",
                    x.Inspection.TypeBase,
                    InspectionId = x.Inspection.Id,
                    x.TypeDocumentGji
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
