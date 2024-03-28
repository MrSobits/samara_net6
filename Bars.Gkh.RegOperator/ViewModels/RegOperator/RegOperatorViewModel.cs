namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Entities;

    public class RegOperatorViewModel : BaseViewModel<RegOperator>
    {
        public override IDataResult List(IDomainService<RegOperator> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Contragent.Municipality.Name,
                    Contragent = x.Contragent.Name,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    ContragentId = x.Contragent.Id,
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Contragent)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<RegOperator> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                                {
                                    x.Id,
                                    Municipality = x.Contragent.Municipality.Name,
                                    ContragentId = x.Contragent.Id,
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