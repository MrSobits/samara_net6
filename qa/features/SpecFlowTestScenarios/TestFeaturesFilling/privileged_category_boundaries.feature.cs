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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Группы льготных категорий граждан\"")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаГруппыЛьготныхКатегорийГражданFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "privileged_category_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Группы льготных категорий граждан\"", "Справочники - Региональный фонд - Группы льготных категорий граждан", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("неудачное добавление группы льготных категорий граждан при незаполненных обязател" +
            "ьных полях")]
        public virtual void НеудачноеДобавлениеГруппыЛьготныхКатегорийГражданПриНезаполненныхОбязательныхПолях()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление группы льготных категорий граждан при незаполненных обязател" +
                    "ьных полях", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 8
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 9
testRunner.Then("запись по этой группе льготных категорий граждан не сохраняется и падает ошибка с" +
                    " текстом \"Не заполнены обязательные поля: Код Наименование Процент льготы Предел" +
                    "ьное значение площади Действует с\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление группы льготных категорий граждан при вводе граничных условий " +
            "в 300 знаков, Код")]
        public virtual void УдачноеДобавлениеГруппыЛьготныхКатегорийГражданПриВводеГраничныхУсловийВ300ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление группы льготных категорий граждан при вводе граничных условий " +
                    "в 300 знаков, Код", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 13
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код 300 симв" +
                    "олов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " \"группа льготных категорий граждан тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 17
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01.01.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"01.02.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 20
testRunner.Then("запись по этой группе льготных категорий граждан присутствует в справочнике групп" +
                    " льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление группы льготных категорий граждан при вводе граничных услови" +
            "й в 301 знаков, Код")]
        public virtual void НеудачноеДобавлениеГруппыЛьготныхКатегорийГражданПриВводеГраничныхУсловийВ301ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление группы льготных категорий граждан при вводе граничных услови" +
                    "й в 301 знаков, Код", ((string[])(null)));
#line 22
this.ScenarioSetup(scenarioInfo);
#line 23
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 24
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код 301 симв" +
                    "олов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " \"группа льготных категорий граждан тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01.01.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"01.02.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 31
testRunner.Then("запись по этой группе льготных категорий граждан не сохраняется и падает ошибка с" +
                    " текстом \"Не заполнены обязательные поля: Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление группы льготных категорий граждан при вводе граничных условий " +
            "в 300 знаков, Наименование")]
        public virtual void УдачноеДобавлениеГруппыЛьготныхКатегорийГражданПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление группы льготных категорий граждан при вводе граничных условий " +
                    "в 300 знаков, Наименование", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line 34
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 35
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " 300 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01.01.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"01.02.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 42
testRunner.Then("запись по этой группе льготных категорий граждан присутствует в справочнике групп" +
                    " льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление группы льготных категорий граждан при вводе граничных услови" +
            "й в 301 знаков, Наименование")]
        public virtual void НеудачноеДобавлениеГруппыЛьготныхКатегорийГражданПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление группы льготных категорий граждан при вводе граничных услови" +
                    "й в 301 знаков, Наименование", ((string[])(null)));
#line 44
this.ScenarioSetup(scenarioInfo);
#line 45
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 46
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " 301 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 49
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 50
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01.01.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 51
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"01.02.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 52
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 53
testRunner.Then("запись по этой группе льготных категорий граждан не сохраняется и падает ошибка с" +
                    " текстом \"Не заполнены обязательные поля: Наименование\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление группы льготных категорий граждан при вводе допустимого формат" +
            "а даты, Действует с")]
        public virtual void УдачноеДобавлениеГруппыЛьготныхКатегорийГражданПриВводеДопустимогоФорматаДатыДействуетС()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление группы льготных категорий граждан при вводе допустимого формат" +
                    "а даты, Действует с", ((string[])(null)));
#line 55
this.ScenarioSetup(scenarioInfo);
#line 56
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 57
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 58
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " \"группа льготных категорий граждан тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 59
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 60
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 61
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01.01.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 62
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"01.02.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 63
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 64
testRunner.Then("запись по этой группе льготных категорий граждан присутствует в справочнике групп" +
                    " льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление группы льготных категорий граждан при вводе недопустимого фо" +
            "рмата даты, Действует с")]
        public virtual void НеудачноеДобавлениеГруппыЛьготныхКатегорийГражданПриВводеНедопустимогоФорматаДатыДействуетС()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление группы льготных категорий граждан при вводе недопустимого фо" +
                    "рмата даты, Действует с", ((string[])(null)));
#line 66
this.ScenarioSetup(scenarioInfo);
#line 67
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 68
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 69
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " \"группа льготных категорий граждан тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 70
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 71
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 72
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01.01.20151\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 73
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"01.02.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 74
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 75
testRunner.Then("запись по этой группе льготных категорий граждан не сохраняется и падает ошибка с" +
                    " текстом \"Не заполнены обязательные поля: Действует с\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление группы льготных категорий граждан при вводе допустимого формат" +
            "а даты, Действует по")]
        public virtual void УдачноеДобавлениеГруппыЛьготныхКатегорийГражданПриВводеДопустимогоФорматаДатыДействуетПо()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление группы льготных категорий граждан при вводе допустимого формат" +
                    "а даты, Действует по", ((string[])(null)));
#line 77
this.ScenarioSetup(scenarioInfo);
#line 78
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 79
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 80
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " \"группа льготных категорий граждан тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 81
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 82
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 83
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01.01.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 84
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"01.02.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 85
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 86
testRunner.Then("запись по этой группе льготных категорий граждан присутствует в справочнике групп" +
                    " льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление группы льготных категорий граждан при вводе недопустимого фо" +
            "рмата даты, Действует по")]
        public virtual void НеудачноеДобавлениеГруппыЛьготныхКатегорийГражданПриВводеНедопустимогоФорматаДатыДействуетПо()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление группы льготных категорий граждан при вводе недопустимого фо" +
                    "рмата даты, Действует по", ((string[])(null)));
#line 88
this.ScenarioSetup(scenarioInfo);
#line 89
testRunner.Given("пользователь добавляет новую группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 90
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 91
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Наименование" +
                    " \"группа льготных категорий граждан тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 92
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Процент льго" +
                    "ты \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 93
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Предельное з" +
                    "начение площади \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 94
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует с " +
                    "\"01.01.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 95
testRunner.And("пользователь у этой группы льготных категорий граждан заполняет поле Действует по" +
                    " \"01.02.20151\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 96
testRunner.When("пользователь сохраняет эту группу льготных категорий граждан", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 97
testRunner.Then("запись по этой группе льготных категорий граждан не сохраняется и падает ошибка с" +
                    " текстом \"Не заполнены обязательные поля: Действует по\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
