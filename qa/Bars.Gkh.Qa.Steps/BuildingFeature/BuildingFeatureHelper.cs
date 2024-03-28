namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class BuildingFeatureHelper : BindingBase
    {
        /// <summary>
        /// Текущий особый признак строения
        /// </summary>
        public static BuildingFeature Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentBuildingFeature"))
                {
                    throw new SpecFlowException("Нет текущего особого признака строения");
                }

                var buildingFeature = ScenarioContext.Current.Get<BuildingFeature>("currentBuildingFeature");

                return buildingFeature;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentBuildingFeature"))
                {
                    ScenarioContext.Current.Remove("currentBuildingFeature");
                }

                ScenarioContext.Current.Add("currentBuildingFeature", value);
            }
        }
    }
}
