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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Материалы кровли\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаМатериалыКровлиFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "roofing_materials_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Материалы кровли\"", "Справочники - Жилищно-коммунальное хозяйство - Материалы кровли", ProgrammingLanguage.CSharp, new string[] {
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("создание нового материала кровли с пустыми полями")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void СозданиеНовогоМатериалаКровлиСПустымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание нового материала кровли с пустыми полями", new string[] {
                        "ignore"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
testRunner.Given("пользователь добавляет новый материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 9
testRunner.When("пользователь сохраняет этот материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 10
testRunner.Then("запись по этому материалу кровли присутствует в справочнике материалов кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового материала кровли при вводе граничных условий в 300 знаков" +
            ", Наименование")]
        public virtual void УдачноеСозданиеНовогоМатериалаКровлиПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового материала кровли при вводе граничных условий в 300 знаков" +
                    ", Наименование", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 13
testRunner.Given("пользователь добавляет новый материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 14
testRunner.And("пользователь у этого материала кровли заполняет поле Наименование 300 символов \"1" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.When("пользователь сохраняет этот материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 16
testRunner.Then("запись по этому материалу кровли присутствует в справочнике материалов кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового материала кровли при вводе граничных условий в 301 знак" +
            "ов, Наименование")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовогоМатериалаКровлиПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового материала кровли при вводе граничных условий в 301 знак" +
                    "ов, Наименование", new string[] {
                        "ignore"});
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
testRunner.Given("пользователь добавляет новый материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 22
testRunner.And("пользователь у этого материала кровли заполняет поле Наименование 301 символов \"1" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.When("пользователь сохраняет этот материал кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.Then("запись по этому материалу кровли отсутствует в справочнике материалов кровли", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 25
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 300 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
