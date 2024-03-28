namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;

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
                value.PropertyOwnerProtocol,
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

        public override IDataResult List(IDomainService<SpecialAccountDecision> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    ProtocolNumber = x.PropertyOwnerProtocol.DocumentNumber,
                    ProtocolDate = x.PropertyOwnerProtocol.DocumentDate,
                    x.TypeOrganization,
                    x.AccountNumber,
                    x.OpenDate,
                    x.CloseDate,
                    //OwnerName = x.RegOperator.Contragent.Name ?? x.ManagingOrganization.Contragent.Name,
                    CreditOrgName = x.CreditOrg.Name,
                    DecisionType = x.PropertyOwnerDecisionType.GetEnumMeta().Display
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}