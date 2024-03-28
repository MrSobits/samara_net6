namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class RegOpAccountDecisionViewModel  : BaseViewModel<RegOpAccountDecision>
    {
        public override IDataResult Get(IDomainService<RegOpAccountDecision> domainService, BaseParams baseParams)
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
                PropertyOwnerProtocol = value.PropertyOwnerProtocol.Id,
                value.PropertyOwnerDecisionType,
                value.MoOrganizationForm,
                value.MethodFormFund,
                RegOperator = value.RegOperator != null ? new
                {
                    value.RegOperator.Id,
                    Contragent = value.RegOperator.Contragent.Name
                } : null
            };

            return new BaseDataResult(res);
        }
    }
}