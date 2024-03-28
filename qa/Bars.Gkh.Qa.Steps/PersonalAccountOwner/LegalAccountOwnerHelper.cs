namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    internal class LegalAccountOwnerHelper : BindingBase
    {
        /// <summary>
        /// Абонент - юр лицо
        /// </summary>
        public static LegalAccountOwner Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("LegalAccountOwnerHelper"))
                {
                    throw new SpecFlowException("Нет текущего Абонента типа юр лицо");
                }

                var current = ScenarioContext.Current.Get<LegalAccountOwner>("LegalAccountOwnerHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("LegalAccountOwnerHelper"))
                {
                    ScenarioContext.Current.Remove("LegalAccountOwnerHelper");
                }

                ScenarioContext.Current.Add("LegalAccountOwnerHelper", value);
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
