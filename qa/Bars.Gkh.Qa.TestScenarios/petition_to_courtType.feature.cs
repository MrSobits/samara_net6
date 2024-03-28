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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Заявления в суд\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляРазделаЗаявленияВСудFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "petition_to_courtType.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Заявления в суд\"", "Справочники - Претензионная и исковая работа - Заявления в суд", ProgrammingLanguage.CSharp, new string[] {
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
#line 6
testRunner.Given("пользователь добавляет новое заявление в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("пользователь у этого заявления в суд заполняет поле Код \"1тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("пользователь у этого заявления в суд заполняет поле Краткое наименование \"1тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("пользователь у этого заявления в суд заполняет поле Полное наименование \"1тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление заявления в суд")]
        public virtual void УспешноеДобавлениеЗаявленияВСуд()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление заявления в суд", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 12
testRunner.When("пользователь сохраняет этот заявление в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 13
testRunner.Then("запись по этому заявлению в суд присутствует в справочнике заявлений в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление заявления в суд")]
        public virtual void УспешноеУдалениеЗаявленияВСуд()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление заявления в суд", ((string[])(null)));
#line 15
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 16
testRunner.When("пользователь сохраняет этот заявление в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.And("пользователь удаляет это заявление в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.Then("запись по этому заявлению в суд отсутствует в справочнике заявлений в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление дубля заявления в суд")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void УспешноеДобавлениеДубляЗаявленияВСуд()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление дубля заявления в суд", new string[] {
                        "ignore"});
#line 22
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 23
testRunner.When("пользователь сохраняет этот заявление в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.Given("пользователь добавляет новое заявление в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 25
testRunner.And("пользователь у этого заявления в суд заполняет поле Код \"1тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.And("пользователь у этого заявления в суд заполняет поле Краткое наименование \"1тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.And("пользователь у этого заявления в суд заполняет поле Полное наименование \"1тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.When("пользователь сохраняет этот заявление в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 29
testRunner.Then("запись по этому заявлению в суд присутствует в справочнике заявлений в суд", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion