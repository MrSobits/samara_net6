namespace Bars.Gkh.Qa.Steps
{
    using System.Collections.Generic;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using TechTalk.SpecFlow;

    public class PaymentPenaltiesListHelper : BindingBase
    {
        public static List<PaymentPenalties> PaymentPenaltiesList
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PaymentPenaltiesList"))
                {
                    throw new SpecFlowException("Список параметров расчета пеней пуст");
                }

                var list = ScenarioContext.Current.Get<List<PaymentPenalties>>("PaymentPenaltiesList");

                return list;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PaymentPenaltiesList"))
                {
                    ScenarioContext.Current.Remove("PaymentPenaltiesList");
                }

                ScenarioContext.Current.Add("PaymentPenaltiesList", value);
            }
        }
    }
}
