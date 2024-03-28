namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Services.DataContracts.GetCreditOpPayment;
    using Bars.Gkh.Services.DataContracts;

    public partial class Service
    {
        /// <summary>
        /// Получить оплаты КР
        /// </summary>
        /// <param name="roId"></param>
        /// <returns></returns>
        public CreditOpPaymentResponse GetCreditOpPayment(string roId)
        {
            var id = roId.ToLong();

            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            var roChargeAccountDomain = this.Container.ResolveDomain<RealityObjectChargeAccount>();
            var roChargeAccountOpDomain = this.Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var typeOfFormingCrProvider = this.Container.Resolve<IRealityObjectDecisionProtocolProxyService>();

            TotalPayments totalPayments;
            Payment[] payments;
            MethodOfForming methodOfForming;

            try
            {
                totalPayments = roChargeAccountDomain.GetAll()
                    .Where(x => x.RealityObject.Id == id)
                    .Select(
                        x => new TotalPayments()
                        {
                            Id = x.Id,
                            TotalCredit = x.Operations.Sum(y => y.ChargedTotal).RoundDecimal(2),
                            TotalPaid = x.Operations.Sum(y => y.PaidTotal + y.PaidPenalty).RoundDecimal(2)
                        }).FirstOrDefault();

                if (totalPayments == null)
                {
                    return new CreditOpPaymentResponse { Result = Result.DataNotFound };
                }

                payments = roChargeAccountOpDomain.GetAll()
                    .Where(x => x.Account.RealityObject.Id == id)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Period.StartDate,
                            x.ChargedTotal,
                            x.PaidTotal
                        })
                    .AsEnumerable()
                    .Select(
                        x => new Payment()
                        {
                            Id = x.Id,
                            Date = x.StartDate,
                            Credit = x.ChargedTotal.RoundDecimal(2),
                            Paid = x.PaidTotal.RoundDecimal(2),
                            ManOrg = this.GetManOrg(id, x.StartDate)
                        }).ToArray();

                var realObj = realityObjectDomain.Get(id);

                methodOfForming = new MethodOfForming()
                {
                    MethodOfFormingOverhaulFund = typeOfFormingCrProvider.GetTypeOfFormingCr(realObj).GetAttribute<DisplayAttribute>().Value,
                };

                var bothProtocolProxy = typeOfFormingCrProvider?.GetBothProtocolProxy(realObj);
                if (bothProtocolProxy != null)
                {
                    methodOfForming.СommonMeetingProtocolDate = bothProtocolProxy.ProtocolDate;
                    methodOfForming.CommonMeetingProtocolNumber = bothProtocolProxy.ProtocolNumber;
                }
            }
            finally
            {
                this.Container.Release(realityObjectDomain);
                this.Container.Release(roChargeAccountDomain);
                this.Container.Release(roChargeAccountOpDomain);
                this.Container.Release(typeOfFormingCrProvider);
            }

            return new CreditOpPaymentResponse
            {
                TotalPayments = totalPayments,
                Payments = payments,
                MethodOfForming = methodOfForming,
                Result = Result.NoErrors
            };

        }

        private string GetManOrg(long roId, DateTime date)
        {
            var manOrgContrRealObjDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();
            var manOrgContrDomain = this.Container.ResolveDomain<ManOrgBaseContract>();

            try
            {
                var manOrgContrRealObjs = manOrgContrRealObjDomain.GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Select(x => x.ManOrgContract.Id)
                    .ToList();

                var manOrg = manOrgContrDomain.GetAll()
                    .Where(x => manOrgContrRealObjs.Contains(x.Id))
                    .Where(x => x.StartDate <= date && (x.EndDate >= date || !x.EndDate.HasValue))
                    .OrderByDescending(x => x.StartDate)
                    .Select(x => x.ManagingOrganization.Contragent.Name)
                    .FirstOrDefault();

                return manOrg;
            }
            finally
            {
                this.Container.Release(manOrgContrRealObjDomain);
                this.Container.Release(manOrgContrDomain);
            }

        }
    }
}
