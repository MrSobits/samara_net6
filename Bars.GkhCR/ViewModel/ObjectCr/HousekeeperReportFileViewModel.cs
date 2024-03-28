namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Enums;

    using Entities;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// The ViewModel
    /// </summary>
    public class HousekeeperReportFileViewModel : BaseViewModel<HousekeeperReportFile>
    {
        public IOverhaulViewModels OverhaulViewModels { get; set; }

        public override IDataResult List(IDomainService<HousekeeperReportFile> domainService, BaseParams baseParams)
        {    
            var loadParams = GetLoadParam(baseParams);
            var reportId = baseParams.Params.GetAsId("reportId");           


            var data = domainService.GetAll()
                .Where(x => x.HousekeeperReport.Id == reportId)              
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.FileInfo
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
         
        }

    }
}