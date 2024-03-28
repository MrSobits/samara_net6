namespace Bars.Gkh.ViewModel.RealityObject
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class RealityObjectAntennaViewModel : BaseViewModel<RealityObjectAntenna>
    {
        public override IDataResult List(IDomainService<RealityObjectAntenna> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            return domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    x.Availability,
                    x.Workability,
                    x.Range,
                    FrequencyFrom = x.FrequencyFrom > 0 ? x.FrequencyFrom + " МГц": "",
                    FrequencyTo = x.FrequencyTo > 0 ? x.FrequencyTo + " МГц" : "",
                    x.NumberApartments,
                    x.FileInfo,
                    x.Reason
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}