namespace Bars.Gkh.RegOperator.Regions.Tatarstan.PriorityParam
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.PriorityParams;
    using Bars.Gkh.RegOperator.Entities;

    public class PaymentToChargePercentPriorityParam : IPriorityParams
    {
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<RealityObjectChargeAccount> ChargeAccountDomain { get; set; }

        public string Id 
        {
            get { return "PaymentToChargePercentPriorityParam"; }
        }

        public string Name
        {
            get { return "Процент оплаты по отношению к начислениям (%)"; }
        }

        public TypeParam TypeParam 
        {
            get { return TypeParam.Quant; }
        }

        private Dictionary<long, decimal> RoCollections { get; set; }

        public object GetValue(RealityObjectStructuralElementInProgrammStage3 obj)
        {
            if (this.RoCollections == null)
            {
                this.InitRoCollections(obj.RealityObject);
            }

            return this.RoCollections.Get(obj.RealityObject.Id);

        }

        private void InitRoCollections(RealityObject ro)
        {
            var mu = this.RealityObjectDomain.GetAll().Where(x => x.Id == ro.Id).Select(x => x.Municipality).FirstOrDefault();

            this.RoCollections = this.ChargeAccountDomain.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == mu.Id)
                .Where(x => x.Operations.Sum(y => y.ChargedTotal) > 0)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    Collection = x.PaidTotal * 100 / x.Operations.Sum(y => y.ChargedTotal)
                })
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.First().Collection.RoundDecimal(2));
        }
    }
}
