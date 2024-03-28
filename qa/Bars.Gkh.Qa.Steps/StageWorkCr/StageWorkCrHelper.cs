namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class StageWorkCrHelper : BindingBase
    {
        /// <summary>
        /// Этапы работы
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("StageWorkCr"))
                {
                    throw new SpecFlowException("Нет текущего этапа работы");
                }

                var current = ScenarioContext.Current.Get<object>("StageWorkCr");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("StageWorkCr"))
                {
                    ScenarioContext.Current.Remove("StageWorkCr");
                }

                ScenarioContext.Current.Add("StageWorkCr", value);
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
