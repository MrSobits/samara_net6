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
    [NUnit.Framework.DescriptionAttribute("Конструктивные элементы жилого дома")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class КонструктивныеЭлементыЖилогоДомаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "realityobjectedit_constructiveelement.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Конструктивные элементы жилого дома", "Жилищный фонд - Реестр жилых домов - Жилой дом - Конструктивные элементы", ProgrammingLanguage.CSharp, new string[] {
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
                        "Name"});
            table2.AddRow(new string[] {
                        "группа конструктивных элементов тест"});
#line 10
testRunner.Given("добавлена группа конструктивных элементов", ((string)(null)), table2, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "ShortName",
                        "Description"});
            table3.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 14
testRunner.Given("добавлена единица измерения", ((string)(null)), table3, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "FullName",
                        "Name",
                        "Code"});
            table4.AddRow(new string[] {
                        "нормативно-правовой документ тест",
                        "тест",
                        "111"});
#line 19
testRunner.Given("добавлен нормативный документ", ((string)(null)), table4, "Дано ");
#line 23
testRunner.Given("пользователь добавляет новый конструктивный элемент", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 24
testRunner.And("пользователь у этого конструктивного элемента заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.And("пользователь у этого конструктивного элемента заполняет поле Код \"234\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.And("пользователь у этого конструктивного элемента заполняет поле Группа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.And("пользователь у этого конструктивного элемента заполняет поле Срок эксплуатации \"1" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.And("пользователь у этого конструктивного элемента заполняет поле Нормативный документ" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.And("пользователь у этого конструктивного элемента заполняет поле Единица измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.When("пользователь сохраняет этот конструктивный элемент", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 32
testRunner.Given("добавлен новый конструктивный элемент жилого дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("создание конструктивных элементов жилого дома")]
        public virtual void СозданиеКонструктивныхЭлементовЖилогоДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание конструктивных элементов жилого дома", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 35
testRunner.When("пользователь сохраняет конструктивный элемент жилого дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 36
testRunner.Then("конструктивный элемент появляется в списке конструктивных элементов дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удаление конструктивных элементов жилого дома")]
        public virtual void УдалениеКонструктивныхЭлементовЖилогоДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удаление конструктивных элементов жилого дома", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 39
testRunner.When("пользователь сохраняет конструктивный элемент жилого дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 40
testRunner.And("пользователь удаляет конструктивный элемент жилого дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.Then("конструктивный элемент отсутствует в списке конструктивных элементов дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion