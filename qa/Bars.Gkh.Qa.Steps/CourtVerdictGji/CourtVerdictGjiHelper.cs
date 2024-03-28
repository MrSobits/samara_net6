namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class CourtVerdictGjiHelper : BindingBase
    {
        /// <summary>
        /// Решения суда
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CourtVerdictGji"))
                {
                    throw new SpecFlowException("Нет текущего решения суда");
                }

                var current = ScenarioContext.Current.Get<object>("CourtVerdictGji");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CourtVerdictGji"))
                {
                    ScenarioContext.Current.Remove("CourtVerdictGji");
                }

                ScenarioContext.Current.Add("CourtVerdictGji", value);
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
