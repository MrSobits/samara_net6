namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    // Отчет "Расходование средств собственников на реализацию программы капитального ремонта"
    public class ProgramCrOwnersSpending : BasePrintForm
    {
        #region параметры

        private long[] municipalityIds;
        private long[] financialSourceIds;
        private int limitType;

        private List<long> programCrIds = new List<long>();

        private DateTime DateStart = DateTime.MinValue;

        private DateTime DateEnd = DateTime.MaxValue;
        
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramCrOwnersSpending"/> class.
        /// </summary>
        public ProgramCrOwnersSpending() : base(new ReportTemplateBinary(Properties.Resources.ProgramCrOwnersSpendingReport))
        {
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.ProgramCrOwnersSpending";
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Расходование средств собственников на реализацию программы капитального ремонта";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "Расходование средств собственников на реализацию программы капитального ремонта";
            }
        }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Отчеты Рег.Фонд";
            }
        }

        /// <summary>
        /// Gets the parameters controller.
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ProgramCrOwnersSpending";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrIds = ParseIds(baseParams.Params["programCrIds"].ToStr());
            
            var financialSourceIdsList = baseParams.Params.GetAs("finSources", string.Empty);
            financialSourceIds = !string.IsNullOrEmpty(financialSourceIdsList)
                                  ? financialSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            limitType = baseParams.Params["returned"].ToInt();
            
            this.DateStart = baseParams.Params["dateStart"].ToDateTime();
            this.DateEnd = baseParams.Params["dateEnd"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["НачалоПериода"] = this.DateStart.ToShortDateString();
            reportParams.SimpleReportParams["ОкончаниеПериода"] = this.DateEnd.ToShortDateString();

            // объекты капитального ремонта
            var objectCrQuery = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                    .Where(x => this.programCrIds.Contains(x.ProgramCr.Id))
                    .WhereIf(this.limitType == 30, x => x.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active)
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var objectCrIds = objectCrQuery.Select(x => x.Id);
            var realtyObjIds = objectCrQuery.Select(x => x.RealityObject.Id);

            var objectCrByMunicipalDict = objectCrQuery
                    .Select(x => new
                    {
                        crObjectId = x.Id,
                        roId = x.RealityObject.Id,
                        muName = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.muName)
                    .ToDictionary(x => x.Key, x => x.Select(y => new { y.roId, y.Address, y.crObjectId }));

            var realtyObjManOrgDict = this.Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
                    .Where(x => x.IncludeDate.HasValue && x.IncludeDate <= this.DateEnd)
                    .Where(x => x.TypeCondition == TypeCondition.Include)
                    .Select(x => new
                    {
                        RealObjId = x.RealityObject.Id,
                        ManOrgName = x.ContractRf.ManagingOrganization.Contragent.Name,
                        x.IncludeDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RealObjId)
                    .ToDictionary(
                        x => x.Key,
                        y =>
                        {
                            var firstOrDefault = y.OrderByDescending(z => z.IncludeDate).FirstOrDefault();
                            return firstOrDefault != null ? firstOrDefault.ManOrgName : string.Empty;
                        });

            // Словарь средств собственника (Лимит)
            var ownerLimitsDict = this.Container.Resolve<IDomainService<FinanceSourceResource>>().GetAll()
                         .WhereIf(this.limitType == 30, x => x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active)
                         .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                         .Select(x => new { crObjectId = x.ObjectCr.Id, x.OwnerResource, finSourceId = x.FinanceSource.Id })
                         .AsEnumerable()
                         .GroupBy(x => x.crObjectId)
                         .ToDictionary(x => x.Key, x => x.Select(y => new { y.OwnerResource, y.finSourceId }).ToList());

            // словарь заявок на перечисление денежных средств
            var transferRequestsDict = this.Container.Resolve<IDomainService<TransferFundsRf>>().GetAll()
                    .Where(x => this.programCrIds.Contains(x.RequestTransferRf.ProgramCr.Id))
                    .WhereIf(this.DateStart != DateTime.MinValue, x => x.RequestTransferRf.DateFrom >= this.DateStart)
                    .WhereIf(this.DateEnd != DateTime.MinValue, x => x.RequestTransferRf.DateFrom <= this.DateEnd)
                    .Where(x => realtyObjIds.Contains(x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        x.Sum,
                        x.RequestTransferRf.TypeProgramRequest,
                        x.RequestTransferRf.DateFrom
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                            x => x.Key,
                            x => x.Select(y => new TransRequest
                            {
                                Sum = y.Sum.ToDecimal(),
                                TypeProgram = y.TypeProgramRequest,
                                DateFrom = y.DateFrom.ToDateTime()
                            })
                                .ToList());

            // словарь наименований ист. фин.
            var finSourceNamesDict =
                this.Container.Resolve<IDomainService<FinanceSource>>()
                    .GetAll()
                    .WhereIf(this.financialSourceIds.Length > 0, x => this.financialSourceIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Name })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id, x => x.Name);
            
            finSourceNamesDict[0] = "Итого по программе КР";

            // словарь Типов программ в зависимости от кода 
            var typeProgramDictionary = new Dictionary<long, List<TypeProgramRequest>>();
            var otherTypes = Enum.GetValues(typeof(TypeProgramRequest))
                    .Cast<TypeProgramRequest>()
                    .Where(tp => tp != TypeProgramRequest.Primary && tp != TypeProgramRequest.Additional)
                    .ToList();

            // итоги
            var generalTotals = new Dictionary<long, Totals>();
            foreach (var finSource in finSourceNamesDict)
            {
                generalTotals[finSource.Key] = new Totals();

                if (finSource.Value == "Финансирование по 185-ФЗ")
                {
                    typeProgramDictionary[finSource.Key] = new List<TypeProgramRequest> { TypeProgramRequest.Primary };
                }
                else if (finSource.Value == "Финансирование по 185-ФЗ (по доп программам)")
                {
                    typeProgramDictionary[finSource.Key] = new List<TypeProgramRequest> { TypeProgramRequest.Additional };
                }
                else
                {
                    typeProgramDictionary[finSource.Key] = otherTypes;
                }
            }

            generalTotals[0] = new Totals();

            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("секцияРайон");

            var i = 0;

            foreach (var key in objectCrByMunicipalDict.Keys.OrderBy(x => x))
            {
                sectionMo.ДобавитьСтроку();
                sectionMo["НаименованиеМО"] = key;

                // итоги по Муниципальногму образованию
                var municipalTotals = new Dictionary<long, Totals>();
                foreach (var finSource in finSourceNamesDict)
                {
                    municipalTotals[finSource.Key] = new Totals();
                }

                municipalTotals[0] = new Totals();

                var objectsCr = objectCrByMunicipalDict[key];

                var sectionHouse = sectionMo.ДобавитьСекцию("секцияДома");
                foreach (var objectCr in objectsCr.OrderBy(x => x.Address))
                {
                    i++;

                    // итоги по дому
                    var objectCrTotals = new Totals();

                    foreach (var finSource in finSourceNamesDict)
                    {
                        if (finSource.Key == 0)
                        {
                            continue;
                        }
                        sectionHouse.ДобавитьСтроку();
                        sectionHouse["Номер"] = i;
                        sectionHouse["Адрес"] = objectCr.Address;

                        sectionHouse["УпрОрганизация"] = realtyObjManOrgDict.ContainsKey(objectCr.roId)
                                                             ? realtyObjManOrgDict[objectCr.roId]
                                                             : string.Empty;

                        sectionHouse["ИсточникФинанс"] = finSource.Value;

                        var limit = 0M;
                        var consumption = 0M;

                        if (ownerLimitsDict.ContainsKey(objectCr.crObjectId))
                        {
                            var limits = ownerLimitsDict[objectCr.crObjectId].Where(x => x.finSourceId == finSource.Key).ToList();

                            if (limits.Count > 0)
                            {
                                limit = limits.Sum(x => x.OwnerResource).ToDecimal();
                            }
                        }

                        var transferRequests = new List<TransRequest>();
                        if (transferRequestsDict.ContainsKey(objectCr.roId))
                        {
                            transferRequests =
                                transferRequestsDict[objectCr.roId].Where(
                                    x => typeProgramDictionary[finSource.Key].Contains(x.TypeProgram)).ToList();
                            if (transferRequests.Count > 0)
                            {
                                consumption = transferRequests.Sum(x => x.Sum);
                            }
                        }

                        sectionHouse["Лимит"] = limit;
                        sectionHouse["Расход"] = consumption;
                        sectionHouse["ОстатокФинансирования"] = limit - consumption;

                        municipalTotals[finSource.Key].Limit += limit;
                        municipalTotals[finSource.Key].Consumption += consumption;
                        municipalTotals[finSource.Key].Balance += limit - consumption;

                        objectCrTotals.Limit += limit;
                        objectCrTotals.Consumption += consumption;
                        objectCrTotals.Balance += limit - consumption;

                        for (int j = 1; j <= 12; j++)
                        {
                            sectionHouse[string.Format("Расход{0}", j)] = 0;
                            var sumList =
                                transferRequests.Where(x => x.DateFrom != DateTime.MinValue)
                                                .Where(x => x.DateFrom.Month == j)
                                                .Select(x => x.Sum)
                                                .ToList();

                            var monthSum = sumList.Sum(x => x);
                            if (sumList.Count == 1)
                            {
                                sectionHouse[string.Format("Расход{0}", j)] = monthSum;
                            }

                            if (sumList.Count > 1)
                            {
                                sectionHouse[string.Format("Расход{0}", j)] = string.Format(
                                    "{0}={1}", string.Join("+", sumList.Select(x => x.ToString("F"))), monthSum.ToString("F"));
                            }

                            objectCrTotals.MonthAmounts[j - 1] += monthSum;
                            municipalTotals[finSource.Key].MonthAmounts[j - 1] = monthSum;
                        }
                    }

                    // заполнение итогов по дому
                    sectionHouse.ДобавитьСтроку();
                    sectionHouse["Номер"] = i;
                    sectionHouse["Адрес"] = objectCr.Address;
                    sectionHouse["УпрОрганизация"] = realtyObjManOrgDict.ContainsKey(objectCr.roId)
                                                         ? realtyObjManOrgDict[objectCr.roId]
                                                         : string.Empty;

                    sectionHouse["ИсточникФинанс"] = "Итого по программе КР";
                    sectionHouse["Лимит"] = objectCrTotals.Limit;
                    sectionHouse["Расход"] = objectCrTotals.Consumption;
                    sectionHouse["ОстатокФинансирования"] = objectCrTotals.Balance;

                    for (int j = 1; j <= 12; j++)
                    {
                        sectionHouse[string.Format("Расход{0}", j)] = objectCrTotals.MonthAmounts[j - 1];
                    }

                    municipalTotals[0] += objectCrTotals;
                }

                // заполнение итогов по мун. образованию
                var sectionMuTotals = sectionMo.ДобавитьСекцию("секцияИтогиРайона");
                foreach (var currentkey in municipalTotals.Keys)
                {
                    sectionMuTotals.ДобавитьСтроку();
                    sectionMuTotals["НаименованиеМО"] = key;
                    sectionMuTotals["ИсточникФинанс"] = finSourceNamesDict[currentkey];
                    sectionMuTotals["Лимит"] = municipalTotals[currentkey].Limit;
                    sectionMuTotals["Расход"] = municipalTotals[currentkey].Consumption;
                    sectionMuTotals["ОстатокФинансирования"] = municipalTotals[currentkey].Balance;

                    for (int j = 1; j <= 12; j++)
                    {
                        sectionMuTotals[string.Format("Расход{0}", j)] = municipalTotals[currentkey].MonthAmounts[j - 1];
                    }

                    generalTotals[currentkey] += municipalTotals[currentkey];
                }
            }

            // заполнение итогов
            var sectionTotals = reportParams.ComplexReportParams.ДобавитьСекцию("секцияИтоги");
            foreach (var currentkey in generalTotals.Keys)
            {
                sectionTotals.ДобавитьСтроку();
                sectionTotals["ИсточникФинанс"] = finSourceNamesDict[currentkey];
                sectionTotals["Лимит"] = generalTotals[currentkey].Limit;
                sectionTotals["Расход"] = generalTotals[currentkey].Consumption;
                sectionTotals["ОстатокФинансирования"] = generalTotals[currentkey].Balance;

                for (int j = 1; j <= 12; j++)
                {
                    sectionTotals[string.Format("Расход{0}", j)] = generalTotals[currentkey].MonthAmounts[j - 1];
                }
            }
        }

        private static List<long> ParseIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                return ids.Split(',').Select(x => x.ToLong()).ToList();
            }

            return new List<long>();
        }

        private class TransRequest
        {
            public decimal Sum { get; set; }

            public DateTime DateFrom { get; set; }

            public TypeProgramRequest TypeProgram { get; set; }
        }

        private class Totals
        {
            public Totals()
            {
                this.MonthAmounts = new decimal[12];
            }

            public decimal Limit { get; set; }

            public decimal Consumption { get; set; }

            public decimal Balance { get; set; }

            public decimal[] MonthAmounts { get; set; }

            public static Totals operator +(Totals x, Totals y)
            {
                var result = new Totals
                {
                    Limit = x.Limit + y.Limit,
                    Consumption = x.Consumption + y.Consumption,
                    Balance = x.Balance + y.Balance
                };

                for (int i = 0; i < 12; i++)
                {
                    result.MonthAmounts[i] = x.MonthAmounts[i] + y.MonthAmounts[i];
                }

                return result;
            }
        }
    }
}
