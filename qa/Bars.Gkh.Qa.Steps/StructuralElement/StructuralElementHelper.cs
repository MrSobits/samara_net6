namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class StructuralElementHelper : BindingBase
    {
        static public Entities.CommonEstateObject.StructuralElement Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("StructuralElementHelper"))
                {
                    throw new SpecFlowException("Нет текущего конструктивного элемента");
                }

                var current = ScenarioContext.Current.Get<Entities.CommonEstateObject.StructuralElement>("StructuralElementHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("StructuralElementHelper"))
                {
                    ScenarioContext.Current.Remove("StructuralElementHelper");
                }

                ScenarioContext.Current.Add("StructuralElementHelper", value);
            }
        }
    }
}