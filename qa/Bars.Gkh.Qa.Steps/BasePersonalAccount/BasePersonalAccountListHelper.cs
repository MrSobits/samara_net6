using System.Collections.Generic;
using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class BasePersonalAccountListHelper : BindingBase
    {

        /// <summary>
        /// Список лицевых счетов
        /// </summary>
        public static List<BasePersonalAccount> Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("BasePersonalAccountListHelper"))
                {
                    var newList = new List<BasePersonalAccount>();
                    ScenarioContext.Current.Add("BasePersonalAccountListHelper", newList);
                    return newList;
                }

                var current = ScenarioContext.Current.Get<List<BasePersonalAccount>>("BasePersonalAccountListHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("BasePersonalAccountListHelper"))
                {
                    ScenarioContext.Current.Remove("BasePersonalAccountListHelper");
                }

                ScenarioContext.Current.Add("BasePersonalAccountListHelper", value);
            }
        }

        public static string GetIds()
        {
            return string.Join(", ", Current.Select(x => x.Id));
        }
    }
}