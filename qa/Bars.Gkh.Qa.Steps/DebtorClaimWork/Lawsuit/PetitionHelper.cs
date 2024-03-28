namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class PetitionHelper : BindingBase
    {
        /// <summary>
        /// Текущее Исковое Заявление
        /// </summary>
        public static Petition Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPetition"))
                {
                    throw new SpecFlowException("Отсутствует текущее Исковое Заявление");
                }

                var document = ScenarioContext.Current.Get<Petition>("CurrentPetition");

                return document;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPetition"))
                {
                    ScenarioContext.Current.Remove("CurrentPetition");
                }

                ScenarioContext.Current.Add("CurrentPetition", value);
            }
        }
    }
}
