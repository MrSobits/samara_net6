namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Справочник заявлений в суд
    /// </summary>
    public static class PetitionToCourtTypeHelper
    {
        public static PetitionToCourtType Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PetitionToCourtTypeHelper"))
                {
                    throw new SpecFlowException("Отсутствует текуще заявление в суд");
                }

                var current = ScenarioContext.Current.Get<PetitionToCourtType>("PetitionToCourtTypeHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PetitionToCourtTypeHelper"))
                {
                    ScenarioContext.Current.Remove("PetitionToCourtTypeHelper");
                }

                ScenarioContext.Current.Add("PetitionToCourtTypeHelper", value);
            }
        }
    }
}
