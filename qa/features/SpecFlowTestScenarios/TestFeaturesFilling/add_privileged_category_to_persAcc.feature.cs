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
    [NUnit.Framework.DescriptionAttribute("добавление категории льготы к лицевому счету")]
    public partial class ДобавлениеКатегорииЛьготыКЛицевомуСчетуFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "add_privileged_category_to_persAcc.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "добавление категории льготы к лицевому счету", "Региональный фонд - Счета - Ресстр лицевых счетов - Карточка лицевого счета - вкл" +
                    "адка \"Категория льготы\"", ProgrammingLanguage.CSharp, ((string[])(null)));
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
testRunner.Given("пользователь добавляет новую категорию льготы к лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление категории льготы")]
        public virtual void УспешноеДобавлениеКатегорииЛьготы()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление категории льготы", ((string[])(null)));
#line 8
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 9
testRunner.Given("пользователь у этой категории льготы заполняет поле Льготная категория \"льгота\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 10
testRunner.And("пользователь у этой категории льготы заполняет поле Действует с \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.And("пользователь у этой категории льготы заполняет поле Действует по \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 12
testRunner.When("пользователь сохраняет эту категорию льготы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 13
testRunner.Then("запись по этой категории льготы присутствует в списке категорий льгот у этого лиц" +
                    "евого счета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление категории льготы")]
        public virtual void УспешноеУдалениеКатегорииЛьготы()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление категории льготы", ((string[])(null)));
#line 15
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 16
testRunner.Given("пользователь у этой категории льготы заполняет поле Льготная категория \"льгота\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 17
testRunner.And("пользователь у этой категории льготы заполняет поле Действует с \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.And("пользователь у этой категории льготы заполняет поле Действует по \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.When("пользователь сохраняет эту категорию льготы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 20
testRunner.And("пользователь удаляет эту категорию льготы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.Then("запись по этой категории льготы отсутствует в списке категорий льгот у этого лице" +
                    "вого счета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление категории льготы с пустым полем \"Льготная категория\"")]
        public virtual void НеудачноеДобавлениеКатегорииЛьготыСПустымПолемЛьготнаяКатегория()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление категории льготы с пустым полем \"Льготная категория\"", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 24
testRunner.Given("пользователь у этой категории льготы заполняет поле Действует с \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 25
testRunner.And("пользователь у этой категории льготы заполняет поле Действует по \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.When("пользователь сохраняет эту категорию льготы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 27
testRunner.And("пользователь удаляет эту категорию льготы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.Then("запись по этой категории льготы отсутствует в списке категорий льгот у этого лице" +
                    "вого счета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 29
testRunner.And("выходит сообщение с текстом \"Не заполнены обязательные поля: Льготная категория\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление категории льготы с пустыми полями")]
        public virtual void НеудачноеДобавлениеКатегорииЛьготыСПустымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление категории льготы с пустыми полями", ((string[])(null)));
#line 31
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 32
testRunner.When("пользователь сохраняет эту категорию льготы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 33
testRunner.And("пользователь удаляет эту категорию льготы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.Then("запись по этой категории льготы отсутствует в списке категорий льгот у этого лице" +
                    "вого счета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 35
testRunner.And("выходит сообщение с текстом \"Не заполнены обязательные поля: Льготная категория, " +
                    "Действует с\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion