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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для добавления приказа к плановой проверке юридических лиц в разделе \"П" +
        "лановые проверки юридических лиц\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляДобавленияПриказаКПлановойПроверкеЮридическихЛицВРазделеПлановыеПроверкиЮридическихЛицFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "add_order_to_base_jurPerson.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для добавления приказа к плановой проверке юридических лиц в разделе \"П" +
                    "лановые проверки юридических лиц\"", "Жилищная инспекция - Основания проверок - Плановые проверки юридических лиц - При" +
                    "каз", ProgrammingLanguage.CSharp, new string[] {
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
                        "те1ст",
                        "те1ст",
                        "те1ст"});
#line 6
testRunner.Given("добавлена организационно-правовая форма", ((string)(null)), table1, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table2.AddRow(new string[] {
                        "те1ст"});
#line 10
testRunner.Given("добавлен контрагент с организационно правовой формой", ((string)(null)), table2, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table3.AddRow(new string[] {
                        "те1ст"});
#line 14
testRunner.And("добавлена управляющая организация для этого контрагента", ((string)(null)), table3, "И ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DateStart",
                        "DateEnd"});
            table4.AddRow(new string[] {
                        "те1стовый план мероприятий",
                        "01.01.2015",
                        "01.01.2016"});
#line 18
testRunner.And("добавлен план проверки юридических лиц", ((string)(null)), table4, "И ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Code",
                        "Fio",
                        "Email"});
            table5.AddRow(new string[] {
                        "1519",
                        "Тесто111вый инспектор",
                        "test@test.test"});
#line 22
testRunner.And("добавлен инспектор", ((string)(null)), table5, "И ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "TypeJurPerson",
                        "DateStart"});
            table6.AddRow(new string[] {
                        "Управляющая организация",
                        "текущая дата"});
#line 26
testRunner.And("добавлена плановая проверка юридических лиц", ((string)(null)), table6, "И ");
#line 30
testRunner.And("пользователь у этой плановой проверки юридических лиц формирует приказ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("пользователь у этого приказа заполняет поле Дата \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("пользователь у этого приказа заполняет поле Период проведения проверки с \"текущая" +
                    " дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And("пользователь у этого приказа заполняет поле по \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.And("пользователь у этого приказа заполняет поле ДЛ, вынесшее Приказ этим инспектором", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление приказа к проверке юридических лиц")]
        public virtual void УспешноеДобавлениеПриказаКПроверкеЮридическихЛиц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление приказа к проверке юридических лиц", ((string[])(null)));
#line 37
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 38
testRunner.When("пользователь сохраняет этот приказ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 39
testRunner.Then("у этой плановой проверки юридических лиц присутствует приказ в списке разделов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion