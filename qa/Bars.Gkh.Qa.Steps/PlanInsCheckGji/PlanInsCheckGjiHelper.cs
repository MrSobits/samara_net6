namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;
    using Bars.GkhGji.Entities;

    using TechTalk.SpecFlow;

    internal class PlanInsCheckGjiHelper : BindingBase
    {
        /// <summary>
        /// Текущий план инспекционных проверок
        /// </summary>
        public static PlanInsCheckGji Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPlanInsCheckGji"))
                {
                    throw new SpecFlowException("Нет текущего плана инспекционных проверок");
                }

                var licenseProvidedDoc = ScenarioContext.Current.Get<PlanInsCheckGji>("CurrentPlanInsCheckGji");

                return licenseProvidedDoc;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPlanInsCheckGji"))
                {
                    ScenarioContext.Current.Remove("CurrentPlanInsCheckGji");
                }

                ScenarioContext.Current.Add("CurrentPlanInsCheckGji", value);
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
