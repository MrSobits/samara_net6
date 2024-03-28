namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities;
    using TechTalk.SpecFlow;

    class TypeProjectHelper
    {
        static public TypeProject Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("TypeProject"))
                {
                    throw new SpecFlowException("Нет текущего типа проекта");
                }

                var typeProject = ScenarioContext.Current.Get<TypeProject>("TypeProject");

                return typeProject;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("TypeProject"))
                {
                    ScenarioContext.Current.Remove("TypeProject");
                }

                ScenarioContext.Current.Add("TypeProject", value);
            }
        }
    }
}
