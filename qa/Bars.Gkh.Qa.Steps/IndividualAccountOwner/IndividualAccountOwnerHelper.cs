namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    internal class IndividualAccountOwnerHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return _type
                       ?? (_type =
                           Type.GetType(
                               "Bars.Gkh.RegOperator.Entities.IndividualAccountOwner, Bars.Gkh.RegOperator"));
            }
        }

        public static dynamic DomainService
        {
            get
            {
                var t = typeof(IDomainService<>).MakeGenericType(Type);
                return Container.Resolve(t);
            }
        }

        /// <summary>
        /// Абонент - физ.лицо
        /// </summary>
        public static IndividualAccountOwner Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("IndibidualAccountOwner"))
                {
                    throw new SpecFlowException("Нет текущего Абонента - физ.лицо");
                }

                var current = ScenarioContext.Current.Get<IndividualAccountOwner>("IndibidualAccountOwner");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("IndibidualAccountOwner"))
                {
                    ScenarioContext.Current.Remove("IndibidualAccountOwner");
                }

                ScenarioContext.Current.Add("IndibidualAccountOwner", value);
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
