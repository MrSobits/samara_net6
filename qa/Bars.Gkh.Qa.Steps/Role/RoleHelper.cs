namespace Bars.Gkh.Qa.Steps
{
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    public class RoleHelper : BindingBase
    {
        static public Role Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("Role"))
                {
                    throw new SpecFlowException("Нет текущей роли");
                }

                var current = ScenarioContext.Current.Get<Role>("Role");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("Role"))
                {
                    ScenarioContext.Current.Remove("Role");
                }

                ScenarioContext.Current.Add("Role", value);
            }
        }
    }
}