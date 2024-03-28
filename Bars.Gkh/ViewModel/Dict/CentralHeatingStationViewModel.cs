namespace Bars.Gkh.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Вьюмодель для справочника ЦТП
    /// </summary>
    public class CentralHeatingStationViewModel : BaseViewModel<CentralHeatingStation>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<CentralHeatingStation> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Name,
                        x.Abbreviation,
                        x.Address.AddressName
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}