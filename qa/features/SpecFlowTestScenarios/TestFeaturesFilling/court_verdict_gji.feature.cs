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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Решения суда\"")]
    public partial class ТесткейсыДляРазделаРешенияСудаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "court_verdict_gji.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Решения суда\"", "Справочники - ГЖИ - Решения суда", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 7
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 8
testRunner.And("пользователь у этого решения суда заполняет поле Наименование \"решение суда тест\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("пользователь у этого решения суда заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление вида суда")]
        public virtual void УспешноеДобавлениеВидаСуда()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление вида суда", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 12
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 13
testRunner.Then("запись по этому решению суда присутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление вида суда")]
        public virtual void УспешноеУдалениеВидаСуда()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление вида суда", ((string[])(null)));
#line 15
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 16
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.And("пользователь удаляет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.Then("запись по этому решению отсутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление дубля вида суда")]
        public virtual void УспешноеДобавлениеДубляВидаСуда()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление дубля вида суда", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 21
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 22
testRunner.Given("пользователь добавляет новое решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 23
testRunner.And("пользователь у этого решения суда заполняет поле Наименование \"решение суда тест\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.And("пользователь у этого решения суда заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.When("пользователь сохраняет это решение суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 26
testRunner.Then("запись по этому решению присутствует в справочнике решений суда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
