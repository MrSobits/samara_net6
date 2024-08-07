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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для справочника \"Должности\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыГраничныхЗначенийДляСправочникаДолжностиFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "positions_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для справочника \"Должности\"", "Справочники - Общие - Должности", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при незаполненных обязательных полях : Наименовани" +
            "е")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриНезаполненныхОбязательныхПоляхНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при незаполненных обязательных полях : Наименовани" +
                    "е", new string[] {
                        "ignore"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 9
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 11
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 12
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при незаполненных обязательных полях : Код")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриНезаполненныхОбязательныхПоляхКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при незаполненных обязательных полях : Код", new string[] {
                        "ignore"});
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 18
testRunner.And("пользователь у этой должности заполняет поле Наименование 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 20
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 21
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление должности при вводе граничных условий в 300 знаков, Наименован" +
            "ие")]
        public virtual void УдачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление должности при вводе граничных условий в 300 знаков, Наименован" +
                    "ие", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 24
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 25
testRunner.And("пользователь у этой должности заполняет поле Наименование 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 28
testRunner.Then("запись по этой должности присутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при вводе граничных условий в 301 знаков, Наименов" +
            "ание")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при вводе граничных условий в 301 знаков, Наименов" +
                    "ание", new string[] {
                        "ignore"});
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 34
testRunner.And("пользователь у этой должности заполняет поле Наименование 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 37
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 38
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление должности при вводе граничных условий в 300 знаков, Код")]
        public virtual void УдачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ300ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление должности при вводе граничных условий в 300 знаков, Код", ((string[])(null)));
#line 40
this.ScenarioSetup(scenarioInfo);
#line 41
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 42
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 43
testRunner.And("пользователь у этой должности заполняет поле Код 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 44
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 45
testRunner.Then("запись по этой должности присутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при вводе граничных условий в 301 знаков, Код")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ301ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при вводе граничных условий в 301 знаков, Код", new string[] {
                        "ignore"});
#line 49
this.ScenarioSetup(scenarioInfo);
#line 50
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 51
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 52
testRunner.And("пользователь у этой должности заполняет поле Код 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 54
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 55
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление должности при вводе граничных условий в 300 знаков, Родительны" +
            "й")]
        public virtual void УдачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ300ЗнаковРодительный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление должности при вводе граничных условий в 300 знаков, Родительны" +
                    "й", ((string[])(null)));
#line 57
this.ScenarioSetup(scenarioInfo);
#line 58
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 59
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 60
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 61
testRunner.And("пользователь у этой должности заполняет поле Родительный 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 62
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 63
testRunner.Then("запись по этой должности присутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при вводе граничных условий в 301 знаков, Родитель" +
            "ный")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ301ЗнаковРодительный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при вводе граничных условий в 301 знаков, Родитель" +
                    "ный", new string[] {
                        "ignore"});
#line 67
this.ScenarioSetup(scenarioInfo);
#line 68
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 69
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 70
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 71
testRunner.And("пользователь у этой должности заполняет поле Родительный 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 72
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 73
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 74
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление должности при вводе граничных условий в 300 знаков, Дательный")]
        public virtual void УдачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ300ЗнаковДательный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление должности при вводе граничных условий в 300 знаков, Дательный", ((string[])(null)));
#line 76
this.ScenarioSetup(scenarioInfo);
#line 77
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 78
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 79
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 80
testRunner.And("пользователь у этой должности заполняет поле Дательный 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 81
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 82
testRunner.Then("запись по этой должности присутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при вводе граничных условий в 301 знаков, Дательны" +
            "й")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ301ЗнаковДательный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при вводе граничных условий в 301 знаков, Дательны" +
                    "й", new string[] {
                        "ignore"});
#line 86
this.ScenarioSetup(scenarioInfo);
#line 87
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 88
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 89
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 90
testRunner.And("пользователь у этой должности заполняет поле Дательный 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 91
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 92
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 93
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление должности при вводе граничных условий в 300 знаков, Винительны" +
            "й")]
        public virtual void УдачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ300ЗнаковВинительный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление должности при вводе граничных условий в 300 знаков, Винительны" +
                    "й", ((string[])(null)));
#line 95
this.ScenarioSetup(scenarioInfo);
#line 96
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 97
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 98
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 99
testRunner.And("пользователь у этой должности заполняет поле Винительный 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 100
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 101
testRunner.Then("запись по этой должности присутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при вводе граничных условий в 301 знаков, Винитель" +
            "ный")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ301ЗнаковВинительный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при вводе граничных условий в 301 знаков, Винитель" +
                    "ный", new string[] {
                        "ignore"});
#line 105
this.ScenarioSetup(scenarioInfo);
#line 106
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 107
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 108
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 109
testRunner.And("пользователь у этой должности заполняет поле Винительный 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 110
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 111
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 112
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление должности при вводе граничных условий в 300 знаков, Творительн" +
            "ый")]
        public virtual void УдачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ300ЗнаковТворительный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление должности при вводе граничных условий в 300 знаков, Творительн" +
                    "ый", ((string[])(null)));
#line 114
this.ScenarioSetup(scenarioInfo);
#line 115
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 116
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 117
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 118
testRunner.And("пользователь у этой должности заполняет поле Творительный 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 119
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 120
testRunner.Then("запись по этой должности присутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при вводе граничных условий в 301 знаков, Творител" +
            "ьный")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ301ЗнаковТворительный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при вводе граничных условий в 301 знаков, Творител" +
                    "ьный", new string[] {
                        "ignore"});
#line 124
this.ScenarioSetup(scenarioInfo);
#line 125
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 126
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 127
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 128
testRunner.And("пользователь у этой должности заполняет поле Творительный 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 129
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 130
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 131
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление должности при вводе граничных условий в 300 знаков, Предложный" +
            "")]
        public virtual void УдачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ300ЗнаковПредложный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление должности при вводе граничных условий в 300 знаков, Предложный" +
                    "", ((string[])(null)));
#line 133
this.ScenarioSetup(scenarioInfo);
#line 134
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 135
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 136
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 137
testRunner.And("пользователь у этой должности заполняет поле Предложный 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 138
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 139
testRunner.Then("запись по этой должности присутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление должности при вводе граничных условий в 301 знаков, Предложн" +
            "ый")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеДобавлениеДолжностиПриВводеГраничныхУсловийВ301ЗнаковПредложный()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление должности при вводе граничных условий в 301 знаков, Предложн" +
                    "ый", new string[] {
                        "ignore"});
#line 143
this.ScenarioSetup(scenarioInfo);
#line 144
testRunner.Given("пользователь добавляет новую должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 145
testRunner.And("пользователь у этой должности заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 146
testRunner.And("пользователь у этой должности заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 147
testRunner.And("пользователь у этой должности заполняет поле Предложный 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 148
testRunner.When("пользователь сохраняет эту должность", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 149
testRunner.Then("запись по этой должности отсутствует в справочнике должностей", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 150
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
