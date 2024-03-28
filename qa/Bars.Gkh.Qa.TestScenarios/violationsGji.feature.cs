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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Нарушения\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляРазделаНарушенияFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "violationsGji.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Нарушения\"", "Справочники - ГЖИ - Нарушения", ProgrammingLanguage.CSharp, new string[] {
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
                        "Code"});
            table1.AddRow(new string[] {
                        "мероприятие по устранению нарушений тест",
                        "тест"});
#line 6
testRunner.Given("добавлено мероприятие по устранению нарушений", ((string)(null)), table1, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "FullName",
                        "Name",
                        "Code"});
            table2.AddRow(new string[] {
                        "нормативный документ тест",
                        "нормативный документ тест",
                        "123"});
#line 10
testRunner.Given("добавлен нормативный документ", ((string)(null)), table2, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Number",
                        "Text"});
            table3.AddRow(new string[] {
                        "нпд1",
                        "тест"});
#line 14
testRunner.Given("добавлен пункт нормативно-правового документа к этому нормативному документу", ((string)(null)), table3, "Дано ");
#line 18
testRunner.Given("пользователь добавляет новое нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 19
testRunner.And("пользователь у этого нарушения заполняет поле Текст нарушения \"нарушение тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление нарушения")]
        public virtual void УспешноеДобавлениеНарушения()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление нарушения", ((string[])(null)));
#line 21
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 22
testRunner.When("пользователь сохраняет это нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 23
testRunner.Then("запись по этому нарушению присутствует в справочнике нарушений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление записи из справочника нарушений")]
        public virtual void УспешноеУдалениеЗаписиИзСправочникаНарушений()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление записи из справочника нарушений", ((string[])(null)));
#line 25
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 26
testRunner.When("пользователь сохраняет это нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 27
testRunner.And("пользователь удаляет это нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.Then("запись по этому нарушению отсутствует в справочнике нарушений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление дубля плана мероприятия к нарушению")]
        public virtual void УспешноеДобавлениеДубляПланаМероприятияКНарушению()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление дубля плана мероприятия к нарушению", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 31
testRunner.When("пользователь сохраняет это нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 32
testRunner.Given("пользователь добавляет новое нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 33
testRunner.And("пользователь у этого нарушения заполняет поле Текст нарушения \"нарушение тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.When("пользователь сохраняет это нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 35
testRunner.Then("запись по этому нарушению присутствует в справочнике нарушений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление нпд и мероприятия по устранению нарушений к нарушению")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void УспешноеДобавлениеНпдИМероприятияПоУстранениюНарушенийКНарушению()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление нпд и мероприятия по устранению нарушений к нарушению", new string[] {
                        "ignore"});
#line 39
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 40
testRunner.When("пользователь сохраняет это нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 41
testRunner.Given("пользователь у этого нарушения добавляет мероприятие по устранению нарушений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 42
testRunner.Given("пользователь у этого нарушения добавляет пункт нормативно-правового документа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 43
testRunner.When("пользователь сохраняет это нарушение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 44
testRunner.Then("запись по этому нарушению присутствует в справочнике нарушений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion