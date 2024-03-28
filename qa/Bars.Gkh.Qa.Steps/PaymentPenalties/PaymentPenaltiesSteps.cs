using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.Qa.Steps
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Controller.Provider;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.RegOperator.Controllers;
    using Bars.Gkh.RegOperator.Entities.Period;

    using NUnit.Framework;

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Domain;
    using FluentAssertions;

    using RegOperator.DomainModelServices;
    using RegOperator.DomainService;

    using TechTalk.SpecFlow;

    [Binding]
    internal class PaymentPenaltiesSteps : BindingBase
    {
        [Given(@"пользователь добавляет новый расчет пеней")]
        public void ДопустимПользовательДобавляетНовыйРасчетПеней()
        {
            PaymentPenaltiesHelper.Current = new PaymentPenalties();
        }

        [Given(@"пользователь у этого расчета пеней заполняет поле Количество дней ""(.*)""")]
        public void ДопустимПользовательУЭтогоРасчетаПенейЗаполняетПолеКоличествоДней(int days)
        {
            PaymentPenaltiesHelper.Current.Days = days;
        }

        [Given(@"пользователь у этого расчета пеней заполняет поле Ставка рефинансирования, % ""(.*)""")]
        public void ДопустимПользовательУЭтогоРасчетаПенейЗаполняетПолеСтавкаРефинансирования(decimal percentage)
        {
            PaymentPenaltiesHelper.Current.Percentage = percentage;
        }

        [Given(@"пользователь у этого расчета пеней заполняет поле Дата начала ""(.*)""")]
        public void ДопустимПользовательУЭтогоРасчетаПенейЗаполняетПолеДатаНачала(string dateStart)
        {
            DateTime date;

            if (!DateTime.TryParse(dateStart, out date))
            {
                throw new SpecFlowException("Не правильный формат даты в заполнении Даты начала расчета пеней");
            }

            PaymentPenaltiesHelper.Current.DateStart = date;
        }

        [Given(@"пользователь у этого расчета пеней заполняет поле Способ формирования фонда ""(.*)""")]
        public void ДопустимПользовательУЭтогоРасчетаПенейЗаполняетПолеСпособФормированияФонда(
            string decisionTypeDisplay)
        {
            var decisionType = EnumHelper.GetFromDisplayValue<CrFundFormationDecisionType>(decisionTypeDisplay);

            PaymentPenaltiesHelper.Current.DecisionType = decisionType;
        }

        [When(@"пользователь сохраняет этот расчет пеней")]
        public void ЕслиПользовательСохраняетЭтотРасчетПеней()
        {
            try
            {
                if (PaymentPenaltiesHelper.Current.Id > 0)
                {
                    PaymentPenaltiesHelper.DomainService.Update(PaymentPenaltiesHelper.Current);
                }
                else
                {
                    PaymentPenaltiesHelper.DomainService.Save(PaymentPenaltiesHelper.Current);
                }

                var queueNumber = 1;

                if (ScenarioContext.Current.Keys.Any(x => x.Contains("PaymentPenaltiesId")))
                {
                    var numList =
                        ScenarioContext.Current.Keys
                            .Where(x => x.Contains("PaymentPenaltiesId"))
                            .Select(x => int.Parse(x.Replace("PaymentPenaltiesId", string.Empty)))
                            .OrderByDescending(x => x);

                    queueNumber = numList.First() + 1;
                }

                ScenarioContext.Current.Add(string.Format("PaymentPenaltiesId{0}", queueNumber),
                    PaymentPenaltiesHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот расчет пеней")]
        public void ЕслиПользовательУдаляетЭтотРасчетПеней()
        {
            try
            {
                PaymentPenaltiesHelper.DomainService.Delete(PaymentPenaltiesHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому расчету пеней присутствует в разделе параметров начисления пени")]
        public void ТоЗаписьПоЭтомуРасчетуПенейПрисутствуетВРазделеПараметровНачисленияПени()
        {
            var paymentPenalties = PaymentPenaltiesHelper.DomainService.Get(PaymentPenaltiesHelper.Current.Id) as object;

            paymentPenalties.Should().NotBeNull(
                string.Format(
                    "запись по этому расчету пеней должна присутствовать в разделе параметров начисления пени. {0}",
                    ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому расчету пеней отсутствует в разделе параметров начисления пени")]
        public void ТоЗаписьПоЭтомуРасчетуПенейОтсутствуетВРазделеПараметровНачисленияПени()
        {
            var paymentPenalties = PaymentPenaltiesHelper.DomainService.Get(PaymentPenaltiesHelper.Current.Id) as object;

            paymentPenalties.Should().BeNull(
                string.Format(
                    "запись по этому расчету пеней должна отсутствовать в разделе параметров начисления пени. {0}",
                    ExceptionHelper.GetExceptions()));
        }


        [Then(@"у этой записи не заполнено поле Дата окончания")]
        public void ТоУЭтойЗаписиНеЗаполненоПолеДатаОкончания()
        {
            var paymentPenalties = PaymentPenaltiesHelper.DomainService.Get(PaymentPenaltiesHelper.Current.Id);

            var dateEndField = (DateTime?) paymentPenalties.DateEnd;

            dateEndField.Should().Be(null, "Дата окончания, должна быть пустой");
        }

        [Then(@"у этой записи поле Дата окончания должна быть ""(.*)""")]
        public void ТоУЭтойЗаписиПолеДатаОкончанияДолжнаБыть(string endDate)
        {
            var paymentPenalties = PaymentPenaltiesHelper.DomainService.Get(PaymentPenaltiesHelper.Current.Id);

            var dateEndField = paymentPenalties.DateEnd;

            DateTime parsedEndDate;

            DateTime.TryParse(endDate, out parsedEndDate);

            dateEndField.Should().Be(parsedEndDate, string.Format("Дата окончания должна быть {0}", endDate));
        }

        [Then(@"у записи с очередностью ""(.*)"" поле Дата окончания должна быть ""(.*)""")]
        public void ТоУЗаписиПодНомеромПолеДатаОкончанияДолжнаБыть(int queueNum, string endDate)
        {
            var paymentPenaltiesId = ScenarioContext.Current[string.Format("PaymentPenaltiesId{0}", queueNum)];

            var paymentPenalties = PaymentPenaltiesHelper.DomainService.Get(paymentPenaltiesId);

            var dateEndField = paymentPenalties.DateEnd;

            DateTime parsedEndDate;

            DateTime.TryParse(endDate, out parsedEndDate);

            dateEndField.Should().Be(parsedEndDate, string.Format("Дата окончания должна быть {0}", endDate));
        }

        [When(@"пользователь вызывает операцию расчета лс")]
        public void ДопустимПользовательВызываетОперациюРасчетаЛс()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"periodId", ChargePeriodHelper.Current.Id}
                }
            };

            try
            {
                baseParams.Params.Add("ids", BasePersonalAccountListHelper.GetIds());
            }
            catch
            {
                baseParams.Params.Add("ids", null);
            }

            var controllerProvider = new ControllerProvider(Container);

            var chargeController = (ChargeController)controllerProvider.GetController(Container, "Charge");

            try
            {
                var result = (JsonNetResult)chargeController.MakeUnacceptedCharge(baseParams);

                var createTaskResult = (CreateTasksDataResult)result.Data;

                if (!createTaskResult.Success)
                {
                    ExceptionHelper.AddException("ChargeController.MakeUnacceptedCharge", createTaskResult.Message);
                }
                else
                {
                    ScenarioContext.Current["TaskEntryParentTaskId"] = createTaskResult.Data.ParentTaskId;
                }
            }
            finally
            {
                Container.Release(chargeController);
            }
        }

        [When(@"в списке задач присутствует запись с Наименованием ""(.*)"" текущей датой и временем импорта")]
        public void ЕслиВСпискеЗадачПрисутствуетЗаписьСНаименованиемТекущейДатойИВременемИмпорта(string name)
        {
            ScenarioContext.Current["TaskEntryList"] = Container.Resolve<IDomainService<TaskEntry>>()
                .GetAll().OrderBy(x => x.ObjectCreateDate).ToList().Last();
        }

        [Then(@"в задачах не появляется запись по расчету лс")]
        public void ТоВЗадачахНеПоявляетсяЗаписьПоРасчетуЛс()
        {
            ScenarioContext.Current["TaskEntryListCount"] = Container.Resolve<IDomainService<TaskEntry>>()
                .GetAll().ToList().Count();
        }

        [Given(@"пользователь проверяет количество задач в разделе Задачи")]
        public void ДопустимПользовательПроверяетКоличествоЗадачВРазделеЗадачи()
        {
            Container.Resolve<IDomainService<TaskEntry>>()
                .GetAll()
                .ToList()
                .Count()
                .Should()
                .Be((int) ScenarioContext.Current["TaskEntryListCount"], "изменилось количество задач в списке Задач");
        }

        [Given(@"в параметрах начисления пени выбрана запись со способом формирования ""(.*)""")]
        public void ДопустимВПараметрахНачисленияПениВыбранаЗаписьСоСпособомФормирования(string decisionType)
        {
            var penalties = Container.Resolve<IDomainService<PaymentPenalties>>().GetAll().ToList();

            CrFundFormationDecisionType type = 0;

            switch (decisionType)
            {
                case "Специальный счет":
                    type = CrFundFormationDecisionType.SpecialAccount;
                    break;
                case "Счет регионального оператора":
                    type = CrFundFormationDecisionType.RegOpAccount;
                    break;
                default:
                    Assert.Fail("Способ формирование {0} не найден", decisionType);
                    break;
            }

            PaymentPenaltiesHelper.Current = penalties.FirstOrDefault(x => x.DecisionType == type);

            PaymentPenaltiesHelper.Current.Should()
                .NotBeNull(String.Format("Не найдена запись со способом формирования {0} в параметрах начисления пени",
                    decisionType));
        }

        [Given(@"у этой записи в параметрах начисления пени заполнено поле Количество дней ""(.*)""")]
        public void ДопустимУЭтойЗаписиВПараметрахНачисленияПениЗаполненоПолеКоличествоДней(int days)
        {
            PaymentPenaltiesHelper.Current.Days = days;
        }

        [Given(@"у этой записи в параметрах начисления пени заполнено поле Ставка рефинансирования, % ""(.*)""")]
        public void ДопустимУЭтойЗаписиВПараметрахНачисленияПениЗаполненоПолеСтавкаРефинансирования(int percentage)
        {
            PaymentPenaltiesHelper.Current.Percentage = percentage;
        }

        [Given(@"пользователь сохраняет эту запись")]
        public void ДопустимПользовательСохраняетЭтуЗапись()
        {
            try
            {
                Container.Resolve<IDomainService<PaymentPenalties>>().SaveOrUpdate(PaymentPenaltiesHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"протоколе расчета есть запись по пени по лс ""(.*)"" за период ""(.*)""")]
        public void ТоПротоколеРасчетаЕстьЗаписьПоПениПоЛсЗаПериод(string account, string period)
        {
            var cargePeriod = Container.Resolve<IDomainService<PeriodCloseCheckResult>>().FirstOrDefault(x => x.Name == period);

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "filter",
                        new List<object>
                        {
                            new DynamicDictionary
                            {
                                {"property", "accountId"},
                                {"value", BasePersonalAccountHelper.Current.Id}
                            },
                            new DynamicDictionary {{"property", "periodId"}, {"value", cargePeriod.Id}}
                        }
                    }
                }
            };

            var result = Container.Resolve<IPersonalAccountSummaryService>().ListPenaltyParameterTrace(baseParams).Data;

            ((IList) result).Cast<object>()
                .Should()
                .NotBeNullOrEmpty(
                    String.Format("Нет записей по Пени в протоколе расчета по лицевому счету {0}, за период {1}",
                        account, period));
        }

        [Given(@"пользователь для текущего ЛС вызывает операцию Установка и изменение пени")]
        public void ЕслиПользовательДляТекущегоЛСВызываетОперациюУстановкаИИзменениеПени()
        {
            var baseParamsSetPenalty = new BaseParams
            {
                Params =
                    new DynamicDictionary {{"AccountId", BasePersonalAccountHelper.Current.Id}, {"DebtPenalty", "0"}},
                Files = new Dictionary<string, FileData> {{"Document", null}}
            };

            ScenarioContext.Current["baseParamsSetPenalty"] = baseParamsSetPenalty;
        }

        [Given(@"пользователь в форме установка и изменение пени заполняет поле Новое пени ""(.*)""")]
        public void ЕслиПользовательЗаполняетПолеНовоеПени(int p0)
        {
            ScenarioContext.Current.Get<BaseParams>("baseParamsSetPenalty").Params["NewPenalty"] = p0;
        }

        [Given(@"пользователь в форме установка и изменение пени заполняет поле Причина изменения пени ""(.*)""")]
        public void ЕслиПользовательЗаполняетПолеПричинаИзмененияПени(string p0)
        {
            ScenarioContext.Current.Get<BaseParams>("baseParamsSetPenalty").Params["Reason"] = p0;
        }

        [When(@"пользователь сохраняет изменения в форме установка и изменение пени")]
        public void ЕслиПользовательСохраняетИзменения()
        {
            var result =
                Container.Resolve<IPersonalAccountChangeService>()
                    .ChangePenalty(ScenarioContext.Current.Get<BaseParams>("baseParamsSetPenalty"));

            if (!result.Success)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, result.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        //[Then(@"в карточке лс появляется запись по операции по текущему периоду")]
        //public void ТоВКарточкеЛсПоявляетсяЗаписьПоОперацииПоТекущемуПериоду()
        //{
        //    var currentPeriod =
        //        Container.Resolve<IDomainService<ChargePeriod>>().GetAll().FirstOrDefault(x => !x.IsClosed);

        //    if (currentPeriod == null)
        //    {
        //        throw new NullReferenceException("Отсутствует открытый период");
        //    }

        //    var baseParams = new BaseParams
        //                         {
        //                             Params =
        //                                 new DynamicDictionary
        //                                     {
        //                                         { "periodId", currentPeriod.Id },
        //                                         {
        //                                             "accountId",
        //                                             BasePersonalAccountHelper.Current.Id
        //                                         }
        //                                     }
        //                         };
        //    var result = Container.Resolve<IPersonalAccountDetailService>().GetPeriodDetail(baseParams).Data;

        //    ScenarioContext.Current["ListPeriodSummaryInfoResult"] = result;
        //}

        //[Then(@"у этой записи по операции по текущему периоду есть детальная информация")]
        //public void ТоУЭтойЗаписиПоОперацииПоТекущемуПериодуЕстьДетальнаяИнформация()
        //{
        //    var periodSummaryId =
        //        ScenarioContext.Current.Get<IEnumerable<PeriodDetail>>("ListPeriodSummaryInfoResult").Last().Id;

        //    var baseParams = new BaseParams
        //    {
        //        Params =
        //        {
        //            {
        //                "filter", new[]
        //                {
        //                    new DynamicDictionary
        //                    {
        //                        {"property", "periodSummaryId"},
        //                        {"value", periodSummaryId}
        //                    }
        //                }
        //            }
        //        }
        //    };

        //    var result = (IEnumerable<PeriodOperationDetail>)Container.Resolve<IPersonalAccountDetailService>().GetPeriodOperationDetail(baseParams).Data;
        //    result.Should()
        //        .NotBeNullOrEmpty("Список операций в детальной информации пуст");

        //    ScenarioContext.Current["ListOperationDetails"] = result;
        //}

        [Then(@"у этой детальной информации по текущему периоду выбираем запись с датой операции ""(.*)""")]
        public void ТоУЭтойДетальнойИнформацииПоТекущемуПериодуВыбираемЗаписьСДатойОперации(string operationDate)
        {
            DateTime parsedOperstionDate;

            if (operationDate == "текущая дата")
            {
                parsedOperstionDate = DateTime.Now.Date;
            }
            else if (!DateTime.TryParse(operationDate, out parsedOperstionDate))
            {
                throw new SpecFlowException(
                    "Не правильный формат даты в выборе детальной информации по текущему периоду в ЛС");
            }

            var operationDetailsList = ScenarioContext.Current.Get<IEnumerable<PeriodOperationDetail>>("ListOperationDetails");

            var currentOperationDetails = operationDetailsList.FirstOrDefault(x => x.Date == parsedOperstionDate);

            ScenarioContext.Current.Add("CurrentOperationDetails", currentOperationDetails);
        }

        [Then(@"у этой записи из детальной информации по текущему периоду заполнено поле Название операции ""(.*)""")]
        public void ТоУЭтойЗаписиИзДетальнойИнформацииПоТекущемуПериодуЗаполненоПолеНазваниеОперации(string operationNameExpected)
        {
            var currentOperationDetails = ScenarioContext.Current.Get<PeriodOperationDetail>("CurrentOperationDetails");

            currentOperationDetails.Name.Should()
                .Be(
                    operationNameExpected,
                    string.Format(
                        "у этой записи из детальной информации по текущему периоду поле Название операции должно быть {0}",
                        operationNameExpected));
        }

        [Then(@"у этой записи из детальной информации по текущему периоду заполнено поле Изменение сальдо ""(.*)""")]
        public void ТоУЭтойЗаписиИзДетальнойИнформацииПоТекущемуПериодуЗаполненоПолеИзменениеСальдо(decimal saldoChangeExpected)
        {
            var currentOperationDetails = ScenarioContext.Current.Get<PeriodOperationDetail>("CurrentOperationDetails");

            currentOperationDetails.SaldoChange.Should()
                .Be(
                    saldoChangeExpected,
                    string.Format(
                        "у этой записи из детальной информации по текущему периоду поле Изменение сальдо должно быть {0}",
                        saldoChangeExpected));
        }


        [Then(@"у этой детальной информации есть запись ""(.*)""")]
        public void ТоУЭтойДетальнойИнформацииЕстьЗапись(string p0)
        {
            var operationDetailsList =
                ScenarioContext.Current.Get<IEnumerable<PeriodOperationDetail>>("ListOperationDetails");
            var operationDetail = operationDetailsList.OrderByDescending(x => x.Date).FirstOrDefault(x => x.Name == p0);

            ScenarioContext.Current["operationDetail"] = operationDetail;
        }

        [Then(@"у этой записи по пени заполнено поле Дата операции ""(.*)""")]
        public void ТоУЭтойЗаписиПоПениЗаполненоПолеДатаОперации(string date)
        {
            var operationDetail = ScenarioContext.Current["operationDetail"] as PeriodOperationDetail;

            if (date == "текущая дата")
            {
                
                DateTime expectDate = DateTime.Now;

                operationDetail.Date.Date.Should().Be(expectDate.Date, "Поле Дата операции не равно " + date);
            }
            else
            {
                operationDetail.Date.Date.Should().Be(DateTime.Parse(date), "Поле Дата операции не равно " + date);
            }
        }

        [Then(@"у этой записи по пени заполнено поле Изменение сальдо ""(.*)""")]
        public void ТоУЭтойЗаписиПоПениЗаполненоПолеИзменениеСальдо(decimal saldoChange)
        {
            var operationDetail = ScenarioContext.Current["operationDetail"] as PeriodOperationDetail;
            operationDetail.SaldoChange.Should().Be(saldoChange, "Изменение сальдо не равно " + saldoChange);
        }

        //[Then(@"у поля Начислено пени есть детальная информация по начисленным пени")]
        //public void ТоУПоляНачисленоПениЕстьДетальнаяИнформацияПоНачисленнымПени()
        //{
        //    var baseParams = new BaseParams
        //    {
        //        Params =
        //        {
        //            {"accId", BasePersonalAccountHelper.Current.Id},
        //            {"fieldName", "ChargedPenalty"}
        //        }
        //    };
        //    var result = Container.Resolve<IPersonalAccountDetailService>().GetFieldDetail(baseParams).Data;
        //    ((List<FieldDetail>) result).Should().NotBeNullOrEmpty("Детальная информация по начисленным пени пуста");
        //    ScenarioContext.Current["GetFieldDetailsResult"] = result;
        //}

        [Then(@"у этой записи по текущему периоду заполнено поле Сумма ""(.*)""")]
        public void ТоУЭтойЗаписиПоТекущемуПериодуЗаполненоПолеСумма(string expectedAmount)
        {
            var openPeriod = ChargePeriodHelper.GetOpenPeriod();

            var amount = expectedAmount.DecimalParse();

            var result = ScenarioContext.Current.Get<List<FieldDetail>>("GetFieldDetailsResult");
            result.Last(x => x.Period == openPeriod.Name).Amount.Should().Be(amount, "Сумма не равна " + expectedAmount);
        }

        [Then(@"в карточке текущего лс заполнено поле Задолженность по пени всего ""(.*)""")]
        public void ТоВКарточкеТекущегоЛсЗаполненоПолеЗадолженностьПоПениВсего(decimal p0)
        {
            var baseParams = new BaseParams
            {
                Params =
                {
                    {"id", BasePersonalAccountHelper.Current.Id}
                }
            };

            var result = Container.Resolve<IViewModel<BasePersonalAccount>>().Get(Container.Resolve<IDomainService<BasePersonalAccount>>(), baseParams).Data;
            var debtPenaly = result.GetType().GetProperty("DebtPenalty").GetValue(result);

            debtPenaly.Should().Be(p0, "Поле задолженность по пени всего не совпадает с " + p0);
        }

        [Then(@"у поля Задолженность по пени есть детальная информация по задолженности пени")]
        public void ТоУПоляЗадолженностьПоПениЕстьДетальнаяИнформацияПоЗадолженностиПени()
        {
            var baseParams = new BaseParams
            {
                Params =
                {
                    {"accId", BasePersonalAccountHelper.Current.Id},
                    {"fieldName", "DebtPenalty"}
                }
            };
            var result = Container.Resolve<IPersonalAccountDetailService>().GetFieldDetail(baseParams).Data;
            ((List<FieldDetail>)result).Should().NotBeNullOrEmpty("Детальная информация по начисленным пени пуста");
            ScenarioContext.Current["GetFieldDetailsResult"] = result;
        }

        [Then(@"у этой записи по текущему периоду в детальной информации по задолженности пени заполнено поле Сумма ""(.*)""")]
        public void ТоУЭтойЗаписиПоТекущемуПериодуВДетальнойИнформацииПоЗадолженностиПениЗаполненоПолеСумма(decimal amount)
        {
            var openPeriod =
                ChargePeriodHelper.GetOpenPeriod();

            var result = ScenarioContext.Current.Get<List<FieldDetail>>("GetFieldDetailsResult");
            result.Last(x => x.Period == openPeriod.Name).Amount.Should().Be(amount, "Сумма не равна " + amount);
        }

        [Given(@"пользователь для текущего ЛС вызывает операцию Установка и изменение сальдо")]
        public void ЕслиПользовательДляТекущегоЛСВызываетОперациюУстановкаИИзменениеСальдо()
        {
            var baseParamsSetBalance = new BaseParams
            {
                Params =
                    new DynamicDictionary
                    {
                        {"AccountId", BasePersonalAccountHelper.Current.Id},
                        {"fakeParam", 1},
                        {"CurrentSaldo", 0},
                    },
                Files = new Dictionary<string, FileData> {{"Document", null}}
            };

            ScenarioContext.Current["baseParamsSetBalance"] = baseParamsSetBalance;
        }

        [Given(@"пользователь в форме установка и изменение сальдо заполняет поле Новое значение ""(.*)""")]
        public void ЕслиПользовательЗаполняетПолеНовоеЗначение(decimal p0)
        {
            var baseParamsSetBalance = ScenarioContext.Current.Get<BaseParams>("baseParamsSetBalance");
            baseParamsSetBalance.Params["NewValue"] = p0;
        }

        [Given(@"пользователь в форме установка и изменение сальдо заполняет поле Причина установки/изменения сальдо ""(.*)""")]
        public void ЕслиПользовательЗаполняетПолеПричинаУстановкиИзмененияСальдо(string p0)
        {
            var baseParamsSetBalance = ScenarioContext.Current.Get<BaseParams>("baseParamsSetBalance");
            baseParamsSetBalance.Params["Reason"] = p0;
        }

        [When(@"пользователь сохраняет изменения в форме установка и изменение сальдо")]
        public void ЕслиПользовательСохраняетИзмененияСальдо()
        {
            ExplicitSessionScope.CallInNewScope(() =>
            {
                try
                {
                    var res =
                        Container.Resolve<IPersonalAccountChangeService>()
                            .ChangePeriodBalance(ScenarioContext.Current.Get<BaseParams>("baseParamsSetBalance"));

                    if (!res.Success)
                    {
                        ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, res.Message);

                        Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);

                    Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
                }
            });
        }

        [Then(@"у поля Начислено взносов по минимальному тарифу всего есть детальная информация по начисленным взносам")]
        public void ТоУПоляНачисленоВзносовПоМинимальномуТарифуВсегоЕстьДетальнаяИнформацияПоНачисленнымВзносам()
        {
            var baseParams = new BaseParams
            {
                Params =
                {
                    {"accId", BasePersonalAccountHelper.Current.Id},
                    {"fieldName", "ChargedBaseTariff"}
                }
            };
            var result = Container.Resolve<IPersonalAccountDetailService>().GetFieldDetail(baseParams).Data;
            ((List<FieldDetail>)result).Should().NotBeNullOrEmpty("Ошибка при получении детальной информации по полю Начислено взносов по минимальному тарифу всего");
            ScenarioContext.Current["GetFieldDetailsResult"] = result;
        }

        [Then(@"у поля Задолженность по взносам всего есть детальная информация по задолженности по взносам")]
        public void ТоУПоляЗадолженностьПоВзносамВсегоЕстьДетальнаяИнформацияПоЗадолженностиПоВзносам()
        {
            var baseParams = new BaseParams
            {
                Params =
                {
                    {"accId", BasePersonalAccountHelper.Current.Id},
                    {"fieldName", "ChargedBaseTariff"}
                }
            };
            var result = Container.Resolve<IPersonalAccountDetailService>().GetFieldDetail(baseParams).Data;
            ((List<FieldDetail>)result).Should().NotBeNullOrEmpty("Ошибка при получении детальной информации по полю Начислено взносов по минимальному тарифу всего");
            ScenarioContext.Current["GetFieldDetailsResult"] = result;
        }
    }
}
