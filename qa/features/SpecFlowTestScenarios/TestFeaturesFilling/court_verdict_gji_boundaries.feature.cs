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
namespace TestFeaturesFilling
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.3.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Решения суда\"")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаРешенияСудаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "court_verdict_gji_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Решения суда\"", "Справочники - ГЖИ - Решения суда", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        public virtual void СозданиеНовогоРешенияСудаСПустымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание нового решения суда с пустыми полями", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 8
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 9
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
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 13
testRunner.And("пользователь у этого решения суда заполняет поле Наименование 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.Then("запись по этому решению суда присутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового решения суда при вводе граничных условий в 301 знаков, " +
            "Наименование")]
        public virtual void НеудачноеСозданиеНовогоРешенияСудаПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового решения суда при вводе граничных условий в 301 знаков, " +
                    "Наименование", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 19
testRunner.And("пользователь у этого решения суда заполняет поле Наименование 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 21
testRunner.Then("запись по этому решению суда отсутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 22
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
#line 26
this.ScenarioSetup(scenarioInfo);
#line 27
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 28
testRunner.And("пользователь у этого решения суда заполняет поле Код 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 30
testRunner.Then("запись по этому решению суда присутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового решения суда при вводе граничных условий в 301 знаков, " +
            "Код")]
        public virtual void НеудачноеСозданиеНовогоРешенияСудаПриВводеГраничныхУсловийВ301ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового решения суда при вводе граничных условий в 301 знаков, " +
                    "Код", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 34
testRunner.And("пользователь у этого решения суда заполняет поле Код 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 36
testRunner.Then("запись по этому решению суда отсутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 37
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Код не должно превышать 300 сим" +
                    "волов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
