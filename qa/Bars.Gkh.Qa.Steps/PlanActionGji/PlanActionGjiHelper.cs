namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;
    using Bars.GkhGji.Entities;

    using TechTalk.SpecFlow;

    internal class PlanActionGjiHelper : BindingBase
    {
        /// <summary>
        /// Мероприятия по устранению нарушений
        /// </summary>
        public static PlanActionGji Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PlanActionGji"))
                {
                    throw new SpecFlowException("Нет текущего плана мероприятия");
                }

                var current = ScenarioContext.Current.Get<PlanActionGji>("PlanActionGji");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PlanActionGji"))
                {
                    ScenarioContext.Current.Remove("PlanActionGji");
                }

                ScenarioContext.Current.Add("PlanActionGji", value);
            }
        }
    }
}
