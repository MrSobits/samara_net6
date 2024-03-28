using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;

    public class DeliveryAgentViewModel : BaseViewModel<DeliveryAgent>
    {
        public override IDataResult List(IDomainService<DeliveryAgent> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Contragent.Municipality.ParentMo == null ? x.Contragent.Municipality.Name : x.Contragent.Municipality.ParentMo.Name,
                    Settlement = x.Contragent.MoSettlement != null ? x.Contragent.MoSettlement.Name : (x.Contragent.Municipality.ParentMo != null ? x.Contragent.Municipality.Name : ""),
                    x.Contragent.Name,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    x.Contragent.Ogrn,
                    x.Contragent.ContragentState

                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<DeliveryAgent> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent.Id,
                    x.Contragent.Name,
                    x.Contragent.ShortName,
                     x.Contragent.OrganizationForm,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    FactAddress = x.Contragent.FiasFactAddress.AddressName,
                    JuridicalAddress = x.Contragent.FiasJuridicalAddress.AddressName,
                    MailingAddress = x.Contragent.FiasMailingAddress.AddressName,
                    x.Contragent.DateRegistration,
                    x.Contragent.Ogrn
                })
                                .FirstOrDefault()
                 );
        }
    }
}