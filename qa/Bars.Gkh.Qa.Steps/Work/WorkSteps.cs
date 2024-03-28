using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NUnit.Framework;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    internal class WorkSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<Work> _cashe = new BindingBase.DomainServiceCashe<Work>();

        private dynamic workService
        {
            get
            {
                try
                {
                    var workServiceType =
                        Type.GetType("Bars.Gkh.Overhaul.DomainService.Impl.WorkService, Bars.Gkh.Overhaul");

                    var workServiceInterface = workServiceType.GetInterface("IWorkService");
                    return Container.Resolve(workServiceInterface);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        [Given(@"пользователь добавляет новый вид работ")]
        public void ДопустимПользовательДобавляетНовыйВидРаботы()
        {
            WorkHelper.Current = new Work();
        }

        [Given(@"пользователь у этого вида работ заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеНаименование(string name)
        {
            WorkHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого вида работ заполняет поле Ед\. измерения")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеЕд_Измерения()
        {
            WorkHelper.Current.UnitMeasure = UnitMeasureHelper.Current;
        }

        [Given(@"пользователь у этого вида работ заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеКод(string code)
        {
            WorkHelper.Current.Code = code;
        }

        [Given(@"пользователь у этого вида работ заполняет поле Норматив ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеНорматив(decimal normative)
        {
            WorkHelper.Current.Normative = normative;
        }

        [Given(@"пользователь у этого вида работ заполняет поле Описание ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеОписание(string description)
        {
            WorkHelper.Current.Description = description;
        }

        [Given(@"пользователь у этого вида работ помечает поле Соответствие 185 ФЗ")]
        public void ДопустимПользовательУЭтогоВидаРаботыПомечаетПолеСоответствие185ФЗ()
        {
            WorkHelper.Current.Consistent185Fz = true;
        }

        [Given(@"пользователь у этого вида работ помечает поле Доп\. работа")]
        public void ЕслиПользовательУЭтогоВидаРаботыПомечаетПолеДоп_Работа()
        {
            WorkHelper.Current.IsAdditionalWork = true;
        }

        [Given(@"пользователь у этого вида работы заполняет поле Тип работ ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыЗаполняетПолеТипРабот(string workTypeName)
        {

            int workType;

            switch (workTypeName)
            {
                case "Работа":
                    {
                        workType = 10;
                        break;
                    }

                case "Услуга":
                    {
                        workType = 20;
                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("типа работ {0} не существует", workTypeName));
                    }
            }

            WorkHelper.Current.TypeWork = (TypeWork)workType;
        }

        [Given(@"пользователь у этого вида работ в источниках финансирования помечает поле (.*)")]
        public void ДопустимПользовательУЭтогоВидаРаботВИсточникахФинансированияПомечаетПоле(string typeName)
        {
            var finSourceType = WorkHelper.GetFinSource(typeName);

            WorkHelper.FinSources.Add((int)finSourceType);
        }

        [Given(@"пользователь у этого вида работ заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            WorkHelper.Current.Name =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого вида работ заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботЗаполняетПолеКодСимволов(int countOfSymbols, string symbol)
        {
            WorkHelper.Current.Code =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого вида работ заполняет поле Описание (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботЗаполняетПолеОписаниеСимволов(int countOfSymbols, string symbol)
        {
            WorkHelper.Current.Description =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"добавлен вид работы")]
        public void ДопустимДобавленВидРаботы(Table workTable)
        {
            var work = new Work
                           {
                               Name = workTable.Rows.First()["Name"],
                               Code = workTable.Rows.First()["Code"],
                               UnitMeasure = this._cashe.Get<UnitMeasure>().GetAll()
                               .OrderByDescending(x => x.ObjectCreateDate)
                               .First(x => x.Name == workTable.Rows.First()["UnitMeasure"])
                           };

            var parsedWork = new DynamicDictionary();

            parsedWork.WriteInstance(work);

            var records = new List<object> { parsedWork };

            var baseParams = new BaseParams();

            baseParams.Params.Add("records", records);
            baseParams.Params.Add("FinSources", new long[0]);

            this.workService.SaveWithFinanceType(baseParams, this._cashe.Current);

            WorkHelper.Current = this._cashe.Current.GetAll()
                        .OrderByDescending(x => x.ObjectCreateDate).First();
        }

        [When(@"пользователь сохраняет этот вид работ")]
        public void ЕслиПользовательСохраняетЭтотВидРаботы()
        {
            var work = new DynamicDictionary();
            work.WriteInstance(WorkHelper.Current);

            var records = new List<object> { work };

            var baseParams = new BaseParams();

            baseParams.Params.Add("records", records);

            baseParams.Params.Add("FinSources", WorkHelper.FinSources.ToArray());

            try
            {
                if (WorkHelper.Current.Id == 0)
                {
                    this.workService.SaveWithFinanceType(baseParams, this._cashe.Current);

                    var lastCreatedWork = this._cashe.Current.GetAll()
                        .OrderByDescending(x => x.ObjectCreateDate).First();

                    if (CommonHelper.IsNow(lastCreatedWork.ObjectCreateDate))
                    {
                        WorkHelper.Current = lastCreatedWork;
                    }
                }
                else
                {
                    this.workService.UpdateWithFinanceType(baseParams, this._cashe.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }

        [When(@"пользователь удаляет этот вид работ")]
        public void ЕслиПользовательУдаляетЭтотВидРабот()
        {
            var records = new long[] { WorkHelper.Current.Id };

            var baseParams = new BaseParams();

            baseParams.Params.Add("records", records);

            try
            {
                workService.DeleteWithFinanceType(baseParams, _cashe.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому виду работ присутствует в реестре видов работ")]
        public void ТоЗаписьПоЭтомуВидуРаботыПрисутствуетВРеестреВидовРабот()
        {
            var work = this._cashe.Current.Get(WorkHelper.Current.Id);
            
            ((Object)work).Should()
                .NotBeNull(
                    string.Format(
                        "вид работ должен присутствовать в справочнике видов работ.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому виду работ отсутствует в реестре видов работ")]
        public void ТоЗаписьПоЭтомуВидуРаботОтсутствуетВРеестреВидовРабот()
        {
            var work = this._cashe.Current.Get(WorkHelper.Current.Id);

            ((Object)work).Should()
                .BeNull(
                    string.Format(
                        "вид работ должен отсутствовать в справочнике видов работ.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"в этом виде работ значение в поле Соответствие 185 ФЗ = (.*)")]
        public void ТоВЭтомВидеРаботЗначениеВПолеСоответствиеФЗTrue(bool consistent183Fz)
        {
            var work = this._cashe.Current.Get(WorkHelper.Current.Id);

           ((Object)work.Consistent185Fz).Should()
                .Be(
                    consistent183Fz,
                    string.Format(
                        "в этом виде работ значение в поле Соответствие 185 ФЗ должно быть {0}. {1}",
                        consistent183Fz,
                                     ExceptionHelper.GetExceptions()));
        }

        [Then(@"в этом виде работ значение в поле Доп\. работа = (.*)")]
        public void ТоВЭтомВидеРаботЗначениеВПолеДоп_РаботаTrue(bool isAdditionalWork)
        {
            var work = this._cashe.Current.Get(WorkHelper.Current.Id);

            ((Object)work.IsAdditionalWork).Should()
                .Be(
                    isAdditionalWork,
                    string.Format(
                        "в этом виде работ значение в поле Доп. работа должно быть {0}. {1}",
                        isAdditionalWork,
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"в этом виде работ в источниках финансирования значение в поле (.*) = (.*)")]
        public void ТоВЭтомВидеРаботЗначениеВПолеБюджетРегионаTrue(string typeName, bool containsField)
        {
            var finSourceType = WorkHelper.GetFinSource(typeName);

            var contains = WorkHelper.ContainsFinSource(finSourceType);

            ((Object)contains).Should().Be(
                containsField,
                string.Format(
                    "в этом виде работ значение в поле {0} должно быть {1}. {2}",
                    typeName,
                    containsField,
                    ExceptionHelper.GetExceptions()));
        }
    }
}
