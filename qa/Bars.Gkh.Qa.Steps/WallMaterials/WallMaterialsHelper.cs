
namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class WallMaterialsHelper : BindingBase
    {
        static public WallMaterial Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("WallMaterial"))
                {
                    throw new SpecFlowException("Нет текущего материала стен");
                }

                var wallMaterial = ScenarioContext.Current.Get<WallMaterial>("WallMaterial");

                return wallMaterial;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("WallMaterial"))
                {
                    ScenarioContext.Current.Remove("WallMaterial");
                }

                ScenarioContext.Current.Add("WallMaterial", value);
            }
        }
    }
}