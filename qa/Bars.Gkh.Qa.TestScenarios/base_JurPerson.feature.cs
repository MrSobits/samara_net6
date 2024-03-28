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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Плановые проверки юридических лиц\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляРазделаПлановыеПроверкиЮридическихЛицFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "base_JurPerson.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Плановые проверки юридических лиц\"", "Жилищная инспекция - Основания проверок - Плановые проверки юридических лиц", ProgrammingLanguage.CSharp, new string[] {
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
        
        public virtual void FeatureBackground()
        {
#line 5
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Code",
                        "OkopfCode"});
            table1.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 6
testRunner.Given("добавлена организационно-правовая форма", ((string)(null)), table1, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table2.AddRow(new string[] {
                        "тест"});
#line 10
testRunner.Given("добавлен контрагент с организационно правовой формой", ((string)(null)), table2, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table3.AddRow(new string[] {
                        "тест"});
#line 14
testRunner.And("добавлена управляющая организация для этого контрагента", ((string)(null)), table3, "И ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DateStart",
                        "DateEnd"});
            table4.AddRow(new string[] {
                        "те1стовый план мероприятий",
                        "01.01.2015",
                        "01.01.2016"});
#line 18
testRunner.And("добавлен план проверки юридических лиц", ((string)(null)), table4, "И ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Code",
                        "Fio",
                        "Email"});
            table5.AddRow(new string[] {
                        "159",
                        "Тестовый инспектор",
                        "test@test.test"});
#line 22
testRunner.And("добавлен инспектор", ((string)(null)), table5, "И ");
#line 26
testRunner.And("пользователь добавляет новую плановую проверку юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле Тип юридичес" +
                    "кого лица \"Управляющая организация\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле Юридическое " +
                    "лицо этим контрагентом", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле План этим пл" +
                    "аном", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле Дата начала " +
                    "проверки \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле Инспекторы э" +
                    "тим инспектором", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление плановой проверки юридических лиц")]
        public virtual void УспешноеДобавлениеПлановойПроверкиЮридическихЛиц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление плановой проверки юридических лиц", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 34
testRunner.When("пользователь сохраняет эту плановую проверку юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 35
testRunner.Then("запись по этой плановой проверки юридических лиц присутствует в справочнике плано" +
                    "вых проверок юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление плановой проверки юридических лиц")]
        public virtual void УспешноеУдалениеПлановойПроверкиЮридическихЛиц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление плановой проверки юридических лиц", ((string[])(null)));
#line 37
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 38
testRunner.When("пользователь сохраняет эту плановую проверку юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 39
testRunner.And("пользователь удаляет эту плановую проверку юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.Then("запись по этой плановой проверки юридических лиц отсутствует в справочнике планов" +
                    "ых проверок юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление дубля плановой проверки юридических лиц")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void УспешноеДобавлениеДубляПлановойПроверкиЮридическихЛиц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление дубля плановой проверки юридических лиц", new string[] {
                        "ignore"});
#line 43
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 44
testRunner.When("пользователь сохраняет эту плановую проверку юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 45
testRunner.Given("пользователь добавляет новую плановую проверку юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 46
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле Тип юридичес" +
                    "кого лица \"Управляющая организация\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле Юридическое " +
                    "лицо этим контрагентом", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле План этим пл" +
                    "аном", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 49
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле Дата начала " +
                    "проверки \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 50
testRunner.And("пользователь у этой плановой проверки юридических лиц заполняет поле Инспекторы э" +
                    "тим инспектором", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 51
testRunner.When("пользователь сохраняет эту плановую проверку юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 52
testRunner.Then("запись по этой плановой проверки юридических лиц присутствует в справочнике плано" +
                    "вых проверок юридических лиц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion