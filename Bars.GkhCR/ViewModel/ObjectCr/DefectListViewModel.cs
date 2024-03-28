namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;

    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.DomainService.GkhParam;

    public class DefectListViewModel : BaseViewModel<DefectList>
    {
        public IOverhaulViewModels OverhaulViewModels { get; set; }

        public override IDataResult List(IDomainService<DefectList> domainService, BaseParams baseParams)
        {
            var formFinanceSource = Container.GetGkhConfig<GkhCrConfig>().DpkrConfig.TypeDefectListView;

            if (OverhaulViewModels != null && formFinanceSource == TypeDefectListView.WithOverhaulData)
            {
                return OverhaulViewModels.DefectListList(domainService, baseParams);
            }

            var loadParams = GetLoadParam(baseParams);

            var objectCrId = baseParams.Params.ContainsKey("objectCrId")
                ? baseParams.Params.GetAsId("objectCrId")
                : loadParams.Filter.GetAsId("objectCrId");
            var twId = baseParams.Params.GetAsId("twId");

            var data =
                domainService.GetAll()
                             .Where(x => x.ObjectCr.Id == objectCrId)
                             .WhereIf(twId > 0, x => x.TypeWork.Id == twId)
                             .Select(
                                 x =>
                                 new
                                     {
                                         x.Id,
                                         WorkName = x.Work.Name,
                                         x.DocumentName,
                                         x.DocumentDate,
                                         x.State,
                                         x.File,
                                         x.Volume,
                                         x.Sum,
                                         x.TypeDefectList
                                     })
                             .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<DefectList> domainService, BaseParams baseParams)
        {
            var formFinanceSource = Container.GetGkhConfig<GkhCrConfig>().DpkrConfig.TypeDefectListView;

            if (OverhaulViewModels != null && formFinanceSource == TypeDefectListView.WithOverhaulData)
            {
                return OverhaulViewModels.DefectListGet(domainService, baseParams);
            }

            return base.Get(domainService, baseParams);
        }
    }
}