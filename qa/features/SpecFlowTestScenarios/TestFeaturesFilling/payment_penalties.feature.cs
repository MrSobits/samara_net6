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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Параметры начисления пени\"")]
    public partial class ТесткейсыДляРазделаПараметрыНачисленияПениFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "payment_penalties.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Параметры начисления пени\"", "Администрирование - Настройки приложения - Единые настройки приложения - раздел в" +
                    " дереве \"Параметры начисления пени\"", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 7
#line 8
testRunner.Given("пользователь добавляет новый расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 9
testRunner.And("пользователь у этого расчета пеней заполняет поле Количество дней \"15\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.And("пользователь у этого расчета пеней заполняет поле Ставка рефинансирования, % \"15\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.And("пользователь у этого расчета пеней заполняет поле Дата начала \"10.01.2115\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление расчета пеней")]
        [NUnit.Framework.TestCaseAttribute("Специальный счет", null)]
        [NUnit.Framework.TestCaseAttribute("Счет регионального оператора", null)]
        public virtual void УспешноеДобавлениеРасчетаПеней(string способФормированияФонда, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление расчета пеней", exampleTags);
#line 14
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 15
testRunner.Given(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 16
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.Then("запись по этому расчету пеней присутствует в разделе параметров начисления пени", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 18
testRunner.And("у этой записи не заполнено поле Дата окончания", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление дубля расчета пеней, с одинаковой датой")]
        [NUnit.Framework.TestCaseAttribute("Специальный счет", null)]
        [NUnit.Framework.TestCaseAttribute("Счет регионального оператора", null)]
        public virtual void НеудачноеДобавлениеДубляРасчетаПенейСОдинаковойДатой(string способФормированияФонда, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление дубля расчета пеней, с одинаковой датой", exampleTags);
#line 25
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 26
testRunner.Given(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 27
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 28
testRunner.And("у этой записи не заполнено поле Дата окончания", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.And("пользователь добавляет новый расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("пользователь у этого расчета пеней заполняет поле Количество дней \"15\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("пользователь у этого расчета пеней заполняет поле Ставка рефинансирования, % \"15\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("пользователь у этого расчета пеней заполняет поле Дата начала \"10.01.2115\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 35
testRunner.Then("запись по этому расчету пеней отсутствует в разделе параметров начисления пени", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 36
testRunner.And("падает ошибка с текстом \"Внимание! Дата начала действия нового значения не может " +
                    "быть раньше уже действующего параметра!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление дубля расчета пеней, с датой дубля < даты оригинала записи")]
        [NUnit.Framework.TestCaseAttribute("Специальный счет", null)]
        [NUnit.Framework.TestCaseAttribute("Счет регионального оператора", null)]
        public virtual void НеудачноеДобавлениеДубляРасчетаПенейСДатойДубляДатыОригиналаЗаписи(string способФормированияФонда, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление дубля расчета пеней, с датой дубля < даты оригинала записи", exampleTags);
#line 43
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 44
testRunner.Given(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 45
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 46
testRunner.And("пользователь добавляет новый расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.And("пользователь у этого расчета пеней заполняет поле Количество дней \"15\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.And("пользователь у этого расчета пеней заполняет поле Ставка рефинансирования, % \"15\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 49
testRunner.And("пользователь у этого расчета пеней заполняет поле Дата начала \"09.01.2115\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 50
testRunner.And(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 51
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 52
testRunner.Then("запись по этому расчету пеней отсутствует в разделе параметров начисления пени", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 53
testRunner.And("падает ошибка с текстом \"Внимание! Дата начала действия нового значения не может " +
                    "быть раньше уже действующего параметра!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление дубля расчета пеней, с датой дубля > даты оригинала записи")]
        [NUnit.Framework.TestCaseAttribute("Специальный счет", null)]
        [NUnit.Framework.TestCaseAttribute("Счет регионального оператора", null)]
        public virtual void УдачноеДобавлениеДубляРасчетаПенейСДатойДубляДатыОригиналаЗаписи(string способФормированияФонда, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление дубля расчета пеней, с датой дубля > даты оригинала записи", exampleTags);
#line 60
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 61
testRunner.Given(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 62
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 63
testRunner.And("пользователь добавляет новый расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 64
testRunner.And("пользователь у этого расчета пеней заполняет поле Количество дней \"15\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 65
testRunner.And("пользователь у этого расчета пеней заполняет поле Ставка рефинансирования, % \"15\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 66
testRunner.And("пользователь у этого расчета пеней заполняет поле Дата начала \"11.01.2115\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 67
testRunner.And(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 68
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 69
testRunner.Then("запись по этому расчету пеней присутствует в разделе параметров начисления пени", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 70
testRunner.And("у этой записи заполнено поле Дата окончания", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление исключения к параметру расчета пеней")]
        [NUnit.Framework.TestCaseAttribute("Специальный счет", null)]
        [NUnit.Framework.TestCaseAttribute("Счет регионального оператора", null)]
        public virtual void УспешноеДобавлениеИсключенияКПараметруРасчетаПеней(string способФормированияФонда, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление исключения к параметру расчета пеней", exampleTags);
#line 78
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 79
testRunner.Given(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 80
testRunner.And("пользователь добавляет исключение лс \"100000001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 81
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 82
testRunner.Then("в списке исключений у этого параметра расчета пеней присутствует исключение для э" +
                    "того лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление исключения из параметра расчета пеней")]
        [NUnit.Framework.TestCaseAttribute("Специальный счет", null)]
        [NUnit.Framework.TestCaseAttribute("Счет регионального оператора", null)]
        public virtual void УспешноеУдалениеИсключенияИзПараметраРасчетаПеней(string способФормированияФонда, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление исключения из параметра расчета пеней", exampleTags);
#line 90
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 91
testRunner.Given(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 92
testRunner.And("пользователь добавляет исключение лс \"100000001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 93
testRunner.And("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 94
testRunner.When("пользователь удаляет это исключение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 95
testRunner.And("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 96
testRunner.Then("в списке исключений у этого параметра расчета пеней отсутствует исключение для эт" +
                    "ого лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Отсутствие повторного выбора лс в исключениях")]
        [NUnit.Framework.TestCaseAttribute("Специальный счет", null)]
        [NUnit.Framework.TestCaseAttribute("Счет регионального оператора", null)]
        public virtual void ОтсутствиеПовторногоВыбораЛсВИсключениях(string способФормированияФонда, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Отсутствие повторного выбора лс в исключениях", exampleTags);
#line 103
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 104
testRunner.Given(string.Format("пользователь у этого расчета пеней заполняет поле Способ формирования фонда {0}", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 105
testRunner.And("пользователь добавляет исключение лс \"100000001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 106
testRunner.And("в списке исключений у этого параметра расчета пеней присутствует исключение для э" +
                    "того лс", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 107
testRunner.When("пользователь сохраняет этот расчет пеней", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 108
testRunner.And("пользователь вызывает список с лс для исключения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 109
testRunner.Then("в этом списке отсутствует лс \"100000001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion