namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class ActionsRemovViolHelper : BindingBase
    {
        /// <summary>
        /// Мероприятия по устранению нарушений
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("ActionsRemovViol"))
                {
                    throw new SpecFlowException("Нет текущего плана отчетного периода");
                }

                var current = ScenarioContext.Current.Get<dynamic>("ActionsRemovViol");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("ActionsRemovViol"))
                {
                    ScenarioContext.Current.Remove("ActionsRemovViol");
                }

                ScenarioContext.Current.Add("ActionsRemovViol", value);
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
