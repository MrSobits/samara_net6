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
    [NUnit.Framework.DescriptionAttribute("Настройка реестра неплательщиков")]
    public partial class НастройкаРеестраНеплательщиковFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "pir_setting_defaulters.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Настройка реестра неплательщиков", "Претензионная работа - Настройка - Настройка реестра неплательщиков", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("успешное сохранение заполненных полей по реструктуризация долга")]
        [NUnit.Framework.TestCaseAttribute("Кол-во дней для приостановления права", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Кол-во дней для расторжения", "1", null)]
        [NUnit.Framework.TestCaseAttribute("Кол-во дней для приостановления права", "", null)]
        [NUnit.Framework.TestCaseAttribute("Кол-во дней для расторжения", "", null)]
        public virtual void УспешноеСохранениеЗаполненныхПолейПоРеструктуризацияДолга(string аттрибут, string значение, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное сохранение заполненных полей по реструктуризация долга", exampleTags);
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
testRunner.When(string.Format("пользователь заполняет поле \"{0}\" значением \"{1}\" и сохраняет настройку реестра н" +
                        "еплательщиков", аттрибут, значение), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 8
testRunner.Then(string.Format("введенное значение записываются в поле \"{0}\"", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное сохранение заполненных полей по реструктуризация долга")]
        [NUnit.Framework.TestCaseAttribute("Кол-во дней для приостановления права", "1,2", null)]
        [NUnit.Framework.TestCaseAttribute("Кол-во дней для расторжения", "1,2", null)]
        [NUnit.Framework.TestCaseAttribute("Кол-во дней для приостановления права", "тест", null)]
        [NUnit.Framework.TestCaseAttribute("Кол-во дней для расторжения", "тест", null)]
        public virtual void НеудачноеСохранениеЗаполненныхПолейПоРеструктуризацияДолга(string аттрибут, string значение, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное сохранение заполненных полей по реструктуризация долга", exampleTags);
#line 18
this.ScenarioSetup(scenarioInfo);
#line 19
testRunner.When(string.Format("пользователь заполняет поле \"{0}\" значением \"{1}\" и сохраняет настройку реестра н" +
                        "еплательщиков", аттрибут, значение), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 20
testRunner.Then(string.Format("введенное значение записываются в поле \"{0}\"", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("формирование претензии когда Тип получателя = Все")]
        public virtual void ФормированиеПретензииКогдаТипПолучателяВсе()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("формирование претензии когда Тип получателя = Все", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
testRunner.When("пользователь в настройке реестра неплательщиков в поле \"Документ претензии\" выбир" +
                    "ает значение \"Формировать\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 32
testRunner.And("в настройке реестра неплательщиков в поле \"Тип получателя\" знаечние = \"Все\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.Then("в реестре неплательщиков \"реестр неплательщиков\" формируется претензия", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("формирование претензии когда Тип получателя = Юр.лицо")]
        public virtual void ФормированиеПретензииКогдаТипПолучателяЮр_Лицо()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("формирование претензии когда Тип получателя = Юр.лицо", ((string[])(null)));
#line 36
this.ScenarioSetup(scenarioInfo);
#line 37
testRunner.When("пользователь в настройке реестра неплательщиков в поле \"Документ претензии\" выбир" +
                    "ает значение \"Формировать\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 38
testRunner.And("в настройке реестра неплательщиков в поле \"Тип получателя\" знаечние = \"Юр.лицо\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.Then("в реестре неплательщиков \"реестр неплательщиков\" формируется претензия, у которых" +
                    " \"Тип абонента\" = \"Юридическое лицо\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 40
testRunner.And("в реестре неплательщиков \"реестр неплательщиков\" не формируется претензия, у кото" +
                    "рых \"Тип абонента\" = \"Физическое лицо\" по сценарию \"отказ в формировании претенз" +
                    "ии\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("формирование претензии когда Тип получателя = Физ.лицо")]
        public virtual void ФормированиеПретензииКогдаТипПолучателяФиз_Лицо()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("формирование претензии когда Тип получателя = Физ.лицо", ((string[])(null)));
#line 43
this.ScenarioSetup(scenarioInfo);
#line 44
testRunner.When("пользователь в настройке реестра неплательщиков в поле \"Документ претензии\" выбир" +
                    "ает значение \"Формировать\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 45
testRunner.And("в настройке реестра неплательщиков в поле \"Тип получателя\" знаечние = \"Физ.лицо\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 46
testRunner.Then("в реестре неплательщиков \"реестр неплательщиков\" формируется претензия, у которых" +
                    " \"Тип абонента\" = \"Физическое лицоо\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 47
testRunner.And("в реестре неплательщиков \"реестр неплательщиков\" не формируется претензия, у кото" +
                    "рых \"Тип абонента\" = \"Юридическое лиц\" по сценарию \"отказ в формировании претенз" +
                    "ии\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("отказ в формировании претензии")]
        public virtual void ОтказВФормированииПретензии()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("отказ в формировании претензии", ((string[])(null)));
#line 50
this.ScenarioSetup(scenarioInfo);
#line 51
testRunner.When("пользователь в настройке реестра неплательщиков в поле \"Документ претензии\" выбир" +
                    "ает значение \"Не формировать\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 52
testRunner.And("пользователь формирует реестр неплательщиков из регионального фонда", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.Then("в реестре неплательщиков \"реестр неплательщиков\" не формируется претензия", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 54
testRunner.And("у записи в реестре неплательщиков претензионной работы значение статуса \"статус\" " +
                    "!= \"Требует формирование претензии\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 55
testRunner.And("у записи в реестре неплательщиков претензионной работы значение статуса \"статус\" " +
                    "!= \"Сформирована претензия\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 56
testRunner.When("пользователь редактирует запись в реестре неплательщиков и производит действие \"С" +
                    "формировать\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 57
testRunner.Then("в списке аттрибутов возможных действий нет аттрибута \"Претензия\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 58
testRunner.And("пользователь формирует исковое заявление \"Исковое заявление\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion