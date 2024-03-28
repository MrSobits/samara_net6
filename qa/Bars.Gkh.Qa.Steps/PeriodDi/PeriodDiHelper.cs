namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class PeriodDiHelper : BindingBase
    {
        /// <summary>
        /// текущий план отчетного периода
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PeriodDi"))
                {
                    throw new SpecFlowException("Нет текущего плана отчетного периода");
                }

                var current = ScenarioContext.Current.Get<object>("PeriodDi");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PeriodDi"))
                {
                    ScenarioContext.Current.Remove("PeriodDi");
                }

                ScenarioContext.Current.Add("PeriodDi", value);
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
