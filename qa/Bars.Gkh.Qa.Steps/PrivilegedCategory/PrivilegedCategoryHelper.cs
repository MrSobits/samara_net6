namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using TechTalk.SpecFlow;

    internal class PrivilegedCategoryHelper : BindingBase
    {
        /// <summary>
        /// Группы льготных категорий граждан
        /// </summary>
        public static PrivilegedCategory Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPrivilegedCategory"))
                {
                    throw new SpecFlowException("Отсутствует текущяя группа льготных категорий граждан");
                }

                var current = ScenarioContext.Current.Get<PrivilegedCategory>("CurrentPrivilegedCategory");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPrivilegedCategory"))
                {
                    ScenarioContext.Current.Remove("CurrentPrivilegedCategory");
                }

                ScenarioContext.Current.Add("CurrentPrivilegedCategory", value);
            }
        }
    }
}
