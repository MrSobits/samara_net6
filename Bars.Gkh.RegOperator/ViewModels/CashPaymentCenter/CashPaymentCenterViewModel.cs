namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

	/// <summary>
	/// ViewModel для <see cref="CashPaymentCenter"/>
	/// </summary>
	public class CashPaymentCenterViewModel : BaseViewModel<CashPaymentCenter>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domain">Домен-сервис для <see cref="CashPaymentCenter"/></param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult List(IDomainService<CashPaymentCenter> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

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
                    x.Contragent.ContragentState,
					x.Identifier
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

		/// <summary>
		/// Получить объект
		/// </summary>
		/// <param name="domainService">Домен-сервис для <see cref="CashPaymentCenter"/></param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения</returns>
		public override IDataResult Get(IDomainService<CashPaymentCenter> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Identifier,
                    x.ConductsAccrual,
                    x.ShowPersonalData,
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
                }).FirstOrDefault());
        }
    }
}