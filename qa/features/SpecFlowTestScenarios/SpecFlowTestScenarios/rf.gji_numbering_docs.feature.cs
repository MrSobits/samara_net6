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
    [NUnit.Framework.DescriptionAttribute("Нумерация документов ГЖИ")]
    public partial class НумерацияДокументовГЖИFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "rf.gji_numbering_docs.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Нумерация документов ГЖИ", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("подготовка ситемы к тестированию нумерации документов ГЖИ (Забайкалье)")]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "Администратор", "Документы ГЖИ", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "Администратор", "Документы ГЖИ", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "Администратор", "Документы ГЖИ", "Зарегистрирован", "Черновик", null)]
        public virtual void ПодготовкаСитемыКТестированиюНумерацииДокументовГЖИЗабайкалье(string документы, string пользователь, string раздел, string статус1, string статус2, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("подготовка ситемы к тестированию нумерации документов ГЖИ (Забайкалье)", exampleTags);
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.Given("Пользователь логин \"Zimin\", пароль \"1qazse4321\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-zabaykalye\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.When("пользователь смотрит на список имеющихся правил в правилах перехода статуса", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 9
testRunner.Then("видит в нем правило = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 10
testRunner.When(string.Format("пользователь добавляет правило перехода статуса с переходом для пользователя \"<по" +
                        "льзователь>\" по разделу \"{0}\" для документа \"<документ>\" со статуса \"{1}\" на ста" +
                        "тус \"{2}\" с правилом = \"\"", раздел, статус1, статус2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 11
testRunner.Then("добавляется новая строка с правилом перехода статуса", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 12
testRunner.When("проверяю настройки ограничений для роли = \"Администратор\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 13
testRunner.Then("по документам <документ> есть возможность настройки доступа к полю \"Номер докумен" +
                    "та\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 14
testRunner.When(string.Format("перевожу документ из статуса \"{0}\" на статус \"{1}\"", статус1, статус2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.Then("отрабатывает правило присвоения номера и у документа <документ> проставляется \"Но" +
                    "мер документа\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Автоматическая нумерация документов ГЖИ с нового года (Забайкаье)")]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "Зарегистрирован", "Черновик", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "Черновик", "Присвоение номера", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "Присвоение номера", "Зарегистрирован", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "Зарегистрирован", "Черновик", null)]
        public virtual void АвтоматическаяНумерацияДокументовГЖИСНовогоГодаЗабайкаье(string тип_Документа, string статус1, string статус2, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Автоматическая нумерация документов ГЖИ с нового года (Забайкаье)", exampleTags);
#line 48
this.ScenarioSetup(scenarioInfo);
#line 49
testRunner.Given("Пользователь логин \"Zimin\", пароль \"1qazse4321\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 50
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-zabaykalye\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.When("пользователь сохраняет документ с незаполненной Датой документа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 54
testRunner.Then("карточка докуемнта не сохраняется и выходит сообщение об ошибке что не заполнена " +
                    "дата документа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 55
testRunner.When("пользоваетль заполняет Дату документа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 56
testRunner.And("сохраняет документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 57
testRunner.And(string.Format("переводит документ из статуса {0} в статус {1}, на котором висит правило присвоен" +
                        "ия номера", статус1, статус2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 58
testRunner.Then(string.Format("система ищет за выбранный год имеющиеся документы {0}", тип_Документа), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 59
testRunner.When("таких документов за выбранный год из даты документа по выбранному типу документа " +
                    "НЕТ и документ первый в году", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Если ");
#line 60
testRunner.Then("нумерация документов одного типа начинается с единицы \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Автоматическая нумерация документов ГЖИ по порядку (Забайкаье)")]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "второе в году", "2050", "1", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Распоряжение", "четвертое в году", "2050", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "второе в году", "2050", "1", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки", "четвертое в году", "2050", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "второе в году", "2050", "1", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Акт обследования", "четвертое в году", "2050", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "второе в году", "2050", "1", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Акт осмотра", "четвертое в году", "2050", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "второе в году", "2050", "1", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Предписание", "четвертое в году", "2050", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "второе в году", "2050", "1", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Протокол", "четвертое в году", "2050", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "второе в году", "2050", "1", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление", "четвертое в году", "2050", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "второе в году", "2050", "1", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Постановление прокуратуры", "четвертое в году", "2050", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "Первый в году", "2050", "без номера", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "второе в году", "2050", "1", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "третье в году", "2050", "2", "был в ручную проставлен 5", null)]
        [NUnit.Framework.TestCaseAttribute("Акт проверки предписания", "четвертое в году", "2050", "5", "6", null)]
        public virtual void АвтоматическаяНумерацияДокументовГЖИПоПорядкуЗабайкаье(string тип_Документа, string документ, string год, string предыдущий_Порядковый_Номер, string номер_Нового_Документа, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Автоматическая нумерация документов ГЖИ по порядку (Забайкаье)", exampleTags);
#line 95
this.ScenarioSetup(scenarioInfo);
#line 96
testRunner.Given("Пользователь логин \"Zimin\", пароль \"1qazse4321\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 97
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-zabaykalye\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 100
testRunner.When("пользоваетль заполняет Дату документа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 101
testRunner.And("сохраняет документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 102
testRunner.And("переводит документ в статус, на котором висит правило присвоения номера", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 103
testRunner.Then(string.Format("система ищет за выбранный год имеющиеся документы {0}", тип_Документа), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 104
testRunner.When(string.Format("такие документы за выбранный год из даты документа по выбранному типу документа Е" +
                        "СТЬ и документ НЕ первый в году {0}", год), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Если ");
#line 105
testRunner.Then(string.Format("система ищет самый большой порядковый номер {0} из указанного года в Дате докумен" +
                        "та", предыдущий_Порядковый_Номер), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 106
testRunner.And(string.Format("добавляет к порядковому номеру документа {0} единицу и присваивает получившийся н" +
                        "омер новому обращению {1}", документ, номер_Нового_Документа), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
