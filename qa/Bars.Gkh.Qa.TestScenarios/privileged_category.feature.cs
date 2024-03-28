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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Группы льготных категорий граждан\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляРазделаГруппыЛьготныхКатегорийГражданFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "privileged_category.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Группы льготных категорий граждан\"", "Справочники - Региональный фонд - Группы льготных категорий граждан", ProgrammingLanguage.CSharp, new string[] {
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
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " \"группа льготных категорий граждан тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01/01/2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 12
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"31/12/2016\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление группы льготных категорий граждан")]
        public virtual void УспешноеДобавлениеГруппыЛьготныхКатегорийГраждан()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление группы льготных категорий граждан", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 15
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 16
testRunner.Then("запись по этой группе льготных категорий граждан присутствует в справочнике групп" +
                    " льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление группы льготных категорий граждан")]
        public virtual void УспешноеУдалениеГруппыЛьготныхКатегорийГраждан()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление группы льготных категорий граждан", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 19
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 20
testRunner.And("пользователь удаляет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.Then("запись по этой группе льготных категорий граждан отсутствует в справочнике групп " +
                    "льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion