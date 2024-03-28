namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    public class NormativeDocItemHelper : BindingBase
    {
        static public NormativeDocItem Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("NormativeDocItem"))
                {
                    throw new SpecFlowException("Нет текущего пункта нормативного документа");
                }

                var normativeDocItem = ScenarioContext.Current.Get<NormativeDocItem>("NormativeDocItem");

                return normativeDocItem;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("NormativeDocItem"))
                {
                    ScenarioContext.Current.Remove("NormativeDocItem");
                }

                ScenarioContext.Current.Add("NormativeDocItem", value);
            }
        }
    }
}
