namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;
    using Bars.GkhGji.Entities;

    using TechTalk.SpecFlow;

    internal class PlanJurPersonGjiHelper : BindingBase
    {
        /// <summary>
        /// текущий план проверок юридических лиц
        /// </summary>
        public static PlanJurPersonGji Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPlanJurPersonGji"))
                {
                    throw new SpecFlowException("Нет текущего плана проверок юридических лиц");
                }

                var licenseProvidedDoc = ScenarioContext.Current.Get<PlanJurPersonGji>("CurrentPlanJurPersonGji");

                return licenseProvidedDoc;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPlanJurPersonGji"))
                {
                    ScenarioContext.Current.Remove("CurrentPlanJurPersonGji");
                }

                ScenarioContext.Current.Add("CurrentPlanJurPersonGji", value);
            }
        }

        public static void ChangeCurrent(string property, object value)
        {
            Type t = Current.GetType();

            PropertyInfo prop = t.GetProperty(property);

            if (prop != null)
            {
                prop.SetValue(Current, value);
            }
        }

        public static object GetPropertyValue(string property)
        {
            Type entityType = Current.GetType();

            return entityType.GetProperty(property).GetValue(Current);
        }
    }
}
