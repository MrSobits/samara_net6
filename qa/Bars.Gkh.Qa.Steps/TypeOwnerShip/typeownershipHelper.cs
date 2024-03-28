namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    class TypeOwnershipHelper : BindingBase
    {
        public static TypeOwnership Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentTypeOwnerShip"))
                {
                    throw new SpecFlowException("Нет текущей формы собственности");
                }

                var typeOwnership = ScenarioContext.Current.Get<TypeOwnership>("CurrentTypeOwnerShip");

                return typeOwnership;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentTypeOwnerShip"))
                {
                    ScenarioContext.Current.Remove("CurrentTypeOwnerShip");
                }

                ScenarioContext.Current.Add("CurrentTypeOwnerShip", value);
            }
        }
    }
    
}
