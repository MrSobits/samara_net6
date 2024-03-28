namespace Bars.Gkh.Qa.Steps
{

    using TechTalk.SpecFlow;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Неподтвержденные начисления
    /// </summary>
    class UnacceptedChargePacketHelper
    {
        static public UnacceptedChargePacket Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("UnacceptedChargePacketHelper"))
                {
                    throw new SpecFlowException("Нет текущего неподтвержденного начисления");
                }

                var current = ScenarioContext.Current.Get<UnacceptedChargePacket>("UnacceptedChargePacketHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("UnacceptedChargePacketHelper"))
                {
                    ScenarioContext.Current.Remove("UnacceptedChargePacketHelper");
                }

                ScenarioContext.Current.Add("UnacceptedChargePacketHelper", value);
            }
        }
    }
}
