namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class StructuralElementGroupHelper : BindingBase
    {
        static public Entities.CommonEstateObject.StructuralElementGroup Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("StructuralElementGroupHelper"))
                {
                    throw new SpecFlowException("Нет текущей группы конструктивных элементов");
                }

                var current = ScenarioContext.Current.Get<Entities.CommonEstateObject.StructuralElementGroup>("StructuralElementGroupHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("StructuralElementGroupHelper"))
                {
                    ScenarioContext.Current.Remove("StructuralElementGroupHelper");
                }

                ScenarioContext.Current.Add("StructuralElementGroupHelper", value);
            }
        }
    }
}