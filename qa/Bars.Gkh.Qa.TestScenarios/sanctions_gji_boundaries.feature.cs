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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Виды санкций\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаВидыСанкцийFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "sanctions_gji_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Виды санкций\"", "Справочники - ГЖИ - Виды санкций", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("создание нового вида санкций с пустыми полями")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void СозданиеНовогоВидаСанкцийСПустымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание нового вида санкций с пустыми полями", new string[] {
                        "ignore"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
testRunner.Given("пользователь добавляет новый вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 9
testRunner.When("пользователь сохраняет этот вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 10
testRunner.Then("запись по этому виду санкций присутствует в справочнике видов санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового вида санкций при вводе граничных условий в 300 знаков, На" +
            "именование")]
        public virtual void УдачноеСозданиеНовогоВидаСанкцийПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового вида санкций при вводе граничных условий в 300 знаков, На" +
                    "именование", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 13
testRunner.Given("пользователь добавляет новый вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 14
testRunner.And("пользователь у этого вида санкций заполняет поле Наименование 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.And("пользователь у этого вида санкций заполняет поле Код \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.When("пользователь сохраняет этот вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.Then("запись по этому виду санкций присутствует в справочнике видов санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового вида санкций при вводе граничных условий в 301 знаков, " +
            "Наименование")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовогоВидаСанкцийПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового вида санкций при вводе граничных условий в 301 знаков, " +
                    "Наименование", new string[] {
                        "ignore"});
#line 21
this.ScenarioSetup(scenarioInfo);
#line 22
testRunner.Given("пользователь добавляет новый вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 23
testRunner.And("пользователь у этого вида санкций заполняет поле Наименование 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.When("пользователь сохраняет этот вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.Then("запись по этому виду санкций отсутствует в справочнике видов санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 26
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 300 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового вида санкций при вводе граничных условий в 300 знаков, Ко" +
            "д")]
        public virtual void УдачноеСозданиеНовогоВидаСанкцийПриВводеГраничныхУсловийВ300ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового вида санкций при вводе граничных условий в 300 знаков, Ко" +
                    "д", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
testRunner.Given("пользователь добавляет новый вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 32
testRunner.And("пользователь у этого вида санкций заполняет поле Код 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And("пользователь у этого вида санкций заполняет поле Наименование \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.When("пользователь сохраняет этот вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 35
testRunner.Then("запись по этому виду санкций присутствует в справочнике видов санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового вида санкций при вводе граничных условий в 301 знаков, " +
            "Код")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовогоВидаСанкцийПриВводеГраничныхУсловийВ301ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового вида санкций при вводе граничных условий в 301 знаков, " +
                    "Код", new string[] {
                        "ignore"});
#line 39
this.ScenarioSetup(scenarioInfo);
#line 40
testRunner.Given("пользователь добавляет новый вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 41
testRunner.And("пользователь у этого вида санкций заполняет поле Код 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.When("пользователь сохраняет этот вид санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 43
testRunner.Then("запись по этому виду санкций отсутствует в справочнике видов санкций", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 44
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Код не должно превышать 300 сим" +
                    "волов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion