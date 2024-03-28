namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    internal class PersonalAccountOwnerHelper : BindingBase
    {
        /// <summary>
        /// Абонент
        /// </summary>
        public static PersonalAccountOwner Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPersonalAccountOwner"))
                {
                    throw new SpecFlowException("Нет текущего Абонента типа юр лицо");
                }

                var current = ScenarioContext.Current.Get<PersonalAccountOwner>("CurrentPersonalAccountOwner");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPersonalAccountOwner"))
                {
                    ScenarioContext.Current.Remove("CurrentPersonalAccountOwner");
                }

                ScenarioContext.Current.Add("CurrentPersonalAccountOwner", value);
            }
        }
    }
}
