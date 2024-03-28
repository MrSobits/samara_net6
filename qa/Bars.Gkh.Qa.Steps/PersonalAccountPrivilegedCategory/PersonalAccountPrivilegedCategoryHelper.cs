namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Льготная категория лицевого счета
    /// </summary>
    
    public static class PersonalAccountPrivilegedCategoryHelper
    {
        static public PersonalAccountPrivilegedCategory Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PersonalAccountPrivilegedCategory"))
                {
                    throw new SpecFlowException("В лицевом счете не найдена категория льготы");
                }

                var current = ScenarioContext.Current.Get<PersonalAccountPrivilegedCategory>("PersonalAccountPrivilegedCategory");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PersonalAccountPrivilegedCategory"))
                {
                    ScenarioContext.Current.Remove("PersonalAccountPrivilegedCategory");
                }

                ScenarioContext.Current.Add("PersonalAccountPrivilegedCategory", value);
            }
        }
    }
}
