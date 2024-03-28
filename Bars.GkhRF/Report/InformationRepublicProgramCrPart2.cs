namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class InformationRepublicProgramCrPart2 : BasePrintForm
    {
        #region Свойства
        private DateTime startDate = DateTime.MinValue;

        private DateTime endDate = DateTime.MaxValue;

        private long[] municipalityIds;

        public InformationRepublicProgramCrPart2()
            : base(new ReportTemplateBinary(Properties.Resources.InformationRepublicProgramCrPart2))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.InformationRepublicProgramCrPart2";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Информация об участии в Республиканской адресной программе по проведению капитального ремонта МКД часть2";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.InformationRepublicProgramCrPart2";
            }
        }

        public override string Name
        {
            get
            {
                return "Информация об участии в Республиканской адресной программе по проведению капитального ремонта МКД часть2";
            }
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            startDate = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            endDate = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();

            reportParams.SimpleReportParams["reportDate"] = endDate.Date.ToShortDateString();

            var periodStartDate = this.startDate;
            var periodEndDate = this.endDate;
            startDate = startDate.Day != 1 ? new DateTime(startDate.Year, startDate.Month + 1, 1) : startDate;
            endDate = DateTime.DaysInMonth(endDate.Year, endDate.Month) != this.endDate.Day ? this.endDate.AddDays(-this.endDate.Day) : this.endDate;
            
            reportParams.SimpleReportParams["dateStart"] = startDate.Date.ToShortDateString();
            reportParams.SimpleReportParams["dateEnd"] = endDate.Date.ToShortDateString();

            if (this.startDate >= this.endDate)
            {
                return;
            }

            var muList = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name, Group = x.Group ?? string.Empty })
                .OrderBy(x => x.Group)
                .ThenBy(x => x.Name)
                .ToList();

            var muDictionary = muList.ToDictionary(x => x.Id, x => new { x.Name, x.Group });
            var alphabeticalGroups = new List<List<long>>();
            var lastGroup = "extraordinaryString";

            foreach (var municipality in muList)
            {
                if (municipality.Group != lastGroup)
                {
                    lastGroup = municipality.Group;
                    alphabeticalGroups.Add(new List<long>());
                }

                if (alphabeticalGroups.Any())
                {
                    alphabeticalGroups.Last().Add(municipality.Id);
                }
            }

            var infoRealObjQuery = serviceRealityObject.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Municipality.Id))
                .Where(x => x.ConditionHouse != ConditionHouse.Razed)
                .Where(x => x.TypeHouse != TypeHouse.SocialBehavior 
                    || (x.TypeOwnership.Name.ToLower() != "государственная" && x.TypeHouse == TypeHouse.SocialBehavior)
                    || (x.TypeOwnership.Name == null  && x.TypeHouse == TypeHouse.SocialBehavior))
                .Where(x => x.TypeHouse != TypeHouse.BlockedBuilding && x.TypeHouse != TypeHouse.Individual)
                .Select(x => new
                {
                    muId = x.Municipality.Id,
                    roId = x.Id,
                    x.Address,
                    AreaLiving = x.AreaLiving ?? 0M,
                    typeHouse = x.TypeHouse,
                    conditHouse = x.ConditionHouse
                });

            // столбцы 3, 10
            var infoRealObjDict = infoRealObjQuery.AsEnumerable()
                .GroupBy(x => x.muId)
                    .ToDictionary(x => x.Key, x => new
                    {
                        countMkd = x.Select(y => y.roId).Distinct().Count(),
                        sumAreaLiv = x.Select(y => new { y.roId, y.AreaLiving }).Distinct(y => y.roId).Sum(y => y.AreaLiving)
                    });

            var realObjIdsQuery = infoRealObjQuery.Select(x => x.roId);

            // столбец 4 (дома, включенные в договор на период отчета)
            var realObjInclContrQuery = Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
                                         .Where(x => realObjIdsQuery.Contains(x.RealityObject.Id))
                                         .Where(x => periodEndDate >= x.ContractRf.DateBegin || x.ContractRf.DateBegin == null)
                                         .Where(x => periodStartDate <= x.ContractRf.DateEnd || x.ContractRf.DateEnd == null)
                                         .Select(x => new
                                                          {
                                                              muId = x.RealityObject.Municipality.Id, 
                                                              roId = x.RealityObject.Id, 
                                                              address = x.RealityObject.Address,
                                                              x.TypeCondition,
                                                              x.IncludeDate,
                                                              x.ExcludeDate,
                                                              x.ContractRf.DocumentDate
                                                          });

            var realObjInclContrDict = realObjInclContrQuery
                                 .AsEnumerable()
                                 .GroupBy(x => x.muId)
                                 .ToDictionary(x => x.Key, x =>
                                     {
                                         var realObjDict = x.GroupBy(y => y.roId)
                                                            .ToDictionary(y => y.Key,
                                                                    y => y
                                                                        .OrderBy(z => z.DocumentDate.HasValue ? (periodEndDate - z.DocumentDate.Value).TotalDays : int.MaxValue)
                                                                        .ThenBy(z => z.ExcludeDate)
                                                                        .Select(z => new {z.IncludeDate, z.TypeCondition}).FirstOrDefault());

                                         var realObjList = realObjDict
                                                            .Where(y => y.Value.IncludeDate.HasValue
                                                                && y.Value.TypeCondition == TypeCondition.Include)
                                                                .Select(y => y.Key)
                                                                .ToList();
                                         var countRealObj = realObjList.Count();
                                         return new { realObjList, countRealObj };
                                     });

            var realObjIdInclContrList = realObjInclContrDict.SelectMany(x => x.Value.realObjList).ToList();

            // столбцы 11, 12, 13, 14
            var paymentRegister = Container.Resolve<IDomainService<PaymentItem>>().GetAll()
                .Where(x => realObjIdsQuery.Contains(x.Payment.RealityObject.Id))
                .Where(x => x.TypePayment == TypePayment.Cr || x.TypePayment == TypePayment.Cr185 || x.TypePayment == TypePayment.HireRegFund)
                .Where(x => x.ManagingOrganization != null)
                .Where(x => x.ChargeDate.HasValue && x.ChargeDate >= startDate && x.ChargeDate <= endDate)
                .Select(x => new
                {
                    muId = x.Payment.RealityObject.Municipality.Id,
                    x.TypePayment,
                    ChargePopulation = x.ChargePopulation ?? 0M,
                    PaidPopulation = x.PaidPopulation ?? 0M
                })
                                 .AsEnumerable()
                                 .GroupBy(x => x.muId)
                                 .ToDictionary(x => x.Key, x => new
                                 {
                                     sumChargeCr = x.Where(y => y.TypePayment == TypePayment.Cr || y.TypePayment == TypePayment.Cr185).Sum(y => y.ChargePopulation),
                                     sumPaidCr = x.Where(y => y.TypePayment == TypePayment.Cr || y.TypePayment == TypePayment.Cr185).Sum(y => y.PaidPopulation),

                                     sumChargeHire = x.Where(y => y.TypePayment == TypePayment.HireRegFund).Sum(y => y.ChargePopulation),
                                     sumPaidHire = x.Where(y => y.TypePayment == TypePayment.HireRegFund).Sum(y => y.PaidPopulation)
                                 });

            var realObjNotContrRegDict = serviceRealityObject.GetAll()
                                            .Where(x => realObjIdsQuery.Contains(x.Id))
                                            .Select(x => new 
                                            {
                                                muId = x.Municipality.Id,
                                                roId = x.Id
                                            })
                                            .AsEnumerable()
                                            .Where(x => !realObjIdInclContrList.Contains(x.roId))
                                            .GroupBy(x => x.muId)
                                            .ToDictionary(x => x.Key, x => x.Select(y => y.roId).Distinct().Count());

            // столбец 6
            var countEmergRealObj = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                                    .Where(x => realObjIdsQuery.Contains(x.Id))
                                    .Where(x => x.ConditionHouse == ConditionHouse.Emergency || x.ConditionHouse == ConditionHouse.Dilapidated)
                                    .Select(x => new
                                    {
                                        roId = x.Id,
                                        muId = x.Municipality.Id
                                    })
                                 .AsEnumerable()
                                 .Where(x => !realObjIdInclContrList.Contains(x.roId))
                                 .GroupBy(x => x.muId)
                                 .ToDictionary(x => x.Key, x => x.Select(y => y.roId).Distinct().Count());

            // столбец 7
            var infoNotPrRealObjDict = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                                .Where(x => realObjIdsQuery.Contains(x.RealityObject.Id))
                                .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable)
                                .Where(x => x.RealityObject.IsBuildSocialMortgage == YesNo.No)
                                .Where(x => periodEndDate >= x.ManOrgContract.StartDate || x.ManOrgContract.StartDate == null)
                                .Where(x => (periodStartDate <= x.ManOrgContract.EndDate && x.ManOrgContract.EndDate > periodEndDate) || x.ManOrgContract.EndDate == null)
                                .Select(x => new
                                {
                                    roId = x.RealityObject.Id,
                                    muId = x.RealityObject.Municipality.Id,
                                    address = x.RealityObject.Address,
                                    x.ManOrgContract.TypeContractManOrgRealObj,
                                    x.ManOrgContract.StartDate,
                                    x.ManOrgContract.EndDate
                                })
                                .AsEnumerable()
                                .Where(x => !realObjIdInclContrList.Contains(x.roId))
                                .GroupBy(x => x.muId)
                                .ToDictionary(x => x.Key, x =>
                                    {
                                        var realObjDict = x.GroupBy(y => y.roId)
                                       .ToDictionary(y => y.Key,
                                               y => y
                                                   .OrderBy(z => z.StartDate.HasValue ? (periodEndDate - z.StartDate.Value).TotalDays : int.MaxValue)
                                                   .ThenBy(z => z.EndDate)
                                                   .Select(z => z.TypeContractManOrgRealObj).FirstOrDefault());

                                        var realObjList = realObjDict
                                                           .Where(y => y.Value == TypeContractManOrg.DirectManag)
                                                               .Select(y => y.Key)
                                                               .ToList();
                                        var countRealObj = realObjList.Count();
                                        return countRealObj;
                                    });

            // столбец 8
            var socMortRealObj = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                                    .Where(x => realObjIdsQuery.Contains(x.Id))
                                    .Where(x => x.ConditionHouse == ConditionHouse.Serviceable)
                                    .Where(x => x.IsBuildSocialMortgage == YesNo.Yes)
                                    .Select(x => new
                                    {
                                        roId = x.Id,
                                        muId = x.Municipality.Id
                                    })
                                 .AsEnumerable()
                                 .Where(x => !realObjIdInclContrList.Contains(x.roId))
                                 .GroupBy(x => x.muId)
                                 .ToDictionary(x => x.Key, x => x.Select(y => y.roId).Distinct().Count());


            var groupSection = reportParams.ComplexReportParams.ДобавитьСекцию("sectionGroup");
            var groupNameSection = groupSection.ДобавитьСекцию("sectionGroupName");
            var section = groupSection.ДобавитьСекцию("section");
            var sectionTotals = groupSection.ДобавитьСекцию("sectionTot");
            
            var counter = 0;
            var totals = new decimal[12];
            foreach (var group in alphabeticalGroups)
            {
                groupSection.ДобавитьСтроку();
                var firstMu = muDictionary[group.First()];

                var hasGroup = false;
                if (firstMu.Group != string.Empty)
                {
                    groupNameSection.ДобавитьСтроку();
                    groupNameSection["columnGr1"] = (++counter).ToStr();
                    groupNameSection["columnGr2"] = firstMu.Group;
                    hasGroup = true;
                }

                var totalsGr = new decimal[12];
                foreach (var muId in group)
                {
                    section.ДобавитьСтроку();
                    section["column1"] = hasGroup ? string.Empty : (++counter).ToStr();
                    section["column2"] = muDictionary[muId].Name;

                    if (infoRealObjDict.ContainsKey(muId))
                    {
                        section["column3"] = infoRealObjDict[muId].countMkd;
                        totals[0] += infoRealObjDict[muId].countMkd;


                        section["column10"] = infoRealObjDict[muId].sumAreaLiv;
                        totals[7] += infoRealObjDict[muId].sumAreaLiv;
                        
                        if (hasGroup)
                        {
                            totalsGr[0] += infoRealObjDict[muId].countMkd;
                            totalsGr[7] += infoRealObjDict[muId].sumAreaLiv;
                        }
                    }

                    var column4 = 0M;
                    if (realObjInclContrDict.ContainsKey(muId))
                    {
                        column4 = realObjInclContrDict[muId].countRealObj;
                        totals[1] += column4;
                        totalsGr[1] += hasGroup ? column4 : 0M;
                    }
                    section["column4"] = column4;

                    var countColumn5 = 0M;                 
                    countColumn5 += realObjNotContrRegDict.ContainsKey(muId) ? realObjNotContrRegDict[muId] : 0M;

                    section["column5"] = countColumn5;
                    var column9 = countColumn5;
                    totals[2] += countColumn5;
                    totalsGr[2] += hasGroup ? countColumn5 : 0M;

                    var totalCount = 0M;
                    var column6 = countEmergRealObj.ContainsKey(muId) ? countEmergRealObj[muId] : 0M;
                    section["column6"] = column6;
                    totals[3] += column6;
                    totalsGr[3] += hasGroup ? column6 : 0M;
                    totalCount += column6;

                    var column7 = 0M;
                    if (infoNotPrRealObjDict.ContainsKey(muId))
                    {
                        column7 = infoNotPrRealObjDict[muId];                   
                        totals[4] += column7;
                        totalCount += column7;

                        if (hasGroup)
                        {
                            totalsGr[4] += column7;
                        }
                    }
                    section["column7"] = column7;

                    var column8 = 0M;
                    if (socMortRealObj.ContainsKey(muId))
                    {
                        column8 = socMortRealObj[muId];
                        totals[5] += column8;
                        totalCount += column8;

                        if (hasGroup)
                        {
                            totalsGr[5] += column8;
                        }
                    }
                    section["column8"] = column8;

                    column9 = column9 != 0 ? (column9 - totalCount) : 0;

                    section["column9"] = column9;
                    totals[6] += column9;
                    totalsGr[6] += hasGroup ? column9 : 0M;

                    var column11 = 0M;
                    var column12 = 0M;
                    var column13 = 0M;
                    var column14 = 0M;
                    if (paymentRegister.ContainsKey(muId))
                    {
                        column11 = paymentRegister[muId].sumChargeCr;                  
                        totals[8] += column11;

                        column12 = paymentRegister[muId].sumPaidCr;
                        totals[9] += column12;

                        column13 = paymentRegister[muId].sumChargeHire;
                        totals[10] += column13;

                        column14 = paymentRegister[muId].sumPaidHire;
                        totals[11] += column14;

                        if (hasGroup)
                        {
                            totalsGr[8] += column11;
                            totalsGr[9] += column12;
                            totalsGr[10] += column13;
                            totalsGr[11] += column14;
                        }
                    }

                    section["column11"] = column11;
                    section["column12"] = column12;
                    section["column13"] = column13;
                    section["column14"] = column14;
                }

                if (hasGroup)
                {
                    this.FillSection(groupNameSection, totalsGr, "columnGr");
                }
            }

            sectionTotals.ДобавитьСтроку();
            this.FillSection(sectionTotals, totals, "columnTot");
        }

        private void FillSection(Section section, decimal[] data, string postfix)
        {
            for (var i = 0; i < data.Length; ++i)
            {
                if (data[i] != 0)
                {
                    section[string.Format("{0}{1}", postfix, (i + 3).ToStr())] = data[i];
                }
            }
        }
    }
}