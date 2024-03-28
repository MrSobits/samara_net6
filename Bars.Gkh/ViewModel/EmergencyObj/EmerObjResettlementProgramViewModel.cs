namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class EmerObjResettlementProgramViewModel : BaseViewModel<EmerObjResettlementProgram>
    {
        public override IDataResult List(IDomainService<EmerObjResettlementProgram> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var emergencyObjId = baseParams.Params.GetAs<long>("emergencyObjId");

            var data = domain.GetAll()
                .Where(x => x.EmergencyObject.Id == emergencyObjId)
                .Select(x => new
                {
                    x.Id,
                    x.CountResidents,
                    x.Area,
                    x.Cost,
                    x.ActualCost,
                    SourceName = x.ResettlementProgramSource.Name
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}