namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.RapidResponseSystem
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    public class RapidResponseSystemAppealResponseViewModel : BaseViewModel<RapidResponseSystemAppealResponse>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<RapidResponseSystemAppealResponse> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var appealDetailsId = baseParams.Params.GetAsId("appealDetailsId");

            var data = domainService.GetAll()
                .WhereIf(id != default(long), x => x.Id == id)
                .WhereIf(appealDetailsId != default(long), x => x.RapidResponseSystemAppealDetails.Id == appealDetailsId)
                .SingleOrDefault();

            return new BaseDataResult(data);
        }
    }
}