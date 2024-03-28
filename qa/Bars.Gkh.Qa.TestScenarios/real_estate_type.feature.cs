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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Типы домов\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляРазделаТипыДомовFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "real_estate_type.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Типы домов\"", "Справочники - Общие - Типы домов", ProgrammingLanguage.CSharp, new string[] {
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
                        "ShortName",
                        "Description"});
            table1.AddRow(new string[] {
                        "тест",
                        "тест",
                        "тест"});
#line 6
testRunner.Given("добавлена единица измерения", ((string)(null)), table1, "Дано ");
#line 10
testRunner.Given("пользователь добавляет Тип группы ООИ с Наименованием \"тестовый тип группы ООИ\" и" +
                    " Кодом \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 11
testRunner.And("пользователь добавляет объект общего имущества с Наименованием \"тестовый объект о" +
                    "бщего имущества\" и Кодом \"1233\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 12
testRunner.And("пользователь добавляет группу конструктивных элементов \"тестовая группа конструкт" +
                    "ивных элементов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 13
testRunner.And("пользователь добавляет конструктивный элемент \"тестовый конструктивный элемент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 16
testRunner.And("пользователь у этого Типа дома заполняет поле Наименование \"тип дома тестовый 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 17
testRunner.And("пользователь у этого Типа дома заполняет поле Код \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.And("пользователь у этого Типа дома заполняет поле Предельная стоимость ремонта \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление Типа дома")]
        public virtual void УспешноеДобавлениеТипаДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление Типа дома", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 21
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 22
testRunner.Then("запись по этому Типу дома присутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление записи из справочника Типы домов")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void УспешноеУдалениеЗаписиИзСправочникаТипыДомов()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление записи из справочника Типы домов", new string[] {
                        "ignore"});
#line 26
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 27
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 28
testRunner.Then("запись по этому Типу дома присутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 29
testRunner.And("пользователь удаляет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.Then("запись по этому Типу дома отсутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление Общей характеристики типа дома")]
        public virtual void УспешноеДобавлениеОбщейХарактеристикиТипаДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление Общей характеристики типа дома", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 33
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 34
testRunner.Given("пользователь добавляет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 35
testRunner.And("заполняет у этого Общего параметра поле Наименование \"тест общая характер\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.And("заполняет у этого Общего параметра поле Минимальное значение \"12312\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.And("заполняет у этого Общего параметра поле Максимальное значение \"12312\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.When("пользователь сохраняет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 39
testRunner.Then("запись по этой Общей характеристике присутствует в Типе дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление Общей характеристики типа дома")]
        public virtual void УспешноеУдалениеОбщейХарактеристикиТипаДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление Общей характеристики типа дома", ((string[])(null)));
#line 41
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 42
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 43
testRunner.Given("пользователь добавляет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 44
testRunner.And("заполняет у этого Общего параметра поле Наименование \"123ntcn\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 45
testRunner.And("заполняет у этого Общего параметра поле Минимальное значение \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 46
testRunner.And("заполняет у этого Общего параметра поле Максимальное значение \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.When("пользователь сохраняет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 48
testRunner.When("пользователь удаляет Общую характеристику типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 49
testRunner.Then("запись по этой Общей характеристике отсутствует в Типе дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление дубля в справочник Типы домов")]
        public virtual void УспешноеДобавлениеДубляВСправочникТипыДомов()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление дубля в справочник Типы домов", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 52
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 53
testRunner.Given("пользователь добавляет новый Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 54
testRunner.And("пользователь у этого Типа дома заполняет поле Наименование \"тип дома тестовый 111" +
                    "1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 55
testRunner.And("пользователь у этого Типа дома заполняет поле Код \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 56
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 57
testRunner.Then("запись по этому Типу дома присутствует в справочнике Типов домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление Конструктивного элемента типа дома")]
        public virtual void УспешноеДобавлениеКонструктивногоЭлементаТипаДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление Конструктивного элемента типа дома", ((string[])(null)));
#line 59
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 60
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 61
testRunner.Given("пользователь добавляет Конструктивный элемент типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 62
testRunner.And("заполняет поле КЭ присутствует отсутствует в доме \"false\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 63
testRunner.When("пользователь сохраняет Конструктивный элемент типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 64
testRunner.Then("запись по этому Конструктивному элементу присутствует в Типе дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление Конструктивного элемента типа дома")]
        public virtual void УспешноеУдалениеКонструктивногоЭлементаТипаДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление Конструктивного элемента типа дома", ((string[])(null)));
#line 66
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 67
testRunner.When("пользователь сохраняет этот Тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 68
testRunner.Given("пользователь добавляет Конструктивный элемент типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 69
testRunner.And("заполняет поле КЭ присутствует отсутствует в доме \"false\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 70
testRunner.When("пользователь сохраняет Конструктивный элемент типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 71
testRunner.When("пользователь удаляет Конструктивный элемент типа жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 72
testRunner.Then("запись по этому Конструктивному элементу отсутствует в Типе дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление муниципальных образований к типу дома")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void УспешноеДобавлениеМуниципальныхОбразованийКТипуДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление муниципальных образований к типу дома", new string[] {
                        "ignore"});
#line 76
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "RegionName"});
            table2.AddRow(new string[] {
                        "1",
                        "1"});
            table2.AddRow(new string[] {
                        "2",
                        "2"});
            table2.AddRow(new string[] {
                        "3",
                        "3"});
#line 77
testRunner.Given("добавлены муниципальные образования", ((string)(null)), table2, "Дано ");
#line 83
testRunner.When("пользователь сохраняет этот тип дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 84
testRunner.And("добавляет к типу муниципальные образования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 85
testRunner.When("пользователь сохраняет эти муниципальные образования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 86
testRunner.Then("запись по этим муниципальным образованиям присутствует в этом типе дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion