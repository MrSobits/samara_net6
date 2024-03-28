namespace Bars.Gkh.RegOperator.ViewModels.Import
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.Utils;

    public class ChesNotMatchAddressViewModel : BaseViewModel<ChesNotMatchAddress>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ChesNotMatchAddress> domainService, BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId("periodId");
            var loadParams = baseParams.GetLoadParam();

            return domainService.GetAll()
                .Where(x => x.Period == null || x.Period.Id == periodId)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}