namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.GkhCr.Enums;
    using Entities;
    using Enums;
    using GkhCr.Entities;
    using Gkh.Domain;

    public class PaymentSrcFinanceDetailsViewModel : BaseViewModel<PaymentSrcFinanceDetails>
    {
        private class ProxyPayment
        {
            public ActPaymentSrcFinanceType FinanceType;
            public decimal Sum;
        }

        public virtual IDomainService<RealityObjectPaymentAccountOperation> PaymentOperationDomain { get; set; }
        public IDomainService<RealObjPaymentAccOperPerfAct> RealObjPaymentAccOperPerfActService { get; set; }
        public IDomainService<RealObjLoanPaymentAccOper> RealObjLoanPaymentAccOperService { get; set; }
        public IDomainService<RealityObjectLoanPayment> RealityObjectLoanPaymentService { get; set; }
        public IDomainService<PaymentSrcFinanceDetails> PaymentSrcFinanceDetailsService { get; set; }


        public virtual IDomainService<PaymentSrcFinanceDetails> Service { get; set; }

        public virtual IDomainService<PerformedWorkAct> PerformedWorkActDomain { get; set; }

        public override IDataResult List(IDomainService<PaymentSrcFinanceDetails> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var paymentId = loadParams.Filter.GetAsId("paymentId");
            var performedWorkActId = loadParams.Filter.GetAsId("performedWorkActId");

            var data =
                domainService.GetAll()
                    .Where(x => x.ActPayment.Id == paymentId)
                    .Select(x => new
                    {
                        x.Id,
                        ActPayment = x.ActPayment.Id,
                        x.SrcFinanceType,
                        x.Balance,
                        x.Payment
                    })
                    .AsEnumerable()
                    .GroupBy(x => (int) x.SrcFinanceType)
                    .ToDictionary(x => x.Key, y => y.Select(z => new {z.Id, z.Balance, z.Payment}).FirstOrDefault());

            var proxyList = GetProxyList(performedWorkActId, data.Any());

            var result = proxyList
                .Select(x => new
                {
                    Id = 
                        data.ContainsKey((int) x.SrcFinanceType)
                            ? data[(int) x.SrcFinanceType].Id
                            : (int) x.SrcFinanceType,
                    IsNew = !data.ContainsKey((int) x.SrcFinanceType),
                    x.SrcFinanceType,
                    ActPayment = paymentId,
                    Balance =
                        data.ContainsKey((int) x.SrcFinanceType) ? data[(int) x.SrcFinanceType].Balance : x.Balance,
                    Payment =
                        data.ContainsKey((int) x.SrcFinanceType) ? data[(int) x.SrcFinanceType].Payment : x.Payment
                })
                .Where(x => x.Balance != 0 || x.Payment != 0)
                // Вообщем просили по задаче 43028 сделать так, чтобы отображались только те записи по которым ест ьсуммы 
                .ToList();

            int totalCount = result.Count();

            return new ListDataResult(result, totalCount);
        }

        private ActPaymentSrcFinanceType GetActPaymentSrcByLoanSrc(TypeSourceLoan typeLoanSrc)
        {
            switch (typeLoanSrc)
            {
                case TypeSourceLoan.FundSubsidy:
                    return ActPaymentSrcFinanceType.FundSubsidy;
                case TypeSourceLoan.TargetSubsidy:
                    return ActPaymentSrcFinanceType.TargetSubsidy;
                case TypeSourceLoan.PaymentByDesicionTariff:
                    return ActPaymentSrcFinanceType.OwnerFundByDecisionTariff;
                case TypeSourceLoan.Penalty:
                    return ActPaymentSrcFinanceType.Penalty;
                case TypeSourceLoan.RegionalSubsidy:
                    return ActPaymentSrcFinanceType.RegionSubsidy;
                case TypeSourceLoan.StimulateSubsidy:
                    return ActPaymentSrcFinanceType.StimulSubsidy;
                default:
                    return ActPaymentSrcFinanceType.OwnerFundByMinTarrif;
            }
        }

        private IEnumerable<PaymentSrcFinanceDetailsProxy> GetProxyList(long performedWorkActId, bool balanceExist)
        {
            var result = new List<PaymentSrcFinanceDetailsProxy>();

            var roId =
                PerformedWorkActDomain.GetAll()
                    .Where(x => x.Id == performedWorkActId)
                    .Select(x => x.ObjectCr.RealityObject.Id)
                    .FirstOrDefault();

                // Получаем операции по счету оплат дома 
            var debetOperationDict = PaymentOperationDomain.GetAll()
                .Where(x => x.OperationStatus != OperationStatus.Pending)
                .Where(x => x.Account.RealityObject.Id == roId)
                .Where(x => x.OperationType != PaymentOperationType.OutcomeAccountPayment
                            && x.OperationType != PaymentOperationType.ExpenseLoan
                            && x.OperationType != PaymentOperationType.OutcomeLoan
                            && x.OperationType != PaymentOperationType.CashService
                            && x.OperationType != PaymentOperationType.OpeningAcc
                            && x.OperationType != PaymentOperationType.CreditPayment
                            && x.OperationType != PaymentOperationType.CreditPercentPayment
                            && x.OperationType != PaymentOperationType.GuaranteesObtainPayment
                            && x.OperationType != PaymentOperationType.GuaranteesForCredit
                            && x.OperationType != PaymentOperationType.UndoPayment
                            && x.OperationType != PaymentOperationType.CancelPayment)
                .GroupBy(x => x.OperationType)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.OperationSum));

            var roPaymentAccOperCreditList = PaymentOperationDomain.GetAll()
              .Where(x => x.OperationStatus != OperationStatus.Pending)
              .Where(x => x.Account.RealityObject.Id == roId)
              .Where(x => x.OperationType == PaymentOperationType.OutcomeAccountPayment
                          || x.OperationType == PaymentOperationType.ExpenseLoan
                          || x.OperationType == PaymentOperationType.OutcomeLoan
                          || x.OperationType == PaymentOperationType.CashService
                          || x.OperationType == PaymentOperationType.OpeningAcc
                          || x.OperationType == PaymentOperationType.CreditPayment
                          || x.OperationType == PaymentOperationType.CreditPercentPayment
                          || x.OperationType == PaymentOperationType.GuaranteesObtainPayment
                          || x.OperationType == PaymentOperationType.GuaranteesForCredit
                          || x.OperationType == PaymentOperationType.UndoPayment
                          || x.OperationType == PaymentOperationType.CancelPayment)
              .Select(x => new
              {
                  x.Id,
                  x.OperationType
              })
              .ToList();

            var outcomeLoanIds =
              roPaymentAccOperCreditList.Where(x => x.OperationType == PaymentOperationType.OutcomeLoan)
                  .Select(x => x.Id).ToArray();

            var expenseLoanIds =
                roPaymentAccOperCreditList.Where(x => x.OperationType == PaymentOperationType.ExpenseLoan)
                    .Select(x => x.Id).ToArray();

            var otherIds = roPaymentAccOperCreditList
                .Where(x => x.OperationType != PaymentOperationType.ExpenseLoan &&
                            x.OperationType != PaymentOperationType.OutcomeLoan)
                .Select(x => x.Id).ToArray();

            var roPayAccPerfActIds = RealObjPaymentAccOperPerfActService.GetAll()
                .Where(x => otherIds.Contains(x.PayAccOperation.Id))
                .Select(x => x.PerformedWorkActPayment.Id).ToArray();

            var outcomeLoanSums = RealObjLoanPaymentAccOperService.GetAll()
                .Where(x => outcomeLoanIds.Contains(x.PayAccOperation.Id))
                .Select(x => new ProxyPayment
                {
                    FinanceType = GetActPaymentSrcByLoanSrc(TypeSourceLoan.FundSubsidy),
                    Sum = x.RealityObjectLoan.LoanSum
                })
                .ToList();

            var expenseLoanSums = RealityObjectLoanPaymentService.GetAll()
                .Where(x => expenseLoanIds.Contains(x.OutcomeOperation.Id))
                .Select(x => new ProxyPayment
                {
                    FinanceType = GetActPaymentSrcByLoanSrc(TypeSourceLoan.FundSubsidy),
                    Sum = x.OutcomeOperation.OperationSum
                })
                .ToList();

            var paymentSums = PaymentSrcFinanceDetailsService.GetAll()
                .Where(x => roPayAccPerfActIds.Contains(x.ActPayment.Id))
                .Select(x => new ProxyPayment
                {
                    FinanceType = x.SrcFinanceType,
                    Sum = x.Payment
                })
                .ToList();

            var commonCreditSumsList = new List<ProxyPayment>();

            commonCreditSumsList.AddRange(outcomeLoanSums);
            commonCreditSumsList.AddRange(expenseLoanSums);
            commonCreditSumsList.AddRange(paymentSums);

            var creditOperationDict = commonCreditSumsList.GroupBy(x => x.FinanceType)
                .ToDictionary(x => x.Key, y => y.Sum(z => z.Sum));

            // Получаем существующие оплаты в этом акте 
            var otherPayments = Service.GetAll()
                .Where(x => x.ActPayment.PerformedWorkAct.Id == performedWorkActId)
                .Select(x => new
                {
                    x.Id,
                    ActPayment = x.ActPayment.Id,
                    x.SrcFinanceType,
                    x.Balance,
                    x.Payment
                })
                .AsEnumerable()
                .GroupBy(x => x.SrcFinanceType)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(z => new PaymentProxy {Id = z.Id, Balance = z.Balance, Payment = z.Payment}).ToList());
            
            /*
            Средства собственников. Сальдо по нему считается из счета оплат дома = Поступление - Оплата займа - Взятие займа у дома +Поступление оплаты займа
            Субсидии фонда = Поступление субсидии фонда
            Региональная субсидия = Поступление региональной субсидии
            Стимулирующая субсидия = Поступление стимулирующей субсидии
            Целевая субсидия = Поступление целевой субсидии
            Займ = Поступление займа
             */

            foreach (ActPaymentSrcFinanceType key in Enum.GetValues(typeof (ActPaymentSrcFinanceType)))
            {
                var item = new PaymentSrcFinanceDetailsProxy {SrcFinanceType = key};

                result.Add(item);

                switch (key)
                {
                    case ActPaymentSrcFinanceType.OwnerFundByMinTarrif:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeByMinTariff))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeByMinTariff];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.OwnerFundByMinTarrif))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.OwnerFundByMinTarrif];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.OwnerFundByDecisionTariff:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeByDecisionTariff))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeByDecisionTariff];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.OwnerFundByDecisionTariff))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.OwnerFundByDecisionTariff];
                        }

                        break;
                    }

                    case ActPaymentSrcFinanceType.FundSubsidy:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeFundSubsidy))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeFundSubsidy];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.FundSubsidy))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.FundSubsidy];
                        }

                        break;
                    }

                    case ActPaymentSrcFinanceType.RegionSubsidy:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeRegionalSubsidy))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeRegionalSubsidy];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.RegionSubsidy))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.RegionSubsidy];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.StimulSubsidy:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeStimulateSubsidy))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeStimulateSubsidy];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.StimulSubsidy))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.StimulSubsidy];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.Loan:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeLoan))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeLoan];
                        }

                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeLoanPayment))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeLoanPayment];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.Loan))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.Loan];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.TargetSubsidy:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeGrantInAid))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeGrantInAid];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.TargetSubsidy))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.TargetSubsidy];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.AccumulatedFunds:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.AccumulatedFunds))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.AccumulatedFunds];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.AccumulatedFunds))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.AccumulatedFunds];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.BankPercent:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.BankPercent))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.BankPercent];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.BankPercent))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.BankPercent];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.Other:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.OtherSources))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.OtherSources];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.Other))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.Other];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.Penalty:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomePenalty))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomePenalty];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.Penalty))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.Penalty];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.Rent:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.RentPaymentIn))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.RentPaymentIn];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.Rent))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.Rent];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.PreviousWorkPayment:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.PreviousWorkPayment))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.PreviousWorkPayment];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.PreviousWorkPayment))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.PreviousWorkPayment];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.SocialSupport:
                    {
                        if (debetOperationDict.ContainsKey(PaymentOperationType.IncomeSocialSupport))
                        {
                            item.Balance += debetOperationDict[PaymentOperationType.IncomeSocialSupport];
                        }

                        if (creditOperationDict.ContainsKey(ActPaymentSrcFinanceType.SocialSupport))
                        {
                            item.Balance -= creditOperationDict[ActPaymentSrcFinanceType.SocialSupport];
                        }
                        break;
                    }

                    case ActPaymentSrcFinanceType.Credit:
                    {
                        //Сальдо по источнику:
                        //Если действующее решение на доме на счете регоператора, то тянуть значение из карточки Регионального оператора (Региональный фонд/Формирование регионального фонда/Региональные операторы/карточка регионального оператора/Счета/Расчетные счета) для р/с к которому относится дом (вкладка Обслуживаемые дома).
                        //Если действующее решение на спец.счете значение тянуть из карточки дома счета оплат (с учетом всех записей грида Кредит с данным источником)

                        var realityObjId = PerformedWorkActDomain.GetAll()
                            .Where(x => x.Id == performedWorkActId)
                            .Select(x => x.ObjectCr.RealityObject.Id)
                            .FirstOrDefault();

                        if (realityObjId > 0)
                        {
                            var crFundFormationDecisionDomain =
                                Container.Resolve<IRepository<CrFundFormationDecision>>().GetAll()
                            .Where(x => x.Protocol.RealityObject.Id == realityObjId)
                            .Where(x => x.IsChecked)
                            .OrderByDescending(x => x.Protocol.ProtocolDate)
                            .Select(x => x.Decision)
                            .FirstOrDefault();

                            switch (crFundFormationDecisionDomain)
                            {
                                    case CrFundFormationDecisionType.RegOpAccount:
                                    var calcAccountRealityObjectDomain =
                                        Container.Resolve<IDomainService<CalcAccountRealityObject>>();
                                    var regopCalcAccountDomain = Container.Resolve<IDomainService<RegopCalcAccount>>();

                                    using (Container.Using(calcAccountRealityObjectDomain, regopCalcAccountDomain))
                                    {
                                        item.Balance = calcAccountRealityObjectDomain.GetAll()
                                             .Join(
                                                 regopCalcAccountDomain.GetAll(),
                                                 x => x.Account.Id,
                                                 y => y.Id,
                                                (a, b) => new {CalcAccountRealityObject = a, RegopCalcAccount = b})
                                             .Where(x => x.CalcAccountRealityObject.RealityObject.Id == realityObjId)
                                             .OrderByDescending(x => x.RegopCalcAccount.DateOpen)
                                             .Select(x => x.RegopCalcAccount.Balance)
                                             .FirstOrDefault();
                                    }

                                    break;
                                    case CrFundFormationDecisionType.SpecialAccount:
                                    var realityObjectPaymentAccountOperationDomain =
                                        Container.Resolve<IDomainService<RealityObjectPaymentAccountOperation>>();

                                    using (Container.Using(realityObjectPaymentAccountOperationDomain))
                                    {
                                        var operationSumList = realityObjectPaymentAccountOperationDomain.GetAll()
                                            .Where(x => x.Account != null && x.Account.RealityObject != null)
                                            .Where(x => x.Account.RealityObject.Id == realityObjId)
                                            .Where(x => x.OperationType == PaymentOperationType.OutcomeAccountPayment
                                                        || x.OperationType == PaymentOperationType.ExpenseLoan
                                                        || x.OperationType == PaymentOperationType.OutcomeLoan
                                                        || x.OperationType == PaymentOperationType.OpeningAcc
                                                        || x.OperationType == PaymentOperationType.CashService
                                                        || x.OperationType == PaymentOperationType.CreditPayment
                                                        || x.OperationType == PaymentOperationType.CreditPercentPayment
                                                        || x.OperationType == PaymentOperationType.UndoPayment
                                                        || x.OperationType == PaymentOperationType.CancelPayment)
                                            .Select(x => x.OperationSum)
                                            .ToList();

                                        item.Balance = operationSumList.Sum(x => x);
                                    }

                                    break;
                            }
                        }

                        break;
                    }
                }

                // Вычитаем из полученного Сальда уже оплаченные суммы
                if (otherPayments.ContainsKey(key))
                {
                    item.Balance -= otherPayments[key].Sum(x => x.Payment);
                }
            }

            return result;
        }

        private class PaymentProxy
        {
            public long Id { get; set; }
            public decimal Balance { get; set; }
            public decimal Payment { get; set; }
        }

        private class PaymentSrcFinanceDetailsProxy
        {
            /// <summary>
            /// Тип источника финансирвоания
            /// </summary>
            public virtual ActPaymentSrcFinanceType SrcFinanceType { get; set; }

            /// <summary>
            /// Сальдо
            /// </summary>
            public virtual decimal Balance { get; set; }

            /// <summary>
            /// Оплата
            /// </summary>
            public virtual decimal Payment { get; set; }

        }
    }
}