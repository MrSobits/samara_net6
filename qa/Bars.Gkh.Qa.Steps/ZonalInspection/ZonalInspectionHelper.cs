namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;

    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    class ZonalInspectionHelper : BindingBase
    {
        /// <summary>
        /// Текущяя зонально жилищная инспекция
        /// </summary>
        public static ZonalInspection Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentZonalInspection"))
                {
                    throw new SpecFlowException("Нет текущей зонально жилищной инспекции");
                }

                return ScenarioContext.Current.Get<ZonalInspection>("currentZonalInspection");
            }
            set
            {
                if (ScenarioContext.Current.ContainsKey("currentZonalInspection"))
                {
                    ScenarioContext.Current.Remove("currentZonalInspection");
                }

                ScenarioContext.Current.Add("currentZonalInspection", value);
            }
        }
    }
}
