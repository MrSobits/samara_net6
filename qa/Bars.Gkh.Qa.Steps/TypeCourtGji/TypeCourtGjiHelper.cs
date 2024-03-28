namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class TypeCourtGjiHelper : BindingBase
    {
        /// <summary>
        /// виды судов ГЖИ
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("TypeCourtGji"))
                {
                    throw new SpecFlowException("Нет текущего вида суда гжи");
                }

                var current = ScenarioContext.Current.Get<object>("TypeCourtGji");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("TypeCourtGji"))
                {
                    ScenarioContext.Current.Remove("TypeCourtGji");
                }

                ScenarioContext.Current.Add("TypeCourtGji", value);
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
