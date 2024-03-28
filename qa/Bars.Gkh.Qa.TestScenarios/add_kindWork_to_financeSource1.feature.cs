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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для добавления видов работ к разрезу финансирования в разделе \"Разрезы " +
        "финансирования\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляДобавленияВидовРаботКРазрезуФинансированияВРазделеРазрезыФинансированияFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "add_kindWork_to_financeSource.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для добавления видов работ к разрезу финансирования в разделе \"Разрезы " +
                    "финансирования\"", "Справочники - Капитальный ремонт - Разрезы финансирования", ProgrammingLanguage.CSharp, new string[] {
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
#line 6
testRunner.Given("пользователь добавляет новый разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("пользователь у этого разреза финансирования заполняет поле Наименование \"разрез ф" +
                    "инансирования тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("пользователь у этого рзареза финансирования заполняет поле Код \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "ShortName",
                        "Description"});
            table1.AddRow(new string[] {
                        "тестовая единица измерения",
                        "тест",
                        "тест"});
#line 10
testRunner.And("добавлена единица измерения", ((string)(null)), table1, "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление вида работ в Разрез финансирования")]
        public virtual void УспешноеДобавлениеВидаРаботВРазрезФинансирования()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление вида работ в Разрез финансирования", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "ShortName",
                        "Description"});
            table2.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 15
testRunner.Given("добавлена единица измерения", ((string)(null)), table2, "Дано ");
#line 18
testRunner.And("пользователь добавляет новый вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.And("пользователь у этого вида работ заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.And("пользователь у этого вида работ заполняет поле Ед. измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("пользователь у этого вида работ заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("пользователь у этого вида работ заполняет поле Норматив \"111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("пользователь у этого вида работ заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.When("пользователь сохраняет этот вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.And("пользователь сохраняет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.And("пользователь разрезу финансирования добавляет вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.Then("запись по этому виду работ присутствует в разрезе финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление вида работ из Разреза финансирования")]
        public virtual void УспешноеУдалениеВидаРаботИзРазрезаФинансирования()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление вида работ из Разреза финансирования", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "ShortName",
                        "Description"});
            table3.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 30
testRunner.Given("добавлена единица измерения", ((string)(null)), table3, "Дано ");
#line 33
testRunner.And("пользователь добавляет новый вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.And("пользователь у этого вида работ заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.And("пользователь у этого вида работ заполняет поле Ед. измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.And("пользователь у этого вида работ заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.And("пользователь у этого вида работ заполняет поле Норматив \"111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("пользователь у этого вида работ заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.When("пользователь сохраняет этот вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 40
testRunner.And("пользователь сохраняет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.And("пользователь разрезу финансирования добавляет вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.And("пользователь удаляет вид работ из разреза финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 43
testRunner.Then("запись по этому виду работ отсутствует в разрезе финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное удаление разреза финансирования")]
        public virtual void НеудачноеУдалениеРазрезаФинансирования()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное удаление разреза финансирования", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "ShortName",
                        "Description"});
            table4.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 46
testRunner.Given("добавлена единица измерения", ((string)(null)), table4, "Дано ");
#line 49
testRunner.And("пользователь добавляет новый вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 50
testRunner.And("пользователь у этого вида работ заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 51
testRunner.And("пользователь у этого вида работ заполняет поле Ед. измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 52
testRunner.And("пользователь у этого вида работ заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.And("пользователь у этого вида работ заполняет поле Норматив \"111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 54
testRunner.And("пользователь у этого вида работ заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 55
testRunner.When("пользователь сохраняет этот вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 56
testRunner.And("пользователь сохраняет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 57
testRunner.And("пользователь разрезу финансирования добавляет вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 58
testRunner.And("пользователь удаляет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 59
testRunner.Then("запись по этому виду работ присутствует в разрезе финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 60
testRunner.And("падает ошибка с текстом \"Виды работ источников финансирования\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление дубля вида работ к разрезу финансирования")]
        public virtual void НеудачноеДобавлениеДубляВидаРаботКРазрезуФинансирования()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление дубля вида работ к разрезу финансирования", ((string[])(null)));
#line 62
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "ShortName",
                        "Description"});
            table5.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 63
testRunner.Given("добавлена единица измерения", ((string)(null)), table5, "Дано ");
#line 66
testRunner.And("пользователь добавляет новый вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 67
testRunner.And("пользователь у этого вида работ заполняет поле Наименование \"тест99999\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 68
testRunner.And("пользователь у этого вида работ заполняет поле Ед. измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 69
testRunner.And("пользователь у этого вида работ заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 70
testRunner.And("пользователь у этого вида работ заполняет поле Норматив \"111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 71
testRunner.And("пользователь у этого вида работ заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 72
testRunner.When("пользователь сохраняет этот вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 73
testRunner.And("пользователь сохраняет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 74
testRunner.And("пользователь разрезу финансирования добавляет вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 75
testRunner.Given("пользователь добавляет новый вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 76
testRunner.And("пользователь у этого вида работ заполняет поле Наименование \"тест99999\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 77
testRunner.And("пользователь у этого вида работ заполняет поле Ед. измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 78
testRunner.And("пользователь у этого вида работ заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 79
testRunner.And("пользователь у этого вида работ заполняет поле Норматив \"111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 80
testRunner.And("пользователь у этого вида работ заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 81
testRunner.When("пользователь сохраняет этот вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 82
testRunner.And("пользователь разрезу финансирования добавляет вид работ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 83
testRunner.Then("дубли записей по этим видам работ отсутствуют в этом разрезе финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion