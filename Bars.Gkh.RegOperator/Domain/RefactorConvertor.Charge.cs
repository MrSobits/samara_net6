namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Entities;
    using Entities.ValueObjects;
    using Gkh.Domain;
    using NHibernate.Linq;
    using Repository;
    using ValueObjects;

    public partial class RefactorConvertor
    {
        public int ProcessCharges()
        {
            var chargesDomain = this.container.ResolveDomain<PersonalAccountCharge>();
            var operations = this.container.ResolveRepository<MoneyOperation>().GetAll();
            var period = this.container.Resolve<IChargePeriodRepository>().GetCurrentPeriod();

            int take = 10000;

            var count = 0;

            while (true)
            {
                var charges = chargesDomain.GetAll()
                    .Where(x => x.IsFixed)
                    .Where(x => operations.All(s => s.OriginatorGuid != x.Guid))
                    .Take(take)
                    .Fetch(x => x.BasePersonalAccount)
                    .ThenFetch(x => x.BaseTariffWallet)
                    .Fetch(x => x.BasePersonalAccount)
                    .ThenFetch(x => x.DecisionTariffWallet)
                    .Fetch(x => x.BasePersonalAccount)
                    .ThenFetch(x => x.PenaltyWallet)
                    .ToList();

                if (charges.Any())
                {
                    count += this.ProcessChargesPortion(charges, period);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        private int ProcessChargesPortion(IEnumerable<PersonalAccountCharge> charges, ChargePeriod period)
        {
            return 0;
        }
    }
}