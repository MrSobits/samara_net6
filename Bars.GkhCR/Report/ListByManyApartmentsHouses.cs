namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;

    /// <summary>
    /// Перечень многоквартирных домов
    /// </summary>
    public class ListByManyApartmentsHouses : BasePrintForm
    {
        #region Свойства

        protected long ProgramCrId;
        protected long[] MunicipalityIds;
        protected long[] FinanceIds;
        protected DateTime ReportDate;
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Municipality"/>
        /// </summary>
        public IDomainService<Municipality> MunicipalityDomainService { get; set; }

        public ListByManyApartmentsHouses()
            : base(new ReportTemplateBinary(Properties.Resources.ListByManyApartmentsHouses))
        {
        }

        public override string RequiredPermission => "Reports.CR.ListByManyApartmentsHouses";

        public override string Name => "Перечень многоквартирных домов";

        public override string Desciption => "Перечень многоквартирных домов";

        public override string GroupName => "Формы для фонда";

        public override string ParamsController => "B4.controller.report.ListByManyApartmentsHouses";

        protected sealed class DataProxy
        {
            public string Address { get; set; }

            public DateTime DateEnterExp { get; set; }

            public DateTime DateLastOverhaul { get; set; }

            public string WallMat { get; set; }

            public int CountFloor { get; set; }

            public int? CountEnt { get; set; }

            public decimal AreaMkd { get; set; }

            public decimal AreaLivNotLivMkd { get; set; }

            public decimal AreaLivOwned { get; set; }

            public int CountPerson { get; set; }

            public decimal Sum { get; set; }

            public string WorkNames { get; set; }

            public DateTime WorksDateEnd { get; set; }
        }

        protected sealed class FinanceSourceDataProxy
        {
            public decimal BudgetMu { get; set; }
            public decimal BudgetSubject { get; set; }
            public decimal FundResource { get; set; }
            public decimal OwnerResource { get; set; }
        }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {

            this.ProgramCrId = baseParams.Params["programCrId"].ToLong();

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.MunicipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];

            if (this.MunicipalityIds.Length == 0)
            {
                this.MunicipalityIds = this.MunicipalityDomainService.GetAll()
                    .Where(x => x.Level != TypeMunicipality.UrbanSettlement)
                    .Select(x => x.Id)
                    .ToArray();
            }

            var financeIdsList = baseParams.Params.GetAs("financeIds", string.Empty);
            this.FinanceIds = !string.IsNullOrEmpty(financeIdsList) ? financeIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
            this.ReportDate = baseParams.Params["reportDate"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        protected virtual decimal GetPartialCost(DataProxy data)
        {
            return (data.AreaMkd != 0M ? data.Sum / data.AreaMkd : data.Sum).RoundDecimal(2);
        }

        protected virtual decimal GetPartialCostTotal(List<DataProxy> data)
        {
            var area = data.SafeSum(x => x.AreaMkd);
            var sum = data.SafeSum(x => x.Sum);

            return (area != 0M ? sum / area : sum).RoundDecimal(2);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            #region Запросы
            var serviceTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceFinanceSourceResource = this.Container.Resolve<IDomainService<FinanceSourceResource>>();

            // данные для полей 2 - 13, 20
            var typeWorkDataByObjectCrDict = this.GetTypeWorkDataByObjectCrDict(serviceTypeWork);

            // данные для полей 14 - 17
            var finSrcResourceByObjectCrDict = this.GetFinSrcResourceByObjectCrDict(serviceFinanceSourceResource);

            var municipalityDict = this.MunicipalityDomainService.GetAll()
                .WhereIf(this.MunicipalityIds.Length > 0, x => this.MunicipalityIds.Contains(x.Id))
                .Select(x => new { muId = x.Id, muName = x.Name })
                .ToDictionary(x => x.muId, x => x.muName);

            #endregion

            #region Вывод Данных

            var munSection = reportParams.ComplexReportParams.ДобавитьСекцию("municipality");
            var section = munSection.ДобавитьСекцию("section");
            var num = 0;
            var allAreaMkd = 0M;
            var allAreaLivNotLivMkd = 0M;
            var allAreaLivOwned = 0M;
            var allCountPerson = 0;
            var allSum = 0M;
            var allFundResource = 0M;
            var allBudgetSubject = 0M;
            var allBudgetMu = 0M;
            var allOwnerResource = 0M;
            var allLimCost = 0M;
            var allWorksDateEnd = DateTime.MinValue;
            foreach (var munId in typeWorkDataByObjectCrDict)
            {
                munSection.ДобавитьСтроку();
                munSection["МунОбр"] = municipalityDict[munId.Key];
                var totAreaMkd = 0M;
                var totAreaLivNotLivMkd = 0M;
                var totAreaLivOwned = 0M;
                var totCountPerson = 0;
                var totSum = 0M;
                var totFundResource = 0M;
                var totBudgetSubject = 0M;
                var totBudgetMu = 0M;
                var totOwnerResource = 0M;
                var totLimCost = 0M;
                var totWorksDateEnd = DateTime.MinValue;
                foreach (var objCr in munId.Value)
                {
                    section.ДобавитьСтроку();
                    ++num;
                    section["номер"] = num;
                    section["адресМкд"] = objCr.Value.Address;
                    if (objCr.Value.DateEnterExp != DateTime.MinValue)
                    {
                        section["датаВвода"] = objCr.Value.DateEnterExp.Date.Year;
                    }

                    if (objCr.Value.DateLastOverhaul != DateTime.MinValue)
                    {
                        section["датаКонца"] = objCr.Value.DateLastOverhaul.Date.Year;
                    }

                    section["материалСтен"] = objCr.Value.WallMat;
                    if (objCr.Value.CountFloor > 0)
                    {
                        section["колЭтажей"] = objCr.Value.CountFloor;
                    }

                    if (objCr.Value.CountEnt > 0)
                    {
                        section["колПодъездов"] = objCr.Value.CountEnt;
                    }

                    if (objCr.Value.AreaMkd > 0M)
                    {
                        section["ОбщПлощВсего"] = objCr.Value.AreaMkd;
                        totAreaMkd += objCr.Value.AreaMkd;
                    }

                    if (objCr.Value.AreaLivNotLivMkd > 0M)
                    {
                        section["ОбщПлощПомещВсего"] = objCr.Value.AreaLivNotLivMkd;
                        totAreaLivNotLivMkd += objCr.Value.AreaLivNotLivMkd;
                    }

                    if (objCr.Value.AreaLivOwned > 0M)
                    {
                        section["ОбщПлощЖил"] = objCr.Value.AreaLivOwned;
                        totAreaLivOwned += objCr.Value.AreaLivOwned;
                    }

                    if (objCr.Value.CountPerson > 0M)
                    {
                        section["КолЖител"] = objCr.Value.CountPerson;
                        totCountPerson += objCr.Value.CountPerson;
                    }

                    section["ВидРемонта"] = objCr.Value.WorkNames;
                    section["ИтогоСтоим"] = objCr.Value.Sum;
                    totSum += objCr.Value.Sum;

                    var dFinSour = finSrcResourceByObjectCrDict;
                    if (dFinSour.ContainsKey(munId.Key) && dFinSour[munId.Key].ContainsKey(objCr.Key))
                    {
                        section["СтоимФонд"] = dFinSour[munId.Key][objCr.Key].FundResource;
                        totFundResource += dFinSour[munId.Key][objCr.Key].FundResource;

                        section["СтоимФедБюдж"] = dFinSour[munId.Key][objCr.Key].BudgetSubject;
                        totBudgetSubject += dFinSour[munId.Key][objCr.Key].BudgetSubject;

                        section["СтоимМестБюдж"] = dFinSour[munId.Key][objCr.Key].BudgetMu;
                        totBudgetMu += dFinSour[munId.Key][objCr.Key].BudgetMu;

                        section["СтоимТСЖ"] = dFinSour[munId.Key][objCr.Key].OwnerResource;
                        totOwnerResource += dFinSour[munId.Key][objCr.Key].OwnerResource;
                    }

                    section["УделСтоим"] = this.GetPartialCost(objCr.Value);
                    section["ПредСтоим"] = 9000;
                    totLimCost += 9000;

                    section["ПланДатаЗаверш"] = objCr.Value.WorksDateEnd;
                    if (totWorksDateEnd < objCr.Value.WorksDateEnd)
                    {
                        totWorksDateEnd = objCr.Value.WorksDateEnd;
                    }
                }

                munSection["ИтОбщПлщВс"] = totAreaMkd;
                allAreaMkd += totAreaMkd;

                munSection["ИтОбщПлщПомщВс"] = totAreaLivNotLivMkd;
                allAreaLivNotLivMkd += totAreaLivNotLivMkd;

                munSection["ИтОбщПлщЖил"] = totAreaLivOwned;
                allAreaLivOwned += totAreaLivOwned;

                munSection["ИтКолЖит"] = totCountPerson;
                allCountPerson += totCountPerson;

                munSection["ПолнИтСтм"] = totSum;
                allSum += totSum;

                munSection["ИтСтмФонд"] = totFundResource;
                allFundResource += totFundResource;

                munSection["ИтСтмФедБдж"] = totBudgetSubject;
                allBudgetSubject += totBudgetSubject;

                munSection["ИтСтмМстБдж"] = totBudgetMu;
                allBudgetMu += totBudgetMu;

                munSection["ИтСтмТСЖ"] = totOwnerResource;
                allOwnerResource += totOwnerResource;

                munSection["ИтУдСтм"] = this.GetPartialCostTotal(munId.Value.Select(x => x.Value).ToList());

                munSection["ИтПрдСтм"] = totLimCost;
                allLimCost += totLimCost;

                munSection["ИтДатаЗаверш"] = totWorksDateEnd;
                if (allWorksDateEnd < totWorksDateEnd)
                {
                    allWorksDateEnd = totWorksDateEnd;
                }
            }

            reportParams.SimpleReportParams["ВсОбщПлщВс"] = allAreaMkd;
            reportParams.SimpleReportParams["ВсОбщПлщПмщ"] = allAreaLivNotLivMkd;
            reportParams.SimpleReportParams["ВсОбщПлщЖил"] = allAreaLivOwned;
            reportParams.SimpleReportParams["ВсКолЖит"] = allCountPerson;
            reportParams.SimpleReportParams["ВсИтСтм"] = allSum;
            reportParams.SimpleReportParams["ВсСтмФонд"] = allFundResource;
            reportParams.SimpleReportParams["ВсСтмФедБдж"] = allBudgetSubject;
            reportParams.SimpleReportParams["ВсСтмМстБдж"] = allBudgetMu;
            reportParams.SimpleReportParams["ВсСтмТСЖ"] = allOwnerResource;
            reportParams.SimpleReportParams["ВсУдСтм"] = this.GetPartialCostTotal(typeWorkDataByObjectCrDict.Values.SelectMany(x => x.Values).ToList());
            reportParams.SimpleReportParams["ВсПрдСтм"] = allLimCost;
            reportParams.SimpleReportParams["ВсДатаЗаверш"] = allWorksDateEnd;
            #endregion
        }

        protected virtual Dictionary<long, Dictionary<long, FinanceSourceDataProxy>> GetFinSrcResourceByObjectCrDict(IDomainService<FinanceSourceResource> serviceFinanceSourceResource)
        {
            var finSrcResourceByObjectCrDict = serviceFinanceSourceResource.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.ProgramCrId)
                .WhereIf(this.MunicipalityIds.Length > 0, x => this.MunicipalityIds.Contains(x.ObjectCr.RealityObject.MoSettlement.Id))
                .WhereIf(this.FinanceIds.Length > 0, x => this.FinanceIds.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    objectCrId = x.ObjectCr.Id,
                    municipId = x.ObjectCr.RealityObject.MoSettlement.Id,
                    x.ObjectCr.RealityObject.MoSettlement.Name,
                    x.ObjectCr.RealityObject.Address,
                    BudgetMu = x.BudgetMu ?? 0M,
                    BudgetSubject = x.BudgetSubject ?? 0M,
                    FundResource = x.FundResource ?? 0M,
                    OwnerResource = x.OwnerResource ?? 0M
                })
                .AsEnumerable()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Address)
                .GroupBy(x => x.municipId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.objectCrId)
                        .ToDictionary(
                            y => y.Key,
                            y => new FinanceSourceDataProxy
                            {
                                BudgetMu = y.Sum(z => z.BudgetMu.RoundDecimal(2)),
                                BudgetSubject = y.Sum(z => z.BudgetSubject.RoundDecimal(2)),
                                FundResource = y.Sum(z => z.FundResource.RoundDecimal(2)),
                                OwnerResource = y.Sum(z => z.OwnerResource.RoundDecimal(2)),
                            }));
            return finSrcResourceByObjectCrDict;
        }

        protected virtual Dictionary<long, Dictionary<long, DataProxy>> GetTypeWorkDataByObjectCrDict(IDomainService<TypeWorkCr> serviceTypeWork)
        {
            var typeWorkDataByObjectCrDict = serviceTypeWork.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.ProgramCrId)
                .WhereIf(this.MunicipalityIds.Length > 0, x => this.MunicipalityIds.Contains(x.ObjectCr.RealityObject.MoSettlement.Id))
                .WhereIf(this.FinanceIds.Length > 0, x => this.FinanceIds.Contains(x.FinanceSource.Id))
                .Select(
                    x => new
                    {
                        objectCrId = x.ObjectCr.Id,
                        municipId = x.ObjectCr.RealityObject.MoSettlement.Id,
                        municipName = x.ObjectCr.RealityObject.MoSettlement.Name,
                        address = x.ObjectCr.RealityObject.Address,
                        dateEnterExp = x.ObjectCr.RealityObject.DateCommissioning ?? DateTime.MinValue,
                        dateLastOverhaul = x.ObjectCr.RealityObject.DateLastOverhaul ?? DateTime.MinValue,
                        wallMaterial = x.ObjectCr.RealityObject.WallMaterial.Name,
                        countFloor = x.ObjectCr.RealityObject.MaximumFloors ?? 0,
                        countEntr = x.ObjectCr.RealityObject.NumberEntrances,
                        areaMkd = x.ObjectCr.RealityObject.AreaMkd ?? 0M,
                        areaLivingNotLivingMkd = x.ObjectCr.RealityObject.AreaLivingNotLivingMkd ?? 0M,
                        areaLivingOwned = x.ObjectCr.RealityObject.AreaLivingOwned ?? 0M,
                        countPerson = x.ObjectCr.RealityObject.NumberLiving ?? 0,
                        sum = x.Sum ?? 0M,
                        workName = x.Work.Name,
                        DateEndWork = x.DateEndWork ?? DateTime.MinValue
                    })
                .AsEnumerable()
                .OrderBy(x => x.municipName)
                .ThenBy(x => x.address)
                .GroupBy(x => x.municipId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.objectCrId)
                        .ToDictionary(
                            y => y.Key,
                            y => new DataProxy
                            {
                                Address = y.Select(z => z.address).FirstOrDefault(),
                                DateEnterExp = y.Select(z => z.dateEnterExp).FirstOrDefault(),
                                DateLastOverhaul = y.Select(z => z.dateLastOverhaul).FirstOrDefault(),
                                WallMat = y.Select(z => z.wallMaterial).FirstOrDefault(),
                                CountFloor = y.Select(z => z.countFloor).FirstOrDefault(),
                                CountEnt = y.Select(z => z.countEntr).FirstOrDefault(),
                                AreaMkd = y.Select(z => z.areaMkd).FirstOrDefault(),
                                AreaLivNotLivMkd = y.Select(z => z.areaLivingNotLivingMkd).FirstOrDefault(),
                                AreaLivOwned = y.Select(z => z.areaLivingOwned).FirstOrDefault(),
                                CountPerson = y.Select(z => z.countPerson).FirstOrDefault(),
                                Sum = y.Sum(z => z.sum.RoundDecimal(2)),
                                WorkNames = string.Join(", ", y.Select(z => z.workName).Where(z => !string.IsNullOrEmpty(z))),
                                WorksDateEnd = y.Max(z => z.DateEndWork)
                            }));

            return typeWorkDataByObjectCrDict;
        }
    }
}
