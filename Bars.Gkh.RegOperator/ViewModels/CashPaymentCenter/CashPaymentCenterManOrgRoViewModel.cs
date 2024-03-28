namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;

    public class CashPaymentCenterManOrgRoViewModel : BaseViewModel<CashPaymentCenterManOrgRo>
    {
        public override IDataResult List(IDomainService<CashPaymentCenterManOrgRo> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var cpcManOrgId = loadParams.Filter.GetAs<long>("cpcManOrgId");

            if (cpcManOrgId == 0)
            {
                return new BaseDataResult(false, "Не удалось получить дома УК");
            }

            var data = domain.GetAll()
                .Where(x => x.CashPaymentCenterManOrg.Id == cpcManOrgId)
                .Select(x => new
                {
                    x.Id, 
                    RealityObject = x.RealityObject.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    x.DateStart,
                    x.DateEnd
                })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Address);
            
            return new ListDataResult(data.ToList(), data.Count());
        }
    }
}