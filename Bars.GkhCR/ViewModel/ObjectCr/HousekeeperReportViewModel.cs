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
    public class HousekeeperReportViewModel : BaseViewModel<HousekeeperReport>
    {
        public IOverhaulViewModels OverhaulViewModels { get; set; }

        public override IDataResult List(IDomainService<HousekeeperReport> domainService, BaseParams baseParams)
        {    
            var loadParams = GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");           


            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)              
                .Select(x => new
                {
                    x.Id,
                    FIO = x.RealityObjectHousekeeper != null? x.RealityObjectHousekeeper.FIO:"",
                    PhoneNumber = x.RealityObjectHousekeeper != null ? x.RealityObjectHousekeeper.PhoneNumber:"",
                    x.ReportDate,
                    x.ReportNumber,
                    x.CheckDate,
                    x.CheckTime,
                    x.State,
                    x.IsArranged
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
         
        }
        public override IDataResult Get(IDomainService<Entities.HousekeeperReport> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var obj =
                domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.Answer,
                        x.CheckDate,
                        x.CheckTime,
                        x.Description,
                        x.IsArranged,
                        x.ObjectCr,
                        x.ObjectCreateDate,
                        x.ObjectEditDate,
                        x.ObjectVersion,
                        x.RealityObjectHousekeeper,
                        FIO = x.RealityObjectHousekeeper != null ? x.RealityObjectHousekeeper.FIO : "",
                        PhoneNumber = x.RealityObjectHousekeeper != null ? x.RealityObjectHousekeeper.PhoneNumber : "",
                        x.ReportDate,
                        x.ReportNumber                       

                    })
                    .FirstOrDefault();

            return new BaseDataResult(obj);
        }

    }
}