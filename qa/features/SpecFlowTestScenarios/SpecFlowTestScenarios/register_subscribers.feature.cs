﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.3.0
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SpecFlowTestScenarios
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.3.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("доработки к Реестру абонентов")]
    public partial class ДоработкиКРееструАбонентовFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "register_subscribers.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "доработки к Реестру абонентов", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("проверка вывода контрагентов для абонента с типом  = \"юридическое лицо\"")]
        [NUnit.Framework.TestCaseAttribute("Счет юр.лица", null)]
        public virtual void ПроверкаВыводаКонтрагентовДляАбонентаСТипомЮридическоеЛицо(string типомАбонента, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка вывода контрагентов для абонента с типом  = \"юридическое лицо\"", exampleTags);
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.When(string.Format("пользователь добавляет абонента с типом абонента \"{0}\"", типомАбонента), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 7
testRunner.Then("в списке контрагентов доступны только те, у которых \"нет\" привязки к другим абоне" +
                    "нтам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка вывода контрагентов для абонента с типом  = \"физическое лицо\"")]
        [NUnit.Framework.TestCaseAttribute("Счет физ.лица", null)]
        public virtual void ПроверкаВыводаКонтрагентовДляАбонентаСТипомФизическоеЛицо(string типомАбонента, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка вывода контрагентов для абонента с типом  = \"физическое лицо\"", exampleTags);
#line 14
this.ScenarioSetup(scenarioInfo);
#line 15
testRunner.When(string.Format("пользователь добавляет абонента с типом абонента \"{0}\"", типомАбонента), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 16
testRunner.Then("в списке контрагентов доступны \"все\" абоненты", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Льготная категория\" для абонента с типом  = \"юридическое лицо\"")]
        [NUnit.Framework.TestCaseAttribute("Счет юр.лица", null)]
        public virtual void ЗаполнениеПоляЛьготнаяКатегорияДляАбонентаСТипомЮридическоеЛицо(string типомАбонента, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Льготная категория\" для абонента с типом  = \"юридическое лицо\"", exampleTags);
#line 23
this.ScenarioSetup(scenarioInfo);
#line 24
testRunner.When(string.Format("пользователь добавляет абонента с типом абонента \"{0}\"", типомАбонента), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.Then("в списке полей \"нет\" поля \"Льготная категория\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Льготная категория\" для абонента с типом  = \"физическое лицо\"")]
        [NUnit.Framework.TestCaseAttribute("Счет физ.лица", null)]
        public virtual void ЗаполнениеПоляЛьготнаяКатегорияДляАбонентаСТипомФизическоеЛицо(string типомАбонента, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Льготная категория\" для абонента с типом  = \"физическое лицо\"", exampleTags);
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
testRunner.When(string.Format("пользователь добавляет абонента с типом абонента \"{0}\"", типомАбонента), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 34
testRunner.Then("в списке полей \"есть\" поле \"Льготная категория\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
