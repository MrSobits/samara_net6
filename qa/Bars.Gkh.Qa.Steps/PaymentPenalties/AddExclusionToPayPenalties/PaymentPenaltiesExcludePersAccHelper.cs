namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using TechTalk.SpecFlow;

    public class PaymentPenaltiesExcludePersAccHelper : BindingBase
    {
        public static PaymentPenaltiesExcludePersAcc Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PaymentPenaltiesExcludePersAccHelper"))
                {
                    throw new SpecFlowException("Нет текущего исключения расчета пеней");
                }

                var current = ScenarioContext.Current.Get<PaymentPenaltiesExcludePersAcc>("PaymentPenaltiesExcludePersAccHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PaymentPenaltiesExcludePersAccHelper"))
                {
                    ScenarioContext.Current.Remove("PaymentPenaltiesExcludePersAccHelper");
                }

                ScenarioContext.Current.Add("PaymentPenaltiesExcludePersAccHelper", value);
            }
        }
    }
}
