namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Modules.FIAS;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Castle.Windsor;
    using Decisions.Nso.Entities;
    using Entities;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;
    using Overhaul.Entities;
    using Overhaul.Enum;
    using Utils;
    using Bars.Gkh.Utils;

    public class NotificationFormFundReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        public NotificationFormFundReport() : base(new ReportTemplateBinary(Properties.Resources.NotificationFormFundReport))
        {

        }

        public override string Name
        {
            get { return "Реестр уведомлений о выбранном способе формирования фонда КР"; }
        }

        public override string Desciption
        {
            get { return "Реестр уведомлений о выбранном способе формирования фонда КР"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.NotificationFormFundReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.NotificationFormFundReport"; }
        }

        private int _year;

        public override void SetUserParams(BaseParams baseParams)
        {
            _year = baseParams.Params.GetAs<int>("year");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var regopDomain = Container.ResolveDomain<RegOperator>();
            var notifDomain = Container.ResolveDomain<DecisionNotification>();
            var accownerDomain = Container.ResolveDomain<AccountOwnerDecision>();
            var crfundDomain = Container.ResolveDomain<CrFundFormationDecision>();
            var creditorgDecDomain = Container.ResolveDomain<CreditOrgDecision>();
            var manorgRoDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
            var ropayaccOperationDomain = Container.ResolveDomain<RealityObjectPaymentAccountOperation>();
            var paysizeMuDomain = Container.ResolveDomain<PaymentSizeMuRecord>();
            var monthlyFeeDomain = Container.ResolveDomain<MonthlyFeeAmountDecision>();
            var paymCrSpecAccNotRegopDomain = Container.ResolveDomain<PaymentCrSpecAccNotRegop>();

            using (Container.Using(
                notifDomain,
                accownerDomain,
                crfundDomain,
                regopDomain,
                creditorgDecDomain,
                manorgRoDomain,
                ropayaccOperationDomain,
                paysizeMuDomain,
                monthlyFeeDomain,
                paymCrSpecAccNotRegopDomain))
            {
                var dateStart = new DateTime(_year, 1, 1);
                var dateEnd = new DateTime(_year, 12, 31);

                var notifFilterQuery = notifDomain.GetAll()
                    .Where(x => x.Date.Year == _year);

                var regopContragent =
                    regopDomain.GetAll()
                        .Select(x => x.Contragent)
                        .FirstOrDefault(x => x.ContragentState == ContragentState.Active);

                var creditOrgDict = creditorgDecDomain.GetAll()
                    .Where(y => notifFilterQuery.Any(x => x.Protocol.Id == y.Protocol.Id))
                    .Where(x => x.Protocol.ProtocolDate >= dateStart)
                    .Where(x => x.Protocol.ProtocolDate <= dateEnd)
                    .OrderByDescending(x => x.Protocol.ProtocolDate)
                    .Select(x => new
                    {
                        ProtocolId = x.Protocol.Id,
                        x.StartDate,
                        x.Decision
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ProtocolId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new {x.StartDate, x.Decision}).ToList());

                var manorgRoDict = manorgRoDomain.GetAll()
                    .Where(y => notifFilterQuery.Any(x => x.Protocol.RealityObject.Id == y.RealityObject.Id))
                    .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                    .Where(x => x.ManOrgContract.ManagingOrganization.Contragent != null)
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.ManOrgContract.TypeContractManOrgRealObj,
                        x.ManOrgContract.ManagingOrganization.Contragent,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate
                    })
                    .Where(x => x.StartDate <= dateEnd)
                    .Where(x => !x.EndDate.HasValue || x.EndDate >= dateStart)
                    .OrderByDescending(x => x.StartDate)
                    .ThenByDescending(x => x.TypeContractManOrgRealObj)
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new {x.Contragent, x.StartDate, x.EndDate}).ToList());

                var crfundDict = crfundDomain.GetAll()
                    .Where(y => notifFilterQuery.Any(x => x.Protocol.Id == y.Protocol.Id))
                    .OrderByDescending(x => x.Protocol.ProtocolDate)
                    .Select(x => new
                    {
                        x.Protocol.Id,
                        x.StartDate,
                        x.Decision
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => new {x.Decision, x.StartDate}).ToList());

                var accountOwnerDict = accownerDomain.GetAll()
                    .Where(y => notifFilterQuery.Any(x => x.Protocol.Id == y.Protocol.Id))
                    .Select(x => new
                    {
                        x.Protocol.Id,
                        x.DecisionType
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.DecisionType).First());

                var prevYearRoSummary = ropayaccOperationDomain.GetAll()
                    .Where(x => x.Date.Year == _year - 1)
                    .Select(x => new
                    {
                        RoId = x.Account.RealityObject.Id,
                        x.OperationSum,
                        x.OperationType
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key,
                        y => y.Sum(x => x.OperationType.IsIncome()
                            ? x.OperationSum
                            : -Math.Abs(x.OperationSum)));
                var paymCrsDict = paymCrSpecAccNotRegopDomain.GetAll()
                    .Where(x => notifFilterQuery.Any(y => y.Protocol.RealityObject.Id == x.RealityObject.Id))
                    .Where(x => x.Period.StartDate.Year == _year)
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.AmountIncome,
                        x.EndYearBalance,
                        Date = x.Period.StartDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y =>
                    {
                        var totalIncome = 0m;

                        var incomeOperations = new Dictionary<int, decimal>();

                        decimal? endYearBalance = null;
                        var last = y.LastOrDefault();
                        if (last != null)
                        {
                            endYearBalance = last.EndYearBalance;
                        }

                        foreach (var payment in y)
                        {
                            totalIncome += payment.AmountIncome ?? 0;

                            if (!incomeOperations.ContainsKey(payment.Date.Month))
                            {
                                incomeOperations[payment.Date.Month] = 0m;
                            }

                            incomeOperations[payment.Date.Month] += payment.AmountIncome ?? 0;
                        }

                        return new { TotalIncome = totalIncome, Operations = incomeOperations, EndYearBalance = endYearBalance };
                    });

                var ropaysDict = ropayaccOperationDomain.GetAll()
                    .Where(y => notifFilterQuery.Any(x => x.Protocol.RealityObject.Id == y.Account.RealityObject.Id))
                    .Where(x => x.Date.Year == _year)
                    .Select(x => new
                    {
                        RoId = x.Account.RealityObject.Id,
                        OperationSum = (decimal?)x.OperationSum,
                        x.OperationType,
                        x.Date
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y =>
                    {
                        var totalIncome = 0m;
                        var totalOutcome = 0m;

                        var incomeOperations = new Dictionary<int, decimal>();

                        foreach (var payment in y)
                        {
                            if (payment.OperationType.IsIncome())
                            {
                                totalIncome += payment.OperationSum ?? 0;

                                if (!incomeOperations.ContainsKey(payment.Date.Month))
                                {
                                    incomeOperations[payment.Date.Month] = 0m;
                                }

                                incomeOperations[payment.Date.Month] += payment.OperationSum ?? 0;
                            }
                            else
                            {
                                totalOutcome += Math.Abs(payment.OperationSum ?? 0);
                            }
                        }

                        return new { TotalIncome = totalIncome, TotalOutcome = totalOutcome, Operations = incomeOperations };
                    });

                var paysizeDict = paysizeMuDomain.GetAll()
                    .Where(x => x.PaymentSizeCr.TypeIndicator == TypeIndicator.MinSizeSqMetLivinSpace)
                    .Select(x => new
                    {
                        x.Municipality.Id,
                        x.PaymentSizeCr.PaymentSize,
                        x.PaymentSizeCr.DateStartPeriod,
                        x.PaymentSizeCr.DateEndPeriod
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        y => y
                            .Select(x => new {x.PaymentSize, x.DateStartPeriod, x.DateEndPeriod})
                            .ToList());

                var monthlyFeeDict = monthlyFeeDomain.GetAll()
                    .Where(y => notifFilterQuery.Any(x => x.Protocol.Id == y.Protocol.Id))
                    .Select(x => new
                    {
                        x.Protocol.Id,
                        x.Decision
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.SelectMany(x => x.Decision).ToList());

                var data = notifFilterQuery
                    .Select(x => new
                    {
                        ProtocolId = x.Protocol.Id,
                        RoId = x.Protocol.RealityObject.Id,
                        Municipality = x.Protocol.RealityObject.Municipality.Name,
                        MunicipalityId = x.Protocol.RealityObject.Municipality.Id,
                        Settlement = x.Protocol.RealityObject.MoSettlement.Name,
                        x.Protocol.RealityObject.Address,
                        x.Protocol.RealityObject.FiasAddress,
                        x.Protocol.RealityObject.AreaLiving,
                        x.Date,
                        x.OpenDate,
                        x.CloseDate,
                        x.RegistrationDate,
                        x.AccountNum
                    });

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                var i = 0;

                foreach (var notif in data)
                {
                    section.ДобавитьСтроку();

                    section["Number1"] = ++i;
                    section["Municipality"] = notif.Municipality;
                    section["Settlement"] = notif.Settlement;
                    section["Place"] = notif.FiasAddress.With(fa => fa.PlaceName);
                    section["Street"] = notif.FiasAddress.With(fa => fa.StreetName);
                    section["House"] = GetFullHouseNumber(notif.FiasAddress);
                    section["AreaLiving"] = notif.AreaLiving;
                    section["DateNotification"] =
                        notif.Date != DateTime.MinValue
                            ? notif.Date.ToShortDateString()
                            : null;

                    var monthlyFee = 0m;

                    if (monthlyFeeDict.ContainsKey(notif.ProtocolId))
                    {
                        var currentfee = monthlyFeeDict[notif.ProtocolId]
                            .Where(x => x.From <= notif.Date)
                            .Where(x => !x.To.HasValue || x.To >= notif.Date)
                            .OrderByDescending(x => x.To ?? DateTime.MaxValue)
                            .FirstOrDefault();

                        if (currentfee != null)
                        {
                            monthlyFee = currentfee.Value;
                        }
                        else if (paysizeDict.ContainsKey(notif.MunicipalityId))
                        {
                            monthlyFee =
                                paysizeDict[notif.MunicipalityId]
                                    .Where(x => x.DateStartPeriod <= notif.Date)
                                    .Where(x => !x.DateEndPeriod.HasValue || x.DateEndPeriod >= notif.Date)
                                    .OrderByDescending(x => x.DateEndPeriod ?? DateTime.MaxValue)
                                    .FirstOrDefault()
                                    .Return(x => x.PaymentSize);
                        }
                    }
                    else if (paysizeDict.ContainsKey(notif.MunicipalityId))
                    {
                        monthlyFee =
                            paysizeDict[notif.MunicipalityId]
                                .Where(x => x.DateStartPeriod <= notif.Date)
                                .Where(x => !x.DateEndPeriod.HasValue || x.DateEndPeriod >= notif.Date)
                                .OrderByDescending(x => x.DateEndPeriod ?? DateTime.MaxValue)
                                .FirstOrDefault()
                                .Return(x => x.PaymentSize);
                    }

                    section["MonthlyFee"] = monthlyFee;

                    if (accountOwnerDict.ContainsKey(notif.ProtocolId)
                        && accountOwnerDict[notif.ProtocolId] == AccountOwnerDecisionType.Custom)
                    {
                        var manOrgContragent =
                            manorgRoDict.ContainsKey(notif.RoId)
                                ? manorgRoDict[notif.RoId]
                                    .Where(x => x.StartDate <= notif.Date)
                                    .FirstOrDefault(x => !x.EndDate.HasValue || x.EndDate >= notif.Date)
                                    .Return(x => x.Contragent)
                                : null;

                        section["Owner"] = manOrgContragent.Return(x => x.Name);
                        section["Inn"] = manOrgContragent.Return(x => x.Inn);
                    }
                    else
                    {
                        section["Owner"] = regopContragent.Return(x => x.Name);
                        section["Inn"] = regopContragent.Return(x => x.Inn);
                    }

                    if (creditOrgDict.ContainsKey(notif.ProtocolId))
                    {
                        var creditOrgDecision =
                            creditOrgDict[notif.ProtocolId]
                                .Where(x => x.StartDate <= notif.Date)
                                .OrderByDescending(x => x.StartDate)
                                .FirstOrDefault()
                                .Return(x => x.Decision);

                        section["Bank"] = creditOrgDecision.Return(x => x.Name);
                    }

                    if (crfundDict.ContainsKey(notif.ProtocolId))
                    {
                        var crFundDecision = crfundDict[notif.ProtocolId]
                            .Where(x => x.StartDate <= notif.Date)
                            .OrderByDescending(x => x.StartDate)
                            .FirstOrDefault();

                        switch (crFundDecision.Return(x => x.Decision, CrFundFormationDecisionType.Unknown))
                        {
                            case CrFundFormationDecisionType.RegOpAccount:
                                section["RegopAccount"] = notif.AccountNum;
                                break;
                            case CrFundFormationDecisionType.SpecialAccount:
                                section["SpecialAccount"] = notif.AccountNum;
                                section["SpecialAccountDateOpen"] =
                                    notif.OpenDate != DateTime.MinValue
                                        ? notif.OpenDate.ToShortDateString()
                                        : null;
                                break;
                        }
                    }

                    var planCollection = notif.AreaLiving.GetValueOrDefault() * monthlyFee;

                    if (accountOwnerDict.ContainsKey(notif.ProtocolId)
                        && accountOwnerDict[notif.ProtocolId] == AccountOwnerDecisionType.Custom)
                    {
                        if (paymCrsDict.ContainsKey(notif.RoId))
                        {
                            var currentPaymCrs = paymCrsDict[notif.RoId];

                            for (int q = 0; q <= 3; q++)
                            {
                                var qSum = 0m;

                                for (int m = 1; m <= 3; m++)
                                {
                                    //вычисляем номер месяца
                                    var currentMonthNumber = q * 3 + m;

                                    qSum += currentPaymCrs.Operations.Get(currentMonthNumber);
                                    section["Month" + currentMonthNumber] = FormatDecimal(currentPaymCrs.Operations.Get(currentMonthNumber));
                                }

                                var quarterNumber = q + 1;

                                section["Q" + quarterNumber] = FormatDecimal(qSum);
                                section["Q" + quarterNumber + "Percent"] = FormatDecimal(planCollection != 0 ? qSum * 100 / planCollection : 0);
                            }

                            foreach (var operation in currentPaymCrs.Operations)
                            {
                                section["Month" + operation.Key] = FormatDecimal(operation.Value.RegopRoundDecimal(2));
                            }

                            var saldoIn = prevYearRoSummary.Get(notif.RoId);
                            var planYearCollection = planCollection * 12;

                            section["Year"] = currentPaymCrs.EndYearBalance.HasValue
                                    ? FormatDecimal(currentPaymCrs.EndYearBalance.Value)
                                    : null;
                            section["YearPercent"] =
                                currentPaymCrs.EndYearBalance.HasValue && currentPaymCrs.EndYearBalance.Value != 0 && currentPaymCrs.TotalIncome != 0
                                    ? FormatDecimal(currentPaymCrs.EndYearBalance.Value / (saldoIn + planYearCollection))
                                    : null;
                        }
                    }
                    else
                    {
                        if (ropaysDict.ContainsKey(notif.RoId))
                        {
                            var currentRoPays = ropaysDict[notif.RoId];

                            for (int q = 0; q <= 3; q++)
                            {
                                var qSum = 0m;

                                for (int m = 1; m <= 3; m++)
                                {
                                    //вычисляем номер месяца
                                    var currentMonthNumber = q * 3 + m;

                                    qSum += currentRoPays.Operations.Get(currentMonthNumber);
                                    section["Month" + currentMonthNumber] = FormatDecimal(currentRoPays.Operations.Get(currentMonthNumber));
                                }

                                var quarterNumber = q + 1;

                                section["Q" + quarterNumber] = FormatDecimal(qSum);
                                section["Q" + quarterNumber + "Percent"] = FormatDecimal(planCollection != 0 ? qSum * 100 / planCollection : 0);
                            }

                            foreach (var operation in currentRoPays.Operations)
                            {
                                section["Month" + operation.Key] = FormatDecimal(operation.Value.RegopRoundDecimal(2));
                            }

                            var saldoOut = currentRoPays.TotalIncome - currentRoPays.TotalOutcome;
                            var saldoIn = prevYearRoSummary.Get(notif.RoId);
                            var planYearCollection = planCollection * 12;

                            section["Year"] = FormatDecimal(saldoOut);
                            section["YearPercent"] =
                                saldoIn + planYearCollection - currentRoPays.TotalOutcome != 0 && saldoOut != 0
                                    ? FormatDecimal(saldoOut * 100 / (saldoIn + planYearCollection - currentRoPays.TotalOutcome))
                                    : null;
                        }
                    }
                }
            }
        }

        private string GetFullHouseNumber(FiasAddress fiasAddress)
        {
            var fullHouseNumber = new StringBuilder();
            
            if (fiasAddress != null)
            {
                fullHouseNumber.Append(fiasAddress.House);
                if (fiasAddress.Letter.IsNotEmpty())
                {
                    fullHouseNumber.Append(", лит. ").Append(fiasAddress.Letter);
                }

                if (fiasAddress.Housing.IsNotEmpty())
                {
                    fullHouseNumber.Append(", корп. ").Append(fiasAddress.Housing);
                }

                if (fiasAddress.Building.IsNotEmpty())
                {
                    fullHouseNumber.Append(", секц. ").Append(fiasAddress.Building);
                }
            }

            return fullHouseNumber.ToString();
        }

        private string FormatDecimal(decimal value)
        {
            return value != 0m ? value.RegopRoundDecimal(2).ToString(CultureInfo.InvariantCulture) : null;
        }
    }
}