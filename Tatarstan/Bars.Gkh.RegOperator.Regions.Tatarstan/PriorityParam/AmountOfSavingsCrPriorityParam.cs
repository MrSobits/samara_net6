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
    using Bars.GkhRf.Entities;

    public class AmountOfSavingsCrPriorityParam : IPriorityParams
    {
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<TransferRfRecObj> TransferRfRecObjDomain { get; set; }

        public IDomainService<RealityObjectChargeAccount> ChargeAccountDomain { get; set; }

        public string Id 
        {
            get { return "AmountOfSavingsCrPriorityParam"; }
        }

        public string Name
        {
            get { return "Сумма накоплений на кап.ремонт (на 1 кв.м.)"; }
        }

        public TypeParam TypeParam 
        {
            get { return TypeParam.Quant; }
        }

        private Dictionary<long, decimal> RoSumPerSquareMeter { get; set; }

        public object GetValue(RealityObjectStructuralElementInProgrammStage3 obj)
        {
            if (RoSumPerSquareMeter == null)
            {
                InitRoSumPerSquareMeter(obj.RealityObject);
            }

            return RoSumPerSquareMeter.Get(obj.RealityObject.Id);

        }

        private void InitRoSumPerSquareMeter(RealityObject ro)
        {
            var mu = RealityObjectDomain.GetAll().Where(x => x.Id == ro.Id).Select(x => x.Municipality).FirstOrDefault();

            var rfTransfers = TransferRfRecObjDomain.GetAll()
            .Where(x => x.TransferRfRecord.State.FinalState)
            .Where(x => x.RealityObject.Municipality.Id == mu.Id)
            .Select(x => new
            {
                x.RealityObject.Id,
                x.Sum
            })
            .AsEnumerable()
            .GroupBy(x => x.Id)
            .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Sum.ToDecimal()));

            RoSumPerSquareMeter = ChargeAccountDomain.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == mu.Id)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.PaidTotal,
                    x.RealityObject.AreaMkd
                })
                .ToList()
                .Select(x => new
                {
                    x.RoId,
                    Sum = x.AreaMkd > 0 ? (x.PaidTotal + rfTransfers.Get(x.RoId)) / x.AreaMkd.ToDecimal() : 0M
                })
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.First().Sum);
        }
    }
}
