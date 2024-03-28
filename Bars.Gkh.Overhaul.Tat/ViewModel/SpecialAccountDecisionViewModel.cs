namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class SpecialAccountDecisionViewModel : BaseViewModel<SpecialAccountDecision>
    {
        public override IDataResult Get(IDomainService<SpecialAccountDecision> domainService, BaseParams baseParams)
        {
            var value = domainService.Get(baseParams.Params["id"].To<long>());

            var res = new
            {
                value.Id,
                RealityObject = value.RealityObject.Id,
                PropertyOwnerProtocol = value.PropertyOwnerProtocol.Id,
                value.PropertyOwnerDecisionType,
                value.MoOrganizationForm,
                value.MethodFormFund,
                RegOperator = value.RegOperator != null
                    ? new
                    {
                        value.RegOperator.Id,
                        Contragent = value.RegOperator.Contragent.Name
                    }
                    : null,
                ManagingOrganization = value.ManagingOrganization != null
                    ? new
                    {
                        value.ManagingOrganization.Id,
                        ContragentName = value.ManagingOrganization.Contragent.Name
                    }
                    : null,
                value.AccountNumber,
                value.OpenDate,
                value.CloseDate,
                value.BankHelpFile,
                value.TypeOrganization,
                value.MailingAddress,
                value.Inn,
                value.Kpp,
                value.Ogrn,
                value.Okpo,
                value.Bik,
                value.CorrAccount,
                CreditOrg = value.CreditOrg != null
                    ? new
                    {
                        value.CreditOrg.Id,
                        value.CreditOrg.Name
                    }
                    : null
            };

            return new BaseDataResult(res);
        }
    }
}