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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Контрагенты\"")]
    public partial class ТесткейсыДляРазделаКонтрагентыFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "contragent.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Контрагенты\"", "Участники процесса - Контрагенты - Контрагенты", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "place",
                        "street",
                        "houseNumber"});
            table1.AddRow(new string[] {
                        "kamchatka",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "75"});
#line 7
testRunner.Given("добавлен жилой дом", ((string)(null)), table1, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "OkopfCode"});
            table2.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 11
testRunner.Given("добавлена организационно-правовая форма", ((string)(null)), table2, "Дано ");
#line 15
testRunner.Given("пользователь добавляет нового контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 16
testRunner.And("пользователь у этого контрагента заполняет поле Наименование \"тест111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 17
testRunner.And("пользователь у этого контрагента заполняет поле Организационно-правовая форма", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.And("пользователь у этого контрагента заполняет поле ИНН \"5702001741\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.And("пользователь у этого контрагента заполняет поле КПП \"771501001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.And("пользователь у этого контрагента заполняет поле Юридический адрес этим жилым домо" +
                    "м", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("пользователь у этого контрагента заполняет поле Фактический адрес этим жилым домо" +
                    "м", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("пользователь у этого контрагента заполняет поле Адрес за пределами субъекта \"Адре" +
                    "с за пределами субъекта\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("пользователь у этого контрагента заполняет поле ОГРН \"1025700517292\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление контрагента c заполненными обязательными полями")]
        public virtual void УспешноеДобавлениеКонтрагентаCЗаполненнымиОбязательнымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление контрагента c заполненными обязательными полями", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 27
testRunner.When("пользователь сохраняет этого контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 28
testRunner.Then("запись по этой форме присутствует в реестре контрагентов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление контрагента с заполненным доп.полем ИНН")]
        public virtual void УспешноеДобавлениеКонтрагентаСЗаполненнымДоп_ПолемИНН()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление контрагента с заполненным доп.полем ИНН", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 31
testRunner.When("пользователь заполняет поле ИНН \"5702001741\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 32
testRunner.And("пользователь сохраняет этого контрагента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.Then("запись по этой форме присутствует в реестре контрагентов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
