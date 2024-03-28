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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Кредитные организации\"")]
    public partial class ТесткейсыДляРазделаКредитныеОрганизацииFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "credit_org.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Кредитные организации\"", "Участники процесса - Контрагенты - Кредитные организации", ProgrammingLanguage.CSharp, ((string[])(null)));
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
testRunner.Given("пользователь добавляет новую кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование \"кредитная " +
                    "организация тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "city",
                        "street",
                        "houseNumber"});
            table1.AddRow(new string[] {
                        "kamchatka",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "test"});
            table1.AddRow(new string[] {
                        "sahalin",
                        "Костромское",
                        "Новая",
                        "1211"});
            table1.AddRow(new string[] {
                        "testregion",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "test999"});
#line 9
testRunner.And("пользователь выбирает Адрес", ((string)(null)), table1, "И ");
#line 15
testRunner.And("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление кредитной организации")]
        public virtual void УспешноеДобавлениеКредитнойОрганизации()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление кредитной организации", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 18
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 19
testRunner.Then("запись по этой кредитной организации присутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление кредитной организации")]
        public virtual void УспешноеУдалениеКредитнойОрганизации()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление кредитной организации", ((string[])(null)));
#line 21
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 22
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 23
testRunner.When("пользователь удаляет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.Then("запись по этой кредитной организации отсутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление дубля кредитной организации")]
        public virtual void НеудачноеДобавлениеДубляКредитнойОрганизации()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление дубля кредитной организации", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 27
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 28
testRunner.Given("пользователь добавляет новую кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 29
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование \"кредитная " +
                    "организация тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "city",
                        "street",
                        "houseNumber"});
            table2.AddRow(new string[] {
                        "kamchatka",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "test"});
            table2.AddRow(new string[] {
                        "sahalin",
                        "Костромское",
                        "Новая",
                        "1211"});
            table2.AddRow(new string[] {
                        "testregion",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "t12est"});
#line 31
testRunner.And("пользователь выбирает Адрес", ((string)(null)), table2, "И ");
#line 37
testRunner.And("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 40
testRunner.Then("падает ошибка с текстом \"Кредитная организация с таким ИНН уже существует\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
