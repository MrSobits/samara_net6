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
namespace Bars.Gkh.Qa.TestScenarios
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Решения суда\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаРешенияСудаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "court_verdict_gji_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Решения суда\"", "Справочники - ГЖИ - Решения суда", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("создание нового решения суда с пустыми полями")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void СозданиеНовогоРешенияСудаСПустымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание нового решения суда с пустыми полями", new string[] {
                        "ignore"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 9
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 10
testRunner.Then("запись по этому решению суда присутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового решения суда при вводе граничных условий в 300 знаков, На" +
            "именование")]
        public virtual void УдачноеСозданиеНовогоРешенияСудаПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового решения суда при вводе граничных условий в 300 знаков, На" +
                    "именование", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 13
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 14
testRunner.And("пользователь у этого решения суда заполняет поле Наименование 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 16
testRunner.Then("запись по этому решению суда присутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового решения суда при вводе граничных условий в 301 знаков, " +
            "Наименование")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовогоРешенияСудаПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового решения суда при вводе граничных условий в 301 знаков, " +
                    "Наименование", new string[] {
                        "ignore"});
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 22
testRunner.And("пользователь у этого решения суда заполняет поле Наименование 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.Then("запись по этому решению отсутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 25
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 300 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового решения суда при вводе граничных условий в 300 знаков, Ко" +
            "д")]
        public virtual void УдачноеСозданиеНовогоРешенияСудаПриВводеГраничныхУсловийВ300ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового решения суда при вводе граничных условий в 300 знаков, Ко" +
                    "д", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 30
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 31
testRunner.And("пользователь у этого решения суда заполняет поле Наименование \"11111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("пользователь у этого решения суда заполняет поле Код 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 34
testRunner.Then("запись по этому решению суда присутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового решения суда при вводе граничных условий в 301 знаков, " +
            "Код")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовогоРешенияСудаПриВводеГраничныхУсловийВ301ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового решения суда при вводе граничных условий в 301 знаков, " +
                    "Код", new string[] {
                        "ignore"});
#line 38
this.ScenarioSetup(scenarioInfo);
#line 39
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 40
testRunner.And("пользователь у этого решения суда заполняет поле Наименование \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.And("пользователь у этого решения суда заполняет поле Код 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 43
testRunner.Then("запись по этому решению отсутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 44
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Код не должно превышать 300 сим" +
                    "волов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion