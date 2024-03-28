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
    [NUnit.Framework.DescriptionAttribute("смена абонента у лицевого счета")]
    public partial class СменаАбонентаУЛицевогоСчетаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "change_personal_account_owner.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "смена абонента у лицевого счета", "Регионалтный фонд - Счета - Реестр лицевых счетов", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        "kamchatka",
                        "Многоквартирный",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "test"});
            table1.AddRow(new string[] {
                        "sahalin",
                        "Многоквартирный",
                        "Костромское",
                        "Новая",
                        "1211"});
#line 6
testRunner.Given("в реестр жилых домов добавлен новый дом", ((string)(null)), table1, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "RoomNum",
                        "Area",
                        "LivingArea",
                        "Type",
                        "OwnershipType"});
            table2.AddRow(new string[] {
                        "1",
                        "51",
                        "35",
                        "Жилое",
                        "Частная"});
#line 11
testRunner.And("у этого дома добавлено помещение", ((string)(null)), table2, "И ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Surname",
                        "FirstName",
                        "secondName",
                        "BirthDate",
                        "IdentityType",
                        "IdentitySerial",
                        "IdentityNumber"});
            table3.AddRow(new string[] {
                        "1",
                        "1",
                        "1",
                        "12.06.1961",
                        "10",
                        "9206",
                        "612345"});
            table3.AddRow(new string[] {
                        "2",
                        "2",
                        "2",
                        "12.06.1961",
                        "10",
                        "9206",
                        "612345"});
#line 15
testRunner.And("добавлен абонент типа Счет физ.лица", ((string)(null)), table3, "И ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "OpenDate",
                        "RealityObjectAddress",
                        "RoomInfo"});
            table4.AddRow(new string[] {
                        "kamchatka",
                        "01.01.2015",
                        "с. Никольское, ул. 50 лет Октября, д. test",
                        "1"});
            table4.AddRow(new string[] {
                        "sahalin",
                        "01.01.2015",
                        "с. Костромское, ул. Новая, д. 1211",
                        "1"});
#line 25
testRunner.And("добавлено помещение абоненту типа Счет физ.лица с Фио 111", ((string)(null)), table4, "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("смена абонента")]
        public virtual void СменаАбонента()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("смена абонента", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 31
testRunner.When("пользователь выбирает этот лицевой счет", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 32
testRunner.And("пользователь выбирает действие Смена абонента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And("пользователь в смене абонента заполняет поле Новый владелец абонентом 2 2 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.And("пользователь в смене абонента заполняет поле Дата начала действия \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.And("пользователь сохраняет эту смену абонента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.Then("в истории изменений этого лицевого счета присутствует запись", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 37
testRunner.And("у этой записи Наименованием параметра = смена абонента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("у этой записи Описание измененного атрибута = Смена абонента ЛС с \"1 1 1\" на \"2 2" +
                    " 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.And("у этой записи Значение =", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.And("у этой записи Дата начала действия значения = \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.And("у этой записи Дата установки значения = \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.And("у этой записи заполнено поле Пользователь", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 43
testRunner.And("у к этому лицевому счету привязан абонент 2 2 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
