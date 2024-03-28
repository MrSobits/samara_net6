namespace Bars.Gkh.Qa.Steps
{
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class TaskEntryHelper : BindingBase
    {
        /// <summary>
        /// Текущая задача
        /// </summary>
        public static TaskEntry Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentTaskEntry"))
                {
                    throw new SpecFlowException("Нет текущей задачи");
                }

                var currentTaskEntry = ScenarioContext.Current.Get<TaskEntry>("CurrentTaskEntry");

                return currentTaskEntry;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentTaskEntry"))
                {
                    ScenarioContext.Current.Remove("CurrentTaskEntry");
                }

                ScenarioContext.Current.Add("CurrentTaskEntry", value);
            }
        }
    }
}
