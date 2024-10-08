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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Материалы кровли\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляРазделаМатериалыКровлиFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "roofing_materials.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Материалы кровли\"", "Справочники - Жилищно-коммунальное хозяйство - Материалы кровли", ProgrammingLanguage.CSharp, new string[] {
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
testRunner.Given("пользователь добавляет новый материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("пользователь у этого материала кровли заполняет поле Наименование \"материал кровл" +
                    "и тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление материала кровли")]
        public virtual void УспешноеДобавлениеМатериалаКровли()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление материала кровли", ((string[])(null)));
#line 9
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 10
testRunner.When("пользователь сохраняет этот материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 11
testRunner.Then("запись по этому материалу кровли присутствует в справочнике материалов кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление записи из справочника материалов кровли")]
        public virtual void УспешноеУдалениеЗаписиИзСправочникаМатериаловКровли()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление записи из справочника материалов кровли", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 14
testRunner.When("пользователь сохраняет этот материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.And("пользователь удаляет этот материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.Then("запись по этому материалу кровли отсутствует в справочнике материалов кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление дубля материала кровли")]
        public virtual void УспешноеДобавлениеДубляМатериалаКровли()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление дубля материала кровли", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 19
testRunner.Given("пользователь добавляет новый материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 20
testRunner.And("пользователь у этого материала кровли заполняет поле Наименование \"материал кровл" +
                    "и тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.When("пользователь сохраняет этот материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 22
testRunner.Then("запись по этому материалу кровли присутствует в справочнике материалов кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
