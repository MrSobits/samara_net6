namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;

    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    class MeteringDeviceHelper : BindingBase
    {
        /// <summary>
        /// Текущий прибор учёта
        /// </summary>
        public static MeteringDevice Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentMeteringDevice"))
                {
                    throw new SpecFlowException("Нет текущего прибора учёта");
                }

                return ScenarioContext.Current.Get<MeteringDevice>("currentMeteringDevice");
            }
            set
            {
                ScenarioContext.Current.Add("currentMeteringDevice", value);
            }
        }
    }
}
