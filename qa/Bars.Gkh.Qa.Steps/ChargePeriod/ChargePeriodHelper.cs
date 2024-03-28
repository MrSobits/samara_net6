using Bars.Gkh.RegOperator.Enums;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities.Period;

    using TechTalk.SpecFlow;

    internal class ChargePeriodHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return typeof(PeriodCloseCheckResult);
            }
        }

        public static IDomainService<PeriodCloseCheckResult> DomainService
        {
            get
            {
                return Container.Resolve<IDomainService<PeriodCloseCheckResult>>();
            }
        }

        /// <summary>
        /// Период
        /// </summary>
        public static PeriodCloseCheckResult Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentChargePeriod"))
                {
                    throw new SpecFlowException("Отсутствует текущий Период");
                }

                var current = ScenarioContext.Current.Get<PeriodCloseCheckResult>("CurrentChargePeriod");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentChargePeriod"))
                {
                    ScenarioContext.Current.Remove("CurrentChargePeriod");
                }

                ScenarioContext.Current.Add("CurrentChargePeriod", value);
            }
        }

        public static PeriodCloseCheckResult GetOpenPeriod()
        {
            var openPeriod =
                Container.Resolve<IDomainService<PeriodCloseCheckResult>>().GetAll().FirstOrDefault(x => x.CheckState != PeriodCloseCheckStateType.Pending);

            if (openPeriod == null)
            {
                throw new SpecFlowException("Отсутствует открытый период");
            }

            return openPeriod;
        }
    }
}
