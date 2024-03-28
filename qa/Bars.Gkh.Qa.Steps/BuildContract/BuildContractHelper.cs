namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhCr.Entities;

    using TechTalk.SpecFlow;

    internal class BuildContractHelper : BindingBase
    {
        /// <summary>
        /// Текущий Договор подряда КР
        /// </summary>
        public static BuildContract Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentBuildContract"))
                {
                    throw new SpecFlowException("Нет текущего договора подряда КР");
                }

                var current = ScenarioContext.Current.Get<BuildContract>("currentBuildContract");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentBuildContract"))
                {
                    ScenarioContext.Current.Remove("currentBuildContract");
                }

                ScenarioContext.Current.Add("currentBuildContract", value);
            }
        }
    }
}
