
namespace Bars.Gkh.Decisions.Nso.Entities.Decisions
{
    using System;
    using System.Linq;
    using B4;
    using Castle.Windsor;
    using Overhaul.Entities;
    using Overhaul.Enum;

    public class PaymentAndFundDecisions
    {
        public void Init(IWindsorContainer container, long municipalityId)
        {
            var psDomain = container.Resolve<IDomainService<PaysizeRecord>>();

            MinFundPaymentSize =
                (from z in (psDomain.GetAll()
                    .Where(
                        v =>
                            ((v.Paysize.DateStart <= DateTime.Now && v.Paysize.DateEnd >= DateTime.Now) ||
                             v.Paysize.DateEnd == null) &&
                            v.Municipality.Id == municipalityId &&
                            v.Paysize.Indicator == TypeIndicator.MinSizeSqMetLivinSpace)
                    .Select(v => new {v.Value, DateEnd = v.Paysize.DateEnd ?? DateTime.MaxValue})
                    ).ToArray()
                    orderby z.DateEnd ascending
                    select z.Value
                    ).FirstOrDefault();

            MinFundSize = (from z in (psDomain.GetAll()
                .Where(
                    v =>
                        ((v.Paysize.DateEnd >= DateTime.Now && v.Paysize.DateStart >= DateTime.Now) ||
                         v.Paysize.DateEnd == null) &&
                        v.Municipality.Id == municipalityId &&
                        v.Paysize.Indicator == TypeIndicator.MinSizePercentOfCostRestoration)
                .Select(v => new {v.Value, DateEnd = v.Paysize.DateEnd ?? DateTime.MaxValue})
                ).ToArray()
                orderby z.DateEnd ascending
                select z.Value
                ).FirstOrDefault();
        }

        public decimal? MinFundPaymentSize { get; set; }

        public decimal? MinFundSize { get; set; }

    }
}
