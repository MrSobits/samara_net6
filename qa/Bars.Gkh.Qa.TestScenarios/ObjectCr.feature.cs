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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Объекты капитального ремонта\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляРазделаОбъектыКапитальногоРемонтаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ObjectCr.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Объекты капитального ремонта\"", "Капитальный ремонт - Программы капитального ремонта - Объекты капитального ремонт" +
                    "а", ProgrammingLanguage.CSharp, new string[] {
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
                        "region",
                        "houseType",
                        "city",
                        "street",
                        "houseNumber"});
            table1.AddRow(new string[] {
                        "testregion",
                        "Многоквартирный",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "д. test"});
#line 6
testRunner.Given("в реестр жилых домов добавлен новый дом", ((string)(null)), table1, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DateStart"});
            table2.AddRow(new string[] {
                        "период программы тестовый",
                        "01.01.2015"});
#line 10
testRunner.And("добавлен период программ", ((string)(null)), table2, "И ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Period"});
            table3.AddRow(new string[] {
                        "программа капитального ремонта тестовая",
                        "период программы тестовый"});
#line 14
testRunner.And("добавлена программа капитального ремонта", ((string)(null)), table3, "И ");
#line 18
testRunner.Given("пользователь добавляет новый объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление объекта капитального ремонта")]
        public virtual void УспешноеДобавлениеОбъектаКапитальногоРемонта()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление объекта капитального ремонта", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 21
testRunner.When("пользователь сохраняет этот объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 22
testRunner.Then("запись по этому объекту капитального ремонта присутствует в разделе объектов капи" +
                    "тального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление объекта капитального ремонта")]
        public virtual void УспешноеУдалениеОбъектаКапитальногоРемонта()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление объекта капитального ремонта", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 25
testRunner.When("пользователь сохраняет этот объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 26
testRunner.And("пользователь удаляет этот объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.Then("запись по этому объекту капитального ремонта отсутствует в разделе объектов капит" +
                    "ального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 28
testRunner.Then("запись по этому объекту капитального ремонта присутствует в разделе удаленные объ" +
                    "екты капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление дубля объекта капитального ремонта")]
        public virtual void НеудачноеДобавлениеДубляОбъектаКапитальногоРемонта()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление дубля объекта капитального ремонта", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 31
testRunner.When("пользователь сохраняет этот объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 32
testRunner.Given("пользователь добавляет новый объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 33
testRunner.When("пользователь сохраняет этот объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 34
testRunner.Then("запись по этому объекту капитального ремонта отсутствует в разделе объектов капит" +
                    "ального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 35
testRunner.And("падает ошибка с текстом \"Объект КР с таким жилым домом и программой уже существуе" +
                    "т!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное восстановление объекта капитального ремонта")]
        public virtual void УспешноеВосстановлениеОбъектаКапитальногоРемонта()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное восстановление объекта капитального ремонта", ((string[])(null)));
#line 37
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 38
testRunner.When("пользователь сохраняет этот объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 39
testRunner.And("пользователь удаляет этот объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.Then("запись по этому объекту капитального ремонта отсутствует в разделе объектов капит" +
                    "ального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 41
testRunner.Then("запись по этому объекту капитального ремонта присутствует в разделе удаленные объ" +
                    "екты капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 42
testRunner.When("пользователь восстанавливает этот объект капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 43
testRunner.Then("запись по этому объекту капитального ремонта присутствует в разделе объектов капи" +
                    "тального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
