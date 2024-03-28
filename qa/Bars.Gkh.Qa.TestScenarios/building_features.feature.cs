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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для справочника \"Особые признаки строения\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляСправочникаОсобыеПризнакиСтроенияFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "building_features.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для справочника \"Особые признаки строения\"", "Справочники - Жилищно-коммунальное хозяйство - Особые признаки строения", ProgrammingLanguage.CSharp, new string[] {
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
testRunner.Given("пользователь добавляет новый особый признак строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("пользователь у этого особого признака строения заполняет поле Наименование \"тип о" +
                    "бслуживания тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("пользователь у этого особого признака строения заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление особого признака строения")]
        public virtual void УспешноеДобавлениеОсобогоПризнакаСтроения()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление особого признака строения", ((string[])(null)));
#line 10
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 11
testRunner.When("пользователь сохраняет этот особый признак строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 12
testRunner.Then("запись по этому особому признаку строения присутствует в справочнике особых призн" +
                    "аков строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление записи из справочника особых признаков строения")]
        public virtual void УспешноеУдалениеЗаписиИзСправочникаОсобыхПризнаковСтроения()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление записи из справочника особых признаков строения", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 15
testRunner.When("пользователь сохраняет этот особый признак строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 16
testRunner.And("пользователь удаляет этот особый признак строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 17
testRunner.Then("запись по этому особому признаку строения отсутствует в справочнике особых призна" +
                    "ков строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление дубля особого признака строения")]
        public virtual void УспешноеДобавлениеДубляОсобогоПризнакаСтроения()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление дубля особого признака строения", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 20
testRunner.When("пользователь сохраняет этот особый признак строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 21
testRunner.Given("пользователь добавляет новый особый признак строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 22
testRunner.And("пользователь у этого особого признака строения заполняет поле Наименование \"тип о" +
                    "бслуживания тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("пользователь у этого особого признака строения заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.When("пользователь сохраняет этот особый признак строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.Then("запись по этому особому признаку строения присутствует в справочнике особых призн" +
                    "аков строения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion