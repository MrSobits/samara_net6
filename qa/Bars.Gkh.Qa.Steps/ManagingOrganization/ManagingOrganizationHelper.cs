namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class ManagingOrganizationHelper : BindingBase
    {
        /// <summary>
        /// Управляющие организации
        /// </summary>
        public static Entities.ManagingOrganization Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("ManagingOrganizationHelper"))
                {
                    throw new SpecFlowException("Нет текущей управляющей организации");
                }

                var current = ScenarioContext.Current.Get<Entities.ManagingOrganization>("ManagingOrganizationHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("ManagingOrganizationHelper"))
                {
                    ScenarioContext.Current.Remove("ManagingOrganizationHelper");
                }

                ScenarioContext.Current.Add("ManagingOrganizationHelper", value);
            }
        }
    }
}
