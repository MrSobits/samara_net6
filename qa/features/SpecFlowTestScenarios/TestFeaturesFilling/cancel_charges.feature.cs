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
    [NUnit.Framework.DescriptionAttribute("отмена начислений")]
    public partial class ОтменаНачисленийFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "cancel_charges.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "отмена начислений", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Успешная отмена начислений для одного лс")]
        public virtual void УспешнаяОтменаНачисленийДляОдногоЛс()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Успешная отмена начислений для одного лс", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line 4
testRunner.Given("пользователь выбирает Период \"2014 Ноябрь\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 5
testRunner.And("пользователь в реестре ЛС выбирает лицевой счет \"010032137\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 6
testRunner.When("пользователь для текущего ЛС вызывает операцию Отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 7
testRunner.And("в форме Отмены начислений присутствует запись по отменяемым начислениям", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Муниципальный район \"Петро" +
                    "павловск-Камчатский городской округ\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Адрес \"г. Петропавловск-Ка" +
                    "мчатский, пр-кт. 50 лет Октября, д. 8\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Номер ЛС \"010032137\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Сумма начислений за период" +
                    " \"222,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 12
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Отменить начисления в разм" +
                    "ере \"222,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 13
testRunner.And("пользователь заполняет поле Период \"закрытый период\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.And("пользователь заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.And("пользователь заполняет поле Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.And("пользователь сохраняет изменения в форме отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 17
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 18
testRunner.And("у этой записи, в истории изменений ЛС, Наименование параметра \"Отмена начислений\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Отмена начи" +
                    "сления за период 2014 Ноябрь на сумму 222,00000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"222,00000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.Given("пользователь выбирает Период \"2015 Февраль\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 25
testRunner.Then("в карточке этого лицевого счета, в операциях за текущий период, присутствует запи" +
                    "сь \"Отмена начислений по базовому тарифу\" где изменение сальдо = -222,00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 26
testRunner.Then("у поля Начислено взносов по минимальному тарифу всего есть детальная информация п" +
                    "о начисленным взносам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 28
testRunner.And("у этой записи по текущему периоду заполнено поле Сумма \"-222,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("у поля Задолженность по взносам всего есть детальная информация по задолженности " +
                    "по взносам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("у этой записи по текущему периоду заполнено поле Сумма \"-222,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Успешная отмена начислений для нескольких лс")]
        public virtual void УспешнаяОтменаНачисленийДляНесколькихЛс()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Успешная отмена начислений для нескольких лс", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 36
testRunner.Given("пользователь выбирает Период \"2015 Январь\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 37
testRunner.When("пользователь для ЛС \"100000876\" и ЛС \"100000877\" вызывает операцию Отмена начисле" +
                    "ний", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 38
testRunner.And("присутствует запись по отменяемым начислениям по ЛС \"100000876\" в форме Отмены на" +
                    "числений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Муниципальный район \"Караг" +
                    "инский муниципальный район\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Адрес \"п. Оссора, ул. Лука" +
                    "шевского, д. 69а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Сумма начислений за период" +
                    " \"479,56\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Отменить начисления в разм" +
                    "ере \"479,56\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 43
testRunner.And("присутствует запись по отменяемым начислениям по ЛС \"100000877\" в форме Отмены на" +
                    "числений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 44
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Муниципальный район \"Караг" +
                    "инский муниципальный район\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 45
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Адрес \"п. Оссора, ул. Лука" +
                    "шевского, д. 69а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 46
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Сумма начислений за период" +
                    " \"370,88\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Отменить начисления в разм" +
                    "ере \"370,88\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.And("пользователь заполняет поле Период \"закрытый период\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 49
testRunner.And("пользователь заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 50
testRunner.And("пользователь заполняет поле Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 51
testRunner.And("пользователь сохраняет изменения для двух лс в форме отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 52
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"100000876\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 53
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 54
testRunner.And("у этой записи, в истории изменений ЛС, Наименование параметра \"Отмена начислений\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 55
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Отмена начи" +
                    "сления за период 2014 Январь на сумму 479.56000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 56
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"479.56000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 57
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 59
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 60
testRunner.Given("пользователь выбирает Период \"текущий\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 61
testRunner.Then("в карточке этого лицевого счета, в операциях за текущий период, присутствует запи" +
                    "сь \"Отмена начислений по базовому тарифу\" где изменение сальдо = -479,56000", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 62
testRunner.Then("у поля Начислено взносов по минимальному тарифу всего есть детальная информация п" +
                    "о начисленным взносам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 64
testRunner.And("у этой записи по текущему периоду заполнено поле Сумма \"-479,56000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 66
testRunner.And("у поля Задолженность по взносам всего есть детальная информация по задолженности " +
                    "по взносам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 68
testRunner.And("у этой записи по текущему периоду заполнено поле Сумма \"-479,56000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 69
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"100000877\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 70
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 71
testRunner.And("у этой записи, в истории изменений ЛС, Наименование параметра \"Отмена начислений\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 72
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Отмена начи" +
                    "сления за период 2015 Январь на сумму 370.88000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 73
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"370.88000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 74
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 76
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 77
testRunner.Given("пользователь выбирает Период \"текущий\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 78
testRunner.Then("в карточке этого лицевого счета, в операциях за текущий период, присутствует запи" +
                    "сь \"Отмена начислений по базовому тарифу\" где изменение сальдо = -370,88", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 79
testRunner.Then("у поля Начислено взносов по минимальному тарифу всего есть детальная информация п" +
                    "о начисленным взносам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 81
testRunner.And("у этой записи по текущему периоду заполнено поле Сумма \"-370,88\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 83
testRunner.And("у поля Задолженность по взносам всего есть детальная информация по задолженности " +
                    "по взносам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 85
testRunner.And("у этой записи по текущему периоду заполнено поле Сумма \"-370,88\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("отмена пени у закрытого с долгом, у закрытого лс, у неактивного лс")]
        [NUnit.Framework.TestCaseAttribute("010032304", null)]
        [NUnit.Framework.TestCaseAttribute("010124407", null)]
        [NUnit.Framework.TestCaseAttribute("100000007", null)]
        public virtual void ОтменаПениУЗакрытогоСДолгомУЗакрытогоЛсУНеактивногоЛс(string лС, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("отмена пени у закрытого с долгом, у закрытого лс, у неактивного лс", exampleTags);
#line 87
this.ScenarioSetup(scenarioInfo);
#line 88
testRunner.Given("пользователь выбирает Период \"2014 Ноябрь\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 89
testRunner.And(string.Format("пользователь в реестре ЛС выбирает лицевой счет \"{0}\"", лС), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 90
testRunner.When("пользователь для текущего ЛС вызывает операцию Отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 91
testRunner.And("пользователь заполняет поле Период \"закрытый период\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 92
testRunner.And("пользователь заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 93
testRunner.And("пользователь заполняет поле Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 94
testRunner.And("пользователь сохраняет изменения в форме отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 95
testRunner.Then("в карточке ЛС в истории изменений нет записи \"Отмена начислений\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачная повторная отмена начислений")]
        public virtual void НеудачнаяПовторнаяОтменаНачислений()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачная повторная отмена начислений", ((string[])(null)));
#line 103
this.ScenarioSetup(scenarioInfo);
#line 104
testRunner.Given("пользователь выбирает Период \"2014 Ноябрь\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 105
testRunner.And("пользователь в реестре ЛС выбирает лицевой счет \"100000935\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 106
testRunner.When("пользователь для текущего ЛС вызывает операцию Отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 107
testRunner.And("в форме Отмены начислений присутствует запись по отменяемым начислениям", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 108
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Муниципальный район \"Караг" +
                    "инский муниципальный район\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 109
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Адрес \"п. Оссора, ул. Лука" +
                    "шевского, д. 73\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 110
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Номер ЛС \"100000935\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 111
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Сумма начислений за период" +
                    " \"240,16\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 112
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Отменить начисления в разм" +
                    "ере \"240,16\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 113
testRunner.And("пользователь заполняет поле Период \"закрытый период\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 114
testRunner.And("пользователь заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 115
testRunner.And("пользователь заполняет поле Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 116
testRunner.And("пользователь сохраняет изменения в форме отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 117
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 118
testRunner.And("у этой записи, в истории изменений ЛС, Наименование параметра \"Отмена начислений\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 119
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Отмена начи" +
                    "сления за период 2014 Ноябрь на сумму 240.16000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 120
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"240.16000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 121
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 123
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 124
testRunner.Given("пользователь выбирает Период \"2015 Февраль\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 125
testRunner.Then("в карточке этого лицевого счета, в операциях за текущий период, присутствует запи" +
                    "сь \"Отмена начислений по базовому тарифу\" где изменение сальдо = -240,16", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 126
testRunner.Then("у поля Начислено взносов по минимальному тарифу всего есть детальная информация п" +
                    "о начисленным взносам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 128
testRunner.And("у этой записи по текущему периоду заполнено поле Сумма \"-240,16000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 130
testRunner.And("у поля Задолженность по взносам всего есть детальная информация по задолженности " +
                    "по взносам", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 132
testRunner.And("у этой записи по текущему периоду заполнено поле Сумма \"-240,16000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 133
testRunner.Given("пользователь выбирает Период \"2014 Ноябрь\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 134
testRunner.And("пользователь в реестре ЛС выбирает лицевой счет \"100000935\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 135
testRunner.When("пользователь для текущего ЛС вызывает операцию Отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 136
testRunner.And("в форме Отмены начислений присутствует запись по отменяемым начислениям", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 137
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Муниципальный район \"Караг" +
                    "инский муниципальный район\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 138
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Адрес \"п. Оссора, ул. Лука" +
                    "шевского, д. 73\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 139
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Номер ЛС \"100000935\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 140
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Сумма начислений за период" +
                    " \"0,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 141
testRunner.And("у этой записи по отменяемым начислениям заполнено поле Отменить начисления в разм" +
                    "ере \"0,00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 142
testRunner.And("пользователь заполняет поле Период \"закрытый период\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 143
testRunner.And("пользователь заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 144
testRunner.And("пользователь заполняет поле Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 145
testRunner.And("пользователь сохраняет изменения в форме отмена начислений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 146
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 147
testRunner.And("у этой записи, в истории изменений ЛС, Наименование параметра \"Отмена начислений\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 148
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Отмена начи" +
                    "сления за период 2014 Ноябрь на сумму 240.16000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 149
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"240.16000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 150
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 152
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 153
testRunner.And("количество записей \"Отмена начислений\" в истории = 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion