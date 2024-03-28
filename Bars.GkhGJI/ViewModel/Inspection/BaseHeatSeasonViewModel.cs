namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;
    using Enums;

    public class BaseHeatSeasonViewModel: BaseHeatSeasonViewModel<BaseHeatSeason>
    {
    }

    public class BaseHeatSeasonViewModel<T> : BaseViewModel<T>
        where T : BaseHeatSeason
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var servInspViolStage = Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var servDisp = Container.Resolve<IDomainService<Disposal>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                /*В данном методе по переданному seasonId (Подготовка к отопительному сезону) получаем все распоряжения ГЖИ с типом*/
                var seasonId = baseParams.Params.GetAs<long>("seasonId");

                // Получаем список идентификаторов проверок отопительного сезона для данного seasonId
                var listInspectionIds = domainService.GetAll().Where(x => x.HeatingSeason.Id == seasonId).Select(x => x.Id).ToList();

                var inspectionsViolations = servInspViolStage.GetAll()
                    .Where(x => listInspectionIds.Contains(x.InspectionViolation.Inspection.Id) && x.Document.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(x => new
                    {
                        InspectionId = x.Document.Inspection.Id,
                        InsViolId = x.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.InspectionId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.InsViolId).Count());

                // Теперь получаем все основные распоряжения по проверкам Отопительного сезона

                var data = servDisp.GetAll()
                    .Where(x => listInspectionIds.Contains(x.Inspection.Id) && x.TypeDisposal == TypeDisposalGji.Base)
                    .Select(x => new
                    {
                        x.Inspection.Id,
                        DisposalDocumentNumber = x.DocumentNumber,
                        DisposalDocumentDate = x.DocumentDate
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.DisposalDocumentNumber,
                        x.DisposalDocumentDate,
                        CountViol = inspectionsViolations.ContainsKey(x.Id) ? inspectionsViolations[x.Id] : 0
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(servDisp);
                Container.Release(servInspViolStage);
            }
        }
    }
}