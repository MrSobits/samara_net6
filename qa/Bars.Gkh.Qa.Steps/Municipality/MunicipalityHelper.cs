namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;

    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    class MunicipalityHelper : BindingBase
    {
        /// <summary>
        /// Текущее муниципальное образование
        /// </summary>
        public static Municipality Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentMunicipality"))
                {
                    throw new SpecFlowException("Нет текущего муниципального образования");
                }

                var municipality = ScenarioContext.Current.Get<Municipality>("currentMunicipality");

                return municipality;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentMunicipality"))
                {
                    ScenarioContext.Current.Remove("currentMunicipality");
                }

                ScenarioContext.Current.Add("currentMunicipality", value);
            }
        }
    }
}
