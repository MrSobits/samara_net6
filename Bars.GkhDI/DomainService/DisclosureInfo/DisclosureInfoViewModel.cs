namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.Gkh.Enums;
    using Entities;

	/// <summary>
	/// ViewModel для DisclosureInfo
	/// </summary>
	public class DisclosureInfoViewModel : BaseViewModel<DisclosureInfo>
    {
		/// <summary>
		/// Получить объект
		/// </summary>
		/// <param name="domainService">Домен</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
        public override IDataResult Get(IDomainService<DisclosureInfo> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

            var disclosureInfoService = this.Container.Resolve<IDisclosureInfoService>();

            return obj != null && obj.ManagingOrganization != null ?
                new BaseDataResult(new
                       {
                           obj.Id,
                           obj.State,
                           ContragentName = obj.ManagingOrganization.Contragent.Name,
                           obj.PeriodDi,
                           obj.ManagingOrganization,
                           PeriodDiName = obj.PeriodDi.Name,
                           ContragentId = obj.ManagingOrganization.Contragent.Id,
                           ManagingOrgId = obj.ManagingOrganization.Id,
                           obj.ManagingOrganization.TypeManagement,
                           FioDirector = disclosureInfoService.GetPositionByCode(obj.ManagingOrganization.Contragent.Id, obj.PeriodDi, new List<string> { "1", "4" }),
                           FiasJurAddressName = obj.ManagingOrganization.Contragent.FiasJuridicalAddress != null
                                             ? obj.ManagingOrganization.Contragent.FiasJuridicalAddress.AddressName
                                             : string.Empty,
                           obj.ManagingOrganization.Contragent.Ogrn,
                           obj.ManagingOrganization.Contragent.OgrnRegistration,
                           ActivityDateStart = obj.ManagingOrganization.Contragent.DateRegistration.HasValue ? obj.ManagingOrganization.Contragent.DateRegistration.Value.ToShortDateString() : string.Empty,
                           FiasMailAddressName = obj.ManagingOrganization.Contragent.FiasMailingAddress != null
                                              ? obj.ManagingOrganization.Contragent.FiasMailingAddress.AddressName
                                              : string.Empty,
                           FiasFactAddressName = obj.ManagingOrganization.Contragent.FiasFactAddress != null
                                              ? obj.ManagingOrganization.Contragent.FiasFactAddress.AddressName
                                              : string.Empty,
                           obj.ManagingOrganization.Contragent.Phone,
                           obj.ManagingOrganization.Contragent.Fax,
                           obj.ManagingOrganization.Contragent.Email,
                           obj.ManagingOrganization.Contragent.OfficialWebsite,
                           obj.ManagingOrganization.IsDispatchCrrespondedFact,
                           obj.ManagingOrganization.DispatchPhone,
                           obj.ManagingOrganization.DispatchAddress,
						   obj.ManagingOrganization.DispatchFile,
					       obj.ManagingOrganization.NumberEmployees,
                           FioMemberAudit = disclosureInfoService.GetPositionByCode(obj.ManagingOrganization.Contragent.Id, obj.PeriodDi, new List<string> { "5" }),
                           FioMemberManagement = disclosureInfoService.GetPositionByCode(obj.ManagingOrganization.Contragent.Id, obj.PeriodDi, new List<string> { "6" }),
                           obj.AdminResponsibility,
                           obj.TerminateContract,
                           obj.MembershipUnions,
                           obj.FundsInfo,
                           obj.DocumentWithoutFunds,
                           obj.ContractsAvailability,
                           obj.NumberContracts,
                           obj.SizePayments,
                           obj.UnhappyEventCount,
                           obj.DismissedWork,
                           obj.DismissedEngineer,
                           obj.DismissedAdminPersonnel,
                           obj.Work,
                           obj.Engineer,
                           obj.AdminPersonnel,
                           HasLicense = obj.HasLicense == 0 ? YesNoNotSet.NotSet : obj.HasLicense,
                           obj.ManagingOrganization.ShareMo,
                           obj.ManagingOrganization.ShareSf
                       }) : new BaseDataResult();
        }
    }
}