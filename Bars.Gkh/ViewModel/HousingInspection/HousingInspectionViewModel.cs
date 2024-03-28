namespace Bars.Gkh.ViewModel.HousingInspection
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.HousingInspection;
    using Bars.Gkh.Utils;

    public class HousingInspectionViewModel : BaseViewModel<HousingInspection>
    {
        public override IDataResult List(IDomainService<HousingInspection> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            return domainService.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        Municipality = x.Contragent.Municipality.Name,
                        Contragent = x.Contragent.Name,
                        x.Contragent.Inn,
                        x.Contragent.Kpp,
                        ContragentId = x.Contragent.Id
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Contragent)
                .ToListDataResult(loadParams, this.Container);
        }

        public override IDataResult Get(IDomainService<HousingInspection> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(
                        x => new
                        {
                            x.Id,
                            Municipality = x.Contragent.Municipality.Name,
                            Contragent = x.Contragent.Name,
                            x.Contragent.Inn,
                            x.Contragent.Kpp,
                            FactAddress = x.Contragent.FiasFactAddress.AddressName,
                            x.Contragent.Phone,
                            x.Contragent.Email,
                            x.Contragent.Ogrn
                        })
                    .FirstOrDefault()
            );
        }
    }
}