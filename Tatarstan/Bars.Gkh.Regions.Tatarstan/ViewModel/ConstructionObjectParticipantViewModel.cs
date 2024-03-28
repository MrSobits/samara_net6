namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
	using System.Collections.Generic;
	using System.Linq;

    using Bars.B4;
	using Bars.B4.Utils;
	using Bars.Gkh.Domain;
	using Bars.Gkh.DomainService;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Regions.Tatarstan.Entities;

	/// <summary>
	/// ViewModel для Участник объекта строительства
	/// </summary>
	public class ConstructionObjectParticipantViewModel : BaseViewModel<ConstructionObjectParticipant>
    {
		/// <summary>
		/// Сервис для Контрагент
		/// </summary>
		public IContragentService ContragentService { get; set; }

		/// <summary>
		/// Получить список участников объекта строительства
		/// </summary>
		/// <param name="domain">Домен-сервис</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult List(IDomainService<ConstructionObjectParticipant> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAsId("objectId");

	        var contragentIds = domain.GetAll()
		        .Where(x => x.ConstructionObject.Id == objectId)
				.Where(x => x.Contragent != null)
		        .Select(x => x.Contragent.Id)
		        .ToArray();

	        var contactsDict = this.ContragentService.GetActualManagerContacts(contragentIds);

			var data = domain.GetAll()
		        .Where(x => x.ConstructionObject.Id == objectId)
				.AsEnumerable()
		        .Select(
			        x =>
			        {
				        var contact = this.GetContact(x.Contragent, contactsDict);
				        return new
				        {
					        x.Id,
					        x.ParticipantType,
					        x.CustomerType,
					        Contragent = x.Contragent.Return(y => y.Name),
					        ContragentInn = x.Contragent.Return(y => y.Inn),
					        ContragentContactName = contact.Return(y => y.FullName),
					        ContragentContactPhone = contact.Return(y => y.Phone)
				        };
			        })
				.AsQueryable()
		        .Filter(loadParams, this.Container)
		        .Order(loadParams);

            var totalCount = data.Count();

            return new ListDataResult(data.Paging(loadParams).ToList(), totalCount);
        }

		private ContragentContact GetContact(Contragent contragent, IDictionary<long, ContragentContact> contactsDict)
		{
			if (contragent == null)
			{
				return null;
			}

			return contactsDict.ContainsKey(contragent.Id) ? contactsDict[contragent.Id] : null;
		}
    }
}