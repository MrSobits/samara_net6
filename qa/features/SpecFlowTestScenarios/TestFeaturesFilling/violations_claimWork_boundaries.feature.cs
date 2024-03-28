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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Виды нарушений договора подряда\"")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаВидыНарушенийДоговораПодрядаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "violations_claimWork_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Виды нарушений договора подряда\"", "Справочники - ГЖИ - Виды нарушений договора подряда", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("создание нового вида нарушения договора подряда с пустыми полями")]
        public virtual void СозданиеНовогоВидаНарушенияДоговораПодрядаСПустымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание нового вида нарушения договора подряда с пустыми полями", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
testRunner.Given("пользователь добавляет новый вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 8
testRunner.When("пользователь сохраняет этот вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 9
testRunner.Then("запись по этому виду нарушения договора подряда присутствует в справочнике видов " +
                    "нарушений договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового вида нарушения договора подряда при вводе граничных услов" +
            "ий в 1000 знаков, Наименование")]
        public virtual void УдачноеСозданиеНовогоВидаНарушенияДоговораПодрядаПриВводеГраничныхУсловийВ1000ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового вида нарушения договора подряда при вводе граничных услов" +
                    "ий в 1000 знаков, Наименование", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
testRunner.Given("пользователь добавляет новый вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 13
testRunner.And("пользователь у этого вида нарушения договора подряда заполняет поле Наименование " +
                    "1000 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.When("пользователь сохраняет этот вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.Then("запись по этому виду нарушения договора подряда присутствует в справочнике видов " +
                    "нарушений договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание вида нарушения договора подряда при вводе граничных условий в " +
            "1001 знаков, Наименование")]
        public virtual void НеудачноеСозданиеВидаНарушенияДоговораПодрядаПриВводеГраничныхУсловийВ1001ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание вида нарушения договора подряда при вводе граничных условий в " +
                    "1001 знаков, Наименование", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
testRunner.Given("пользователь добавляет новый вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 19
testRunner.And("пользователь у этого вида нарушения договора подряда заполняет поле Наименование " +
                    "1001 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.When("пользователь сохраняет этот вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 21
testRunner.Then("запись по этому виду нарушения договора подряда отсутствует в справочнике видов н" +
                    "арушений договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 22
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 1000 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового вида нарушения договора подряда при вводе граничных услов" +
            "ий в 300 знаков, Код")]
        public virtual void УдачноеСозданиеНовогоВидаНарушенияДоговораПодрядаПриВводеГраничныхУсловийВ300ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового вида нарушения договора подряда при вводе граничных услов" +
                    "ий в 300 знаков, Код", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 27
testRunner.Given("пользователь добавляет новый вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 28
testRunner.And("пользователь у этого вида нарушения договора подряда заполняет поле Код 300 симво" +
                    "лов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.When("пользователь сохраняет этот вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 30
testRunner.Then("запись по этому виду нарушения договора подряда присутствует в справочнике видов " +
                    "нарушений договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание вида нарушения договора подряда при вводе граничных условий в " +
            "301 знаков, Код")]
        public virtual void НеудачноеСозданиеВидаНарушенияДоговораПодрядаПриВводеГраничныхУсловийВ301ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание вида нарушения договора подряда при вводе граничных условий в " +
                    "301 знаков, Код", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
testRunner.Given("пользователь добавляет новый вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 34
testRunner.And("пользователь у этого вида нарушения договора подряда заполняет поле Код 301 симво" +
                    "лов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.When("пользователь сохраняет этот вид нарушения договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 36
testRunner.Then("запись по этому виду нарушения договора подряда отсутствует в справочнике видов н" +
                    "арушений договора подряда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 37
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Код не должно превышать 300 сим" +
                    "волов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion