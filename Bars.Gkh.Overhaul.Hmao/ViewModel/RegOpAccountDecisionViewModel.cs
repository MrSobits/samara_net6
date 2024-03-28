namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class RegOpAccountDecisionViewModel  : BaseViewModel<RegOpAccountDecision>
    {
        public override IDataResult Get(IDomainService<RegOpAccountDecision> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params["id"].To<long>();
            var value = domainService.GetAll().FirstOrDefault(x => x.Id == id);

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
                //RegOperator = value.RegOperator != null ? new
                //{
                //    Id = value.RegOperator.Id,
                //    Contragent = value.RegOperator.Contragent.Name
                //} : null
            };

            return new BaseDataResult(res);
        }
    }
}