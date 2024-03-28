namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class MkdChangeNotificationViewModel : BaseViewModel<MkdChangeNotification>
	{
		public IDomainService<ContragentContact> ContragentContactDomainService { get; set; }

		public override IDataResult List(IDomainService<MkdChangeNotification> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

			var data = domainService.GetAll()
				.Select(x => new
				{
					x.Id,
					x.RegistrationNumber,
					x.InboundNumber,
					x.RegistrationDate,
                    Municipality = x.RealityObjectFantom.RealityObject.Municipality.Name ?? x.RealityObjectFantom.MunicipalityFantom.Name,
                    Settlement = x.RealityObjectFantom.RealityObject.MoSettlement.Name ?? x.RealityObjectFantom.SettlementFantom.Name,
					Address = x.RealityObjectFantom.RealityObject.Address ?? x.RealityObjectFantom.Fantom,
					NotificationCause = x.NotificationCause.Name,
					OldMkdManagementMethod = x.OldMkdManagementMethod.Name,
					OldManagingOrganization = x.OldManagingOrganization.Contragent.Name,
					x.OldInn,
					x.OldOgrn,
					NewMkdManagementMethod = x.NewMkdManagementMethod.Name,
					NewManagingOrganization = x.NewManagingOrganization.Contragent.Name,
					x.NewInn,
					x.NewOgrn,
					x.NewJuridicalAddress,
					x.NewManager,
					x.NewPhone,
					x.NewEmail,
					x.NewOfficialSite,
					x.NewActCopyDate,
					x.State
				})
				.Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

		public override IDataResult Get(IDomainService<MkdChangeNotification> domainService, BaseParams baseParams)
        {
            var notif = domainService.Get(baseParams.Params.GetAsId());

			var oldContragent = notif.OldManagingOrganization.ReturnSafe(x => x.Contragent);
			var newContragent = notif.NewManagingOrganization.ReturnSafe(x => x.Contragent);

			if (notif.NewManager == null)
			{
				var contragentId = newContragent.ReturnSafe(x => x.Id);
				var contact = this.ContragentContactDomainService.GetAll()
					.FirstOrDefault(x => x.Contragent.Id == contragentId);

				notif.NewManager = contact != null ? contact.FullName : "";
			}

			return new BaseDataResult(new
            {
				notif.Id,
				notif.Date,
				notif.RegistrationNumber,
				notif.InboundNumber,
				notif.RegistrationDate,
				FiasAddress = this.GetAddress(notif.RealityObjectFantom),
				NotificationCause = notif.NotificationCause.Name,
				OldMkdManagementMethod = notif.OldMkdManagementMethod.Name,
				OldManagingOrganization = oldContragent.ReturnSafe(x => x.Name),
				OldInn = notif.OldInn ?? oldContragent.ReturnSafe(x => x.Inn),
				OldOgrn = notif.OldOgrn ?? oldContragent.ReturnSafe(x => x.Ogrn),
				NewMkdManagementMethod = notif.NewMkdManagementMethod.Name,
				NewManagingOrganization = newContragent.ReturnSafe(x => x.Name),
				NewInn = notif.NewInn ?? newContragent.ReturnSafe(x => x.Inn),
				NewOgrn = notif.NewOgrn ?? newContragent.ReturnSafe(x => x.Ogrn),
				NewJuridicalAddress = notif.NewJuridicalAddress ?? newContragent.ReturnSafe(x => x.JuridicalAddress),
				notif.NewManager,
				NewPhone = notif.NewPhone ?? newContragent.ReturnSafe(x => x.Phone),
				NewEmail = notif.NewEmail ?? newContragent.ReturnSafe(x => x.Email),
				NewOfficialSite = notif.NewOfficialSite ?? newContragent.ReturnSafe(x => x.OfficialWebsite),
				notif.NewActCopyDate,
				notif.State
            });
        }

		#region Private methods

		private FiasAddress GetAddress(RealityObjectFantom roFantom)
		{
			var address = roFantom.RealityObject != null && roFantom.RealityObject.FiasAddress != null
				? roFantom.RealityObject.FiasAddress.AddressName
				: roFantom.Fantom;

			//возвращаем пустышку, чтобы на клиенте не заполнялись ненужные поля 
			//и работал контрол выбора адреса
			return new FiasAddress() { AddressName = address };
		}

		#endregion
	}
}