namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using TechTalk.SpecFlow;

    public class LicenseProvidedDocsHelper
    {
        public static LicenseProvidedDoc Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("LicenseProvidedDoc"))
                {
                    throw new SpecFlowException("Нет текущей лицензии для выдачи документов");
                }

                var licenseProvidedDoc = ScenarioContext.Current.Get<LicenseProvidedDoc>("LicenseProvidedDoc");

                return licenseProvidedDoc;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("LicenseProvidedDoc"))
                {
                    ScenarioContext.Current.Remove("LicenseProvidedDoc");
                }

                ScenarioContext.Current.Add("LicenseProvidedDoc", value);
            }
        }
    }
}
