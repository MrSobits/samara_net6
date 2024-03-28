namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class SpecialAccountDecisionViewModel : BaseViewModel<SpecialAccountDecision>
    {
        public override IDataResult Get(IDomainService<SpecialAccountDecision> domainService, BaseParams baseParams)
        {
            var value = domainService.Get(baseParams.Params["id"].To<long>());

            if (value == null)
            {
                return new BaseDataResult(null);
            }

            var res = new
            {
                value.Id,
                PropertyOwnerProtocol = value.PropertyOwnerProtocol.Id,
                RealityObject = value.PropertyOwnerProtocol.RealityObject.Id,
                value.PropertyOwnerDecisionType,
                value.MoOrganizationForm,
                value.MethodFormFund,

                //RegOperator = value.RegOperator != null ? new
                //{
                //    value.RegOperator.Id,
                //    Contragent = value.RegOperator.Contragent.Name
                //} : null,
                ManagingOrganization = value.ManagingOrganization != null ? new
                {
                    value.ManagingOrganization.Id,
                    ContragentName = value.ManagingOrganization.Contragent.Name
                } : null,
                value.AccountNumber,
                value.OpenDate,
                value.CloseDate,
                value.BankHelpFile,
                value.TypeOrganization,
                CreditOrg = value.CreditOrg != null ? new
                {
                    value.CreditOrg.Id,
                    value.CreditOrg.Name,
                    value.CreditOrg.MailingAddress,
                    value.CreditOrg.Inn,
                    value.CreditOrg.Kpp,
                    value.CreditOrg.Ogrn,
                    value.CreditOrg.Okpo,
                    value.CreditOrg.Bik,
                    value.CreditOrg.CorrAccount,
                } : null

            };

            return new BaseDataResult(res);
        }
    }
}