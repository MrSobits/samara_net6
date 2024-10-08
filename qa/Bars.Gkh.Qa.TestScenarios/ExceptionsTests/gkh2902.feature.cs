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
namespace Bars.Gkh.Qa.TestScenarios.ExceptionsTests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Привязка лицевого счета к Расчетно-кассовому центру")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ПривязкаЛицевогоСчетаКРасчетно_КассовомуЦентруFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "gkh2902.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Привязка лицевого счета к Расчетно-кассовому центру", "Учатники процесса - Роли контрагента - Расчетно кассовые центры", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("привязка лс к ркц")]
        public virtual void ПривязкаЛсКРкц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("привязка лс к ркц", ((string[])(null)));
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.Given("пользователь выбирает РКЦ у которого контрагент \"Муниципальное бюджетное учрежден" +
                    "ие \"Никольская районная библиотека\"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 7
testRunner.And("пользователь в объектах РКЦ добавляет новый объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора \"те" +
                    "кущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("пользователь у этого объекта РКЦ заполняет поле Дата окончания действия договора " +
                    "\"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.And("пользователь у этого объекта РКЦ добавляет лицевой счет \"100000002\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.When("пользователь сохраняет этот объект РКЦ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 12
testRunner.Then("запись по этому объекту присутствует в списке объектов у этого РКЦ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 13
testRunner.Given("пользователь выбирает РКЦ у которого контрагент \"Администрация Алеутского муницип" +
                    "ального района\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 14
testRunner.Then("запись по этому объекту отсутствует в списке объектов у этого РКЦ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удаление лс из ркц")]
        public virtual void УдалениеЛсИзРкц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удаление лс из ркц", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
testRunner.Given("пользователь выбирает РКЦ у которого контрагент \"Муниципальное бюджетное учрежден" +
                    "ие \"Никольская районная библиотека\"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 19
testRunner.Given("пользователь в объектах РКЦ добавляет новый объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 20
testRunner.And("пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора \"те" +
                    "кущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("пользователь у этого объекта РКЦ заполняет поле Дата окончания действия договора " +
                    "\"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("пользователь у этого объекта РКЦ добавляет лицевой счет \"100000388\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.When("пользователь сохраняет этот объект РКЦ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.And("пользователь удаляет этот объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.Then("запись по этому объекту отсутствует в списке объектов у этого РКЦ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("обновление списка объектов ркц")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void ОбновлениеСпискаОбъектовРкц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("обновление списка объектов ркц", new string[] {
                        "ignore"});
#line 29
this.ScenarioSetup(scenarioInfo);
#line 30
testRunner.Given("пользователь выбирает РКЦ у которого контрагент \"Никольская районная библиотека\"\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 31
testRunner.Given("пользователь в объектах РКЦ добавляет новый объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 32
testRunner.And("пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора \"те" +
                    "кущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And("пользователь у этого объекта РКЦ заполняет поле Дата окончания действия договора " +
                    "\"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.And("пользователь у этого объекта РКЦ добавляет лицевой счет \"100000388\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.When("пользователь сохраняет этот объект РКЦ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 36
testRunner.And("пользользователь открывает раздел Объекты", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.Then("список с объектами автоматически обновляется", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
