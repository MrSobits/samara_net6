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
    [NUnit.Framework.DescriptionAttribute("Расчет начислений")]
    public partial class РасчетНачисленийFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "calculation_charges.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Расчет начислений", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Расчет нулевой доли собственности для лс при нулевой доли собственности или площа" +
            "ди помещения")]
        public virtual void РасчетНулевойДолиСобственностиДляЛсПриНулевойДолиСобственностиИлиПлощадиПомещения()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Расчет нулевой доли собственности для лс при нулевой доли собственности или площа" +
                    "ди помещения", ((string[])(null)));
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.Given("пользователь вызывает операцию расчета лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("в задачах появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("у этой записи по расчету лс заполнено поле Дата запуска \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("у этой записи по расчету лс заполнено поле Наименование \"Расчет задолжности ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.And("у этой записи по расчету лс заполнено поле Статус \"Успешно выполнена\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.And("у этой записи по расчету лс заполнено поле Процент выполнения \"100\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 12
testRunner.And("у этой записи по расчету лс заполнено поле Ход выполнения \"Завершено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 13
testRunner.And("в Реестре неподтвержденных начислений появляется запись начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.And("у этой записи начислений Состояние \"Ожидание\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.When("пользователь подтверждает эту запись начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 16
testRunner.Then("у этой записи начислений Состояние \"Подтверждено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 17
testRunner.And("в протоколе расчета по текущему периоду у ЛС \"\" есть запись по начислению", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.And("у этой записи для ЛС \"\" заполнено поле С-по \"дата открытия периода начислений\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.And("у этой записи для ЛС \"\" заполнено поле Тариф на КР \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.And("у этой записи для ЛС \"\" заполнено поле Доля собственности \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("у этой записи для ЛС \"\" заполнено поле Площадь помещения \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("у этой записи для ЛС \"\" заполнено поле Количество дней \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("у этой записи для ЛС \"\" заполнено поле Итого \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.And("в протоколе расчета по текущему периоду у ЛС \"\" есть запись по начислению", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.And("у этой записи для ЛС \"\" заполнено поле С-по \"дата открытия периода начислений\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.And("у этой записи для ЛС \"\" заполнено поле Тариф на КР \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.And("у этой записи для ЛС \"\" заполнено поле Доля собственности \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.And("у этой записи для ЛС \"\" заполнено поле Площадь помещения \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("у этой записи для ЛС \"\" заполнено поле Количество дней \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("у этой записи для ЛС \"\" заполнено поле Итого \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Расчет нулевой доли собственности для лс при нулевом тарифе")]
        public virtual void РасчетНулевойДолиСобственностиДляЛсПриНулевомТарифе()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Расчет нулевой доли собственности для лс при нулевом тарифе", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 35
testRunner.Given("в размере взносов на кр есть запись показателя", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 36
testRunner.And("у этой записи показателя заполнено поле Окончание периода \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.And("у этой записи показателя есть детальная информация", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("у этой детальной информации есть запись по муниципальному образованию \"Никольское" +
                    " сельское поселение\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.And("у этогй записи по муниципальному образованию заполнено поле Размер взноса \"7,8\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.And("пользователь вызывает операцию расчета лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.And("в задачах появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.And("у этой записи по расчету лс заполнено поле Дата запуска \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 43
testRunner.And("у этой записи по расчету лс заполнено поле Наименование \"Расчет задолжности ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 44
testRunner.And("у этой записи по расчету лс заполнено поле Статус \"Успешно выполнена\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 45
testRunner.And("у этой записи по расчету лс заполнено поле Процент выполнения \"100\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 46
testRunner.And("у этой записи по расчету лс заполнено поле Ход выполнения \"Завершено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.And("в Реестре неподтвержденных начислений появляется запись начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.And("у этой записи начислений Состояние \"Ожидание\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 49
testRunner.When("пользователь подтверждает эту запись начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 50
testRunner.Then("у этой записи начислений Состояние \"Подтверждено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 51
testRunner.And("в протоколе расчета по текущему периоду у ЛС \"\" есть запись по начислению", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 52
testRunner.And("у этой записи для ЛС \"\" заполнено поле С-по \"дата открытия периода начислений\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.And("у этой записи для ЛС \"\" заполнено поле Тариф на КР \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 54
testRunner.And("у этой записи для ЛС \"\" заполнено поле Доля собственности \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 55
testRunner.And("у этой записи для ЛС \"\" заполнено поле Площадь помещения \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 56
testRunner.And("у этой записи для ЛС \"\" заполнено поле Количество дней \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 57
testRunner.And("у этой записи для ЛС \"\" заполнено поле Итого \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 58
testRunner.And("И у этогй записи по муниципальному образованию заполнено поле Размер взноса \"0,00" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("отсутствие расчета начислений для лс со статусами \"Закрыт\" и \"Не активен\"")]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        public virtual void ОтсутствиеРасчетаНачисленийДляЛсСоСтатусамиЗакрытИНеАктивен(string лс, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("отсутствие расчета начислений для лс со статусами \"Закрыт\" и \"Не активен\"", exampleTags);
#line 61
this.ScenarioSetup(scenarioInfo);
#line 62
testRunner.Given("пользователь вызывает операцию расчета лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 63
testRunner.And("в задачах появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 64
testRunner.And("у этой записи по расчету лс заполнено поле Дата запуска \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 65
testRunner.And("у этой записи по расчету лс заполнено поле Наименование \"Расчет задолжности ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 66
testRunner.And("у этой записи по расчету лс заполнено поле Статус \"Успешно выполнена\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 67
testRunner.And("у этой записи по расчету лс заполнено поле Процент выполнения \"100\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 68
testRunner.And("у этой записи по расчету лс заполнено поле Ход выполнения \"Завершено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 69
testRunner.And("в Реестре неподтвержденных начислений появляется запись начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 70
testRunner.And("у этой записи начислений Состояние \"Ожидание\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 71
testRunner.When("пользователь подтверждает эту запись начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 72
testRunner.Then("у этой записи начислений Состояние \"Подтверждено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 73
testRunner.And("у этой записи начислений есть детальная информация по начислениям", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 74
testRunner.And(string.Format("у этой детальной информации по начислениям отсутствует запись по ЛС \"{0}\"", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 75
testRunner.And(string.Format("в протоколе расчета по текущему периоду у ЛС \"{0}\" отсутствует запись по начислен" +
                        "ию", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("отсутствие расчета начислений при отсутствии настройки в Настройке параметров")]
        public virtual void ОтсутствиеРасчетаНачисленийПриОтсутствииНастройкиВНастройкеПараметров()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("отсутствие расчета начислений при отсутствии настройки в Настройке параметров", ((string[])(null)));
#line 83
this.ScenarioSetup(scenarioInfo);
#line 84
testRunner.Given("пользователь в единых настройках приложения заполняет поле Счет регионального опе" +
                    "ратора \"false\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 85
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет регио" +
                    "нального оператора \"false\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 86
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет \"fals" +
                    "e\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 87
testRunner.And("пользователь в единых настройках приложения заполняет поле Не выбран \"false\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 88
testRunner.When("пользователь вызывает операцию расчета лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 89
testRunner.Then("в задачах не появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("расчет начислений при заполненной настройке в Настройке параметров со всеми включ" +
            "енными параметрами")]
        [NUnit.Framework.TestCaseAttribute("140014331", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        public virtual void РасчетНачисленийПриЗаполненнойНастройкеВНастройкеПараметровСоВсемиВключеннымиПараметрами(string лс, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("расчет начислений при заполненной настройке в Настройке параметров со всеми включ" +
                    "енными параметрами", exampleTags);
#line 92
this.ScenarioSetup(scenarioInfo);
#line 93
testRunner.Given("пользователь в единых настройках приложения заполняет поле Счет регионального опе" +
                    "ратора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 94
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет регио" +
                    "нального оператора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 95
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет \"true" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 96
testRunner.And("пользователь в единых настройках приложения заполняет поле Не выбран \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 97
testRunner.When(string.Format("пользователь вызывает операцию расчета лс для ЛС \"{0}\"", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 98
testRunner.Then("в задачах появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 99
testRunner.And("у этой записи по расчету лс заполнено поле Дата запуска \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 100
testRunner.And("у этой записи по расчету лс заполнено поле Наименование \"Расчет задолжности ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 101
testRunner.And("у этой записи по расчету лс заполнено поле Статус \"Успешно выполнена\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 102
testRunner.And("у этой записи по расчету лс заполнено поле Процент выполнения \"100\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 103
testRunner.And("у этой записи по расчету лс заполнено поле Ход выполнения \"Завершено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 104
testRunner.And("в Реестре неподтвержденных начислений появляется запись начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("расчет начислений при заполненной настройке в Настройке параметров по одному пара" +
            "метру")]
        [NUnit.Framework.TestCaseAttribute("Счет регионального оператора", "", null)]
        [NUnit.Framework.TestCaseAttribute("Специальный счет регионального оператора", "", null)]
        [NUnit.Framework.TestCaseAttribute("Специальный счет", "", null)]
        [NUnit.Framework.TestCaseAttribute("Не выбран", "", null)]
        public virtual void РасчетНачисленийПриЗаполненнойНастройкеВНастройкеПараметровПоОдномуПараметру(string способФормированияФонда, string лс, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("расчет начислений при заполненной настройке в Настройке параметров по одному пара" +
                    "метру", exampleTags);
#line 115
this.ScenarioSetup(scenarioInfo);
#line 116
testRunner.Given(string.Format("пользователь в единых настройках приложения заполняет поле \"{0}\" \"true\"", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 117
testRunner.When(string.Format("пользователь вызывает операцию расчета лс для ЛС \"{0}\"", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 118
testRunner.Then("в задачах появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 119
testRunner.And("у этой записи по расчету лс заполнено поле Дата запуска \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 120
testRunner.And("у этой записи по расчету лс заполнено поле Наименование \"Расчет задолжности ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 121
testRunner.And("у этой записи по расчету лс заполнено поле Статус \"Успешно выполнена\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 122
testRunner.And("у этой записи по расчету лс заполнено поле Процент выполнения \"100\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 123
testRunner.And("у этой записи по расчету лс заполнено поле Ход выполнения \"Завершено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 124
testRunner.And("в Реестре неподтвержденных начислений появляется запись начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 125
testRunner.And("у этой записи начислений есть детальная информация по начислениям", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 126
testRunner.And(string.Format("у этой детальной информации по начислениям присутствует запись по ЛС \"{0}\"", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("отсутствие расчета начислений для лс, у которых дома с состояниями Аварийный и Сн" +
            "есен")]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        public virtual void ОтсутствиеРасчетаНачисленийДляЛсУКоторыхДомаССостояниямиАварийныйИСнесен(string лс, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("отсутствие расчета начислений для лс, у которых дома с состояниями Аварийный и Сн" +
                    "есен", exampleTags);
#line 136
this.ScenarioSetup(scenarioInfo);
#line 137
testRunner.When(string.Format("пользователь вызывает операцию расчета лс для ЛС \"{0}\"", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 138
testRunner.Then(string.Format("в задачах не появляется запись по расчету ЛС \"{0}\"", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачный расчет начислений при заполненной настройке с протоколом НЕ в конечном " +
            "статусе")]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        public virtual void НеудачныйРасчетНачисленийПриЗаполненнойНастройкеСПротоколомНЕВКонечномСтатусе(string лс, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачный расчет начислений при заполненной настройке с протоколом НЕ в конечном " +
                    "статусе", exampleTags);
#line 146
this.ScenarioSetup(scenarioInfo);
#line 147
testRunner.Given("пользователь в единых настройках приложения заполняет поле Счет регионального опе" +
                    "ратора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 148
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет регио" +
                    "нального оператора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 149
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет \"true" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 150
testRunner.And("пользователь в единых настройках приложения заполняет поле Не выбран \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 151
testRunner.When(string.Format("пользователь вызывает операцию расчета лс для ЛС \"{0}\"", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 152
testRunner.Then("в задачах не появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачный расчет начислений при заполненной настройке с протоколом в конечном ста" +
            "тусе, но с датой вступления в силу > текущей даты")]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        public virtual void НеудачныйРасчетНачисленийПриЗаполненнойНастройкеСПротоколомВКонечномСтатусеНоСДатойВступленияВСилуТекущейДаты(string лс, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачный расчет начислений при заполненной настройке с протоколом в конечном ста" +
                    "тусе, но с датой вступления в силу > текущей даты", exampleTags);
#line 162
this.ScenarioSetup(scenarioInfo);
#line 163
testRunner.Given("пользователь в единых настройках приложения заполняет поле Счет регионального опе" +
                    "ратора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 164
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет регио" +
                    "нального оператора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 165
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет \"true" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 166
testRunner.And("пользователь в единых настройках приложения заполняет поле Не выбран \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 167
testRunner.When(string.Format("пользователь вызывает операцию расчета лс для ЛС \"{0}\"", лс), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 168
testRunner.Then("в задачах не появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачный расчет начислений при заполненной настройке с протоколом, в котором вед" +
            "ение лс = Собственниками")]
        public virtual void НеудачныйРасчетНачисленийПриЗаполненнойНастройкеСПротоколомВКоторомВедениеЛсСобственниками()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачный расчет начислений при заполненной настройке с протоколом, в котором вед" +
                    "ение лс = Собственниками", ((string[])(null)));
#line 178
this.ScenarioSetup(scenarioInfo);
#line 179
testRunner.Given("пользователь в единых настройках приложения заполняет поле Счет регионального опе" +
                    "ратора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 180
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет регио" +
                    "нального оператора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 181
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет \"true" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 182
testRunner.And("пользователь в единых настройках приложения заполняет поле Не выбран \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 183
testRunner.When("пользователь вызывает операцию расчета лс для ЛС \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 184
testRunner.Then("в задачах не появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачный расчет начислений для лс, добавленных в ркц с \"РКЦ проводит начисления\"" +
            " = true")]
        public virtual void НеудачныйРасчетНачисленийДляЛсДобавленныхВРкцСРКЦПроводитНачисленияTrue()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачный расчет начислений для лс, добавленных в ркц с \"РКЦ проводит начисления\"" +
                    " = true", ((string[])(null)));
#line 187
this.ScenarioSetup(scenarioInfo);
#line 188
testRunner.Given("пользователь в единых настройках приложения заполняет поле Счет регионального опе" +
                    "ратора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 189
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет регио" +
                    "нального оператора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 190
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет \"true" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 191
testRunner.And("пользователь в единых настройках приложения заполняет поле Не выбран \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 192
testRunner.When("пользователь вызывает операцию расчета лс для ЛС \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 193
testRunner.Then("в задачах не появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачный расчет начислений для лс, не участвующих а программе КР")]
        public virtual void НеудачныйРасчетНачисленийДляЛсНеУчаствующихАПрограммеКР()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачный расчет начислений для лс, не участвующих а программе КР", ((string[])(null)));
#line 196
this.ScenarioSetup(scenarioInfo);
#line 197
testRunner.Given("пользователь в единых настройках приложения заполняет поле Счет регионального опе" +
                    "ратора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 198
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет регио" +
                    "нального оператора \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 199
testRunner.And("пользователь в единых настройках приложения заполняет поле Специальный счет \"true" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 200
testRunner.And("пользователь в единых настройках приложения заполняет поле Не выбран \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 201
testRunner.When("пользователь вызывает операцию расчета лс для ЛС \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 202
testRunner.Then("в задачах не появляется запись по расчету лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
