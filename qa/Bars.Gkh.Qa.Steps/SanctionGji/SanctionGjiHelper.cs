namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class SanctionGjiHelper : BindingBase
    {
        /// <summary>
        /// Виды санкций
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("SanctionGji"))
                {
                    throw new SpecFlowException("Нет текущего вида санкций");
                }

                var current = ScenarioContext.Current.Get<object>("SanctionGji");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("SanctionGji"))
                {
                    ScenarioContext.Current.Remove("SanctionGji");
                }

                ScenarioContext.Current.Add("SanctionGji", value);
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
