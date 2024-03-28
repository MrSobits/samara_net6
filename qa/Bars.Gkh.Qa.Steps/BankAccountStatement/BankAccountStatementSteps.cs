namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Controller.Provider;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using FluentAssertions;

    using NHibernate.Util;

    using NUnit.Framework;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    internal class BankAccountStatementSteps : BindingBase
    {
        private BaseParams BaseParams = new BaseParams
        {
            Params = new DynamicDictionary(),
            Files = new Dictionary<string, FileData>()
        };

        private Array RecordsList;

        private IDomainService<BasePersonalAccount> basePersDomainService = Container.Resolve<IDomainService<BasePersonalAccount>>();
        
        [Given(@"пользователь добавляет новую банковскую операцию")]
        public void ДопустимПользовательДобавляетНовуюБанковскуюОперацию()
        {
            BankAccountStatementHelper.Current = new BankAccountStatement();
        }

        [Given(@"добавлена банковская операция")]
        public void ДопустимДобавленаБанковскаяОперация(Table table)
        {
            foreach (var row in table.Rows)
            {
                row["MoneyDirection"] = EnumHelper.GetFromDisplayValue(typeof(MoneyDirection), row["MoneyDirection"]).ToString();

                if (row["DateReceipt"] == "текущая дата")
                {
                    row["DateReceipt"] = DateTime.Today.ToString();
                }
            }

            BankAccountStatementListHelper.Current = (List<BankAccountStatement>)table.CreateSet<BankAccountStatement>();

            foreach (var bankAccountStatement in BankAccountStatementListHelper.Current)
            {
                BankAccountStatementHelper.Current = bankAccountStatement;
                ЕслиПользовательСохраняетЭтуЭтойБанковскуюОперацию();
            }
        }

        [Given(@"пользователь выбирает банковскую операцию, у которой номер ""(.*)"", дата документа ""(.*)"", тип ""(.*)"" и статус ""(.*)""")]
        public void ДопустимПользовательВыбираетБанковскуюОперациюУКоторойНомерДатаДокументаТипИСтатус(
            string documentNum,
            string documentDate,
            string displayedMoneyDirection,
            string displayedDistributeState)
        {
            var moneyDirection = EnumHelper.GetFromDisplayValue<MoneyDirection>(displayedMoneyDirection);
            var distribiteState = EnumHelper.GetFromDisplayValue<DistributionState>(displayedDistributeState);
            var parsedDocumentDate = documentDate.DateParse();
            //Не понятно почему но без AsEnumerable() не проходит проверка x.DocumentDate.Date == parsedDocumentDate.Date с приставкой Date
            var bankAccount =
                BankAccountStatementHelper.DomainService.GetAll()
                    .FirstOrDefault(
                        x =>
                        x.DocumentNum == documentNum && x.DocumentDate.Date == parsedDocumentDate
                        && x.MoneyDirection == moneyDirection && x.DistributeState == distribiteState);

            if (bankAccount == null)
            {
                throw new SpecFlowException(
                    string.Format(
                    "отсутствует распределение по оплате, у которого номер {0}, дата документа {1}, тип {2} и статус {3}",
                    documentNum,
                    documentDate,
                    displayedMoneyDirection,
                    displayedDistributeState));
            }

            BankAccountStatementHelper.Current = bankAccount;
        }

        [Given(@"пользователь у этой банковской операции заполняет поле Дата документа ""(.*)""")]
        public void ДопустимПользовательУЭтойБанковскойОперацииЗаполняетПолеДатаДокумента(string docDate)
        {
            DateTime date;

            if (docDate == "текущая дата")
            {
                date = DateTime.Today;
            }
            else
            {
                if (!DateTime.TryParse(docDate, out date))
                {
                    throw new SpecFlowException("Не правильный формат даты в заполнении \"Дата документа\" в банковской операции");
                }
            }

            BankAccountStatementHelper.Current.DocumentDate = date;
        }

        [Given(@"пользователь у этой банковской операции заполняет поле Сумма ""(.*)""")]
        public void ДопустимПользовательУЭтойБанковскойОперацииЗаполняетПолеСумма(decimal sum)
        {
            BankAccountStatementHelper.Current.Sum = sum;
        }

        [Given(@"пользователь у этой банковской операции заполняет поле Приход/расход ""(.*)""")]
        public void ДопустимПользовательУЭтойБанковскойОперацииЗаполняетПолеПриходРасход(string moneyDirectionExternal)
        {
            var moneyDirectionType = Type.GetType("Bars.Gkh.RegOperator.Enums.MoneyDirection, Bars.Gkh.RegOperator");

            var moneyDirection = EnumHelper.GetFromDisplayValue(moneyDirectionType, moneyDirectionExternal);

            BankAccountStatementHelper.Current.MoneyDirection = moneyDirection;
        }

        [When(@"пользователь сохраняет эту банковскую операцию")]
        public void ЕслиПользовательСохраняетЭтуЭтойБанковскуюОперацию()
        {
            try
            {
                if (BankAccountStatementHelper.Current.Id > 0)
                {
                    BankAccountStatementHelper.DomainService.Update(BankAccountStatementHelper.Current);
                }
                else
                {
                    BankAccountStatementHelper.DomainService.Save(BankAccountStatementHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь у этой банковской операции для распределения Тип распределения ""(.*)""")]
        public void ЕслиПользовательУЭтойБанковскойОперацииДляРаспределенияТипРаспределения(string p0)
        {
            string code = p0;

            switch (p0)
            {
                case "Субсидия фонда":
                    code = "FundSubsidyDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"RealtyAccountId", RealityObjectHelper.CurrentRealityObject.Id},
                            {"RealityObject", RealityObjectHelper.CurrentRealityObject.Address},
                            {"Sum", 0},
                            {"Municipality", RealityObjectHelper.CurrentRealityObject.Municipality},
                            {"Index", 0}
                        }
                    };
                    break;

                case "Региональная субсидия":
                    code = "RegionalSubsidyDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"RealtyAccountId", RealityObjectHelper.CurrentRealityObject.Id},
                            {"RealityObject", RealityObjectHelper.CurrentRealityObject.Address},
                            {"Sum", 0},
                            {"Municipality", RealityObjectHelper.CurrentRealityObject.Municipality},
                            {"Index", 0}
                        }
                    };
                    break;

                case "Стимулирующая субсидия":
                    code = "StimulateSubsidyDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"RealtyAccountId", RealityObjectHelper.CurrentRealityObject.Id},
                            {"RealityObject", RealityObjectHelper.CurrentRealityObject.Address},
                            {"Sum", 0},
                            {"Municipality", RealityObjectHelper.CurrentRealityObject.Municipality},
                            {"Index", 0}
                        }
                    };
                    break;

                case "Целевая субсидия":
                    code = "TargetSubsidyDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"AccountId", BasePersonalAccountHelper.Current.Id},
                            {"Sum", BankAccountStatementHelper.Current.Sum},
                            {"SumPenality", 0}
                        }
                    };
                    break;

                case "Иные источники поступления":
                    code = "OtherSourcesDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"RealtyAccountId", RealityObjectPaymentAccountHelper.Current.Id},
                            {"RealityObject", RealityObjectHelper.CurrentRealityObject.Address},
                            {"Sum", 1},
                            {"Municipality", RealityObjectHelper.CurrentRealityObject.Municipality.Name},
                            {"Index", 0}
                        }
                    };
                    break;

                case "Поступление процентов банка":
                    code = "BankPercentDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"RealtyAccountId", RealityObjectPaymentAccountHelper.Current.Id},
                            {"RealityObject", RealityObjectHelper.CurrentRealityObject.Address},
                            {"Sum", 1},
                            {"Municipality", RealityObjectHelper.CurrentRealityObject.Municipality.Name},
                            {"Index", 0}
                        }
                    };
                    break;

                case "Платеж КР":
                    code = "TransferCrDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"AccountId", BasePersonalAccountHelper.Current.Id},
                            {"Sum", BankAccountStatementHelper.Current.Sum},
                            {"SumPenality", 0}
                        }
                    };
                    break;

                case "Ранее накопленные средства":
                    code = "AccumulatedFundsDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"AccountId", BasePersonalAccountHelper.Current.Id},
                            {"Sum", BankAccountStatementHelper.Current.Sum},
                            {"SumPenality", 0}
                        }
                    };
                    break;

                case "Средства за ранее выполненные работы":
                    code = "PreviousWorkPaymentDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"RealtyAccountId", RealityObjectPaymentAccountHelper.Current.Id},
                            {"RealityObject", RealityObjectHelper.CurrentRealityObject.Address},
                            {"Sum", 1},
                            {"Municipality", RealityObjectHelper.CurrentRealityObject.Municipality.Name},
                            {"Index", 0}
                        }
                    };
                    break;

                case "Поступление оплат аренды":
                    code = "RentPaymentDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"AccountId", BasePersonalAccountHelper.Current.Id},
                            {"Sum", BankAccountStatementHelper.Current.Sum},
                            {"SumPenality", 0}
                        }
                    };
                    break;

                case "Оплата по мировому соглашению":
                    code = "RestructAmicableAgreementDistribution";
                    RecordsList = new[]
                    {
                        new DynamicDictionary
                        {
                            {"AccountId", BasePersonalAccountHelper.Current.Id},
                            {"Sum", BankAccountStatementHelper.Current.Sum},
                            {"SumPenality", 0}
                        }
                    };
                    break;

                case "Возврат субсидии фонда":
                    code = "RefundFundSubsidyDistribution";
                    break;
                case "Возврат региональной субсидии":
                    code = "RefundRegionalSubsidyDistribution";
                    break;
                case "Возврат стимулирующей субсидии":
                    code = "RefundStimulateSubsidyDistribution";
                    break;
                case "Возврат целевой субсидии":
                    code = "RefundTargetSubsidyDistribution";
                    break;
                case "Оплата акта":
                    code = "PerformedWorkActsDistribution";
                    break;
                case "Возврат средств":
                    code = "RefundDistribution";
                    break;
                case "Возврат МСП":
                    code = "RefundMspDistribution";
                    break;
                case "Возврат пени":
                    code = "RefundPenaltyDistribution";
                    break;
                case "Комиссия за ведение счета кредитной организацией":
                    code = "ComissionForAccountServiceDistribution";
                    break;
                case "Оплата заявки":
                    code = "TransferContractorDistribution";
                    break;
                default:
                    throw new Exception("нет такого типа распределения");
            }

            BaseParams.Params["code"] = code;
        }

        [When(@"пользователь выбирает этот лицевой счет")]
        public void ЕслиПользовательВыбираетЭтотЛицевойСчет()
        {
            BaseParams.Params["distributionId"] = BankAccountStatementHelper.Current.Id;
            BaseParams.Params["distributionSource"] = "10";
            BaseParams.Params["distribSum"] = "1";
            BaseParams.Params["distributeOn"] = "";
            BaseParams.Params["Balance"] = "0";
            BaseParams.Params["DistrSum"] = "1";
            BaseParams.Params["records"] = RecordsList;
        }

        [When(@"пользователь применяет распределение")]
        public void ЕслиПользовательПрименяетРаспределение()
        {
            dynamic provider = Container.Resolve<IDistributionProvider>();
            var result = (BaseDataResult) provider.Apply(BaseParams);

            if ( !result.Success )
            {
                ExceptionHelper.AddException("IDistributionProvider.Apply", result.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [When(@"пользователь отменяет распределение по оплате, у которой номер ""(.*)"", дата документа ""(.*)"", тип ""(.*)"" и статус ""(.*)""")]
        public void ЕслиПользовательОтменяетРаспределениеПоОплатеУКоторойНомерДатаДокументаТипИСтатус(
            string documentNum,
            string documentDate,
            string displayedMoneyDirection,
            string displayedDistributeState)
        {
            DateTime parsedDocumentDate;

            if (!DateTime.TryParse(documentDate, out parsedDocumentDate))
            {
                throw new SpecFlowException(
                    "Не правильный формат даты в заполнении \"Дата документа\" в банковской операции");
            }

            var moneyDirection = EnumHelper.GetFromDisplayValue<MoneyDirection>(displayedMoneyDirection);
            var distribiteState = EnumHelper.GetFromDisplayValue<DistributionState>(displayedDistributeState);

            //Не понятно почему но без AsEnumerable() не проходит проверка x.DocumentDate.Date == parsedDocumentDate.Date с приставкой Date
            var bankAccount =
                BankAccountStatementHelper.DomainService.GetAll().AsEnumerable()
                    .FirstOrDefault(
                        x =>
                        x.DocumentNum == documentNum && x.DocumentDate.Date == parsedDocumentDate.Date
                        && x.MoneyDirection == moneyDirection && x.DistributeState == distribiteState);

            if (bankAccount == null)
            {
                throw new SpecFlowException(
                    string.Format(
                    "отсутствует распределение по оплате, у которого номер {0}, дата документа {1}, тип {2} и статус {3}",
                    documentNum,
                    documentDate,
                    displayedMoneyDirection,
                    displayedDistributeState));
            }

            BankAccountStatementHelper.Current = bankAccount;

            var baseParams = new BaseParams
            {
                Params =
                                         {
                                             { "distributionId", bankAccount.Id },
                                             { "distributionSource", ((IDistributable)bankAccount).Source }
                                         }
            };

            var result = Container.Resolve<IDistributionProvider>().Undo(baseParams);

            if (!result.Success)
            {
                ExceptionHelper.AddException("IDistributionProvider.Undo", result.Message);
            }
        }

        [When(@"пользователь производит действие зачисления по кнопке Распределить для банковской операции с типом Приход")]
        public void ЕслиПользовательПроизводитДействиеЗачисленияПоКнопкеРаспределитьДляБанковскойОперацииСТипомПриход()
        {
            BankAccountStatementHelper.Current = BankAccountStatementListHelper.Current.Where(x => x.MoneyDirection == MoneyDirection.Income).FirstOrDefault();
        }

        [When(@"пользователь производит действие зачисления по кнопке Распределить для банковской операции с типом Расход")]
        public void ЕслиПользовательПроизводитДействиеЗачисленияПоКнопкеРаспределитьДляБанковскойОперацииСТипомРасход()
        {
            BankAccountStatementHelper.Current = BankAccountStatementListHelper.Current.Where(x => x.MoneyDirection == MoneyDirection.Outcome).FirstOrDefault();
        }

        [Then(@"запись по этой банковской операции присутствует в разделе банковских операций")]
        public void ТоЗаписьПоЭтойБанковскойОперацииПрисутствуетВРазделеБанковскихОпераций()
        {
            var bankAccountStatement = BankAccountStatementHelper.DomainService.Get(BankAccountStatementHelper.Current.Id);

            bankAccountStatement.Should().NotBeNull(
                string.Format(
                "запись по этой банковской операции должна присутствовать в разделе банковских операций. {0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"в истории изменений этого лицевого счета присутствует запись")]
        public void ТоВИсторииИзмененийЭтогоЛицевогоСчетаПрисутствуетЗапись()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"id", ScenarioContext.Current["accId"]}
                }
            };

            var result = Container.Resolve<IPersonalAccountOperationLogService>().GetOperationLog(baseParams);

            ScenarioContext.Current["BasePersonalAccountOperationLog"] = result;
        }

        [Then(@"у этой записи Наименованием параметра ""(.*)""")]
        public void ТоУЭтойЗаписиНаименованиемПараметра(string parametrName)
        {
            var resultData = ((ListDataResult) ScenarioContext.Current["BasePersonalAccountOperationLog"]).Data;
            ScenarioContext.Current["BasePersonalAccountOperation"] = ((IQueryable<dynamic>)resultData).ToList().First();
            ((PersonalAccountOperationLogEntry) ScenarioContext.Current["BasePersonalAccountOperation"]).ParameterName
                .Should().Be(parametrName);
        }

        [Then(@"у этой записи Описание измененного атрибута = ""(.*)""")]
        public void ТоУЭтойЗаписиОписаниеИзмененногоАтрибута(string propertyDescription)
        {
            ((PersonalAccountOperationLogEntry) ScenarioContext.Current["BasePersonalAccountOperation"])
                .PropertyDescription.Replace("\\", string.Empty).Replace("\"", string.Empty)
                .Should().Be(propertyDescription);
        }

        [Then(@"в списке банковских операций у этой банковской операции статус = Распределена")]
        public void ТоВСпискеБанковскихОперацийУЭтойБанковскойОперацииСтатусРаспределена()
        {
            var bankAccountStatement  = Container.Resolve<IDomainService<BankAccountStatement>>().Get(BankAccountStatementHelper.Current.Id);
            if (bankAccountStatement.DistributeState == DistributionState.NotDistributed)
            {
                Assert.Fail("статус банковской операции = не распределена");
            }
        }

        //[Then(@"в карточке этого лицевого счета, в операциях за текущий период, присутствует запись ""(.*)"" с датой операции из этой банковской операции и Изменение сальдо = (.*)")]
        //public void ТоВКарточкеЭтогоЛицевогоСчетаВОперацияхЗаТекущийПериодПрисутствуетЗаписьСДатойОперацииИзЭтойБанковскойОперацииИИзменениеСальдо(string operationName, decimal saldoChange)
        //{
        //    var controllerProvider = new ControllerProvider(Container);

        //    dynamic controllerChargePeriod = controllerProvider.GetController(Container, "ChargePeriod");

        //    long periodId = 0;
        //    var period = controllerChargePeriod.List(new BaseParams());
        //    foreach (dynamic x in ((IList)((ListDataResult)period.Data).Data))
        //    {
        //        if (x.GetType().GetProperty("IsClosed").GetValue(x) == false)
        //        {
        //            periodId = x.GetType().GetProperty("Id").GetValue(x);
        //            break;
        //        }
        //    }

        //    var bP = new BaseParams { Params = new DynamicDictionary { { "accountId", BasePersonalAccountHelper.Current.Id }, { "periodId",  periodId.ToString()} } };

        //    var service = Container.Resolve<IPersonalAccountDetailService>();
        //    var result = service.GetPeriodDetail(bP);

        //    foreach (dynamic x in result.Data)
        //    {
        //        if (x.GetType().GetProperty("PeriodId").GetValue(x) == periodId)
        //        {
        //            periodId = x.GetType().GetProperty("Id").GetValue(x);
        //            break;
        //        }
        //    }

        //    var baseParamsforListOperDet = new BaseParams
        //    {
        //        Params =
        //            new DynamicDictionary
        //            {
        //                {
        //                    "filter", new[]
        //                    {
        //                        new DynamicDictionary
        //                        {
        //                            {"property", "periodSummaryId"},
        //                            {"value", periodId}
        //                        }
        //                    }
        //                }
        //            }
        //    };

        //    var listOperationDetails = (IList)service.GetPeriodOperationDetail( baseParamsforListOperDet ).Data;

        //    listOperationDetails.Should().NotBeNullOrEmpty("В данном периоде список операций  для текущего лицевого счета пуст");

        //    switch (operationName)
        //    {
        //        case "Платеж КР":
        //            operationName = "Оплата по базовому тарифу";
        //            break;

        //        case "Поступление оплат аренды":
        //            operationName = "Поступление оплаты аренды";
        //            break;
        //    }

        //    var operation = listOperationDetails.Cast<PeriodOperationDetail>()
        //        .FirstOrDefault(x => x.SaldoChange == saldoChange && x.Date == BankAccountStatementHelper.Current.DateReceipt && x.Name == operationName);

        //    operation.Should()
        //        .NotBeNull("не найдена операция, где: Название " + operationName + ", сальдо = " + saldoChange + ", дата= " +
        //                   BankAccountStatementHelper.Current.DocumentDate);
        //}

        //[Then(@"у ЛС в карточке лицевого счета по текущему периода есть сумма по столбцу Перерасчет")]
        //public void ТоУЛСВКарточкеЛицевогоСчетаПоТекущемуПериодаЕстьСуммаПоСтолбцуПерерасчет()
        //{
        //    var bP = new BaseParams
        //    {
        //        Params =
        //            new DynamicDictionary
        //            {
        //                {"accountId", BasePersonalAccountHelper.Current.Id},
        //                {"periodId", ChargePeriodHelper.Current.Id.ToString()}
        //            }
        //    };

        //    var service = Container.Resolve<IPersonalAccountDetailService>();
        //    var result = service.GetPeriodDetail(bP);

        //    foreach (dynamic x in (IQueryable)(result.Data))
        //    {
        //        if (x.GetType().GetProperty("PeriodId").GetValue(x) == (long) ChargePeriodHelper.Current.Id)
        //        {
        //            ScenarioContext.Current["periodIdListOperationDetails"] = x.GetType().GetProperty("Id").GetValue(x).ToString();

        //            if (x.GetType().GetProperty("SaldoOut").GetValue(x) == 0)
        //            {
        //                Assert.Fail("Сумма по столбцу Перерасчет в карточке ЛС не должна = 0");
        //            }
        //            break;
        //        }
        //    }
        //}

        //[Then(@"у этого текущего периода есть детальная информация")]
        //public void ТоУЭтогоТекущегоПериодаЕстьДетальнаяИнформация()
        //{
        //    var service = Container.Resolve<IPersonalAccountDetailService>();

        //    var baseParamsforListOperDet = new BaseParams
        //    {
        //        Params =
        //            new DynamicDictionary
        //            {
        //                {
        //                    "filter", new[]
        //                    {
        //                        new DynamicDictionary
        //                        {
        //                            {"property", "periodSummaryId"},
        //                            {"value", (string)ScenarioContext.Current["periodIdListOperationDetails"]}
        //                        }
        //                    }
        //                }
        //            }
        //    };

        //    var listOperationDetails = (IList)service.GetPeriodOperationDetail(baseParamsforListOperDet).Data;

        //    ScenarioContext.Current["listOperationDetails"] = listOperationDetails;
        //    listOperationDetails.Should().NotBeEmpty();
        //}

        [Then(@"по этому текущему периоду есть протокол")]
        public void ТоПоЭтомуТекущемуПериодуЕстьПротокол()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "filter", new[]
                        {
                            new DynamicDictionary
                            {
                                {"property", "accountId"},
                                {"value", BasePersonalAccountHelper.Current.Id}
                            },
                            new DynamicDictionary
                            {
                                {"property", "periodId"},
                                {
                                    "value", ChargePeriodHelper.Current.Id

                                }
                            }
                        }
                    }
                }
            };
            
            var result = (ListDataResult)Container.Resolve<IPersonalAccountSummaryService>().ListReCalcParameterTrace(baseParams);
        }

        [Then(@"у этой записи Значение = ""(.*)""")]
        public void ТоУЭтойЗаписиЗначение(string name)
        {
            ((PersonalAccountOperationLogEntry)ScenarioContext.Current["BasePersonalAccountOperation"])
                .PropertyValue.Replace("\\", string.Empty).Replace("\"", string.Empty)
                .Should().Be(name);
        }

        [Then(@"в протоколе расчета по текущему периоду у ЛС есть запись по начислению")]
        public void ТоВПротоколеРасчетаПоТекущемуПериодуУЛСЕстьЗаписьПоНачислению()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "filter", new[]
                        {
                            new DynamicDictionary
                            {
                                {"property", "accountId"},
                                {"value", BasePersonalAccountHelper.Current.Id}
                            },
                            new DynamicDictionary
                            {
                                {"property", "periodId"},
                                {
                                    "value", ChargePeriodHelper.Current.Id

                                }
                            }
                        }
                    }
                }
            };

            var result =
                (ListDataResult)
                    Container.Resolve<IPersonalAccountSummaryService>().ListChargeParameterTrace(baseParams);

            ScenarioContext.Current["ListChargeParameterTraceResult"] = ((List<Object>) result.Data).First();
        }

        [Then(@"у этой записи для ЛС заполнено поле С-по ""(.*)""")]
        public void ТоУЭтойЗаписиДляЛСЗаполненоПолеС_По(string p0)
        {
            var result = ScenarioContext.Current["ListChargeParameterTraceResult"];
            result.GetType().GetProperty("Period").GetValue(result).Should().Be(p0, "Поле период не совпадает с " + p0);
        }

        [Then(@"у этой записи для ЛС заполнено поле Тариф на КР ""(.*)""")]
        public void ТоУЭтойЗаписиДляЛСЗаполненоПолеТарифНаКР(decimal p0)
        {
            var result = ScenarioContext.Current["ListChargeParameterTraceResult"];
            result.GetType().GetProperty("Tariff").GetValue(result).Should().Be(p0, "Поле Тариф не совпадает с " + p0);
        }

        [Then(@"у этой записи для ЛС заполнено поле Доля собственности ""(.*)""")]
        public void ТоУЭтойЗаписиДляЛСЗаполненоПолеДоляСобственности(decimal p0)
        {
            var result = ScenarioContext.Current["ListChargeParameterTraceResult"];
            result.GetType().GetProperty("Share").GetValue(result).Should().Be(p0, "Поле Тариф не совпадает с " + p0);
        }

        [Then(@"у этой записи для ЛС заполнено поле Площадь помещения ""(.*)""")]
        public void ТоУЭтойЗаписиДляЛСЗаполненоПолеПлощадьПомещения(decimal p0)
        {
            var result = ScenarioContext.Current["ListChargeParameterTraceResult"];
            result.GetType().GetProperty("RoomArea").GetValue(result).Should().Be(p0, "Поле Тариф не совпадает с " + p0);
        }

        [Then(@"у этой записи для ЛС заполнено поле Количество дней ""(.*)""")]
        public void ТоУЭтойЗаписиДляЛСЗаполненоПолеКоличествоДней(decimal p0)
        {
            var result = ScenarioContext.Current["ListChargeParameterTraceResult"];
            result.GetType().GetProperty("CountDays").GetValue(result).Should().Be(p0, "Поле Тариф не совпадает с " + p0);
        }

        [Then(@"у этой записи для ЛС заполнено поле Итого ""(.*)""")]
        public void ТоУЭтойЗаписиДляЛСЗаполненоПолеИтого(decimal p0)
        {
            var result = ScenarioContext.Current["ListChargeParameterTraceResult"];
            result.GetType().GetProperty("Summary").GetValue(result).Should().Be(p0, "Поле Тариф не совпадает с " + p0);
        }

        [Then(@"у этой записи Дата начала действия значения = ""(.*)""")]
        public void ТоУЭтойЗаписиДатаНачалаДействияЗначения(string p0)
        {
            if (p0 == "текущая дата")
            {
                var operation = ScenarioContext.Current.Get<PersonalAccountOperationLogEntry>("BasePersonalAccountOperation");

                var dateTime = operation.DateActualChange;
                
                dateTime.ToDateTime().ToString("d").Should().Be(DateTime.Today.ToString("d"));
            }
        }

        [Then(@"у этой записи Дата установки значения = ""(.*)""")]
        public void ТоУЭтойЗаписиДатаУстановкиЗначения(string p0)
        {
            if (p0 == "текущая дата")
            {
                var operation = ScenarioContext.Current.Get<PersonalAccountOperationLogEntry>("BasePersonalAccountOperation");

                var dateTime = operation.DateApplied.ToLocalTime();

                dateTime.ToString("d").Should().Be(DateTime.Today.ToString("d"));
            }
        }

        [Then(@"у к этому лицевому счету привязан абонент ""(.*)""")]
        public void ТоУКЭтомуЛицевомуСчетуПривязанАбонент(string p0)
        {
            Container.Resolve<IDomainService<BasePersonalAccount>>().Get(BasePersonalAccountListHelper.Current[0].Id).AccountOwner.Name.Should().Be(p0, "к лицевому счету должен быть привязан абонент" + p0);
        }

        [Then(@"у этой банковской операции статус ""(.*)""")]
        public void ТоУЭтойБанковскойОперацииСтатус(string displayedDistributeState)
        {
            var distributionState = EnumHelper.GetFromDisplayValue<DistributionState>(displayedDistributeState);

            BankAccountStatementHelper.Current.DistributeState.Should()
                .Be(
                    distributionState,
                    string.Format("у этой банковской операции статус должен быть {0}", displayedDistributeState));
        }

        [Then(@"у этого лицевого счета присутствует детализация по Уплачено взносов по минимальному тарифу всего = (.*)")]
        public void ТоУЭтогоЛицевогоСчетаПрисутствуетДетализацияПоУплаченоВзносовПоМинимальномуТарифуВсего(int payment)
        {
            
            var vm = Container.Resolve<IViewModel<BasePersonalAccount>>();

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"id", BasePersonalAccountHelper.Current.Id}
                }
            };

            var result = vm.Get(Container.Resolve<IDomainService<BasePersonalAccount>>(), baseParams);

            result.Data.GetType().GetProperty("PaymentBaseTariff").GetValue(result.Data).ToDecimal().Should().Be(payment);
        }

        //[Then(@"у этой детализации присутствует запись оплаты")]
        //public void ТоУЭтойДетализацииПрисутствуетЗаписьОплаты()
        //{
        //    var baseParams = new BaseParams
        //    {
        //        Params = new DynamicDictionary
        //        {
        //            {"fieldName", "PaymentBaseTariff"},
        //            {"accId", BasePersonalAccountHelper.Current.Id}
        //        }
        //    };

        //    var getFieldDetailsResult = (IList)Container.Resolve<IPersonalAccountDetailService>().GetFieldDetail(baseParams).Data;
        //    getFieldDetailsResult.Should().NotBeNullOrEmpty();

        //    ScenarioContext.Current["getFieldDetailsResult"] = getFieldDetailsResult;
        //}

        [Then(@"у этой записи по оплате заполнено поле Период = открытый период")]
        public void ТоУЭтойЗаписиПоОплатеЗаполненоПолеПериодОткрытыйПериод()
        {
            var controllerProvider = new ControllerProvider(Container);

            dynamic controllerChargePeriod = controllerProvider.GetController(Container, "ChargePeriod");

            string periodName ="";

            var period = controllerChargePeriod.List(new BaseParams());
            foreach (dynamic x in ((IList)((ListDataResult)period.Data).Data))
            {
                if (x.GetType().GetProperty("IsClosed").GetValue(x) == false)
                {
                    periodName = x.GetType().GetProperty("Name").GetValue(x);
                    break;
                }
            }

            var getFieldDetailsResult = ScenarioContext.Current.Get<IList>("getFieldDetailsResult").First();

            var periodOperationName =
                getFieldDetailsResult.GetType().GetProperty("Period").GetValue(getFieldDetailsResult);
                
            periodOperationName.Should().Be(periodName, "операция проведена не в открытом периоде = ", periodName);
        }

        [Then(@"у этой записи номера заполнено поле Сумма = сумме всех записей из детализации о этому лс по Уплачено взносов по минимальному тарифу всего")]
        public void ТоУЭтойЗаписиНомераЗаполненоПолеСуммаСуммеВсехЗаписейИзДетализацииОЭтомуЛсПоУплаченоВзносовПоМинимальномуТарифуВсего()
        {
            var getFieldDetailsResult = ScenarioContext.Current.Get<IList>("getFieldDetailsResult").First();

            var amount =
                getFieldDetailsResult.GetType().GetProperty("Amount").GetValue(getFieldDetailsResult);

            amount.Should().Be(BankAccountStatementHelper.Current.Sum, "Сумма операций по текущему периоду не совпадает с суммой в банковской операции");
        }

        [Then(@"в карточке этого дома, в разделе счет оплат, в операциях за текущий период, присутствует запись ""(.*)"" с датой операции из этой банковской операции и Изменение сальдо = (.*)")]
        public void ТоВКарточкеЭтогоДомаВРазделеСчетОплатВОперацияхЗаТекущийПериодПрисутствуетЗаписьСДатойОперацииИзЭтойБанковскойОперацииИИзменениеСальдо(string p0, int p1)
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "filter", new[]
                        {
                            new DynamicDictionary
                            {
                                {"property", "paymentAccountId"},
                                {"value", RealityObjectPaymentAccountHelper.Current.Id}

                            },
                            new DynamicDictionary
                            {
                                {"property", "isCredit"},
                                {"value", "false"}

                            }
                        }
                    }
                }
            };

            var service = Container.Resolve<ITransferService>();
            var result = service.ListTransferForPaymentAccount(baseParams);

            var res = ((IQueryable<TransferDto>)result.Data).First();
            res.Amount.Should().Be(p1, "Изменения сальдо не совпадает с ожидаемым");
            res.OperationDate.Date.Should().Be(DateTime.Today.Date, "Дата операции не совпадает с сегодняшней датой");
            res.Reason.Should().Be(p0, "Название типа распределения должно быть " + p0);
        }
    }
}
