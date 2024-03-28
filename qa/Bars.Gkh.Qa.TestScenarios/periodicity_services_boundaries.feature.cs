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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Периодичность услуг\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаПериодичностьУслугFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "periodicity_services_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Периодичность услуг\"", "Справочники - Раскрытие информации - Периодичность услуг", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("создание новой периодичности услуг с незаполнеными полями, Наименование")]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.CategoryAttribute("sahalin")]
        public virtual void СозданиеНовойПериодичностиУслугСНезаполненымиПолямиНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание новой периодичности услуг с незаполнеными полями, Наименование", new string[] {
                        "sahalin",
                        "ignore"});
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
testRunner.Given("пользователь добавляет новую периодичность услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 10
testRunner.And("пользователь у этой периодичности услуг заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.When("пользователь сохраняет эту периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 12
testRunner.Then("запись по этой периодичности услуг присутствует в разделе периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("создание новой периодичности услуг с незаполнеными полями, Код")]
        public virtual void СозданиеНовойПериодичностиУслугСНезаполненымиПолямиКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание новой периодичности услуг с незаполнеными полями, Код", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 15
testRunner.Given("пользователь добавляет новую периодичность услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 16
testRunner.And("пользователь у этой периодичности услуг заполняет поле Наименование \"тесттетсттет" +
                    "тстеттстест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 17
testRunner.When("пользователь сохраняет эту периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 18
testRunner.Then("запись по этой периодичности услуг присутствует в разделе периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание новой периодичности услуг при вводе граничных условий в 300 знак" +
            "ов, Код")]
        public virtual void УдачноеСозданиеНовойПериодичностиУслугПриВводеГраничныхУсловийВ300ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание новой периодичности услуг при вводе граничных условий в 300 знак" +
                    "ов, Код", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
testRunner.Given("пользователь добавляет новую периодичность услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 22
testRunner.And("пользователь у этой периодичности услуг заполняет поле Наименование \"тесттетсттет" +
                    "тстеттстест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("пользователь у этой периодичности услуг заполняет поле Код 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.When("пользователь сохраняет эту периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.Then("запись по этой периодичности услуг присутствует в разделе периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание новой периодичности услуг при вводе граничных условий в 301 зн" +
            "аков, Код")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовойПериодичностиУслугПриВводеГраничныхУсловийВ301ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание новой периодичности услуг при вводе граничных условий в 301 зн" +
                    "аков, Код", new string[] {
                        "ignore"});
#line 29
this.ScenarioSetup(scenarioInfo);
#line 30
testRunner.Given("пользователь добавляет новую периодичность услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 31
testRunner.And("пользователь у этой периодичности услуг заполняет поле Наименование \"тесттетсттет" +
                    "тстеттстест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("пользователь у этой периодичности услуг заполняет поле Код 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.When("пользователь сохраняет эту периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 34
testRunner.Then("запись по этой периодичности услуг отсутствует в разделе периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 35
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Код не должно превышать 300 сим" +
                    "волов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание новой периодичности услуг при вводе граничных условий в 300 знак" +
            "ов, Наименование")]
        public virtual void УдачноеСозданиеНовойПериодичностиУслугПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание новой периодичности услуг при вводе граничных условий в 300 знак" +
                    "ов, Наименование", ((string[])(null)));
#line 39
this.ScenarioSetup(scenarioInfo);
#line 40
testRunner.Given("пользователь добавляет новую периодичность услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 41
testRunner.And("пользователь у этой периодичности услуг заполняет поле Наименование 300 символов " +
                    "\"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.When("пользователь сохраняет эту периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 43
testRunner.Then("запись по этой периодичности услуг присутствует в разделе периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание новой периодичности услуг при вводе граничных условий в 301 зн" +
            "аков, Наименование")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовойПериодичностиУслугПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание новой периодичности услуг при вводе граничных условий в 301 зн" +
                    "аков, Наименование", new string[] {
                        "ignore"});
#line 47
this.ScenarioSetup(scenarioInfo);
#line 48
testRunner.Given("пользователь добавляет новую периодичность услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 49
testRunner.And("пользователь у этой периодичности услуг заполняет поле Наименование 301 символов " +
                    "\"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 50
testRunner.When("пользователь сохраняет эту периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 51
testRunner.Then("запись по этой периодичности услуг отсутствует в разделе периодичности услуг", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 52
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 300 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
