namespace Bars.Gkh.RegOperator.Report
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Properties;
    using Bars.Gkh.Utils;

    public class CollectionPercentReport : BasePrintForm
    {
        private readonly IDomainService<RealityObject> _roDomain;
        private readonly IDomainService<ChargePeriod> _periodDomain;
        private readonly IDomainService<BasePersonalAccount> _persAccDomain;
        private readonly IDomainService<Transfer> _transferDomain;
        private readonly IDomainService<PersonalAccountPeriodSummary> _persAccPeriodSumDomain;

        private long mrId;
        private long moId;
        private string locality;
        private long[] roIds;
        private long[] periodIds;

        public CollectionPercentReport(
            IDomainService<RealityObject> roDomain,
            IDomainService<ChargePeriod> periodDomain,
            IDomainService<BasePersonalAccount> persAccDomain,
            IDomainService<Transfer> transferDomain,
            IDomainService<PersonalAccountPeriodSummary> persAccPeriodSumDomain)
            : base(new ReportTemplateBinary(Resources.CollectionPercentReport))
        {
            this._roDomain = roDomain;
            this._periodDomain = periodDomain;
            this._persAccDomain = persAccDomain;
            this._transferDomain = transferDomain;
            this._persAccPeriodSumDomain = persAccPeriodSumDomain;
        }

        public override string Desciption
        {
            get { return "Отчет определения процента собираемости"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CollectionPercentReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.CollectionPercentReport"; }
        }

        public override string Name
        {
            get { return "Отчет определения процента собираемости"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.mrId = baseParams.Params["municipalityParent"].ToLong();
            this.moId = baseParams.Params["municipality"].ToLong();
            this.locality = baseParams.Params["locality"].ToStr();
            var roIdsList = baseParams.Params.GetAs("roIds", string.Empty);
            this.roIds = !string.IsNullOrEmpty(roIdsList)
                ? roIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];
            var periodIdsList = baseParams.Params.GetAs("periodIds", string.Empty);
            this.periodIds = !string.IsNullOrEmpty(periodIdsList)
                ? periodIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var periodQuery = this._periodDomain.GetAll()
                .WhereIf(this.periodIds.Any(), x => this.periodIds.Contains(x.Id));

            reportParams.SimpleReportParams["Период"] = periodQuery.Select(x => x.Name).AggregateWithSeparator(", ");

            var roQuery = this._roDomain.GetAll()
                .WhereIf(this.mrId != 0, x => x.Municipality.Id == this.mrId)
                .WhereIf(this.moId != 0, x => x.MoSettlement.Id == this.moId)
                .WhereIf(this.locality != string.Empty, x => x.FiasAddress.PlaceName == this.locality)
                .WhereIf(this.roIds.Any(), x => this.roIds.Contains(x.Id));

            // получаем общую инф-цию по выбранным домам
            var realObjs = roQuery
                .OrderBy(x => x.Municipality.Name)
                .ThenBy(x => x.MoSettlement.Name)
                .ThenBy(x => x.FiasAddress.PlaceName)
                .ThenBy(x => x.Address)
                .Select(
                    x => new
                    {
                        x.Id,
                        MuName = x.Municipality.Name,
                        SettlName = x.MoSettlement.Name,
                        x.Address,
                        x.FiasAddress.PlaceName,
                        x.FiasAddress.StreetName,
                        x.FiasAddress.House,
                        x.FiasAddress.Housing
                    })
                .ToList();

            var roBaseTariffWalletGuidDict = this._persAccDomain.GetAll()
                .Where(x => roQuery.Any(y => x.Room.RealityObject.Id == y.Id))
                .Select(
                    x => new
                    {
                        x.Room.RealityObject.Id,
                        x.BaseTariffWallet.WalletGuid
                    })
                .ToList()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.WalletGuid));

            var roDecisTariffWalletGuidDict = this._persAccDomain.GetAll()
                .Where(x => roQuery.Any(y => x.Room.RealityObject.Id == y.Id))
                .Select(
                    x => new
                    {
                        x.Room.RealityObject.Id,
                        x.DecisionTariffWallet.WalletGuid
                    })
                .ToList()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.WalletGuid));

            var roPenaltyWalletGuidDict = this._persAccDomain.GetAll()
                .Where(x => roQuery.Any(y => x.Room.RealityObject.Id == y.Id))
                .Select(
                    x => new
                    {
                        x.Room.RealityObject.Id,
                        x.PenaltyWallet.WalletGuid
                    })
                .ToList()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.WalletGuid));

            var paymentDebtTransferDict = this._transferDomain.GetAll()
                .Where(x => periodQuery.Any(y => y.StartDate <= x.PaymentDate && (!y.EndDate.HasValue || y.EndDate >= x.PaymentDate)))
                .Where(x => x.Operation.CanceledOperation == null)
                .Select(
                    x => new
                    {
                        x.TargetGuid,
                        Amount = x.Amount.RegopRoundDecimal(2)
                    })
                .ToList()
                .GroupBy(x => x.TargetGuid)
                .ToDictionary(x => x.Key, x => x.SafeSum(o => o.Amount));

            var paymentCreditTransferDict = this._transferDomain.GetAll()
                .Where(x => periodQuery.Any(y => y.StartDate <= x.PaymentDate && (!y.EndDate.HasValue || y.EndDate >= x.PaymentDate)))
                .Where(x => x.Operation.CanceledOperation != null)
                .Select(
                    x => new
                    {
                        x.SourceGuid,
                        Amount = x.Amount.RegopRoundDecimal(2)
                    })
                .ToList()
                .GroupBy(x => x.SourceGuid)
                .ToDictionary(x => x.Key, x => x.SafeSum(o => o.Amount));

            var persAccPeriodSummDict = this._persAccPeriodSumDomain.GetAll()
                .Where(x => roQuery.Any(y => x.PersonalAccount.Room.RealityObject.Id == y.Id))
                .Where(x => periodQuery.Any(y => y.Id == x.Period.Id))
                .Select(
                    x => new
                    {
                        RoId = x.PersonalAccount.Room.RealityObject.Id,
                        ChargedByBaseTariff = (x.ChargedByBaseTariff + x.RecalcByBaseTariff).RegopRoundDecimal(2),
                        ChargeByDecision = (x.ChargeTariff - x.ChargedByBaseTariff + x.RecalcByDecisionTariff).RegopRoundDecimal(2),
                        Penalty = (x.Penalty + x.RecalcByPenalty).RegopRoundDecimal(2),
                        ChargeTotal = (x.ChargeTariff + x.RecalcByBaseTariff + x.Penalty + x.BaseTariffChange).RegopRoundDecimal(2) // TODO fix recalc
                    })
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        BaseTariff = x.SafeSum(y => y.ChargedByBaseTariff),
                        DecisTariff = x.SafeSum(y => y.ChargeByDecision),
                        Penalty = x.SafeSum(y => y.Penalty),
                        Total = x.SafeSum(y => y.ChargeTotal)
                    });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");
            var index = 0;

            foreach (var realObj in realObjs)
            {
                section.ДобавитьСтроку();
                section["Номер"] = ++index;
                section["МуницРайон"] = realObj.MuName;
                section["МуницОбр"] = realObj.SettlName;

                section["НасПункт"] = realObj.PlaceName;

                var housing = !realObj.Housing.IsEmpty() ? ", к. {0}".FormatUsing(realObj.Housing) : string.Empty;
                section["Адрес"] = "{0}, д. {1}{2}".FormatUsing(realObj.StreetName, realObj.House, housing);

                var paymentDebtBaseTariffSum = 0M;
                var paymentCreditBaseTariffSum = 0M;
                var paymentBaseTariffSum = 0M;

                var paymentDebtDecisTariffSum = 0M;
                var paymentCreditDecisTariffSum = 0M;
                var paymentDecisTariffSum = 0M;

                var paymentDebtPenaltySum = 0M;
                var paymentCreditPenaltySum = 0M;
                var paymentPenaltySum = 0M;

                if (roBaseTariffWalletGuidDict.ContainsKey(realObj.Id))
                {
                    foreach (var baseTariffGuid in roBaseTariffWalletGuidDict[realObj.Id])
                    {
                        paymentDebtBaseTariffSum += paymentDebtTransferDict.ContainsKey(baseTariffGuid)
                            ? paymentDebtTransferDict[baseTariffGuid]
                            : 0M;
                        paymentCreditBaseTariffSum += paymentCreditTransferDict.ContainsKey(baseTariffGuid)
                            ? paymentCreditTransferDict[baseTariffGuid]
                            : 0M;
                    }
                    paymentBaseTariffSum = paymentDebtBaseTariffSum - paymentCreditBaseTariffSum;
                }

                if (roDecisTariffWalletGuidDict.ContainsKey(realObj.Id))
                {
                    foreach (var decisTariffGuid in roDecisTariffWalletGuidDict[realObj.Id])
                    {
                        paymentDebtDecisTariffSum += paymentDebtTransferDict.ContainsKey(decisTariffGuid)
                            ? paymentDebtTransferDict[decisTariffGuid]
                            : 0M;
                        paymentCreditDecisTariffSum += paymentCreditTransferDict.ContainsKey(decisTariffGuid)
                            ? paymentCreditTransferDict[decisTariffGuid]
                            : 0M;
                    }
                    paymentDecisTariffSum = paymentDebtDecisTariffSum - paymentCreditDecisTariffSum;
                }

                if (roPenaltyWalletGuidDict.ContainsKey(realObj.Id))
                {
                    foreach (var penaltyGuid in roPenaltyWalletGuidDict[realObj.Id])
                    {
                        paymentDebtPenaltySum += paymentDebtTransferDict.ContainsKey(penaltyGuid)
                            ? paymentDebtTransferDict[penaltyGuid]
                            : 0M;
                        paymentCreditPenaltySum += paymentCreditTransferDict.ContainsKey(penaltyGuid)
                            ? paymentCreditTransferDict[penaltyGuid]
                            : 0M;
                    }
                    paymentPenaltySum = paymentDebtPenaltySum - paymentCreditPenaltySum;
                }

                var chargedBaseTariffSum = persAccPeriodSummDict.ContainsKey(realObj.Id)
                    ? persAccPeriodSummDict[realObj.Id].BaseTariff
                    : 0M;
                var chargedDecisTariffSum = persAccPeriodSummDict.ContainsKey(realObj.Id)
                    ? persAccPeriodSummDict[realObj.Id].DecisTariff
                    : 0M;
                var chargedPenaltySum = persAccPeriodSummDict.ContainsKey(realObj.Id)
                    ? persAccPeriodSummDict[realObj.Id].Penalty
                    : 0M;
                var chargedTotal = persAccPeriodSummDict.ContainsKey(realObj.Id)
                    ? persAccPeriodSummDict[realObj.Id].Total
                    : 0M;

                section["НачисленоМин"] = chargedBaseTariffSum;
                section["НачисленоСверх"] = chargedDecisTariffSum;
                section["НачисленоПени"] = chargedPenaltySum;
                section["НачисленоИтого"] = chargedTotal;

                var paymentTotal = paymentBaseTariffSum + paymentDecisTariffSum + paymentPenaltySum;
                section["ОплаченоМин"] = paymentBaseTariffSum;
                section["ОплаченоСверх"] = paymentDecisTariffSum;
                section["ОплаченоПени"] = paymentPenaltySum;
                section["ОплаченоИтого"] = paymentTotal;

                var debtBaseTariffSum = chargedBaseTariffSum - paymentBaseTariffSum;
                var debtDecisTariffSum = chargedDecisTariffSum - paymentDecisTariffSum;
                var debtPenaltySum = chargedPenaltySum - paymentPenaltySum;
                section["ЗадолжМин"] = debtBaseTariffSum;
                section["ЗадолжСверх"] = debtDecisTariffSum;
                section["ЗадолжПени"] = debtPenaltySum;
                section["ЗадолжИтого"] = debtBaseTariffSum + debtDecisTariffSum + debtPenaltySum;

                section["ПроцентМин"] = chargedBaseTariffSum > 0 && paymentBaseTariffSum > 0
                    ? decimal.Round((paymentBaseTariffSum / chargedBaseTariffSum) * 100, 2)
                    : 0;
                section["ПроцентСверх"] = chargedDecisTariffSum > 0 && paymentDecisTariffSum > 0
                    ? decimal.Round((paymentDecisTariffSum / chargedDecisTariffSum) * 100, 2)
                    : 0;
                section["ПроцентПени"] = chargedPenaltySum > 0 && paymentPenaltySum > 0
                    ? decimal.Round((paymentPenaltySum / chargedPenaltySum) * 100, 2)
                    : 0;
                section["ПроцентИтого"] = chargedTotal > 0 && paymentTotal > 0
                    ? decimal.Round((paymentTotal / chargedTotal) * 100, 2)
                    : 0;
            }
        }
    }
}