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
namespace SpecFlowTestScenarios
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.3.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Доработка реестра Объектов КР")]
    public partial class ДоработкаРеестраОбъектовКРFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "objects_cr.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Доработка реестра Объектов КР", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("удаление дома из реестра объекта кр")]
        public virtual void УдалениеДомаИзРеестраОбъектаКр()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удаление дома из реестра объекта кр", ((string[])(null)));
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.When("у объекта \"объект кр\" капитального ремонта \"количество видов работ\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 7
testRunner.Then("пользователь успешно удаляет объект \"объект кр\" из реестра Объектов КР", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("сохранение удаленных объектов в \"Удаленные объекты капитального ремонта\"")]
        [NUnit.Framework.TestCaseAttribute("Программа", null)]
        [NUnit.Framework.TestCaseAttribute("Муниципальный район", null)]
        [NUnit.Framework.TestCaseAttribute("Адрес", null)]
        public virtual void СохранениеУдаленныхОбъектовВУдаленныеОбъектыКапитальногоРемонта(string аттрибут, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("сохранение удаленных объектов в \"Удаленные объекты капитального ремонта\"", exampleTags);
#line 10
this.ScenarioSetup(scenarioInfo);
#line 11
testRunner.When("у объекта \"объект кр\" капитального ремонта \"количество видов работ\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 12
testRunner.And("пользователь успешно удаляет объект \"объект кр\" из реестра Объектов КР", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 13
testRunner.And("у удаленного объекта капитального ремонта \"объект кр\" отметка по программе кр \"пр" +
                    "ограмма кр\" = \"На основе ДПКР\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.Then("удаленный объект капитального ремонта \"объект кр\" есть в списке удаленных домов в" +
                    " реестре \"Удаленные объекты капитального ремонта\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 15
testRunner.And(string.Format("в реестре \"Удаленные объекты капитального ремонта\" в списке аттрибутов есть \"{0}\"" +
                        "", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("редактирование удаленного объекта кр")]
        [NUnit.Framework.TestCaseAttribute("Объект недвижимости", null)]
        [NUnit.Framework.TestCaseAttribute("Программа", null)]
        public virtual void РедактированиеУдаленногоОбъектаКр(string аттрибут, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("редактирование удаленного объекта кр", exampleTags);
#line 24
this.ScenarioSetup(scenarioInfo);
#line 25
testRunner.Given("в реестре \"Удаленные объекты капитального ремонта\" \"количество\" записей != 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 26
testRunner.When("пользователь редактирует удаленный объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 27
testRunner.And(string.Format("в списке аттрибутов удаленного объекта кр \"объект кр\" есть аттрибуты \"{0}\"", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.Then("по удаленному объекту кр \"объект кр\" есть реестр \"Журнал изменений\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 29
testRunner.And("в реестре есть \"Журнал изменений\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Действие\" = аттрибуту \"Действие\" из в" +
                    "ида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Вид работы\" = аттрибуту \"Вид работы\" " +
                    "из вида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Источник финансирвоания\" = аттрибуту " +
                    "\"Источник финансирвоания\" из вида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Объем\" = аттрибуту \"Объем\" из вида ра" +
                    "бот удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Сумма (руб.)\" = аттрибуту \"Сумма (руб" +
                    ".)\" из вида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Год выполнения по Долгосрочной програ" +
                    "мме\" = аттрибуту \"Год выполнения по Долгосрочной программе\" из вида работ удален" +
                    "ного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Новый год выполнения\" = аттрибуту \"Но" +
                    "вый год выполнения\" из вида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Дата изменения\" = аттрибуту \"Дата изм" +
                    "енения\" из вида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Пользователь\" = аттрибуту \"Пользовате" +
                    "ль\" из вида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Причина\" = аттрибуту \"Причина\" из вид" +
                    "а работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Дата документа\" = аттрибуту \"Дата док" +
                    "умента\" из вида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Документ (основание)\" = аттрибуту \"До" +
                    "кумент (основание)\" из вида работ удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.And("в списке аттррибутов журнала есть аттрибут \"Файл\" = аттрибуту \"Файл\" из вида рабо" +
                    "т удаленного объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Восстановление удаленных объектов кр")]
        [NUnit.Framework.TestCaseAttribute("Объект недвижимости", null)]
        [NUnit.Framework.TestCaseAttribute("Программа", null)]
        public virtual void ВосстановлениеУдаленныхОбъектовКр(string аттрибут, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Восстановление удаленных объектов кр", exampleTags);
#line 50
this.ScenarioSetup(scenarioInfo);
#line 51
testRunner.Given("в реестре \"Удаленные объекты капитального ремонта\" \"количество\" записей != 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 52
testRunner.When("пользователь в реестре \"Удаленные объекты капитального ремонта\" выбирает объекты " +
                    "кр \"объект кр\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 53
testRunner.And("вызывает процедуру восстановления объекта кр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 54
testRunner.Then("выбранные объекты кр \"объект кр\" появляются в реестре \"Объекты капитального ремон" +
                    "та\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 55
testRunner.And("выбранных объектов кр \"объект кр\" нет в реестре \"Удаленные объекты капитального р" +
                    "емонта\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 56
testRunner.And(string.Format("у выбранного объектов кр \"объект кр\" в реестре \"Объекты капитального ремонта\" ест" +
                        "ь аттрибуты \"{0}\"", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 57
testRunner.And("в реестре объектов кр для объекта кр \"объект кр\" есть \"Журнал изменений\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
