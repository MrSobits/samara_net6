namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class PeriodicityTemplateServiceHelper : BindingBase
    {
        /// <summary>
        /// Периодичность услуг
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PeriodicityTemplateService"))
                {
                    throw new SpecFlowException("Нет текущей Периодичности услуг");
                }

                var current = ScenarioContext.Current.Get<object>("PeriodicityTemplateService");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PeriodicityTemplateService"))
                {
                    ScenarioContext.Current.Remove("PeriodicityTemplateService");
                }

                ScenarioContext.Current.Add("PeriodicityTemplateService", value);
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
