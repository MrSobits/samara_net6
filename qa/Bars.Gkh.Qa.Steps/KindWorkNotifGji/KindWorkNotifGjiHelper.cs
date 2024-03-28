namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class KindWorkNotifGjiHelper : BindingBase
    {
        /// <summary>
        /// текущий вид работ(уведомления)
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentKindWorkNotifGji"))
                {
                    throw new SpecFlowException("Нет текущего вида работ(уведомления)");
                }

                var licenseProvidedDoc = ScenarioContext.Current.Get<object>("CurrentKindWorkNotifGji");

                return licenseProvidedDoc;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentKindWorkNotifGji"))
                {
                    ScenarioContext.Current.Remove("CurrentKindWorkNotifGji");
                }

                ScenarioContext.Current.Add("CurrentKindWorkNotifGji", value);
            }
        }

        public static void ChangeCurrent(string property, object value)
        {
            Type t = KindWorkNotifGjiHelper.Current.GetType();

            var prop = t.GetProperty(property);

            if (prop != null)
            {
                prop.SetValue(KindWorkNotifGjiHelper.Current, value);
            }
        }
    }
}
