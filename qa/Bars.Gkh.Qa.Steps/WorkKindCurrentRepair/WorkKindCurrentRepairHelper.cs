namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class WorkKindCurrentRepairHelper : BindingBase
    {
        /// <summary>
        /// Текущий вид работ текущего ремонта
        /// </summary>
        public static WorkKindCurrentRepair Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentWorkKindCurrentRepair"))
                {
                    throw new SpecFlowException("Нет текущего вида работ текущего ремонта");
                }

                var workKindCurrentRepair = ScenarioContext.Current.Get<WorkKindCurrentRepair>("CurrentWorkKindCurrentRepair");

                return workKindCurrentRepair;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentWorkKindCurrentRepair"))
                {
                    ScenarioContext.Current.Remove("CurrentWorkKindCurrentRepair");
                }

                ScenarioContext.Current.Add("CurrentWorkKindCurrentRepair", value);
            }
        }
    }
}
