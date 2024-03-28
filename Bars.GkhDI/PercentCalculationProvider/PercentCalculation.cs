namespace Bars.GkhDi.PercentCalculationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    public class PercentCalculation : BasePercentCalculation
    {
        #region property injections
        public IDomainService<AdminResp> AdminRespDomain { get; set; }
        public IDomainService<FundsInfo> FundsInfoDomain { get; set; }
        public IDomainService<Documents> DocumentsDomain { get; set; }
        public IDomainService<FinActivity> FinActivityDomain { get; set; }
        public IDomainService<FinActivityAudit> FinActivityAuditDomain { get; set; }
        public IDomainService<FinActivityDocs> FinActivityDocsDomain { get; set; }
        public IDomainService<FinActivityDocByYear> FinActivityDocByYearDomain { get; set; }
        public IDomainService<FinActivityManagRealityObj> FinActivityManagRealityObjDomain { get; set; }
        public IDomainService<ManagingOrgWorkMode> ManagingOrgWorkModeDomain { get; set; }
        public IDomainService<ManagingOrgMembership> ManagingOrgMembershipDomain { get; set; }
        public IDomainService<InformationOnContracts> InformationOnContractsDomain { get; set; }
        public IDomainService<TehPassportValue> TehPassportValueDomain { get; set; }
        public IDomainService<HousingService> HousingServiceDomain { get; set; }
        public IDomainService<CommunalService> CommunalServiceDomain { get; set; }
        public IDomainService<CapRepairService> CapRepairServiceDomain { get; set; }
        public IDomainService<TemplateService> TemplateServiceDomain { get; set; }

        public IDisclosureInfoService DisclosureInfoService { get; set; }
        #endregion

    public override bool CheckByPeriod(PeriodDi periodDi)
        {
            return periodDi.DateStart >= new DateTime(2013, 1, 1) && periodDi.DateStart < new DateTime(2015, 01, 01);
        }

        protected override Dictionary<long, PercCalcResult> CalculateManOrgsInfo(IEnumerable<DisclosureInfo> disInfos, IQueryable<DisclosureInfo> diQuery)
        {
            var dictMoPerc = new Dictionary<long, PercCalcResult>();

            var adminRespDisInfoIds = this.AdminRespDomain
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .Select(x => x.DisclosureInfo.Id)
                         .Distinct()
                         .ToDictionary(x => x);

            var hasFundsInfoDisInfos = this.FundsInfoDomain
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .Select(x => x.DisclosureInfo.Id)
                         .Distinct()
                         .ToDictionary(x => x);

            var documentsDisInfo = this.DocumentsDomain
                    .GetAll()
                    .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                    .ToDictionary(
                        x => x.DisclosureInfo.Id,
                        y => new
                            {
                                y.NotAvailable,
                                FileProjectContract = y.FileProjectContract != null
                            });

            var finActivityDisInfo = this.FinActivityDomain
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .ToDictionary(
                             x => x.DisclosureInfo.Id,
                             y => new { y.TaxSystem, MoTypeManagement = y.DisclosureInfo.ManagingOrganization.TypeManagement });

            var finActivityAuditYearsDi = this.FinActivityAuditDomain
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                         .GroupBy(y => y.ManagingOrganization.Id)
                         .ToDictionary(
                             y => y.Key, x => x.Select(z => new { z.Year, z.TypeAuditStateDi, z.File }).ToList());

            var finActivityDocsDisInfo = this.FinActivityDocsDomain
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .ToDictionary(x => x.DisclosureInfo.Id, y => new
                                                                          {
                                                                              y.BookkepingBalance,
                                                                              y.BookkepingBalanceAnnex
                                                                          });

            var finActivityDocsByYearDi = this.FinActivityDocByYearDomain
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                         .GroupBy(y => y.ManagingOrganization.Id)
                         .ToDictionary(y => y.Key, x => x.Select(y => new
                                                                          {
                                                                              y.Year,
                                                                              y.TypeDocByYearDi
                                                                          }));

            var finActManorgRealityObjs = this.FinActivityManagRealityObjDomain
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .GroupBy(x => x.DisclosureInfo.Id)
                         .ToDictionary(
                             y => y.Key,
                             x =>
                             x.Where(y => y.PresentedToRepay != null && y.ReceivedProvidedService != null).Select(z => z.RealityObject.Id).Distinct()
                              .ToArray());

            var workModeService = this.ManagingOrgWorkModeDomain.GetAll()
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                .GroupBy(x => x.ManagingOrganization.Id)
                .ToDictionary(x => x.Key, y => new
                {
                    DispatcherWork = y.Any(z => z.TypeMode == TypeMode.DispatcherWork),
                    ReceptionCitizens = y.Any(z => z.TypeMode == TypeMode.ReceptionCitizens),
                    WorkMode = y.Any(z => z.TypeMode == TypeMode.WorkMode)
                });

            var hasManOrgMembershipMoIds = this.ManagingOrgMembershipDomain
                    .GetAll()
                    .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                    .Where(
                        x =>
                        (x.DateStart.Value >= this.period.DateStart.Value && this.period.DateEnd.Value >= x.DateStart.Value)
                        || (this.period.DateStart.Value >= x.DateStart.Value
                            && ((x.DateEnd.HasValue && x.DateEnd.Value >= this.period.DateStart.Value) || !x.DateEnd.HasValue)))
                    .Select(x => x.ManagingOrganization.Id)
                    .Distinct()
                    .ToDictionary(x => x);

            var hasManOrgrealObjMoIds = this.ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.ManOrgContract.EndDate >= this.period.DateStart.Value.AddYears(-1))
                .Where(x => x.ManOrgContract.EndDate < this.period.DateEnd.Value.AddYears(-1))
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManOrgContract.ManagingOrganization.Id))
                .Select(x => x.ManOrgContract.ManagingOrganization.Id)
                .Distinct()
                .ToDictionary(x => x);

            var hasInfoOnContrIds = this.InformationOnContractsDomain
                                  .GetAll()
                                  .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.DisclosureInfo.ManagingOrganization.Id) &&
                                      (((x.DateStart.HasValue && x.DateStart >= this.period.DateStart.Value || !x.DateStart.HasValue)
                                             && (x.DateStart.HasValue && this.period.DateEnd.Value >= x.DateStart))
                                             || ((x.DateStart.HasValue && this.period.DateStart.Value >= x.DateStart || !x.DateStart.HasValue)
                                             && (x.DateEnd.HasValue && x.DateEnd >= this.period.DateStart.Value || !x.DateEnd.HasValue || x.DateEnd <= DateTime.MinValue))))
                                  .Select(x => x.DisclosureInfo.ManagingOrganization.Id)
                                  .Distinct()
                                  .ToDictionary(x => x);

            var periodYear = 0;
            if (this.period.DateStart != null)
            {
                periodYear = this.period.DateStart.Value.Year;
            }

            foreach (var disclosureInfo in disInfos)
            {
                var positionsCount = 0;
                var completePositionsCount = 0;
                var tempPositionsCount = 0;
                var tempCompletePositionsCount = 0;
                decimal? percent = 0;

                #region Административная ответственность

                tempPositionsCount = 1;

                if (disclosureInfo.AdminResponsibility == YesNoNotSet.No)
                {
                    percent = null;
                    tempPositionsCount = 0;
                }

                if (disclosureInfo.AdminResponsibility == YesNoNotSet.Yes)
                {
                    percent = adminRespDisInfoIds.ContainsKey(disclosureInfo.Id) ? 100 : 0;
                }

                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                this.DisInfoPercents.Add(
                    new DisclosureInfoPercent
                        {
                            Code = "AdminResponsibilityPercent",
                            TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                            Percent = percent,
                            CalcDate = DateTime.Now.Date,
                            PositionsCount = tempPositionsCount,
                            CompletePositionsCount = tempCompletePositionsCount,
                            ActualVersion = 1,
                            DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                        });

                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;

                #endregion Административная ответственность

                #region Документы

                percent = 0;
                tempPositionsCount = 1;
                tempCompletePositionsCount = 0;

                if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK)
                {
                    tempPositionsCount--;
                }

                if (documentsDisInfo.ContainsKey(disclosureInfo.Id))
                {
                    if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ
                        && documentsDisInfo[disclosureInfo.Id].NotAvailable)
                    {
                        tempPositionsCount--;
                    }
                    else
                    {
                        if (disclosureInfo.ManagingOrganization.TypeManagement != TypeManagementManOrg.JSK && documentsDisInfo[disclosureInfo.Id].FileProjectContract)
                        {
                            tempCompletePositionsCount++;
                        }
                    }

                    percent = tempPositionsCount != 0 ? (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2) : 100m;
                }

                this.DisInfoPercents.Add(
                    new DisclosureInfoPercent
                        {
                            Code = "DocumentsPercent",
                            TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                            Percent = percent,
                            CalcDate = DateTime.Now.Date,
                            PositionsCount = tempPositionsCount,
                            CompletePositionsCount = tempCompletePositionsCount,
                            ActualVersion = 1,
                            DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                        });

                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;

                #endregion Документы

                #region Финансовая деятельность

                percent = 0;
                tempPositionsCount = 13;
                tempCompletePositionsCount = 0;

                if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK)
                {
                    tempPositionsCount -= 9;
                }

                if (finActivityDisInfo.ContainsKey(disclosureInfo.Id))
                {
                    if (finActivityDisInfo[disclosureInfo.Id].TaxSystem != null)
                    {
                        tempCompletePositionsCount++;
                    }

                    if (finActivityDisInfo[disclosureInfo.Id].MoTypeManagement
                        != TypeManagementManOrg.UK)
                    {
                        if (finActivityAuditYearsDi.ContainsKey(disclosureInfo.ManagingOrganization.Id))
                        {
                            if (finActivityAuditYearsDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear
                                    && (x.File != null || x.TypeAuditStateDi == TypeAuditStateDi.NotInspect)))
                            {
                                tempCompletePositionsCount++;
                            }

                            if (finActivityAuditYearsDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear - 1
                                    && (x.File != null || x.TypeAuditStateDi == TypeAuditStateDi.NotInspect)))
                            {
                                tempCompletePositionsCount++;
                            }

                            if (finActivityAuditYearsDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear - 2
                                    && (x.File != null || x.TypeAuditStateDi == TypeAuditStateDi.NotInspect)))
                            {
                                tempCompletePositionsCount++;
                            }
                        }
                    }

                    if (finActivityDocsDisInfo.ContainsKey(disclosureInfo.Id) && finActivityDocsDisInfo[disclosureInfo.Id].BookkepingBalance != null)
                    {
                        tempCompletePositionsCount++;
                    }

                    if (finActivityDocsDisInfo.ContainsKey(disclosureInfo.Id) && finActivityDocsDisInfo[disclosureInfo.Id].BookkepingBalanceAnnex != null)
                    {
                        tempCompletePositionsCount++;
                    }

                    if (finActivityDisInfo[disclosureInfo.Id].MoTypeManagement != TypeManagementManOrg.UK && finActivityDocsByYearDi.ContainsKey(disclosureInfo.ManagingOrganization.Id))
                    {
                        if (finActivityDocsByYearDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear
                                && (x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome)))
                        {
                            tempCompletePositionsCount++;
                        }

                        if (finActivityDocsByYearDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear - 1
                                && (x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome)))
                        {
                            tempCompletePositionsCount++;
                        }

                        if (finActivityDocsByYearDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear - 1
                                && (x.TypeDocByYearDi == TypeDocByYearDi.ReportEstimateIncome)))
                        {
                            tempCompletePositionsCount++;
                        }

                        if (finActivityDocsByYearDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear
                                && (x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory)))
                        {
                            tempCompletePositionsCount++;
                        }

                        if (finActivityDocsByYearDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear - 1
                                && (x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory)))
                        {
                            tempCompletePositionsCount++;
                        }

                        if (finActivityDocsByYearDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear - 2
                                && (x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory)))
                        {
                            tempCompletePositionsCount++;
                        }
                    }

                    if ((!this.RealObjByDi.ContainsKey(disclosureInfo.Id) || this.RealObjByDi[disclosureInfo.Id].Length == 0) || (finActManorgRealityObjs.ContainsKey(disclosureInfo.Id)
                            && this.RealObjByDi[disclosureInfo.Id].Length == finActManorgRealityObjs[disclosureInfo.Id].Where(x => this.RealObjByDi[disclosureInfo.Id].Contains(x)).Distinct().Count()))
                    {
                        tempCompletePositionsCount++;
                    }

                    percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);
                }

                this.DisInfoPercents.Add(
                    new DisclosureInfoPercent
                        {
                            Code = "FinActivityPercent",
                            TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                            Percent = percent,
                            CalcDate = DateTime.Now.Date,
                            PositionsCount = tempPositionsCount,
                            CompletePositionsCount = tempCompletePositionsCount,
                            ActualVersion = 1,
                            DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                        });

                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;

                #endregion Финансовая деятельность

                #region Сведения о фондах

                percent = null;
                tempPositionsCount = 0;
                tempCompletePositionsCount = 0;

                if (disclosureInfo.ManagingOrganization.TypeManagement != TypeManagementManOrg.UK)
                {
                    percent = 0;
                    tempPositionsCount = 1;

                    if (disclosureInfo.FundsInfo == YesNoNotSet.No && disclosureInfo.DocumentWithoutFunds != null)
                    {
                        percent = null;
                        tempPositionsCount = 0;
                    }

                    if (disclosureInfo.FundsInfo == YesNoNotSet.Yes && disclosureInfo.SizePayments != null)
                    {
                        var hasFundsInfo = hasFundsInfoDisInfos.ContainsKey(disclosureInfo.Id);

                        percent = hasFundsInfo ? 100 : 0;
                    }

                    tempCompletePositionsCount = percent == 100 ? 1 : 0;
                }

                this.DisInfoPercents.Add(new DisclosureInfoPercent
                                                 {
                                                     Code = "FundsInfoPercent",
                                                     TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                                                     Percent = percent,
                                                     CalcDate = DateTime.Now.Date,
                                                     PositionsCount = tempPositionsCount,
                                                     CompletePositionsCount = tempCompletePositionsCount,
                                                     ActualVersion = 1,
                                                     DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                                                 });
                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;

                #endregion Сведения о фондах

                #region Общие сведения

                tempPositionsCount = 0;
                tempCompletePositionsCount = 0;
                
                // ФИО руководителя
                tempPositionsCount++;
                if (!string.IsNullOrEmpty(this.DisclosureInfoService.GetPositionByCode(
                    disclosureInfo.ManagingOrganization.Contragent.Id,
                    disclosureInfo.PeriodDi,
                    new List<string> { "1", "4" })))
                {
                    tempCompletePositionsCount++;
                }

                tempPositionsCount++;

                // ОГРН
                if (!string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.Ogrn)
                    && !string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.OgrnRegistration))
                {
                    tempCompletePositionsCount++;
                }

                // Почтовый адрес
                tempPositionsCount++;
                if (!string.IsNullOrEmpty(
                        disclosureInfo.ManagingOrganization.Contragent.FiasMailingAddress != null
                            ? disclosureInfo.ManagingOrganization.Contragent.FiasMailingAddress.AddressName
                            : string.Empty))
                {
                    tempCompletePositionsCount++;
                }

                // Фактический адрес
                tempPositionsCount++;
                if (!string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.FiasFactAddress != null
                                                  ? disclosureInfo.ManagingOrganization.Contragent.FiasFactAddress.AddressName : string.Empty))
                {
                    tempCompletePositionsCount++;
                }

                // Телефон
                tempPositionsCount++;
                if (!string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.Phone))
                {
                    tempCompletePositionsCount++;
                }

                // E-mail
                tempPositionsCount++;
                if (!string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.Email))
                {
                    tempCompletePositionsCount++;
                }

                // официальный сайт
                if (disclosureInfo.ManagingOrganization.Contragent.IsSite)
                {
                    tempPositionsCount++;
                    if (!string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.OfficialWebsite))
                    {
                        tempCompletePositionsCount++;
                    }
                }

                if (disclosureInfo.ManagingOrganization.TypeManagement != TypeManagementManOrg.UK)
                {
                    // Члены правления
                    tempPositionsCount++;
                    if (!string.IsNullOrEmpty(this.DisclosureInfoService.GetPositionByCode(disclosureInfo.ManagingOrganization.Contragent.Id, disclosureInfo.PeriodDi, new List<string> { "5" })))
                    {
                        tempCompletePositionsCount++;
                    }

                    // Члены ревизионной комисии
                    tempPositionsCount++;
                    if (!string.IsNullOrEmpty(this.DisclosureInfoService.GetPositionByCode(disclosureInfo.ManagingOrganization.Contragent.Id, disclosureInfo.PeriodDi, new List<string> { "6" })))
                    {
                        tempCompletePositionsCount++;
                    }
                }

                tempPositionsCount += disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK ? 3 : 2;
                if (workModeService.ContainsKey(disclosureInfo.ManagingOrganization.Id))
                {
                    // Режим Работы
                    if (workModeService[disclosureInfo.ManagingOrganization.Id].WorkMode)
                    {
                        tempCompletePositionsCount++;
                    }

                    if (workModeService[disclosureInfo.ManagingOrganization.Id].ReceptionCitizens)
                    {
                        tempCompletePositionsCount++;
                    }

                    if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK)
                    {
                        if (workModeService[disclosureInfo.ManagingOrganization.Id].DispatcherWork)
                        {
                            tempCompletePositionsCount++;
                        }
                    }
                }

                percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

                this.DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "GeneralDataPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                });

                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;

                #endregion Общие сведения

                #region Членство объединениях

                tempPositionsCount = 1;
                percent = 0;

                if (disclosureInfo.MembershipUnions == YesNoNotSet.No)
                {
                    percent = null;
                    tempPositionsCount = 0;
                }

                if (disclosureInfo.MembershipUnions == YesNoNotSet.Yes)
                {
                    var hasManOrgMembership = hasManOrgMembershipMoIds.ContainsKey(disclosureInfo.ManagingOrganization.Id);

                    percent = hasManOrgMembership ? 100 : 0;
                }

                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                this.DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "MembershipUnionsPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                });

                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;
                #endregion Членство в объединениях

                #region Сведения о расторгнутых договорах

                tempPositionsCount = 1;
                percent = 0;

                if (disclosureInfo.TerminateContract == YesNoNotSet.No)
                {
                    percent = null;
                    tempPositionsCount = 0;
                }

                if (disclosureInfo.TerminateContract == YesNoNotSet.Yes)
                {
                    var hasManOrgrealObj = hasManOrgrealObjMoIds.ContainsKey(disclosureInfo.ManagingOrganization.Id);

                    percent = hasManOrgrealObj ? 100 : 0;
                }

                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                this.DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "TerminateContractPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                });

                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;
                #endregion Сведения о расторгнутых договорах

                #region Сведения о договорах
                tempPositionsCount = 0;
                tempCompletePositionsCount = 0;
                percent = null;

                if (disclosureInfo.ManagingOrganization.TypeManagement != TypeManagementManOrg.UK)
                {
                    percent = 0;
                    tempPositionsCount = 1;

                    if (disclosureInfo.ContractsAvailability == YesNoNotSet.No)
                    {
                        percent = null;
                        tempPositionsCount = 0;
                    }

                    if (disclosureInfo.ContractsAvailability == YesNoNotSet.Yes && disclosureInfo.NumberContracts != null)
                    {
                        var hasInfoOnContr = hasInfoOnContrIds.ContainsKey(disclosureInfo.ManagingOrganization.Id);

                        percent = hasInfoOnContr ? 100 : 0;
                    }

                    tempCompletePositionsCount = percent == 100 ? 1 : 0;
                }

                this.DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "InfoOnContrPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                });

                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;
                #endregion Сведения о договорах

                percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);

                this.DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "ManOrgInfoPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = positionsCount,
                    CompletePositionsCount = completePositionsCount,
                    ActualVersion = 1,
                    DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                });

                dictMoPerc.Add(disclosureInfo.Id,
                    new PercCalcResult
                        {
                            Percent = percent.ToDecimal(),
                            CompletePositionCount = completePositionsCount,
                            PositionCount = positionsCount
                        });
            }

            return dictMoPerc;
        }

        protected override void CalculateRealObj()
        {
            var diRoQuery = this.DisInfoRoDomain
                .GetAll().Where(x =>
                    this.moRoQuery.Any(y =>
                        y.RealityObject.Id == x.RealityObject.Id && y.ManOrgContract.ManagingOrganization.Id == x.ManagingOrganization.Id) &&
                    x.PeriodDi.Id == this.period.Id);

            var diRealObjs = diRoQuery
                        .Select(x => new
                        {
                            x.Id,
                            x.ReductionPayment,
                            RealObjId = x.RealityObject.Id,
                            x.PlaceGeneralUse,
                            x.NonResidentialPlacement,
                            x.RealityObject.NumberLifts, 
                            x.PeriodDi
                        }).ToList();

            var planReductionExpensePercentDict = this.GetPlanReductionExpensePercentDict(diRoQuery);

            var infoAboutReductionPaymentDiIds = this.InfoAboutReductionPaymentDomain.GetAll()
                                                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                                                .Select(x => x.DisclosureInfoRealityObj.Id)
                                                .Distinct()
                                                .ToArray();

            var planWorkServicePercentDict = this.GetPlanWorkServicePercentDict(diRoQuery);

            var nonResidentialPlacementDiIds = this.NonResidentialPlacementDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.DisclosureInfoRealityObj.RealityObject.Id)
                                && ((x.DateStart.HasValue && (x.DateStart >= this.period.DateStart)
                                        && x.DateStart.HasValue && (this.period.DateEnd >= x.DateStart))
                                        || ((x.DateStart.HasValue && (this.period.DateStart >= x.DateStart) || !x.DateStart.HasValue)
                                            && (x.DateEnd.HasValue && (x.DateEnd >= this.period.DateStart) || !x.DateEnd.HasValue))))
                .Select(x => x.DisclosureInfoRealityObj.RealityObject.Id)
                .Distinct()
                .ToDictionary(x => x);

            var infoAboutUseCommonFacilitiesDiIds = this.InfoAboutUseCommonFacilitiesDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.DisclosureInfoRealityObj.RealityObject.Id)
                               && ((x.DateStart.HasValue && (x.DateStart >= this.period.DateStart)
                                        && x.DateStart.HasValue && (this.period.DateEnd >= x.DateStart))
                                        || ((x.DateStart.HasValue && (this.period.DateStart >= x.DateStart) || !x.DateStart.HasValue)
                                            && (x.DateEnd.HasValue && (x.DateEnd >= this.period.DateStart) || !x.DateEnd.HasValue))))
                .Select(x => x.DisclosureInfoRealityObj.RealityObject.Id)
                .Distinct()
                .ToDictionary(x => x);

            var documentRealObjs = this.DocumentsRealityObjDomain.GetAll()
                 .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                 .Select(x => new
                    {
                        DisclosureInfoRealityObjId = x.DisclosureInfoRealityObj.Id,
                        FileActState = ((long?)x.FileActState.Id) != null,
                        FileCatalogRepair = ((long?)x.FileCatalogRepair.Id) != null,
                        FileReportPlanRepair = ((long?)x.FileReportPlanRepair.Id) != null
                    })
                 .AsEnumerable()
                 .GroupBy(x => x.DisclosureInfoRealityObjId)
                 .ToDictionary(
                     x => x.Key,
                     y => y.Select(z => new { z.FileActState, z.FileCatalogRepair, z.FileReportPlanRepair }).FirstOrDefault());

            var services = this.BaseServiceDomain.GetAll()
               .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
               .Where(x => x.TemplateService.IsMandatory &&
                    (x.TemplateService.ActualYearStart.HasValue && x.DisclosureInfoRealityObj.PeriodDi.DateStart.Value.Year >= x.TemplateService.ActualYearStart.Value ||
                        !x.TemplateService.ActualYearStart.HasValue)
                    && (x.TemplateService.ActualYearEnd.HasValue && x.DisclosureInfoRealityObj.PeriodDi.DateStart.Value.Year <= x.TemplateService.ActualYearEnd.Value ||
                        !x.TemplateService.ActualYearEnd.HasValue))
               .Select(x => new
                    {
                        DisclosureInfoRealityObjId = x.DisclosureInfoRealityObj.Id,
                        x.Id,
                        x.TemplateService.Code,
                        x.TemplateService.KindServiceDi,
                        RealObjId = x.DisclosureInfoRealityObj.RealityObject.Id
               })
               .AsEnumerable()
               .GroupBy(x => x.DisclosureInfoRealityObjId)
               .ToDictionary(
                    x => x.Key, 
                    y => y.Select(x => new ServiceProxy
                               {
                                   Id = x.Id,
                                   Code = x.Code.ToInt(),
                                   KindServiceDi = x.KindServiceDi,
                                   DiRealObjId = x.DisclosureInfoRealityObjId,
                                   RealObjId = x.RealObjId
                    }));

            // Списки на исключение из подсчета
            var housingServices = this.GetHousingServicesToExcludeDict(diRoQuery);
            var communalServices = this.GetCommunalServicesToExcludeDict(diRoQuery);
            var repairServices = this.GetRepairServicesToExcludeDict(diRoQuery);
            var capRepairServices = this.GetCapRepairServicesToExcludeDict(diRoQuery);

            var serv7RealObjs = this.TehPassportValueDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id) && x.FormCode == "Form_3_7" && x.CellCode == "1:3" && x.Value == "0")
                .Select(x => x.TehPassport.RealityObject.Id).Distinct()
                .ToDictionary(x => x);

            var tariffForConsumers = this.TariffForConsumersDomain.GetAll()
                                    .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                                    .Select(x => x.BaseService.Id)
                                    .Distinct()
                                    .ToDictionary(x => x);

            var tariffForRsoItemsDiIds = this.TariffForRsoDomain.GetAll()
                                    .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                                    .Select(x => x.BaseService.Id).Distinct()
                                    .ToDictionary(x => x);

            var repairServiceIsDisclosedDict = this.GetRepairServiceIsDisclosedDict(diRoQuery);
            
            foreach (var diRealityObj in diRealObjs)
            {
                var PositionsCount = 0;
                var CompletePositionsCount = 0;
                var tempPositionsCount = 0;
                var tempCompletePositionsCount = 0;
                decimal? percent;

                #region План мер по снижению расходов

                percent = planReductionExpensePercentDict.ContainsKey(diRealityObj.Id) ? planReductionExpensePercentDict[diRealityObj.Id] : 0;
                tempPositionsCount = 1;
                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                this.RealObjPercents.Add(new DiRealObjPercent
                {
                                                    Code = "PlanReductionExpensePercent",
                                                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                                                    Percent = percent,
                                                    CalcDate = DateTime.Now.Date,
                                                    PositionsCount = tempPositionsCount,
                                                    CompletePositionsCount = tempCompletePositionsCount,
                                                    ActualVersion = 1,
                                                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                                                });
                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion План мер по снижению расходов

                #region Сведения о случаях снижения платы
                percent = 0;
                tempPositionsCount = 1;
                if (diRealityObj.ReductionPayment == YesNoNotSet.No)
                {
                    percent = null;
                    tempPositionsCount = 0;
                }

                if (diRealityObj.ReductionPayment == YesNoNotSet.Yes)
                {
                    var hasInfoAboutReductionPayment = infoAboutReductionPaymentDiIds.Contains(diRealityObj.Id);

                    percent = hasInfoAboutReductionPayment ? 100 : 0;
                }

                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                this.RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "InfoAboutReductionPaymentPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion Сведения о случаях снижения платы

                #region Документы

                tempPositionsCount = 3;
                tempCompletePositionsCount = 0;

                if (documentRealObjs.ContainsKey(diRealityObj.Id) && documentRealObjs[diRealityObj.Id].FileActState)
                {
                    tempCompletePositionsCount++;
                }

                if (documentRealObjs.ContainsKey(diRealityObj.Id) && documentRealObjs[diRealityObj.Id].FileCatalogRepair)
                {
                    tempCompletePositionsCount++;
                }

                if (documentRealObjs.ContainsKey(diRealityObj.Id) && documentRealObjs[diRealityObj.Id].FileReportPlanRepair)
                {
                    tempCompletePositionsCount++;
                }

                percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

                this.RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "DocumentsDiRealObjPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;
                #endregion Документы

                #region План работ по содержанию и ремонту

                percent = planWorkServicePercentDict.ContainsKey(diRealityObj.Id) ? planWorkServicePercentDict[diRealityObj.Id] : 0;
                tempPositionsCount = 1;
                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                this.RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "PlanWorkServiceRepairPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });


                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;
                #endregion План работ по содержанию и ремонту

                #region Сведения об использовании нежилых помещений

                tempPositionsCount = 1;
                tempCompletePositionsCount = 0;

                if (diRealityObj.NonResidentialPlacement == YesNoNotSet.No)
                {
                    tempPositionsCount = 0;
                }
                else
                {
                    if (diRealityObj.NonResidentialPlacement == YesNoNotSet.Yes && nonResidentialPlacementDiIds.ContainsKey(diRealityObj.RealObjId))
                    {
                        tempCompletePositionsCount = 1;
                    }
                }

                percent = tempPositionsCount != 0 ? (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2) : (decimal?)null;

                this.RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "NonResidentialPlacementPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion Сведения об использовании нежилых помещений

                #region Сведения об использовании мест общего пользования

                tempPositionsCount = 1;
                tempCompletePositionsCount = 0;

                if (diRealityObj.PlaceGeneralUse == YesNoNotSet.No)
                {
                    tempPositionsCount--;
                }
                else
                {
                    if (diRealityObj.PlaceGeneralUse == YesNoNotSet.Yes && infoAboutUseCommonFacilitiesDiIds.ContainsKey(diRealityObj.RealObjId))
                    {
                        tempCompletePositionsCount++;
                    }
                }

                percent = tempPositionsCount != 0 ? (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2) : (decimal?)null;

                this.RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "PlaceGeneralUsePercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;
                #endregion Сведения об использовании мест общего пользования

                #region Услуги
                this.TempCompleteCodes.Clear();
                this.TempNullCodes.Clear();

                tempPositionsCount = this.TemplateServiceDomain.GetAll()
                    .Count(x => x.IsMandatory &&
                        (x.ActualYearStart.HasValue && diRealityObj.PeriodDi.DateStart.Value.Year >= x.ActualYearStart.Value || !x.ActualYearStart.HasValue)
                        && (x.ActualYearEnd.HasValue && diRealityObj.PeriodDi.DateStart.Value.Year <= x.ActualYearEnd.Value || !x.ActualYearEnd.HasValue));

                tempCompletePositionsCount = 0;
                percent = 0;

                if (serv7RealObjs.ContainsKey(diRealityObj.RealObjId))
                {
                    this.TempNullCodes.Add(7);
                }

                if (diRealityObj.NumberLifts == 0)
                {
                    this.TempNullCodes.Add(27);
                    this.TempNullCodes.Add(28);
                }

                if (services.ContainsKey(diRealityObj.Id))
                {
                    var servs = this.CheckRequiredServices(services[diRealityObj.Id], housingServices, communalServices, repairServices, capRepairServices, serv7RealObjs, diRealityObj.NumberLifts);

                    foreach (var service in servs)
                    {
                        var servPositionsCount = 1;
                        var servCompletePositionsCount = 1;
                        decimal servPercent;

                        servPositionsCount++;
                        if (tariffForConsumers.ContainsKey(service.Id))
                        {
                            servCompletePositionsCount++;
                        }

                        if (service.KindServiceDi == KindServiceDi.Communal)
                        {
                            servPositionsCount++;
                            if (tariffForRsoItemsDiIds.ContainsKey(service.Id))
                            {
                                servCompletePositionsCount++;
                            }
                        }

                        if (service.KindServiceDi == KindServiceDi.Repair)
                        {
                            servPositionsCount++;
                            if (repairServiceIsDisclosedDict.ContainsKey(service.Id))
                            {
                                servCompletePositionsCount++;
                            }
                        }

                        if (service.KindServiceDi == KindServiceDi.Housing
                            || service.KindServiceDi == KindServiceDi.Managing || service.KindServiceDi == KindServiceDi.CapitalRepair)
                        {
                            servPercent = servCompletePositionsCount == 2 ? 100 : 50;
                        }
                        else
                        {
                            servPercent = 30 + (servCompletePositionsCount - 1) * 35;
                        }

                        if (servPercent == 100)
                        {
                            this.TempCompleteCodes.Add(service.Code);
                        }

                        this.ServicePercents.Add(
                            new ServicePercent
                                {
                                    Code = "ServicePercent",
                                    TypeEntityPercCalc = TypeEntityPercCalc.Service,
                                    Percent = servPercent,
                                    CalcDate = DateTime.Now.Date,
                                    PositionsCount = servPositionsCount,
                                    CompletePositionsCount = servCompletePositionsCount,
                                    ActualVersion = 1,
                                    Service = new BaseService { Id = service.Id }
                                });
                    }
                }

                var nullCodes = this.TempNullCodes.Where(x => !this.TempCompleteCodes.Contains(x)).ToList();
                tempPositionsCount -= nullCodes.Count();
                tempCompletePositionsCount += this.TempCompleteCodes.Count();

                percent = decimal.Divide(tempCompletePositionsCount, tempPositionsCount).RoundDecimal(2) * 100;

                this.RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "ServicesPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion Услуги

                percent = (decimal.Divide(CompletePositionsCount, PositionsCount) * 100).RoundDecimal(2);
                this.RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "DiRealObjPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = PositionsCount,
                    CompletePositionsCount = CompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                this.DictRoPerc.Add(
                    diRealityObj.RealObjId, new PercCalcResult
                                                    {
                                                        Percent = percent.ToDecimal(),
                                                        CompletePositionCount = CompletePositionsCount,
                                                        PositionCount = PositionsCount
                                                    });
            }
        }

        protected virtual Dictionary<long, decimal> GetPlanReductionExpensePercentDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            var planReductionExpenseByDiRealObj = this.PlanReductionExpenseWorksDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.PlanReductionExpense.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.PlanReductionExpense.DisclosureInfoRealityObj.Id)
                .Select(x => new { x.Key, count = x.Select(y => y.PlanReductionExpense.Id).Distinct().Count() })
                .ToDictionary(x => x.Key, x => x.count);

            var allPlanReductionExpenseByDiRealObj = this.PlanReductionExpenseDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .Select(x => new { x.Key, count = x.Count() })
                .ToDictionary(x => x.Key, x => x.count);

            var result = allPlanReductionExpenseByDiRealObj
                .Where(x => planReductionExpenseByDiRealObj.ContainsKey(x.Key))
                .Select(x => new { x.Key, all = x.Value, unique = planReductionExpenseByDiRealObj[x.Key] })
                .ToDictionary(x => x.Key, x => x.all != 0 ? decimal.Divide(x.unique, x.all) * 100 : 0);

            return result;
        }

        protected virtual Dictionary<long, decimal> GetPlanWorkServicePercentDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            var planWorkServiceByDiRealObj = this.PlanWorkServiceRepairWorksDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.PlanWorkServiceRepair.DisclosureInfoRealityObj.Id))
                .Where(x => x.DateStart.HasValue && x.DateEnd.HasValue && x.Cost.HasValue)
                .GroupBy(x => x.PlanWorkServiceRepair.DisclosureInfoRealityObj.Id)
                .Select(x => new { x.Key, count = x.Select(y => y.PlanWorkServiceRepair.Id).Distinct().Count() })
                .ToDictionary(x => x.Key, x => x.count);

            var allPlanWorkServiceByDiRealObj = this.PlanWorkServiceRepairDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .Select(x => new { x.Key, count = x.Count() })
                .ToDictionary(x => x.Key, x => x.count);

            var result = allPlanWorkServiceByDiRealObj
                .Where(x => planWorkServiceByDiRealObj.ContainsKey(x.Key))
                .Select(x => new { x.Key, all = x.Value, unique = planWorkServiceByDiRealObj[x.Key] })
                .ToDictionary(x => x.Key, x => x.all != 0 ? decimal.Divide(x.unique, x.all) * 100 : 0);

            return result;
        }

        protected virtual Dictionary<long, long> GetRepairServiceIsDisclosedDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            var serviceIsDisclosed = this.WorkRepairListDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => x.BaseService.Id)
                .Distinct()
                .ToDictionary(x => x);

            return serviceIsDisclosed;
        }

        protected virtual Dictionary<long, long> GetHousingServicesToExcludeDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            // Список на исключение из подсчета:
            //      Тип предоставления услуги = Собственники отказались, и загружен протокол отказа
            //      или Если Услуга предоставляется без участия УО и указан поставщик

            var housingServicesToExcludeDict = this.HousingServiceDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => (x.TypeOfProvisionService == TypeOfProvisionServiceDi.OwnersRefused && x.Protocol != null)
                        || (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo && x.Provider != null))
                .Select(x => x.Id)
                .Distinct()
                .ToDictionary(x => x);

            return housingServicesToExcludeDict;
        }

        protected virtual Dictionary<long, long> GetCommunalServicesToExcludeDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            // Список на исключение из подсчета:
            //    тип предоставления услуги = Услуга не предоставляется,
            //    или Услуга предоставляется без участия УО + указан поставщик

            var communalServicesToExcludeDict = this.CommunalServiceDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable
                            || (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo && x.Provider != null))
                .Select(x => x.Id)
                .Distinct()
                .ToDictionary(x => x);

            return communalServicesToExcludeDict;
        }

        protected virtual Dictionary<long, long> GetRepairServicesToExcludeDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            // Список на исключение из подсчета:
            //    тип предоставления услуги = Услуга не предоставляется,
            //    или Услуга предоставляется без участия УО + указан поставщик

            var repairServicesToExcludeDict = this.RepairServiceDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable
                    || (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo && x.Provider != null))
                .Select(x => x.Id)
                .Distinct()
                .ToDictionary(x => x);

            return repairServicesToExcludeDict;
        }

        protected virtual Dictionary<long, long> GetCapRepairServicesToExcludeDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            // Список на исключение из подсчета:
            //    тип предоставления услуги = Услуга не предоставляется,
            //    или Услуга предоставляется без участия УО + указан поставщик

            var capRepairServicesToExcludeDict = this.CapRepairServiceDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable
                            || (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo && x.Provider != null))
                .Select(x => x.Id)
                .Distinct()
                .ToDictionary(x => x);

            return capRepairServicesToExcludeDict;
        }

        protected List<ServiceProxy> CheckRequiredServices(
          IEnumerable<ServiceProxy> services,
          Dictionary<long, long> housingServices,
          Dictionary<long, long> communalServices,
          Dictionary<long, long> repairServices,
          Dictionary<long, long> capRepairServices,
          Dictionary<long, long> serv7RealObjs,
          long? numberLifts)
        {

            var nullServiceIds = new List<long>();
            foreach (var service in services)
            {
                // Если услуга = Жилищная
                // Если исключен из подсчета, то не учитывать.
                // Если в доме в поле Количество лифтов = О, не учитывать услуги
                if (service.KindServiceDi == KindServiceDi.Housing)
                {
                    if (housingServices.ContainsKey(service.Id)
                        || (this.liftServCodes.Contains(service.Code) && numberLifts == 0) || (service.Code == 7 && serv7RealObjs.ContainsKey(service.RealObjId)))
                    {
                        this.AddServiceNullPercent(service.Id);
                        this.TempNullCodes.Add(service.Code);
                        nullServiceIds.Add(service.Id);
                        continue;
                    }
                }

                // Если услуга = Коммунальная
                // Если исключен из подсчета, то не учитывать.
                if (service.KindServiceDi == KindServiceDi.Communal)
                {
                    if (communalServices.ContainsKey(service.Id))
                    {
                        this.AddServiceNullPercent(service.Id);
                        this.TempNullCodes.Add(service.Code);
                        nullServiceIds.Add(service.Id);
                        continue;
                    }
                }

                // Если услуга = Ремонт
                // Если исключен из подсчета, то не учитывать.
                if (service.KindServiceDi == KindServiceDi.Repair)
                {
                    if (repairServices.ContainsKey(service.Id))
                    {
                        this.AddServiceNullPercent(service.Id);
                        this.TempNullCodes.Add(service.Code);
                        nullServiceIds.Add(service.Id);
                        continue;
                    }
                }

                // Если услуга = Кап.ремонт
                // Если исключен из подсчета, то не учитывать.
                if (service.KindServiceDi == KindServiceDi.CapitalRepair)
                {
                    if (capRepairServices.ContainsKey(service.Id))
                    {
                        this.AddServiceNullPercent(service.Id);
                        this.TempNullCodes.Add(service.Code);
                        nullServiceIds.Add(service.Id);
                    }
                }
            }

            return services.Where(x => !nullServiceIds.Contains(x.Id)).ToList();
        }
    }
}