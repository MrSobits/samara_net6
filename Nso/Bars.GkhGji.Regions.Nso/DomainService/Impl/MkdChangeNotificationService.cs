namespace Bars.GkhGji.Regions.Nso.DomainService.Impl
{
	using Bars.B4;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Enums;
	using System.Linq;

	public class MkdChangeNotificationService : IMkdChangeNotificationService
	{
		public IDomainService<ManagingOrganization> ManOrgDomainService { get; set; }
		public IDomainService<ContragentContact> ContragentContactDomainService { get; set; }
		public IDomainService<RealityObject> RealityObjectDomain { get; set; }
		public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }

		public IDataResult GetManagingOrgDetails(BaseParams baseParams)
		{
			var manOrgId = baseParams.Params.GetAs<long>("manOrgId");
			var manOrg = ManOrgDomainService.GetAll()
				.FirstOrDefault(x => x.Id == manOrgId);

			if (manOrg == null)
			{
				return new BaseDataResult(false);
			}

			var contact = ContragentContactDomainService.GetAll()
				.FirstOrDefault(x => x.Contragent.Id == manOrg.Contragent.Id);
			
			return new BaseDataResult(new
			{
				manOrg.Contragent.Inn,
				manOrg.Contragent.Ogrn,
				manOrg.Contragent.JuridicalAddress,
				manOrg.Contragent.Phone,
				manOrg.Contragent.Email,
				manOrg.Contragent.OfficialWebsite,
				Manager = contact != null ? contact.FullName : ""
			});
		}
		
		public IDataResult GetManagingOrgByAddressName(BaseParams baseParams)
		{
			var addressName = baseParams.Params.GetAs<string>("addressName");

			var realityObject = RealityObjectDomain.GetAll()
					.FirstOrDefault(x => x.FiasAddress.AddressName == addressName);

			if (realityObject == null)
			{
				return new BaseDataResult(false);
			}

			var manOrgRo = ManOrgContractRealityObjectDomain.GetAll()
				.FirstOrDefault(x => x.RealityObject.Id == realityObject.Id &&
				                     x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag);

			if (manOrgRo == null)
			{
				return new BaseDataResult(false);
			}

			return new BaseDataResult(manOrgRo.ManOrgContract.ManagingOrganization);
		}
	}
}
