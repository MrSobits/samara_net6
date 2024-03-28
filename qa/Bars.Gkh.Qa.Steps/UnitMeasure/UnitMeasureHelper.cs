namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;

    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;
    
    class UnitMeasureHelper : BindingBase
    {
        /// <summary>
        /// Текущая единица измерения
        /// </summary>
        public static UnitMeasure Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentUnitMeasure"))
                {
                    throw new SpecFlowException("Нет текущей единицы измерения");
                }

                var currentUnitMeasure = ScenarioContext.Current.Get<UnitMeasure>("CurrentUnitMeasure");

                return currentUnitMeasure;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentUnitMeasure"))
                {
                    ScenarioContext.Current.Remove("CurrentUnitMeasure");
                }

                ScenarioContext.Current.Add("CurrentUnitMeasure", value);
            }
        }
    }
}
