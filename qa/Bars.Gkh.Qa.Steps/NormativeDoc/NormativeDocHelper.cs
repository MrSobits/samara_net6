namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class NormativeDocHelper : BindingBase
    {
        static public NormativeDoc Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("NormativeDoc"))
                {
                    throw new SpecFlowException("Нет текущего нормативного документа");
                }

                var normativeDoc = ScenarioContext.Current.Get<NormativeDoc>("NormativeDoc");

                return normativeDoc;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("NormativeDoc"))
                {
                    ScenarioContext.Current.Remove("NormativeDoc");
                }

                ScenarioContext.Current.Add("NormativeDoc", value);
            }
        }
    }
}
