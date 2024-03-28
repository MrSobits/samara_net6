namespace Bars.Gkh.RegOperator.Report
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Utils;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    public class PersonalAccountChargeReport : BasePrintForm
    {
        public IDomainService<BasePersonalAccount> BasePersonalAccService { get; set; }
        public IDomainService<RealityObject> RealObjDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }
        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }
        public IDomainService<PeriodSummaryBalanceChange> SaldoOutChangeDomain { get; set; }

        public IRealityObjectDecisionsService RealityObjectDecisionsService { get; set; }

        private long[] mrIds;
        private long[] moIds;
        private long[] roIds;

        public PersonalAccountChargeReport()
            : base(new ReportTemplateBinary(Properties.Resources.PersonalAccChargeReport))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var roQuery =
                RealObjDomain.GetAll()
                    .WhereIf( mrIds.Any(), x => x.Municipality != null && mrIds.Contains(x.Municipality.Id))
                    .WhereIf( moIds.Any(), x => x.MoSettlement != null && moIds.Contains(x.MoSettlement.Id))
                    .WhereIf( roIds.Any(), x => roIds.Contains(x.Id));

            if (RealityObjectDecisionsService == null)
            {
                throw new ReportProviderException("Отчет не реализован для данного региона");
            }

            var decisionsDict = RealityObjectDecisionsService.GetActualDecisionForCollection<CrFundFormationDecision>(roQuery.Select(x => x), true);

            var regopDecRoIds = decisionsDict.Where(x => x.Value.Decision == CrFundFormationDecisionType.RegOpAccount)
                .Select(x => x.Key.Id)
                .ToDictionary(x => x);

            var allRos = roQuery
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    MrName = x.Municipality != null ? x.Municipality.Name : string.Empty,
                    Level = x.MoSettlement != null ? x.MoSettlement.Level : TypeMunicipality.Settlement,
                    Moname = x.MoSettlement != null ? x.MoSettlement.Name : string.Empty,
                    x.FiasAddress.PlaceName
                })
                .AsEnumerable()
                .Where(x => regopDecRoIds.ContainsKey(x.Id))
                .Distinct(x => x.Id)
                .OrderBy(x => x.MrName)
                .ThenBy(x => x.Moname)
                .ThenBy(x => x.Address)
                .ToList();

            var allPeriodSummary =
            PeriodSummaryDomain.GetAll()
                .WhereIf(mrIds.Any(), x => mrIds.Contains(x.PersonalAccount.Room.RealityObject.Municipality.Id))
                .WhereIf(moIds.Any(), x => moIds.Contains(x.PersonalAccount.Room.RealityObject.MoSettlement.Id))
                .WhereIf(roIds.Any(), x => roIds.Contains(x.PersonalAccount.Room.RealityObject.Id))
                .Select(x => new
                {
                    x.PersonalAccount.Room.RealityObject.Id,
                    x.ChargeTariff,
                    x.RecalcByBaseTariff,
                    BalanceChange = x.BaseTariffChange + x.DecisionTariffChange + x.PenaltyChange,
                    x.RecalcByDecisionTariff,
                    x.ChargedByBaseTariff,
                    x.TariffDecisionPayment,
                })
                .AsEnumerable()
                .Where(x => regopDecRoIds.ContainsKey(x.Id))
                .GroupBy(x => x.Id)
                .ToDictionary(z => z.Key, x =>
                    {
                        var chargeTariff = x.Sum(y => (decimal?)y.ChargeTariff) ?? 0;
                        var chargedByBaseTariff = x.Sum(y => (decimal?)y.ChargedByBaseTariff) ?? 0;
                        var recalcBaseTariff = x.SafeSum(y => y.RecalcByBaseTariff);
                        var recalcDecTariff = x.SafeSum(y => y.RecalcByDecisionTariff);
                        
                        return new
                        {
                            chargedTariff = chargedByBaseTariff + recalcBaseTariff,
                            chargedOwnerDecision = chargeTariff - chargedByBaseTariff + recalcDecTariff
                        };
                });

            var accountChargeInfo = PersonalAccountChargeDomain.GetAll()
                .Where(x => x.IsActive)
                .WhereIf(mrIds.Any(), x => mrIds.Contains(x.BasePersonalAccount.Room.RealityObject.Municipality.Id))
                .WhereIf(moIds.Any(), x => moIds.Contains(x.BasePersonalAccount.Room.RealityObject.MoSettlement.Id))
                .WhereIf(roIds.Any(), x => roIds.Contains(x.BasePersonalAccount.Room.RealityObject.Id))
                .Select(x => new
                {
                    x.BasePersonalAccount.Room.RealityObject.Id,
                    x.Penalty
                })
                .AsEnumerable()
                .Where(x => regopDecRoIds.ContainsKey(x.Id))
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => new
                {
                    Penalty = y.SafeSum(z => z.Penalty)
                });

            var saldoOutChange = SaldoOutChangeDomain.GetAll()
                .WhereIf(mrIds.Any(), x => mrIds.Contains(x.PeriodSummary.PersonalAccount.Room.RealityObject.Municipality.Id))
                .WhereIf(moIds.Any(), x => moIds.Contains(x.PeriodSummary.PersonalAccount.Room.RealityObject.MoSettlement.Id))
                .WhereIf(roIds.Any(), x => roIds.Contains(x.PeriodSummary.PersonalAccount.Room.RealityObject.Id))
                .Select(x => new
                {
                    x.PeriodSummary.PersonalAccount.Room.RealityObject.Id,
                    Diff = (decimal?)(x.NewValue - x.CurrentValue)
                })
                .AsEnumerable()
                .Where(x => regopDecRoIds.ContainsKey(x.Id))
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => new
                {
                    Sum = y.SafeSum(z => z.Diff.ToDecimal())
                });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRo");

            foreach (var ro in allRos)
            {
                section.ДобавитьСтроку();

                section["НазваниеМр"] = ro.MrName;
                section["НазваниеМо"] = ro.Moname;
                section["Нас.пункт"] = ro.PlaceName;
                section["Адрес"] = ro.Address;

                var chargedTariff = allPeriodSummary.ContainsKey(ro.Id) ? allPeriodSummary[ro.Id].chargedTariff : 0m;
                var chargedPenaltyTotal = accountChargeInfo.ContainsKey(ro.Id) ? accountChargeInfo[ro.Id].Penalty : 0m;
                var chargedOwnerDecision = allPeriodSummary.ContainsKey(ro.Id) ? allPeriodSummary[ro.Id].chargedOwnerDecision : 0m;


                section["МинимальныйРазмерВзноса"] = chargedTariff != 0m ? chargedTariff.ToString() : string.Empty;
                section["СверхМинимальногоРазмераВзноса"] = chargedOwnerDecision != 0m ? chargedOwnerDecision.ToString() : string.Empty;
                section["Пени"] = chargedPenaltyTotal != 0m ? chargedPenaltyTotal.ToString() : string.Empty;
                section["ПроцентНачисленныйБанком"] = saldoOutChange.ContainsKey(ro.Id) ? saldoOutChange[ro.Id].Sum.ToString() : string.Empty;
                section["ИтогоНачислено"] = chargedTariff + chargedPenaltyTotal + chargedOwnerDecision;
            }


            var allChargedTariff = allPeriodSummary.Sum(x => x.Value.chargedTariff);
            var allChargedPenaltyTotal = accountChargeInfo.Sum(x => x.Value.Penalty);
            var allChargedOwnerDecision = allPeriodSummary.Sum(x => x.Value.chargedOwnerDecision);


            reportParams.SimpleReportParams["ИтогоМинимальныйРазмерВзноса"] = allChargedTariff;
            reportParams.SimpleReportParams["ИтогоСверхМинимальногоРазмераВзноса"] = allChargedOwnerDecision;
            reportParams.SimpleReportParams["ИтогоПени"] = allChargedPenaltyTotal;
            reportParams.SimpleReportParams["ИтогоПроцентНачисленныйБанком"] = saldoOutChange.Sum(x => x.Value.Sum);
            reportParams.SimpleReportParams["ВсегоНачислено"] = allChargedTariff + allChargedOwnerDecision +
                                                                allChargedPenaltyTotal;


        }

        public override void SetUserParams(BaseParams baseParams)
        {
            mrIds = baseParams.Params.GetAs("mrIds", string.Empty).ToLongArray();

            moIds = baseParams.Params.GetAs("moIds", string.Empty).ToLongArray();

            roIds = baseParams.Params.GetAs("roIds", string.Empty).ToLongArray();
        }

        public override string Name
        {
            get { return "Отчет о начислениях"; }
        }

        public override string Desciption
        {
            get { return "Отчет о начислениях"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.PersonalAccountChargeReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.PersonalAccountChargeReport"; }
        }
    }
}
