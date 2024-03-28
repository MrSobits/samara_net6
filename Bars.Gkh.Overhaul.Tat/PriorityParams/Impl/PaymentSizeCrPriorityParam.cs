namespace Bars.Gkh.Overhaul.Tat.PriorityParams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Utils;

    public class PaymentSizeCrPriorityParam : IPriorityParams, IQualitPriorityParam
    {
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<MinAmountDecision> MinAmountDecisionDomain { get; set; }

        public IDomainService<PaymentSizeMuRecord> PaymentSizeMuRecordDomain { get; set; }

        public string Id 
        {
            get { return "PaymentSizeCrPriorityParam"; }
        }

        public Type EnumType
        {
            get
            {
                return typeof(PaymentSizeCrType);
            }
        }

        public string Name
        {
            get { return "Размер взноса на кап.ремонт"; }
        }

        public TypeParam TypeParam 
        {
            get { return TypeParam.Qualit; }
        }

        // дому у которых размер взноса выше минимального
        private HashSet<long> RealObjWithOverPaySizeIds { get; set; }

        public object GetValue(RealityObjectStructuralElementInProgrammStage3 obj)
        {
            if (RealObjWithOverPaySizeIds == null)
            {
                InitRealObjWithOverPaySize(obj.RealityObject);
            }

           return RealObjWithOverPaySizeIds.Contains(obj.RealityObject.Id)
                ? PaymentSizeCrType.OverMinSize
                : PaymentSizeCrType.NoMoreMinSize;

        }

        private void InitRealObjWithOverPaySize(RealityObject realityObject)
        {
            var date = DateTime.Now;
            var mu = RealityObjectDomain.GetAll().Where(x => x.Id == realityObject.Id).Select(x => x.Municipality).FirstOrDefault();

            var paySize = GetMunicipalityPaymentSize(mu, date);

            RealObjWithOverPaySizeIds = MinAmountDecisionDomain.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == mu.Id)
                .Where(x => x.PaymentDateStart <= date && (!x.PaymentDateEnd.HasValue || x.PaymentDateEnd >= date))
                .Select(x => new
                {
                    x.PropertyOwnerProtocol.DocumentDate,
                    x.SizeOfPaymentOwners,
                    x.RealityObject.Id
                })
                .ToList()
                .GroupBy(x => x.Id).Select(x => new
                {
                    x.Key,
                    x.OrderByDescending(y => y.DocumentDate).First().SizeOfPaymentOwners
                })
                .Where(x => x.SizeOfPaymentOwners > paySize)
                .Select(x => x.Key)
                .ToHashSet();
        }

        private decimal GetMunicipalityPaymentSize(Municipality mu, DateTime date)
        {
            return PaymentSizeMuRecordDomain.GetAll()
                    .Where(x => x.PaymentSizeCr.DateStartPeriod <= date
                        && (!x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod >= date))
                    .Where(x => x.PaymentSizeCr.TypeIndicator == TypeIndicator.MinSizeSqMetLivinSpace)
                    .Where(x => mu.Id == x.Municipality.Id)
                    .Select(x => x.PaymentSizeCr.PaymentSize)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();
        }
    }
}
