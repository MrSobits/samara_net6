namespace Bars.GkhGji.Regions.Nso.Export
{
	using Bars.B4;
	using Bars.B4.Modules.DataExport.Domain;
	using Bars.GkhGji.Regions.Nso.Entities;
	using System.Collections;
	using System.Linq;

	public class MkdChangeNotificationExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
			var domainService = Container.Resolve<IDomainService<MkdChangeNotification>>();
			var loadParams = GetLoadParam(baseParams);

            return domainService.GetAll()
				.Select(x => new
				{
					x.Id,
					x.RegistrationNumber,
					x.InboundNumber,
					x.RegistrationDate,
					Municipality = x.RealityObjectFantom.RealityObject.Municipality.Name ?? "",
					Settlement = x.RealityObjectFantom.RealityObject.MoSettlement.Name ?? "",
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
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}