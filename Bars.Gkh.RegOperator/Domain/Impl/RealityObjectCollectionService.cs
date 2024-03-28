namespace Bars.Gkh.RegOperator.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    public class RealityObjectCollectionService : IRealityObjectCollectionService
    {
        public IDomainService<RealityObjectChargeAccountOperation> RoChargeAccountOperation { get; set; }

        public Dictionary<long, decimal> FillRealityObjectCollectionDictionary()
        {
            // Вычитаем год
            var startDate = DateTime.Now.AddYears(-1);

            var roCharges = RoChargeAccountOperation.GetAll()
                .Where(x => x.Period.StartDate > startDate)
                .Select(x => new { RoId = x.Account.RealityObject.Id, x.ChargedTotal, x.PaidTotal })
                .ToList();

            var result = roCharges.GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    x =>
                        {
                            var paid = x.SafeSum(y => y.PaidTotal);
                            var charged = x.SafeSum(y => y.ChargedTotal);

                            if (charged != 0)
                            {
                                return paid / charged;
                            }
                            return 0;
                        }
                );

            return result;
        }
    }
}