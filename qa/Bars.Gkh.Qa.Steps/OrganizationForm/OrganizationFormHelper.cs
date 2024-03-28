namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;

    using Bars.Gkh.Qa.Utils;

    using Bars.Gkh.Entities;

    class OrganizationFormHelper : BindingBase
    {
        /// <summary>
        /// Текущая организационно-правовая форма
        /// </summary>
        public static OrganizationForm CurrentOrganizationForm
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentOrganizationForm"))
                {
                    throw new SpecFlowException("Нет текущей организационно-правовой формы");
                }

                var organizationForm = ScenarioContext.Current.Get<OrganizationForm>("currentOrganizationForm");

                return organizationForm;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentOrganizationForm"))
                {
                    ScenarioContext.Current.Remove("currentOrganizationForm");
                }

                ScenarioContext.Current.Add("currentOrganizationForm", value);
            }
        }
    }
}
