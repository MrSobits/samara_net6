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
    [NUnit.Framework.DescriptionAttribute("объединение вкладко История операций и История изменений в одну")]
    public partial class ОбъединениеВкладкоИсторияОперацийИИсторияИзмененийВОднуFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "changes_history_personal_acc.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "объединение вкладко История операций и История изменений в одну", "останется История изменений, История операций будет не видна для пользователя", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("добавление операций в Историю изменений")]
        [NUnit.Framework.TestCaseAttribute("Смена абонента", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение пени", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение сальдо", null)]
        [NUnit.Framework.TestCaseAttribute("Закрытие", null)]
        [NUnit.Framework.TestCaseAttribute("Слияние", null)]
        public virtual void ДобавлениеОперацийВИсториюИзменений(string операция, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("добавление операций в Историю изменений", exampleTags);
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
testRunner.When(string.Format("пользователь производит операцию {0}", операция), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 9
testRunner.Then("появляется запись по операции в Истории изменений по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("перенос информации из Истории операций в Историю изменений")]
        [NUnit.Framework.TestCaseAttribute("Смена абонента", "Дата операции", "Дата установки значения", null)]
        [NUnit.Framework.TestCaseAttribute("Смена абонента", "Вид операции", "Наименование параметра", null)]
        [NUnit.Framework.TestCaseAttribute("Смена абонента", "Описание события", "Описание измененного атрибута", null)]
        [NUnit.Framework.TestCaseAttribute("Смена абонента", "Пользователь", "Пользователь", null)]
        [NUnit.Framework.TestCaseAttribute("Смена абонента", "Дата начала действия", "Дата начала действия значения", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение пени", "Дата операции", "Дата установки значения", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение пени", "Вид операции", "Наименование параметра", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение пени", "Описание события", "Описание измененного атрибута", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение пени", "Пользователь", "Пользователь", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение пени", "Дата начала действия", "Дата начала действия значения", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение сальдо", "Дата операции", "Дата установки значения", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение сальдо", "Вид операции", "Наименование параметра", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение сальдо", "Описание события", "Описание измененного атрибута", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение сальдо", "Пользователь", "Пользователь", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение сальдо", "Дата начала действия", "Дата начала действия значения", null)]
        [NUnit.Framework.TestCaseAttribute("Закрытие", "Дата операции", "Дата установки значения", null)]
        [NUnit.Framework.TestCaseAttribute("Закрытие", "Вид операции", "Наименование параметра", null)]
        [NUnit.Framework.TestCaseAttribute("Закрытие", "Описание события", "Описание измененного атрибута", null)]
        [NUnit.Framework.TestCaseAttribute("Закрытие", "Пользователь", "Пользователь", null)]
        [NUnit.Framework.TestCaseAttribute("Закрытие", "Дата начала действия", "Дата начала действия значения", null)]
        [NUnit.Framework.TestCaseAttribute("Слияние", "Дата операции", "Дата установки значения", null)]
        [NUnit.Framework.TestCaseAttribute("Слияние", "Вид операции", "Наименование параметра", null)]
        [NUnit.Framework.TestCaseAttribute("Слияние", "Описание события", "Описание измененного атрибута", null)]
        [NUnit.Framework.TestCaseAttribute("Слияние", "Пользователь", "Пользователь", null)]
        [NUnit.Framework.TestCaseAttribute("Слияние", "Дата начала действия", "Дата начала действия значения", null)]
        public virtual void ПереносИнформацииИзИсторииОперацийВИсториюИзменений(string операция, string аттрибут1, string аттрибут2, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("перенос информации из Истории операций в Историю изменений", exampleTags);
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
testRunner.When(string.Format("пользователь произвел операцию {0}", операция), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 21
testRunner.Then("появляется запись по операции в Истории изменений  по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 22
testRunner.And(string.Format("значение из аттрибута {0} в Истории операций записывается в аттрибут {1} в Истори" +
                        "и изменений", аттрибут1, аттрибут2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Значение\" в Истории изменений при смене абонента")]
        [NUnit.Framework.TestCaseAttribute("Новый владелец", "Администрация МУНИЦИПАЛЬНОГО ОБРАЗОВАНИЯ СЕЛЬСКОГО ПОСЕЛЕНИЯ \"СЕЛО Ивашка\"", null)]
        [NUnit.Framework.TestCaseAttribute("Дата начала действия", "05.02.2015", null)]
        public virtual void ЗаполнениеПоляЗначениеВИсторииИзмененийПриСменеАбонента(string аттрибут, string значение, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Значение\" в Истории изменений при смене абонента", exampleTags);
#line 53
this.ScenarioSetup(scenarioInfo);
#line 54
testRunner.When("пользователь в реестре лицевых счетов выбрает лицевой счет и вызывает действие см" +
                    "ены абонента", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 55
testRunner.And(string.Format("заполняет список аттрибутов {0} значениями {1} и сохраняет данные", аттрибут, значение), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 56
testRunner.Then("появляется запись по операции в Истории изменений  по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 57
testRunner.And("в Истории операций запись об операции не отображается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 58
testRunner.And("поле \"Значение\" заполняется значением, которое тянется из аттрибута \"Новый владел" +
                    "ец\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Установка" +
            " и изменение пени\"")]
        [NUnit.Framework.TestCaseAttribute("Новое значение", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Причина изменения пени", "тест", null)]
        public virtual void ЗаполнениеПоляЗначениеВИсторииИзмененийПриПропеденииОперацииУстановкаИИзменениеПени(string аттрибут, string значение, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Установка" +
                    " и изменение пени\"", exampleTags);
#line 66
this.ScenarioSetup(scenarioInfo);
#line 67
testRunner.When("пользователь в реестре лицевых счетов выбрает лицевой счет и вызывает операцию ус" +
                    "тановки и изменения пени", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 68
testRunner.And(string.Format("заполняет список аттрибутов {0} значениями {1} и сохраняет данные", аттрибут, значение), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 69
testRunner.Then("появляется запись по операции в Истории изменений  по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 70
testRunner.And("в Истории операций запись об операции не отображается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 71
testRunner.And("поле \"Значение\" заполняется значением, которое тянется из аттрибута \"Новое значен" +
                    "ие\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Установка" +
            " и изменение сальдо\"")]
        [NUnit.Framework.TestCaseAttribute("Новое значение", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Причина изменения пени", "тест", null)]
        public virtual void ЗаполнениеПоляЗначениеВИсторииИзмененийПриПропеденииОперацииУстановкаИИзменениеСальдо(string аттрибут, string значение, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Установка" +
                    " и изменение сальдо\"", exampleTags);
#line 79
this.ScenarioSetup(scenarioInfo);
#line 80
testRunner.When("пользователь в реестре лицевых счетов выбрает лицевой счет и вызывает операцию ус" +
                    "тановки и изменения сальдо", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 81
testRunner.And(string.Format("заполняет список аттрибутов {0} значениями {1} и сохраняет данные", аттрибут, значение), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 82
testRunner.Then("появляется запись по операции в Истории изменений  по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 83
testRunner.And("в Истории операций запись об операции не отображается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 84
testRunner.And("поле \"Значение\" заполняется значением, которое тянется из аттрибута \"Новое значен" +
                    "ие\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Закрытие\"" +
            "")]
        [NUnit.Framework.TestCaseAttribute("Дата закрытия", "06.02.2015", null)]
        public virtual void ЗаполнениеПоляЗначениеВИсторииИзмененийПриПропеденииОперацииЗакрытие(string аттрибут, string значение, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Закрытие\"" +
                    "", exampleTags);
#line 92
this.ScenarioSetup(scenarioInfo);
#line 93
testRunner.When("пользователь в реестре лицевых счетов выбрает лицевой счет и вызывает операцию за" +
                    "крытия счета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 94
testRunner.And(string.Format("заполняет список аттрибутов {0} значениями {1} и сохраняет данные", аттрибут, значение), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 95
testRunner.Then("появляется запись по операции в Истории изменений  по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 96
testRunner.And("в Истории операций запись об операции не отображается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 97
testRunner.And(string.Format("поле \"Значение\" заполняется значением, которое тянется из поля {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Слияние\" " +
            "и закрытие лс")]
        public virtual void ЗаполнениеПоляЗначениеВИсторииИзмененийПриПропеденииОперацииСлияниеИЗакрытиеЛс()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Слияние\" " +
                    "и закрытие лс", ((string[])(null)));
#line 104
this.ScenarioSetup(scenarioInfo);
#line 105
testRunner.Given("пользователь в реестре лицевых счетов выбрает несколько ЛС по одной квартире и вы" +
                    "зывает операцию слияния счетов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 106
testRunner.When("указывает по одному лс Новую долю собственности = 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 107
testRunner.And("сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 108
testRunner.Then("появляется запись по операции в Истории изменений  по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 109
testRunner.And("в Истории операций запись об операции не отображается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 110
testRunner.And("у счета с Новой долей собственности = 0 статус с \"Открыт\" меняется на \"Закрыт\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 111
testRunner.And("поле \"Значение\" заполняется значением нового статуса счета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Слияние\" " +
            "и смена доли собственности у лс на неравную 0")]
        public virtual void ЗаполнениеПоляЗначениеВИсторииИзмененийПриПропеденииОперацииСлияниеИСменаДолиСобственностиУЛсНаНеравную0()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Значение\" в Истории изменений при пропедении операции \"Слияние\" " +
                    "и смена доли собственности у лс на неравную 0", ((string[])(null)));
#line 114
this.ScenarioSetup(scenarioInfo);
#line 115
testRunner.Given("пользователь в реестре лицевых счетов выбрает несколько ЛС по одной квартире и вы" +
                    "зывает операцию слияния счетов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 116
testRunner.When("указывает по одному лс Новую долю собственности = 0,5", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 117
testRunner.And("сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 118
testRunner.Then("появляется запись по операции в Истории изменений  по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 119
testRunner.And("в Истории операций запись об операции не отображается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 120
testRunner.And("поле \"Значение\" заполняется значением новой доли собственности", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("заполнение поля \"Файл\" в Истории изменений")]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение пени", "Документ основание", null)]
        [NUnit.Framework.TestCaseAttribute("Установка и изменение сальдо", "Документ основание", null)]
        public virtual void ЗаполнениеПоляФайлВИсторииИзменений(string операция, string поле, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("заполнение поля \"Файл\" в Истории изменений", exampleTags);
#line 123
this.ScenarioSetup(scenarioInfo);
#line 124
testRunner.When(string.Format("пользователь в реестре лицевых счетов выбрает лицевой счет и вызывает операцию {0" +
                        "}", операция), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 125
testRunner.And(string.Format("прикрепляет к полю {0} файл формата .txt", поле), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 126
testRunner.Then("появляется запись по операции в Истории изменений  по конкретному ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 127
testRunner.And("в Истории операций запись об операции не отображается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 128
testRunner.And(string.Format("поле \"Файл\" заполняется файлом, которое тянется из поля {0}", поле), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("запись истории изменений площади жилого дома в карточку счета")]
        public virtual void ЗаписьИсторииИзмененийПлощадиЖилогоДомаВКарточкуСчета()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("запись истории изменений площади жилого дома в карточку счета", ((string[])(null)));
#line 136
this.ScenarioSetup(scenarioInfo);
#line 137
testRunner.Given("пользователь выбирает ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 138
testRunner.Given("жилой дом с действующим протоколом решений, с записями по помещениям, к помещению" +
                    " привязан лицевой счет", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 139
testRunner.When("пользователь у жилого дома меняет по помещению общую площадь \"Общая площадь\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 140
testRunner.Then("в истории изменений в карточке ЛС появляется новая запись", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 141
testRunner.And("значение в истории изменений по лс в поле \"Наименование параметра\" = значению в и" +
                    "стории изменений помещения в поле \"Наименование параметра\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 142
testRunner.And("значение в истории изменений по лс в поле \"Описание измененного атрибута\" = значе" +
                    "нию в истории изменений помещения в поле \"Описание измененного атрибута\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 143
testRunner.And("значение в истории изменений по лс в поле \"Значение\" = значению в истории изменен" +
                    "ий помещения в поле \"Значение\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 144
testRunner.And("значение в истории изменений по лс в поле \"Дата начала действия значения\" = значе" +
                    "нию в истории изменений помещения в поле \"Дата начала действия значения\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 145
testRunner.And("значение в истории изменений по лс в поле \"Дата установки значения\" = значению в " +
                    "истории изменений помещения в поле \"Дата установки значения\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 146
testRunner.And("значение в истории изменений по лс в поле \"Пользователь\" = значению в истории изм" +
                    "енений помещения в поле \"Пользователь\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 147
testRunner.And("значение в истории изменений по лс в поле \"Файл\" = значению в истории изменений п" +
                    "омещения в поле \"Файл\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("запись в истории изменений внешного номера ЛС при изменении его в карточке ЛС")]
        public virtual void ЗаписьВИсторииИзмененийВнешногоНомераЛСПриИзмененииЕгоВКарточкеЛС()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("запись в истории изменений внешного номера ЛС при изменении его в карточке ЛС", ((string[])(null)));
#line 150
this.ScenarioSetup(scenarioInfo);
#line 151
testRunner.Given("пользователь выбирает ЛС", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 152
testRunner.When("пользователь у лицевого счета для поля \"Внешний номер ЛС\" вводит в поле \"Дата вст" +
                    "упления значения в силу\" значение \"01.02.2015\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 153
testRunner.And("вводит в поле \"Новое значение\" значение \"111111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 154
testRunner.And("прикрепляет в поле \"Документ-основаниее\" файл", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 155
testRunner.And("сохраняет данные", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 156
testRunner.Then("в истории изменений в карточке ЛС появляется новая запись", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 157
testRunner.And("у новой записи значение в поле \"Наименование аттрибута\" = \"Внешний номер л\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 158
testRunner.And("у новой записи значение в поле \"Описание измененного атрибута\" = У л/с №\"номер ЛС" +
                    "\" изменился номер внешнего лс с \"старый внешний номер\" на \"новый внешний номер\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 159
testRunner.And("у новой записи значение в поле \"Значение\" = \"новый внешний номер\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 160
testRunner.And("у новой записи значение в поле \"Дата начала действия значения\" = \"Дата вступления" +
                    " значения в силу\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 161
testRunner.And("у новой записи значение в поле \"Дата установки значения\" = текущая дата", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 162
testRunner.And("у новой записи значение в поле \"Пользователь\" = пользователь, который произвел из" +
                    "менения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 163
testRunner.And("у новой записи в поле \"Файл\" прикреплен файл из поля \"Документ-основаниее\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
