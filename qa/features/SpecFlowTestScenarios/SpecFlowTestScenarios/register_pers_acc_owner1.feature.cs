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
    [NUnit.Framework.DescriptionAttribute("Реестр абонентов")]
    public partial class РеестрАбонентовFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "register_pers_acc_owner.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Реестр абонентов", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("права доступа")]
        [NUnit.Framework.TestCaseAttribute("Льготная категория", null)]
        public virtual void ПраваДоступа(string права, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("права доступа", exampleTags);
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
testRunner.When("пользователь переходит в Настройку ограничений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 8
testRunner.And("выбирает роль \"Администратор\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.Then(string.Format("в списке аттрибутов есть аттрибут на права {0}", права), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("указание фактического адреса нахождения абонента")]
        public virtual void УказаниеФактическогоАдресаНахожденияАбонента()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("указание фактического адреса нахождения абонента", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
testRunner.When("у абонента \"тип абонента\" = \"Счет физ.лица\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 18
testRunner.And("пользователь в карточке абонента в поле \"Фактический адрес нахождения\" производит" +
                    " вызов справочника ФИАС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.And("заполняет поле \"Регион\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.And("заполняем поле \"Населенный пункт\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("заполняем поле \"Улица\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("заполняем поле \"Дом\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("сохраняет данные", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.Then("в поле \"Фактический адрес нахождения\" сохраняется введенный адрес", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("указание Адреса за пределами субъекта для абонента")]
        public virtual void УказаниеАдресаЗаПределамиСубъектаДляАбонента()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("указание Адреса за пределами субъекта для абонента", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
testRunner.When("у абонента \"тип абонента\" = \"Счет физ.лица\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 29
testRunner.And("пользователь в карточке абонента в поле \"Адрес за пределами субъекта\" заполняет т" +
                    "екстовое поле значением \"г.Казань,- ул.Спартаковская, д.52\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("сохраняет данные", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.Then("в поле \"Адрес за пределами субъекта\" сохраняется введенный адрес", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("блокировка одновременного заполнения полей")]
        [NUnit.Framework.TestCaseAttribute("Фактический адрес нахождения", "Адрес за пределами субъекта", null)]
        [NUnit.Framework.TestCaseAttribute("Адрес за пределами субъекта", "Фактический адрес нахождения", null)]
        [NUnit.Framework.TestCaseAttribute("Фактический адрес нахождения", "Электронная почта", null)]
        [NUnit.Framework.TestCaseAttribute("Адрес за пределами субъекта", "Электронная почта", null)]
        [NUnit.Framework.TestCaseAttribute("Электронная почта", "Фактический адрес нахождения", null)]
        [NUnit.Framework.TestCaseAttribute("Электронная почта", "Адрес за пределами субъекта", null)]
        public virtual void БлокировкаОдновременногоЗаполненияПолей(string поле1, string поле2, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("блокировка одновременного заполнения полей", exampleTags);
#line 34
this.ScenarioSetup(scenarioInfo);
#line 35
testRunner.When("у абонента \"тип абонента\" = \"Счет физ.лица\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 36
testRunner.And(string.Format("пользователь заполняет поле \"{0}\"", поле1), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.Then(string.Format("поле \"{0}\" недоступно для заполнения", поле2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("указание Электронной почты абонента")]
        public virtual void УказаниеЭлектроннойПочтыАбонента()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("указание Электронной почты абонента", ((string[])(null)));
#line 49
this.ScenarioSetup(scenarioInfo);
#line 50
testRunner.When("у абонента \"тип абонента\" = \"Счет физ.лица\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 51
testRunner.And("пользователь в карточке абонента в поле \"Электронная почта\" вводит значение \"test" +
                    "@mail.ru\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 52
testRunner.And("сохраняет данные", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.Then("в поле \"Электронная почта\" сохраняется введенный адрес электронной почты", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("указание Адреса за пределами субъекта в поле \"Адрес для корреспонденции\"")]
        public virtual void УказаниеАдресаЗаПределамиСубъектаВПолеАдресДляКорреспонденции()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("указание Адреса за пределами субъекта в поле \"Адрес для корреспонденции\"", ((string[])(null)));
#line 56
this.ScenarioSetup(scenarioInfo);
#line 57
testRunner.When("у абонента \"тип абонента\" = \"Счет юр.лица\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 58
testRunner.And("пользователь в поле \"Адрес для корреспонденции\" выбирает значение \"Адрес за преде" +
                    "лами субъекта\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 59
testRunner.Then("значение в поле \"Адрес для корреспонденции\" = значение из поля \"Адрес за пределам" +
                    "и субъекта\" по контрагенту из поля \"Контрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("указание E-mail в поле \"Адрес для корреспонденции\"")]
        public virtual void УказаниеE_MailВПолеАдресДляКорреспонденции()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("указание E-mail в поле \"Адрес для корреспонденции\"", ((string[])(null)));
#line 62
this.ScenarioSetup(scenarioInfo);
#line 63
testRunner.When("у абонента \"тип абонента\" = \"Счет юр.лица\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 64
testRunner.And("пользователь в поле \"Адрес для корреспонденции\" выбирает значение \"E-mail\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 65
testRunner.Then("значение в поле \"Адрес для корреспонденции\" = значение из поля \"E-mail\" по контра" +
                    "генту из поля \"Контрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion