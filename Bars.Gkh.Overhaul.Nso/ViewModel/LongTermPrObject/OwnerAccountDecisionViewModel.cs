namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class OwnerAccountDecisionViewModel : BaseViewModel<OwnerAccountDecision>
    {
        public override IDataResult Get(IDomainService<OwnerAccountDecision> domainService, BaseParams baseParams)
        {
            var value = domainService.Get(baseParams.Params["id"].To<long>());

            if (value == null)
            {
                return new BaseDataResult(null);
            }

            var res = new
            {
                value.Id,
                RealityObject = value.RealityObject.Id,
                value.PropertyOwnerProtocol,
                value.PropertyOwnerDecisionType,
                value.MoOrganizationForm,
                value.MethodFormFund,
                value.OwnerAccountType,
                value.DateEnd,
                value.DateStart,
                Contragent = value.Contragent != null
                    ? new { value.Contragent.Id, value.Contragent.Name }
                    : null
            };

            return new BaseDataResult(res);
        }
    }
}