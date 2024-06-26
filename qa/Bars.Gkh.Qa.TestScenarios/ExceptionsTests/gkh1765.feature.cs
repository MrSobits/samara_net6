﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Bars.Gkh.Qa.TestScenarios.ExceptionsTests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Формирование отчета \"Отчет о решениях собственников и ОГВ\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ФормированиеОтчетаОтчетОРешенияхСобственниковИОГВFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "gkh1765.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Формирование отчета \"Отчет о решениях собственников и ОГВ\"", "Отчеты - Панель отчетов - Раздел \"Региональный оператор\" - Отчет о решениях собст" +
                    "венников и ОГВ", ProgrammingLanguage.CSharp, new string[] {
                        "ScenarioInTransaction"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("формирование отчета")]
        [NUnit.Framework.TestCaseAttribute("Алеутский муниципальный район", "Все адреса", "Протокол решений собственников", null)]
        [NUnit.Framework.TestCaseAttribute("Алеутский муниципальный район", "Все адреса", "Протокол органов гос. власти", null)]
        [NUnit.Framework.TestCaseAttribute("Алеутский муниципальный район", "Все адреса", "Нет протокола", null)]
        [NUnit.Framework.TestCaseAttribute("Алеутский муниципальный район", "с. Никольское, ул. 50 лет Октября, д. 27", "Протокол решений собственников", null)]
        [NUnit.Framework.TestCaseAttribute("Алеутский муниципальный район", "с. Никольское, ул. 50 лет Октября, д. 27", "Протокол органов гос. власти", null)]
        [NUnit.Framework.TestCaseAttribute("Алеутский муниципальный район", "с. Никольское, ул. 50 лет Октября, д. 27", "Нет протокола", null)]
        [NUnit.Framework.TestCaseAttribute("Все МО", "Все адреса", "Протокол решений собственников", null)]
        [NUnit.Framework.TestCaseAttribute("Все МО", "Все адреса", "Протокол органов гос. власти", null)]
        [NUnit.Framework.TestCaseAttribute("Все МО", "Все адреса", "Нет протокола", null)]
        [NUnit.Framework.TestCaseAttribute("Все МО", "с. Никольское, ул. 50 лет Октября, д. 27", "Протокол решений собственников", null)]
        [NUnit.Framework.TestCaseAttribute("Все МО", "с. Никольское, ул. 50 лет Октября, д. 27", "Протокол органов гос. власти", null)]
        [NUnit.Framework.TestCaseAttribute("Все МО", "с. Никольское, ул. 50 лет Октября, д. 27", "Нет протокола", null)]
        public virtual void ФормированиеОтчета(string муниципальныеОбразования, string адреса, string типПротокола, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("формирование отчета", exampleTags);
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.When(string.Format("пользователь в параметрах этого отчета заполняет поле Муниципальные образования \"" +
                        "{0}\"", муниципальныеОбразования), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 7
testRunner.And(string.Format("пользователь в параметрах этого отчета заполняет поле Адреса \"{0}\"", адреса), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And(string.Format("пользователь в параметрах этого отчета заполняет поле Тип протокола \"{0}\"", типПротокола), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.Then("происходит вызов печати отчета \"Отчет о решениях собственников и ОГВ\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
