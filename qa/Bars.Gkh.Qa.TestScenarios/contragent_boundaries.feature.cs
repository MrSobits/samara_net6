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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Контрагенты\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаКонтрагентыFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "contragent_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Контрагенты\"", "Участники процесса - Контрагенты - Контрагенты", ProgrammingLanguage.CSharp, new string[] {
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
        
        public virtual void FeatureBackground()
        {
#line 5
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "OkopfCode"});
            table1.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 6
testRunner.Given("добавлена организационно-правовая форма", ((string)(null)), table1, "Дано ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление контрагента при незаполненном обязательном поле наименование" +
            "")]
        public virtual void НеудачноеДобавлениеКонтрагентаПриНезаполненномОбязательномПолеНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление контрагента при незаполненном обязательном поле наименование" +
                    "", ((string[])(null)));
#line 10
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 11
testRunner.Given("пользователь добавляет нового контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 12
testRunner.And("пользователь у этого контрагента заполняет поле Организационно-правовая форма", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 13
testRunner.When("пользователь сохраняет этого контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.Then("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление контрагента при незаполненном поле Организационно-правовая ф" +
            "орма")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеКонтрагентаПриНезаполненномПолеОрганизационно_ПравоваяФорма()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление контрагента при незаполненном поле Организационно-правовая ф" +
                    "орма", new string[] {
                        "ignore"});
#line 19
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 20
testRunner.Given("пользователь добавляет нового контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 21
testRunner.And("пользователь у этого контрагента заполняет поле Наименование 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.When("пользователь сохраняет этого контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.Then("падает ошибка с текстом \"Не заполнены обязательные поля: Организационно-правовая " +
                    "форма\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление контрагента при вводе граничных условий в 300 знаков, Наименов" +
            "ание")]
        public virtual void УдачноеДобавлениеКонтрагентаПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление контрагента при вводе граничных условий в 300 знаков, Наименов" +
                    "ание", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 27
testRunner.Given("пользователь добавляет нового контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 28
testRunner.And("пользователь у этого контрагента заполняет поле Наименование 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.And("пользователь у этого контрагента заполняет поле Организационно-правовая форма", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.When("пользователь сохраняет этого контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 31
testRunner.Then("запись по этому контрагенту присутствует в реестре контрагентов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление контрагента при вводе граничных условий в 301 знаков, Наимен" +
            "ование")]
        public virtual void НеудачноеДобавлениеКонтрагентаПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление контрагента при вводе граничных условий в 301 знаков, Наимен" +
                    "ование", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 34
testRunner.Given("пользователь добавляет нового контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 35
testRunner.And("пользователь у этого контрагента заполняет поле Наименование 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.And("пользователь у этого контрагента заполняет поле Организационно-правовая форма", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.When("пользователь сохраняет этого контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 39
testRunner.Then("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 300 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion