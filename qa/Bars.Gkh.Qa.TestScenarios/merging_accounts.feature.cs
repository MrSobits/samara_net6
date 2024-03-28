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
    [NUnit.Framework.DescriptionAttribute("Слияние лицевых счетов")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class СлияниеЛицевыхСчетовFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "merging_accounts.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Слияние лицевых счетов", "", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("успешное слияние 2х лс")]
        public virtual void УспешноеСлияние2ХЛс()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное слияние 2х лс", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133029\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 6
testRunner.And("пользователь в реестре ЛС выбирает лицевой счет \"140133037\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.When("пользователь для выбранных ЛС вызывает операцию Слияние", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 9
testRunner.Given("пользователь в слиянии ЛС заполняет заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 10
testRunner.And("пользователь в слиянии ЛС прикрепляет файл Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.And("пользователь в слиянии ЛС для ЛС \"140133029\" заполняет поле Новая доля собственно" +
                    "сти \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 13
testRunner.And("пользователь в слиянии ЛС для ЛС \"140133037\" заполняет поле Новая доля собственно" +
                    "сти \"0,5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.When("пользователь в слиянии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133029\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 16
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись с наименованием па" +
                    "раметра \"Закрытие в связи со слиянием\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 17
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Изменение с" +
                    "альдо в связи со слиянием ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"0,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.And("у этой записи, в истории изменений ЛС, Дата установки значения \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("у этого лицевого счета в истории изменений присутствует запись с наименованием па" +
                    "раметра \"Доля собственности\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Изменение д" +
                    "оли собственности в связи со слиянием ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.And("у этой записи, в истории изменений ЛС, Дата установки значения \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.And("у этого ЛС в карточке заполнено поле Статус \"Закрыт\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.And("у этого ЛС в карточке заполнено поле Доля собственности \"0,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133037\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 31
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись с наименованием па" +
                    "раметра \"Изменения сальдо в связи со слиянием\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 32
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Изменение с" +
                    "альдо в связи со слиянием ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"0,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.And("у этой записи, в истории изменений ЛС, Дата установки значения \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.And("у этого лицевого счета в истории изменений присутствует запись с наименованием па" +
                    "раметра \"Доля собственности\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Изменение д" +
                    "оли собственности в связи со слиянием ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"0,5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.And("у этой записи, в истории изменений ЛС, Дата установки значения \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 43
testRunner.And("у этого ЛС в карточке заполнено поле Статус \"Открыт\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 44
testRunner.And("у этого ЛС в карточке заполнено поле Доля собственности \"0,5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное слияние двух закрытых ЛС одной квартиры с нулевой долей собственности")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСлияниеДвухЗакрытыхЛСОднойКвартирыСНулевойДолейСобственности()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное слияние двух закрытых ЛС одной квартиры с нулевой долей собственности", new string[] {
                        "ignore"});
#line 51
this.ScenarioSetup(scenarioInfo);
#line 52
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133045\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 53
testRunner.And("пользователь в реестре ЛС выбирает лицевой счет \"140133053\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 55
testRunner.When("пользователь для выбранных ЛС вызывает операцию Слияние", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 56
testRunner.Given("пользователь в слиянии ЛС заполняет заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 57
testRunner.And("пользователь в слиянии ЛС прикрепляет файл Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 58
testRunner.When("пользователь в слиянии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 59
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133045\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 60
testRunner.Then("у этого лицевого счета в истории изменений отсутствует запись с наименованием пар" +
                    "аметра \"Закрытие в связи со слиянием\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 61
testRunner.And("падает ошибка с текстом \"У лицевых счетов 140133045, 140133053 доля собственности" +
                    " равна 0. Для них слияние невозможно\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное слияние двух открытых ЛС одной квартиры без изменения доли собственност" +
            "и")]
        public virtual void НеудачноеСлияниеДвухОткрытыхЛСОднойКвартирыБезИзмененияДолиСобственности()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное слияние двух открытых ЛС одной квартиры без изменения доли собственност" +
                    "и", ((string[])(null)));
#line 64
this.ScenarioSetup(scenarioInfo);
#line 65
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133061\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 66
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133069\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 68
testRunner.When("пользователь для выбранных ЛС вызывает операцию Слияние", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 69
testRunner.Given("пользователь в слиянии ЛС заполняет заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 70
testRunner.And("пользователь в слиянии ЛС прикрепляет файл Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 71
testRunner.When("пользователь в слиянии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 72
testRunner.Then("у этого лицевого счета в истории изменений отсутствует запись с наименованием пар" +
                    "аметра \"Закрытие в связи со слиянием\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 73
testRunner.And("падает ошибка с текстом \"При слиянии лицевых счетов, должен быть лицевой счет с н" +
                    "улевой долей собственности\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное слияние двух открытых ЛС одной квартиры с уменьшением доли собственност" +
            "и у 1го лс")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСлияниеДвухОткрытыхЛСОднойКвартирыСУменьшениемДолиСобственностиУ1ГоЛс()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное слияние двух открытых ЛС одной квартиры с уменьшением доли собственност" +
                    "и у 1го лс", new string[] {
                        "ignore"});
#line 78
this.ScenarioSetup(scenarioInfo);
#line 79
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133061\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 80
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133069\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 82
testRunner.When("пользователь для выбранных ЛС вызывает операцию Слияние", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 83
testRunner.Given("пользователь в слиянии ЛС заполняет заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 84
testRunner.And("пользователь в слиянии ЛС прикрепляет файл Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 85
testRunner.And("пользователь в слиянии ЛС для ЛС \"140133061\" заполняет поле Новая доля собственно" +
                    "сти \"0,1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 86
testRunner.When("пользователь в слиянии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 87
testRunner.Then("у этого лицевого счета в истории изменений отсутствует запись с наименованием пар" +
                    "аметра \"Закрытие в связи со слиянием\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 88
testRunner.And("падает ошибка с текстом \"При слиянии лицевых счетов, должен быть лицевой счет с н" +
                    "улевой долей собственности Доля собственности не может уменьшится.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное слияние двух открытых ЛС одной квартиры с уменьшением доли собственност" +
            "и у 2х лс")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСлияниеДвухОткрытыхЛСОднойКвартирыСУменьшениемДолиСобственностиУ2ХЛс()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное слияние двух открытых ЛС одной квартиры с уменьшением доли собственност" +
                    "и у 2х лс", new string[] {
                        "ignore"});
#line 93
this.ScenarioSetup(scenarioInfo);
#line 94
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133061\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 95
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133069\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 97
testRunner.When("пользователь для выбранных ЛС вызывает операцию Слияние", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 98
testRunner.Given("пользователь в слиянии ЛС заполняет заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 99
testRunner.And("пользователь в слиянии ЛС прикрепляет файл Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 100
testRunner.And("пользователь в слиянии ЛС для ЛС \"140133061\" заполняет поле Новая доля собственно" +
                    "сти \"0,1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 101
testRunner.And("пользователь в слиянии ЛС для ЛС \"140133069\" заполняет поле Новая доля собственно" +
                    "сти \"0,1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 102
testRunner.When("пользователь в слиянии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 103
testRunner.Then("у этого лицевого счета в истории изменений отсутствует запись с наименованием пар" +
                    "аметра \"Закрытие в связи со слиянием\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 104
testRunner.And("падает ошибка с текстом \"При слиянии лицевых счетов, должен быть лицевой счет с н" +
                    "улевой долей собственности Доля собственности не может уменьшится.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное слияние двух открытых ЛС одной квартиры с обнулением и уменьшением доли" +
            " собственности у 2х лс с долей 0")]
        public virtual void НеудачноеСлияниеДвухОткрытыхЛСОднойКвартирыСОбнулениемИУменьшениемДолиСобственностиУ2ХЛсСДолей0()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное слияние двух открытых ЛС одной квартиры с обнулением и уменьшением доли" +
                    " собственности у 2х лс с долей 0", ((string[])(null)));
#line 107
this.ScenarioSetup(scenarioInfo);
#line 108
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133061\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 109
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133069\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 111
testRunner.When("пользователь для выбранных ЛС вызывает операцию Слияние", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 112
testRunner.Given("пользователь в слиянии ЛС заполняет заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 113
testRunner.And("пользователь в слиянии ЛС прикрепляет файл Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 114
testRunner.And("пользователь в слиянии ЛС для ЛС \"140133061\" заполняет поле Новая доля собственно" +
                    "сти \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 115
testRunner.When("пользователь в слиянии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 116
testRunner.Then("у этого лицевого счета в истории изменений отсутствует запись с наименованием пар" +
                    "аметра \"Закрытие в связи со слиянием\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 117
testRunner.And("падает ошибка с текстом \"При слиянии суммарная доля собственности не должна менят" +
                    "ься.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное слияние двух открытых ЛС одной квартиры с незаполненными полями новой д" +
            "оли")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void НеудачноеСлияниеДвухОткрытыхЛСОднойКвартирыСНезаполненнымиПолямиНовойДоли()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное слияние двух открытых ЛС одной квартиры с незаполненными полями новой д" +
                    "оли", new string[] {
                        "ignore"});
#line 122
this.ScenarioSetup(scenarioInfo);
#line 123
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133061\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 124
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140133069\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 126
testRunner.When("пользователь для выбранных ЛС вызывает операцию Слияние", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 127
testRunner.Given("пользователь в слиянии ЛС заполняет заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 128
testRunner.And("пользователь в слиянии ЛС прикрепляет файл Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 129
testRunner.And("пользователь в слиянии ЛС для ЛС \"140133061\" заполняет поле Новая доля собственно" +
                    "сти \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 130
testRunner.And("пользователь в слиянии ЛС для ЛС \"140133069\" заполняет поле Новая доля собственно" +
                    "сти \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 131
testRunner.When("пользователь в слиянии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 132
testRunner.Then("у этого лицевого счета в истории изменений отсутствует запись с наименованием пар" +
                    "аметра \"Закрытие в связи со слиянием\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 133
testRunner.And("падает ошибка с текстом \"При слиянии лицевых счетов, должен быть лицевой счет с н" +
                    "улевой долей собственности\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion