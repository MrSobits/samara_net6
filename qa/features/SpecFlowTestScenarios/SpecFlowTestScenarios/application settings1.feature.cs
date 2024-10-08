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
    [NUnit.Framework.DescriptionAttribute("доработки в Администрировании - Параметры приложения")]
    public partial class ДоработкиВАдминистрировании_ПараметрыПриложенияFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "application settings.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "доработки в Администрировании - Параметры приложения", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("проверка наличия поля \"ОКАТО региона\"")]
        public virtual void ПроверкаНаличияПоляОКАТОРегиона()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка наличия поля \"ОКАТО региона\"", ((string[])(null)));
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.When("пользователь переходит в Параметры приложения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 7
testRunner.Then("в списке аттрибутов есть \"ОКАТО региона\" числового типа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"ОКАТО региона\"")]
        [NUnit.Framework.TestCaseAttribute("10", "сохранение информации", null)]
        [NUnit.Framework.TestCaseAttribute("9", "сохранение информации", null)]
        [NUnit.Framework.TestCaseAttribute("11", "информация не сохраняется", null)]
        public virtual void ЗаполнениеПоляОКАТОРегиона(string количество_Знаков, string действие, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"ОКАТО региона\"", exampleTags);
#line 10
this.ScenarioSetup(scenarioInfo);
#line 11
testRunner.When(string.Format("пользователь заполняет поле \"ОКАТО региона\" значением с количеством знаков {0}", количество_Знаков), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 12
testRunner.And("сохраняет данные", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 13
testRunner.Then(string.Format("система проихводит действие по обработке информации {0}", действие), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 14
testRunner.When(string.Format("пользователь заполняет поле \"ОКАТО региона\" значением, которое начинается с 0, с " +
                        "количеством знаков {0}", количество_Знаков), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.Then(string.Format("система проихводит действие по обработке информации {0}", действие), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
