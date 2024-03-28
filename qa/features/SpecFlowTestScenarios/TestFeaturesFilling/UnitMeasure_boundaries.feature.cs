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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для справочника \"Единицы измерения\"")]
    public partial class ТесткейсыГраничныхЗначенийДляСправочникаЕдиницыИзмеренияFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "UnitMeasure_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для справочника \"Единицы измерения\"", "Справочники - Общие - Единицы измерения", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("создание новой единицы измерения с пустыми полями")]
        public virtual void СозданиеНовойЕдиницыИзмеренияСПустымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание новой единицы измерения с пустыми полями", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
testRunner.Given("пользователь добавляет новую единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 8
testRunner.When("пользователь сохраняет эту единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 9
testRunner.Then("запись по этой единице измерения присутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание новой единицы измерения при вводе граничных условий в 300 знаков" +
            ", Наименование")]
        public virtual void УдачноеСозданиеНовойЕдиницыИзмеренияПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание новой единицы измерения при вводе граничных условий в 300 знаков" +
                    ", Наименование", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
testRunner.Given("пользователь добавляет новую единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 13
testRunner.And("пользователь у этой единицы измерения заполняет поле Наименование 300 символов \"1" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.And("пользователь у этой единицы измерения заполняет поле Краткое наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.And("пользователь у этой единицы измерения заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.When("пользователь сохраняет эту единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.Then("запись по этой единице измерения присутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание новой единицы измерения при вводе граничных условий в 301 знак" +
            "ов, Наименование")]
        public virtual void НеудачноеСозданиеНовойЕдиницыИзмеренияПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание новой единицы измерения при вводе граничных условий в 301 знак" +
                    "ов, Наименование", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
testRunner.Given("пользователь добавляет новую единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 21
testRunner.And("пользователь у этой единицы измерения заполняет поле Наименование 301 символов \"1" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("пользователь у этой единицы измерения заполняет поле Краткое наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("пользователь у этой единицы измерения заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.When("пользователь сохраняет эту единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.Then("запись по этой единице измерения отсутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 26
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 300 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание новой единицы измерения при вводе граничных условий в 20 знако" +
            "в, Краткое наименование")]
        public virtual void НеудачноеСозданиеНовойЕдиницыИзмеренияПриВводеГраничныхУсловийВ20ЗнаковКраткоеНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание новой единицы измерения при вводе граничных условий в 20 знако" +
                    "в, Краткое наименование", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
testRunner.Given("пользователь добавляет новую единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 32
testRunner.And("пользователь у этой единицы измерения заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And("пользователь у этой единицы измерения заполняет поле Краткое наименование 20 симв" +
                    "олов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.And("пользователь у этой единицы измерения заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.When("пользователь сохраняет эту единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 36
testRunner.Then("запись по этой единице измерения присутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание новой единицы измерения при вводе граничных условий в 21 знако" +
            "в, Краткое наименование")]
        public virtual void НеудачноеСозданиеНовойЕдиницыИзмеренияПриВводеГраничныхУсловийВ21ЗнаковКраткоеНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание новой единицы измерения при вводе граничных условий в 21 знако" +
                    "в, Краткое наименование", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 39
testRunner.Given("пользователь добавляет новую единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 40
testRunner.And("пользователь у этой единицы измерения заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.And("пользователь у этой единицы измерения заполняет поле Краткое наименование 21 симв" +
                    "олов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.And("пользователь у этой единицы измерения заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 43
testRunner.When("пользователь сохраняет эту единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 44
testRunner.Then("запись по этой единице измерения отсутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 45
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Краткое наименование не должно " +
                    "превышать 20 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание новой единицы измерения при вводе граничных условий в 500 знак" +
            "ов, Описание")]
        public virtual void НеудачноеСозданиеНовойЕдиницыИзмеренияПриВводеГраничныхУсловийВ500ЗнаковОписание()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание новой единицы измерения при вводе граничных условий в 500 знак" +
                    "ов, Описание", ((string[])(null)));
#line 49
this.ScenarioSetup(scenarioInfo);
#line 50
testRunner.Given("пользователь добавляет новую единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 51
testRunner.And("пользователь у этой единицы измерения заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 52
testRunner.And("пользователь у этой единицы измерения заполняет поле Краткое наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.And("пользователь у этой единицы измерения заполняет поле Описание 500 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 54
testRunner.When("пользователь сохраняет эту единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 55
testRunner.Then("запись по этой единице измерения присутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание новой единицы измерения при вводе граничных условий в 501 знак" +
            "ов, Описание")]
        public virtual void НеудачноеСозданиеНовойЕдиницыИзмеренияПриВводеГраничныхУсловийВ501ЗнаковОписание()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание новой единицы измерения при вводе граничных условий в 501 знак" +
                    "ов, Описание", ((string[])(null)));
#line 57
this.ScenarioSetup(scenarioInfo);
#line 58
testRunner.Given("пользователь добавляет новую единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 59
testRunner.And("пользователь у этой единицы измерения заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 60
testRunner.And("пользователь у этой единицы измерения заполняет поле Краткое наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 61
testRunner.And("пользователь у этой единицы измерения заполняет поле Описание 501 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 62
testRunner.When("пользователь сохраняет эту единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 63
testRunner.Then("запись по этой единице измерения отсутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 64
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Описание не должно превышать 50" +
                    "0 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
