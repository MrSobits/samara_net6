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
    [NUnit.Framework.DescriptionAttribute("Реестр Займов")]
    public partial class РеестрЗаймовFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "loan.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Реестр Займов", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Формирование займа")]
        public virtual void ФормированиеЗайма()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Формирование займа", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
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
                        "д. test51"});
#line 6
testRunner.Given("в реестр жилых домов добавлен новый дом", ((string)(null)), table1, "Дано ");
#line 10
testRunner.And("добавлен новый счёт оплат текущего дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "houseType",
                        "city",
                        "street",
                        "houseNumber"});
            table2.AddRow(new string[] {
                        "testregion",
                        "Многоквартирный",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "д. test52"});
#line 12
testRunner.Given("в реестр жилых домов добавлен новый дом", ((string)(null)), table2, "Дано ");
#line 16
testRunner.And("добавлен новый счёт оплат текущего дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DateStart"});
            table3.AddRow(new string[] {
                        "период программы тестовый",
                        "01.01.2015"});
#line 18
testRunner.And("добавлен период программ", ((string)(null)), table3, "И ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Period"});
            table4.AddRow(new string[] {
                        "программа капитального ремонта тестовая",
                        "период программы тестовый"});
#line 22
testRunner.And("добавлена программа капитального ремонта", ((string)(null)), table4, "И ");
#line 26
testRunner.Given("пользователь формирует займ для дома по адресу \"с. Никольское ул. 50 лет Октября " +
                    "д. test51\" по текущей программе КР и текущему МО", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 28
testRunner.And("пользователь устанавливает способ формирования займа \"Ручная\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("пользователь выбирает источник займа \"Взносы по минимальному тарифу\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("пользователь у заимодателя c адресом \"с. Никольское ул. 50 лет Октября д. test52\"" +
                    " устанавливает сумму заимствования \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.When("пользователь принимает изменения займа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 36
testRunner.Then("взятие займа прошло успешно", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
