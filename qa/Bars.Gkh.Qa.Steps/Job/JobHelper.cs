namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    internal class JobHelper : BindingBase
    {
        /// <summary>
        /// Работа
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("JobHelper"))
                {
                    throw new SpecFlowException("Нет текущей работы");
                }

                var current = ScenarioContext.Current.Get<object>("JobHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("JobHelper"))
                {
                    ScenarioContext.Current.Remove("JobHelper");
                }

                ScenarioContext.Current.Add("JobHelper", value);
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
