using Bars.Gkh.Entities;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.RoofingMaterials
{
    class RoofingMaterialsHelper
    {
        static public RoofingMaterial Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("RoofingMaterial"))
                {
                    throw new SpecFlowException("Нет текущего материала крыши");
                }

                var roofingMaterial = ScenarioContext.Current.Get<RoofingMaterial>("RoofingMaterial");

                return roofingMaterial;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("RoofingMaterial"))
                {
                    ScenarioContext.Current.Remove("RoofingMaterial");
                }

                ScenarioContext.Current.Add("RoofingMaterial", value);
            }
        }
    }
}
