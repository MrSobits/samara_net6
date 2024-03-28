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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Типы домов\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаТипыДомовFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "real_estate_type_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Типы домов\"", "Справочники - Общие - Типы домов", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("создание нового Типа дома с пустыми полями")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void СозданиеНовогоТипаДомаСПустымиПолями()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание нового Типа дома с пустыми полями", new string[] {
                        "ignore"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 9
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 10
testRunner.Then("запись по этому Типу дома отсутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 11
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового Типа дома при вводе граничных условий в 300 знаков, Наиме" +
            "нование")]
        public virtual void УдачноеСозданиеНовогоТипаДомаПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового Типа дома при вводе граничных условий в 300 знаков, Наиме" +
                    "нование", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 15
testRunner.And("пользователь у этого Типа дома заполняет поле Наименование 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.Then("запись по этому Типу дома присутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового Типа дома при вводе граничных условий в 301 знаков, Наи" +
            "менование")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовогоТипаДомаПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового Типа дома при вводе граничных условий в 301 знаков, Наи" +
                    "менование", new string[] {
                        "ignore"});
#line 21
this.ScenarioSetup(scenarioInfo);
#line 22
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 23
testRunner.And("пользователь у этого Типа дома заполняет поле Наименование 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.Then("запись по этому Типу дома отсутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 26
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 300 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное создание нового Типа дома при вводе граничных условий в 100 знаков, Код")]
        public virtual void УдачноеСозданиеНовогоТипаДомаПриВводеГраничныхУсловийВ100ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное создание нового Типа дома при вводе граничных условий в 100 знаков, Код", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 29
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 30
testRunner.And("пользователь у этого Типа дома заполняет поле Наименование \"тип дома тестовый 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("пользователь у этого Типа дома заполняет поле Код 100 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 33
testRunner.Then("запись по этому Типу дома присутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное создание нового Типа дома при вводе граничных условий в 101 знаков, Код" +
            "")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСозданиеНовогоТипаДомаПриВводеГраничныхУсловийВ101ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное создание нового Типа дома при вводе граничных условий в 101 знаков, Код" +
                    "", new string[] {
                        "ignore"});
#line 37
this.ScenarioSetup(scenarioInfo);
#line 38
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 39
testRunner.And("пользователь у этого Типа дома заполняет поле Наименование \"тип дома тестовый 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.And("пользователь у этого Типа дома заполняет поле Код 101 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 42
testRunner.Then("запись по этому Типу дома отсутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 43
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Код не должно превышать 100 сим" +
                    "волов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление Общей характеристики типа дома с пустым полем, Минимальное зн" +
            "ачение")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void УспешноеДобавлениеОбщейХарактеристикиТипаДомаСПустымПолемМинимальноеЗначение()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление Общей характеристики типа дома с пустым полем, Минимальное зн" +
                    "ачение", new string[] {
                        "ignore"});
#line 47
this.ScenarioSetup(scenarioInfo);
#line 48
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 49
testRunner.And("пользователь у этого Типа дома заполняет поле Наименование \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 50
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 51
testRunner.Given("пользователь добавляет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 52
testRunner.And("заполняет у этого Общего параметра поле Наименование \"тест общая характер\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.And("заполняет у этого Общего параметра поле Максимальное значение \"12312\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 54
testRunner.When("пользователь сохраняет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 55
testRunner.Then("запись по этой Общей характеристике отсутствует в Типе дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 56
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Минимальное значение\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление Общей характеристики типа дома с пустым полем, Максимальное з" +
            "начение")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void УспешноеДобавлениеОбщейХарактеристикиТипаДомаСПустымПолемМаксимальноеЗначение()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление Общей характеристики типа дома с пустым полем, Максимальное з" +
                    "начение", new string[] {
                        "ignore"});
#line 60
this.ScenarioSetup(scenarioInfo);
#line 61
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 62
testRunner.And("пользователь у этого Типа дома заполняет поле Наименование \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 63
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 64
testRunner.Given("пользователь добавляет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 65
testRunner.And("заполняет у этого Общего параметра поле Наименование \"тест общая характер\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 66
testRunner.And("заполняет у этого Общего параметра поле Минимальное значение \"12312\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 67
testRunner.When("пользователь сохраняет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 68
testRunner.Then("запись по этой Общей характеристике присутствует в Типе дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion