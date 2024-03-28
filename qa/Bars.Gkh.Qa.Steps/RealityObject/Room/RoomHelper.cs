namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class RoomHelper : BindingBase
    {
        /// <summary>
        /// текущее помешщение
        /// </summary>
        public static Room Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentRoom"))
                {
                    throw new SpecFlowException("Отсутствует текущее помещение");
                }

                var room = ScenarioContext.Current.Get<Room>("CurrentRoom");

                return room;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentRoom"))
                {
                    ScenarioContext.Current.Remove("CurrentRoom");
                }

                ScenarioContext.Current.Add("CurrentRoom", value);
            }
        }
    }
}
