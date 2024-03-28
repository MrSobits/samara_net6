using System.Collections.Generic;
using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class IndividualAccountListHelper : BindingBase
    {

        /// <summary>
        /// Список абонентов - физ.лицо
        /// </summary>
        public static List<IndividualAccountOwner> Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("IndividualAccountListHelper"))
                {
                    throw new SpecFlowException("Нет текущего списка абонентов типа физ.лицо");
                }

                var current = ScenarioContext.Current.Get<List<IndividualAccountOwner>>("IndividualAccountListHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("IndividualAccountListHelper"))
                {
                    ScenarioContext.Current.Remove("IndividualAccountListHelper");
                }

                ScenarioContext.Current.Add("IndividualAccountListHelper", value);
            }
        }
    }
}