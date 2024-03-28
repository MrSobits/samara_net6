namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class PaymentAgentViewModel : PaymentAgentViewModel<PaymentAgent>
    {
        // Внимание все override и новые метод писать в Generic класс
    }

    public class PaymentAgentViewModel<T> : BaseViewModel<T>
        where T : PaymentAgent
    {
        public override IDataResult List(IDomainService<T> domain, BaseParams baseParams)
        {
            var service = Container.Resolve<IPaymentAgentService>();

            try
            {
                int totalCount;
                return new ListDataResult(service.GetViewModelList(baseParams, out totalCount, false), totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public override IDataResult Get(IDomainService<T> domain, BaseParams baseParams)
        {

                var id = baseParams.Params.GetAs<long>("id");
                var obj = domain.GetAll().FirstOrDefault(x => x.Id == id);

                return obj != null ? new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.Contragent,
                        CtrName = obj.Contragent.Name,
                        CtrShortName = obj.Contragent.ShortName,
                        CtrInn = obj.Contragent.Inn,
                        CtrKpp = obj.Contragent.Kpp,
                        CtrOgrn = obj.Contragent.Ogrn,
                        CtrOrgFormName = obj.Contragent.OrganizationForm != null ? obj.Contragent.OrganizationForm.Name : string.Empty,
                        CtrJurAdress = obj.Contragent.FiasJuridicalAddress != null ? obj.Contragent.FiasJuridicalAddress.AddressName : string.Empty,
                        CtrFactAdress = obj.Contragent.FiasFactAddress != null ? obj.Contragent.FiasFactAddress.AddressName :string.Empty,
                        CtrMailAddress = obj.Contragent.FiasMailingAddress != null ? obj.Contragent.FiasMailingAddress.AddressName : string.Empty,
                        CtrDateReg = obj.Contragent.DateRegistration.HasValue ? obj.Contragent.DateRegistration.Value.ToShortDateString() : string.Empty,
                        obj.Code,
                        obj.PenaltyContractId,
                        obj.SumContractId
                    }) : new BaseDataResult();


        }
    }
} 