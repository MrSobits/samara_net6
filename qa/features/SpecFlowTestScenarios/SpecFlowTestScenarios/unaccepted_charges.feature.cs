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
namespace SpecFlowTestScenarios
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.3.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("проверки по Реестру неподтвержденных начислений")]
    public partial class ПроверкиПоРееструНеподтвержденныхНачисленийFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "unaccepted_charges.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "проверки по Реестру неподтвержденных начислений", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Проверка наличия аттрибутов в реестре неподтвержденных начислений")]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым произведен расчет", null)]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым есть перерасчет", null)]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым есть пени", null)]
        [NUnit.Framework.TestCaseAttribute("итого начислено", null)]
        [NUnit.Framework.TestCaseAttribute("итого перерасчет", null)]
        [NUnit.Framework.TestCaseAttribute("итого пени", null)]
        [NUnit.Framework.TestCaseAttribute("примечание", null)]
        public virtual void ПроверкаНаличияАттрибутовВРеестреНеподтвержденныхНачислений(string аттрибуты, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Проверка наличия аттрибутов в реестре неподтвержденных начислений", exampleTags);
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 10
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.When("пользователь заходит в реестр неподтвержденных начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 12
testRunner.Then(string.Format("в реестре имеются аттрибуты {0}", аттрибуты), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка расчета значения в поле \"общее количество лс, по которым произведен расч" +
            "ет\"")]
        public virtual void ПроверкаРасчетаЗначенияВПолеОбщееКоличествоЛсПоКоторымПроизведенРасчет()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка расчета значения в поле \"общее количество лс, по которым произведен расч" +
                    "ет\"", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 27
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 28
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка расчета значения в поле \"общее количество лс, по которым есть перерасчет" +
            "\"")]
        public virtual void ПроверкаРасчетаЗначенияВПолеОбщееКоличествоЛсПоКоторымЕстьПерерасчет()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка расчета значения в поле \"общее количество лс, по которым есть перерасчет" +
                    "\"", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 34
testRunner.And("система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.Given("пользователь находится в реестре неподтвержденных начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 36
testRunner.When("в реестре есть аттрибут \"общее количество лс, по которым есть перерасчет\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Если ");
#line 37
testRunner.Then("по каждому неподтвержденному начислению значение аттрибута равно: считается колич" +
                    "ество ЛС, у которых признак операции перерасчета = 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка расчета значения в поле \"общее количество лс, по которым есть пени\" в ре" +
            "естре")]
        public virtual void ПроверкаРасчетаЗначенияВПолеОбщееКоличествоЛсПоКоторымЕстьПениВРеестре()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка расчета значения в поле \"общее количество лс, по которым есть пени\" в ре" +
                    "естре", ((string[])(null)));
#line 41
this.ScenarioSetup(scenarioInfo);
#line 42
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 43
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 44
testRunner.Given("пользователь находится в реестре неподтвержденных начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 45
testRunner.When("в реестре есть аттрибут \"общее количество лс, по которым есть пени\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Если ");
#line 46
testRunner.Then("по каждому неподтвержденному начислению значение аттрибута равно: считается колич" +
                    "ество ЛС, у которых сумма пени не равна 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка расчета значения в поле \"итого начислено\" в реестре")]
        public virtual void ПроверкаРасчетаЗначенияВПолеИтогоНачисленоВРеестре()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка расчета значения в поле \"итого начислено\" в реестре", ((string[])(null)));
#line 50
this.ScenarioSetup(scenarioInfo);
#line 51
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 52
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка расчета значения в поле \"итого перерасчет\" в реестре")]
        public virtual void ПроверкаРасчетаЗначенияВПолеИтогоПерерасчетВРеестре()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка расчета значения в поле \"итого перерасчет\" в реестре", ((string[])(null)));
#line 56
this.ScenarioSetup(scenarioInfo);
#line 57
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 58
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка расчета значения в поле \"итого пени\" в реестре")]
        public virtual void ПроверкаРасчетаЗначенияВПолеИтогоПениВРеестре()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка расчета значения в поле \"итого пени\" в реестре", ((string[])(null)));
#line 62
this.ScenarioSetup(scenarioInfo);
#line 63
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 64
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка расчета значения в поле \"примечание\" в реестре")]
        public virtual void ПроверкаРасчетаЗначенияВПолеПримечаниеВРеестре()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка расчета значения в поле \"примечание\" в реестре", ((string[])(null)));
#line 68
this.ScenarioSetup(scenarioInfo);
#line 69
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 70
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка наличия аттрибутов в карточке неподтвержденного начисления")]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым произведен расчет", null)]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым есть перерасчет", null)]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым есть пени", null)]
        [NUnit.Framework.TestCaseAttribute("Номер лицевого счета", null)]
        [NUnit.Framework.TestCaseAttribute("Номер расчетного счета", null)]
        [NUnit.Framework.TestCaseAttribute("Муниципальный район", null)]
        [NUnit.Framework.TestCaseAttribute("Муниципальное образование", null)]
        [NUnit.Framework.TestCaseAttribute("Населенный пункт", null)]
        [NUnit.Framework.TestCaseAttribute("Улица, дом, квартира", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол расчета", null)]
        public virtual void ПроверкаНаличияАттрибутовВКарточкеНеподтвержденногоНачисления(string аттрибуты, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка наличия аттрибутов в карточке неподтвержденного начисления", exampleTags);
#line 74
this.ScenarioSetup(scenarioInfo);
#line 75
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 76
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 77
testRunner.When("пользователь заходит в карточку неподтвержденного начисления", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 78
testRunner.Then(string.Format("в карточке имеются аттрибуты {0}", аттрибуты), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка расчета значений по полям общего количества лс")]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым произведен расчет", null)]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым есть перерасчет", null)]
        [NUnit.Framework.TestCaseAttribute("общее количество лс, по которым есть пени", null)]
        public virtual void ПроверкаРасчетаЗначенийПоПолямОбщегоКоличестваЛс(string аттрибут, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка расчета значений по полям общего количества лс", exampleTags);
#line 96
this.ScenarioSetup(scenarioInfo);
#line 97
testRunner.Given("рассчитывается аналогично сценариям un_chs-2, un_chs-3, un_chs-4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 98
testRunner.When(string.Format("пользователь вызывает действие по аттрибуту {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 99
testRunner.Then("записи в гриде карточки неподтвержденного начисления соответственно отфильтровыва" +
                    "ются", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка перехода из карточки неподтвержденного начисления в протокол расчета")]
        public virtual void ПроверкаПереходаИзКарточкиНеподтвержденногоНачисленияВПротоколРасчета()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка перехода из карточки неподтвержденного начисления в протокол расчета", ((string[])(null)));
#line 110
this.ScenarioSetup(scenarioInfo);
#line 111
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 112
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 113
testRunner.When("пользователь находится в карточке неподтвержденного начисления и переходит по кон" +
                    "кретному лс в протокол расчета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 114
testRunner.Then("пользователю доступна информация из протокола расчета лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка возможности подтверждения неподтвержденного начисления из карточки")]
        public virtual void ПроверкаВозможностиПодтвержденияНеподтвержденногоНачисленияИзКарточки()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка возможности подтверждения неподтвержденного начисления из карточки", ((string[])(null)));
#line 118
this.ScenarioSetup(scenarioInfo);
#line 119
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 120
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 121
testRunner.Given("пользователь находится в карточке неподтвержденного", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 122
testRunner.When("пользователь подтверждает начисление", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 123
testRunner.Then("отрабатывает функция подтверждения по аналогии с реестром неподтвержденных начисл" +
                    "ений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 124
testRunner.And("подтверждается только текущее начисление", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("фильтрация в карточке неподтвержденного начисления")]
        [NUnit.Framework.TestCaseAttribute("Номер лицевого счета", ">", null)]
        [NUnit.Framework.TestCaseAttribute("Номер лицевого счета", "<", null)]
        [NUnit.Framework.TestCaseAttribute("Номер лицевого счета", "!=", null)]
        [NUnit.Framework.TestCaseAttribute("Номер лицевого счета", ">=", null)]
        [NUnit.Framework.TestCaseAttribute("Номер лицевого счета", "<=", null)]
        [NUnit.Framework.TestCaseAttribute("Номер лицевого счета", "с ... по ...", null)]
        [NUnit.Framework.TestCaseAttribute("Номер расчетного счета", ">", null)]
        [NUnit.Framework.TestCaseAttribute("Номер расчетного счета", "<", null)]
        [NUnit.Framework.TestCaseAttribute("Номер расчетного счета", "!=", null)]
        [NUnit.Framework.TestCaseAttribute("Номер расчетного счета", ">=", null)]
        [NUnit.Framework.TestCaseAttribute("Номер расчетного счета", "<=", null)]
        [NUnit.Framework.TestCaseAttribute("Номер расчетного счета", "с ... по ...", null)]
        [NUnit.Framework.TestCaseAttribute("Муниципальный район", "содержит", null)]
        [NUnit.Framework.TestCaseAttribute("Муниципальный район", "не содержит", null)]
        [NUnit.Framework.TestCaseAttribute("Муниципальное образование", "содержит", null)]
        [NUnit.Framework.TestCaseAttribute("Муниципальное образование", "не содержит", null)]
        [NUnit.Framework.TestCaseAttribute("Населенный пункт", "содержит", null)]
        [NUnit.Framework.TestCaseAttribute("Населенный пункт", "не содержит", null)]
        [NUnit.Framework.TestCaseAttribute("Улица, дом, квартира", "содержит", null)]
        [NUnit.Framework.TestCaseAttribute("Улица, дом, квартира", "не содержит", null)]
        public virtual void ФильтрацияВКарточкеНеподтвержденногоНачисления(string аттрибут, string виды_Фильтрации, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("фильтрация в карточке неподтвержденного начисления", exampleTags);
#line 128
this.ScenarioSetup(scenarioInfo);
#line 129
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 130
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-kamchatka\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 131
testRunner.Given("пользователь находится в карточке неподтвержденного", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 132
testRunner.When(string.Format("пользователь использует фильтр аттрибута {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 133
testRunner.Then(string.Format("доступны для использования виды фильтрации {0}", виды_Фильтрации), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion