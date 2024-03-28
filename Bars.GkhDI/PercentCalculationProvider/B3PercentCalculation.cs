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

    public class B3PercentCalculation : BasePercentCalculation
    {
        private List<int> mandatoryServiceCodes = new List<int> { 1, 2, 6, 7, 13, 14, 27, 28, 8, 9, 11, 12, 18, 17, 20, 22 };

        public override bool CheckByPeriod(PeriodDi periodDi)
        {
            return periodDi.DateStart < new DateTime(2013, 1, 1);
        }

        protected override Dictionary<long, PercCalcResult> CalculateManOrgsInfo(IEnumerable<DisclosureInfo> disInfos, IQueryable<DisclosureInfo> diQuery)
        {
            var dictMoPerc = new Dictionary<long, PercCalcResult>();

            var adminRespDisInfoIds =
                Container.Resolve<IDomainService<AdminResp>>()
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .Select(x => x.DisclosureInfo.Id)
                         .Distinct()
                         .ToDictionary(x => x);

            var hasFundsInfoDisInfos =
                Container.Resolve<IDomainService<FundsInfo>>()
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .Select(x => x.DisclosureInfo.Id)
                         .Distinct()
                         .ToDictionary(x => x);

            var documentsDisInfo =
                this.Container.Resolve<IDomainService<Documents>>()
                    .GetAll()
                    .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                    .ToDictionary(
                        x => x.DisclosureInfo.Id,
                        y => new
                            {
                                y.NotAvailable,
                                FileProjectContract = y.FileProjectContract != null
                            });

            var finActivityDisInfo = Container.Resolve<IDomainService<FinActivity>>()
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .ToDictionary(
                             x => x.DisclosureInfo.Id,
                             y => new { y.TaxSystem, MoTypeManagement = y.DisclosureInfo.ManagingOrganization.TypeManagement });

            var finActivityAuditYearsDi =
                Container.Resolve<IDomainService<FinActivityAudit>>()
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                         .GroupBy(y => y.ManagingOrganization.Id)
                         .ToDictionary(
                             y => y.Key, x => x.Select(z => new { z.Year, z.TypeAuditStateDi, z.File }).ToList());

            var finActivityDocsDisInfo =
                Container.Resolve<IDomainService<FinActivityDocs>>()
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .ToDictionary(x => x.DisclosureInfo.Id, y => new
                                                                          {
                                                                              y.BookkepingBalance,
                                                                              y.BookkepingBalanceAnnex
                                                                          });

            var finActivityDocsByYearDi =
                Container.Resolve<IDomainService<FinActivityDocByYear>>()
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                         .GroupBy(y => y.ManagingOrganization.Id)
                         .ToDictionary(y => y.Key, x => x.Select(y => new
                                                                          {
                                                                              y.Year,
                                                                              y.TypeDocByYearDi
                                                                          }));

            var finActManorgRealityObjs =
                Container.Resolve<IDomainService<FinActivityManagRealityObj>>()
                         .GetAll()
                         .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                         .GroupBy(x => x.DisclosureInfo.Id)
                         .ToDictionary(
                             y => y.Key,
                             x =>
                             x.Where(y => y.PresentedToRepay != null && y.ReceivedProvidedService != null).Select(z => z.RealityObject.Id).Distinct()
                              .ToArray());

            var workModeService = Container.Resolve<IDomainService<ManagingOrgWorkMode>>().GetAll().Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                .GroupBy(x => x.ManagingOrganization.Id)
                .ToDictionary(x => x.Key, y => new
                {
                    DispatcherWork = y.Any(z => z.TypeMode == TypeMode.DispatcherWork),
                    ReceptionCitizens = y.Any(z => z.TypeMode == TypeMode.ReceptionCitizens),
                    WorkMode = y.Any(z => z.TypeMode == TypeMode.WorkMode)
                });

            var hasManOrgMembershipMoIds = Container.Resolve<IDomainService<ManagingOrgMembership>>()
                    .GetAll()
                    .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                    .Where(
                        x =>
                        (x.DateStart.Value >= period.DateStart.Value && period.DateEnd.Value >= x.DateStart.Value)
                        || (period.DateStart.Value >= x.DateStart.Value
                            && ((x.DateEnd.HasValue && x.DateEnd.Value >= period.DateStart.Value) || !x.DateEnd.HasValue)))
                    .Select(x => x.ManagingOrganization.Id)
                    .ToDictionary(x => x);

            var hasManOrgrealObjMoIds =
                Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                         .GetAll()
                         .Where(x =>
                             x.ManOrgContract.EndDate >= period.DateStart && x.ManOrgContract.EndDate < period.DateEnd
                             && diQuery.Any(y => y.ManagingOrganization.Id == x.ManOrgContract.ManagingOrganization.Id))
                         .Select(x => x.ManOrgContract.ManagingOrganization.Id).ToDictionary(x => x);

            var hasInfoOnContrIds =
                Container.Resolve<IDomainService<InformationOnContracts>>()
                                  .GetAll()
                                  .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.DisclosureInfo.ManagingOrganization.Id) &&
                                      (((x.DateStart.HasValue && x.DateStart >= period.DateStart.Value || !x.DateStart.HasValue)
                                             && (x.DateStart.HasValue && period.DateEnd.Value >= x.DateStart))
                                             || ((x.DateStart.HasValue && period.DateStart.Value >= x.DateStart || !x.DateStart.HasValue)
                                             && (x.DateEnd.HasValue && x.DateEnd >= period.DateStart.Value || !x.DateEnd.HasValue || x.DateEnd <= DateTime.MinValue))))
                                  .Select(x => x.DisclosureInfo.ManagingOrganization.Id)
                                  .Distinct()
                                  .ToDictionary(x => x);

            var periodYear = 0;
            if (period.DateStart != null)
            {
                periodYear = period.DateStart.Value.Year;
            }

            foreach (var disclosureInfo in disInfos)
            {
                var positionsCount = 0;
                var completePositionsCount = 0;
                var tempPositionsCount = 0;
                var tempCompletePositionsCount = 0;
                decimal? percent = null;

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

                DisInfoPercents.Add(
                    new DisclosureInfoPercent
                        {
                            Code = "AdminResponsibilityPercent",
                            TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                            Percent = percent,
                            CalcDate = DateTime.Now.Date,
                            PositionsCount = tempPositionsCount,
                            CompletePositionsCount = tempCompletePositionsCount,
                            ActualVersion = 2,
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

                DisInfoPercents.Add(
                    new DisclosureInfoPercent
                        {
                            Code = "DocumentsPercent",
                            TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                            Percent = percent,
                            CalcDate = DateTime.Now.Date,
                            PositionsCount = tempPositionsCount,
                            CompletePositionsCount = tempCompletePositionsCount,
                            ActualVersion = 2,
                            DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                        });

                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;

                #endregion Документы

                #region Финансовая деятельность

                percent = 0;
                tempPositionsCount = 12;
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

                    if (finActivityDocsDisInfo.ContainsKey(disclosureInfo.Id))
                    {
                        if (finActivityDocsDisInfo[disclosureInfo.Id].BookkepingBalance != null)
                        {
                            tempCompletePositionsCount++;
                            tempPositionsCount++;
                        }

                        if (finActivityDocsDisInfo[disclosureInfo.Id].BookkepingBalanceAnnex != null)
                        {
                            tempCompletePositionsCount++;
                        }
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

                    if ((!RealObjByDi.ContainsKey(disclosureInfo.Id) || RealObjByDi[disclosureInfo.Id].Length == 0) || (finActManorgRealityObjs.ContainsKey(disclosureInfo.Id)
                        && RealObjByDi[disclosureInfo.Id].Length == finActManorgRealityObjs[disclosureInfo.Id].Where(x => RealObjByDi[disclosureInfo.Id].Contains(x)).Distinct().Count()))
                    {
                        tempCompletePositionsCount++;
                    }

                    percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);
                }

                DisInfoPercents.Add(
                    new DisclosureInfoPercent
                        {
                            Code = "FinActivityPercent",
                            TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                            Percent = percent,
                            CalcDate = DateTime.Now.Date,
                            PositionsCount = tempPositionsCount,
                            CompletePositionsCount = tempCompletePositionsCount,
                            ActualVersion = 2,
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

                DisInfoPercents.Add(new DisclosureInfoPercent
                                                 {
                                                     Code = "FundsInfoPercent",
                                                     TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                                                     Percent = percent,
                                                     CalcDate = DateTime.Now.Date,
                                                     PositionsCount = tempPositionsCount,
                                                     CompletePositionsCount = tempCompletePositionsCount,
                                                     ActualVersion = 2,
                                                     DisclosureInfo = new DisclosureInfo { Id = disclosureInfo.Id }
                                                 });
                positionsCount += tempPositionsCount;
                completePositionsCount += tempCompletePositionsCount;

                #endregion Сведения о фондах

                #region Общие сведения

                tempPositionsCount = 0;
                tempCompletePositionsCount = 0;
                var disclosureInfoService = this.Container.Resolve<IDisclosureInfoService>();

                // ФИО руководителя
                tempPositionsCount++;
                if (!string.IsNullOrEmpty(disclosureInfoService.GetPositionByCode(
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
                    if (!string.IsNullOrEmpty(disclosureInfoService.GetPositionByCode(disclosureInfo.ManagingOrganization.Contragent.Id, disclosureInfo.PeriodDi, new List<string> { "5" })))
                    {
                        tempCompletePositionsCount++;
                    }

                    // Члены ревизионной комисии
                    tempPositionsCount++;
                    if (!string.IsNullOrEmpty(disclosureInfoService.GetPositionByCode(disclosureInfo.ManagingOrganization.Contragent.Id, disclosureInfo.PeriodDi, new List<string> { "6" })))
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

                DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "GeneralDataPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
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

                DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "MembershipUnionsPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
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

                DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "TerminateContractPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
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

                DisInfoPercents.Add(new DisclosureInfoPercent
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

                DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "ManOrgInfoPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = positionsCount,
                    CompletePositionsCount = completePositionsCount,
                    ActualVersion = 2,
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
            var diRoQuery = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>()
                .GetAll().Where(x =>
                    moRoQuery.Any(y =>
                        y.RealityObject.Id == x.RealityObject.Id && y.ManOrgContract.ManagingOrganization.Id == x.ManagingOrganization.Id) &&
                    x.PeriodDi.Id == period.Id);

            var diRealObjs = diRoQuery
                        .Select(x => new
                        {
                            x.Id,
                            x.ReductionPayment,
                            RealObjId = x.RealityObject.Id,
                            x.PlaceGeneralUse,
                            x.NonResidentialPlacement,
                            x.RealityObject.NumberLifts
                        }).ToList();

            var planReductionExpenseDiIds = Container.Resolve<IDomainService<PlanReductionExpense>>().GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id)).Select(x => x.DisclosureInfoRealityObj.Id).Distinct().ToDictionary(x => x);

            var infoAboutReductionPaymentDiIds = Container.Resolve<IDomainService<InfoAboutReductionPayment>>().GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id)).Select(x => x.DisclosureInfoRealityObj.Id).Distinct().ToDictionary(x => x);

            var planWorkServiceRepairDiIds = Container.Resolve<IDomainService<PlanWorkServiceRepair>>().GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id)).Select(x => x.DisclosureInfoRealityObj.Id).Distinct().ToDictionary(x => x);

            var nonResidentialPlacementDiIds = Container.Resolve<IDomainService<NonResidentialPlacement>>()
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id)
                                && (((x.DateStart.HasValue && period.DateStart.HasValue && (x.DateStart.Value >= period.DateStart.Value) || !period.DateStart.HasValue)
                                        && (period.DateEnd.HasValue && x.DateStart.HasValue && (period.DateEnd.Value >= x.DateStart.Value) || !period.DateEnd.HasValue))
                                        || ((x.DateStart.HasValue && period.DateStart.HasValue && (period.DateStart.Value >= x.DateStart.Value) || !x.DateStart.HasValue)
                                            && (x.DateEnd.HasValue && period.DateStart.HasValue
                                                && (x.DateEnd.Value >= period.DateStart.Value) || !x.DateEnd.HasValue))))
                .Select(x => x.DisclosureInfoRealityObj.Id)
                .Distinct()
                .ToDictionary(x => x);

            var infoAboutUseCommonFacilitiesDiIds = Container.Resolve<IDomainService<InfoAboutUseCommonFacilities>>()
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id)
                                && (((x.DateStart.HasValue && period.DateStart.HasValue && (x.DateStart.Value >= period.DateStart.Value) || !period.DateStart.HasValue)
                                        && (period.DateEnd.HasValue && x.DateStart.HasValue && (period.DateEnd.Value >= x.DateStart.Value) || !period.DateEnd.HasValue))
                                        || ((x.DateStart.HasValue && period.DateStart.HasValue && (period.DateStart.Value >= x.DateStart.Value) || !x.DateStart.HasValue)
                                            && (x.DateEnd.HasValue && period.DateStart.HasValue
                                                && (x.DateEnd.Value >= period.DateStart.Value) || !x.DateEnd.HasValue))))
                .Select(x => x.DisclosureInfoRealityObj.Id)
                .Distinct()
                .ToDictionary(x => x);

            var documentRealObjs = Container.Resolve<IDomainService<DocumentsRealityObj>>().GetAll()
                 .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                 .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                 .ToDictionary(
                     x => x.Key,
                     y => y.Select(z => new { FileActState = z.FileActState != null, FileCatalogRepair = z.FileCatalogRepair != null, FileReportPlanRepair = z.FileReportPlanRepair != null }).FirstOrDefault());

            var periodYear = 0;
            if (period.DateStart != null)
            {
                periodYear = period.DateStart.Value.Year;
            }

            var docRealObjProtocols = Container.Resolve<IDomainService<DocumentsRealityObjProtocol>>().GetAll()
                                         .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                                         .GroupBy(x => x.Id)
                                         .ToDictionary(x => x.Key, y => new
                                         {
                                             CurrentYear = y.Any(x => x.Year == periodYear),
                                             PreviosYear = y.Any(x => x.Year == periodYear - 1)
                                         });

            var services = Container.Resolve<IDomainService<BaseService>>().GetAll()
                           .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                           .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                                    .ToDictionary(x => x.Key, y => y.Select(x => new ServiceProxy
                                    {
                                        Id = x.Id,
                                        Code = x.TemplateService.Code.ToInt(),
                                        KindServiceDi = x.TemplateService.KindServiceDi,
                                        DiRealObjId = x.DisclosureInfoRealityObj.Id,
                                        RealObjId = x.DisclosureInfoRealityObj.RealityObject.Id
                                    }).Where(x => mandatoryServiceCodes.Contains(x.Code)));

            var housingServices = Container.Resolve<IDomainService<HousingService>>()
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id) &&
                    (x.TypeOfProvisionService == TypeOfProvisionServiceDi.OwnersRefused && x.Protocol != null)
                        || (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo && x.Provider != null))
                .Select(x => x.Id).ToDictionary(x => x);

            var communalServices = Container.Resolve<IDomainService<CommunalService>>()
                .GetAll().Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id) &&
                    (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable
                            || (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo && x.Provider != null)))
                .Select(x => x.Id).ToDictionary(x => x);

            var repairServices = Container.Resolve<IDomainService<RepairService>>()
                .GetAll().Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id) &&
                    (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable
                            || (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo && x.Provider != null)))
                .Select(x => x.Id).ToDictionary(x => x);

            var serv7RealObjs = Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id) && x.FormCode == "Form_3_7" && x.CellCode == "1:3" && x.Value == "0")
                .Select(x => x.TehPassport.RealityObject.Id).Distinct()
                .ToDictionary(x => x);

            var tariffForConsumers = Container.Resolve<IDomainService<TariffForConsumers>>().GetAll().Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id)).Select(x => x.BaseService.Id).Distinct().ToDictionary(x => x);
            var tariffForRsoItemsDiIds = Container.Resolve<IDomainService<TariffForRso>>().GetAll().Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id)).Select(x => x.BaseService.Id).Distinct().ToDictionary(x => x);

            foreach (var diRealityObj in diRealObjs)
            {
                var PositionsCount = 0;
                var CompletePositionsCount = 0;
                var tempPositionsCount = 0;
                var tempCompletePositionsCount = 0;
                decimal? percent;

                #region План мер по снижению расходов

                percent = planReductionExpenseDiIds.ContainsKey(diRealityObj.Id) ? 100 : 0;
                tempPositionsCount = 1;
                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                RealObjPercents.Add(new DiRealObjPercent
                                                {
                                                    Code = "PlanReductionExpensePercent",
                                                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                                                    Percent = percent,
                                                    CalcDate = DateTime.Now.Date,
                                                    PositionsCount = tempPositionsCount,
                                                    CompletePositionsCount = tempCompletePositionsCount,
                                                    ActualVersion = 2,
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
                    var hasInfoAboutReductionPayment = infoAboutReductionPaymentDiIds.ContainsKey(diRealityObj.Id);

                    percent = hasInfoAboutReductionPayment ? 100 : 0;
                }

                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "InfoAboutReductionPaymentPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion Сведения о случаях снижения платы

                #region Документы

                tempPositionsCount = 1;
                tempCompletePositionsCount = 0;

                if (documentRealObjs.ContainsKey(diRealityObj.Id) && (documentRealObjs[diRealityObj.Id].FileActState 
                    || documentRealObjs[diRealityObj.Id].FileCatalogRepair || documentRealObjs[diRealityObj.Id].FileReportPlanRepair
                    || docRealObjProtocols.ContainsKey(diRealityObj.Id) && (docRealObjProtocols[diRealityObj.RealObjId].CurrentYear || docRealObjProtocols[diRealityObj.Id].PreviosYear)))
                {
                    tempCompletePositionsCount++;
                }

                percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "DocumentsDiRealObjPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;
                #endregion Документы

                #region План работ по содержанию и ремонту

                percent = planWorkServiceRepairDiIds.ContainsKey(diRealityObj.Id) ? 100 : 0;
                tempPositionsCount = 1;
                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "PlanWorkServiceRepairPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
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
                    if (diRealityObj.NonResidentialPlacement == YesNoNotSet.Yes && nonResidentialPlacementDiIds.ContainsKey(diRealityObj.Id))
                    {
                        tempCompletePositionsCount = 1;
                    }
                }

                percent = tempPositionsCount != 0 ? (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2) : (decimal?)null;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "NonResidentialPlacementPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
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
                    if (diRealityObj.PlaceGeneralUse == YesNoNotSet.Yes && infoAboutUseCommonFacilitiesDiIds.ContainsKey(diRealityObj.Id))
                    {
                        tempCompletePositionsCount++;
                    }
                }

                percent = tempPositionsCount != 0 ? (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2) : (decimal?)null;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "PlaceGeneralUsePercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;
                #endregion Сведения об использовании мест общего пользования

                #region Услуги

                TempCompleteCodes.Clear();
                TempNullCodes.Clear();

                tempPositionsCount = mandatoryServiceCodes.Count - 1;
                tempCompletePositionsCount = 0;
                percent = 0;

                if (serv7RealObjs.ContainsKey(diRealityObj.RealObjId))
                {
                    TempNullCodes.Add(7);
                }

                if (diRealityObj.NumberLifts == 0)
                {
                    TempNullCodes.Add(27);
                    TempNullCodes.Add(28);
                }

                if (services.ContainsKey(diRealityObj.Id))
                {
                    var servs = CheckRequiredServices(services[diRealityObj.Id], housingServices, communalServices, repairServices, serv7RealObjs, diRealityObj.NumberLifts);

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

                        if (service.KindServiceDi == KindServiceDi.Communal)
                        {
                            servPercent = 30 + (servCompletePositionsCount - 1) * 35; 
                        }
                        else
                        {
                            servPercent = servCompletePositionsCount == 2 ? 100 : 50;
                        }

                        if (servPercent == 100)
                        {
                            TempCompleteCodes.Add(service.Code);
                        }

                        ServicePercents.Add(
                            new ServicePercent
                                {
                                    Code = "ServicePercent",
                                    TypeEntityPercCalc = TypeEntityPercCalc.Service,
                                    Percent = servPercent,
                                    CalcDate = DateTime.Now.Date,
                                    PositionsCount = servPositionsCount,
                                    CompletePositionsCount = servCompletePositionsCount,
                                    ActualVersion = 2,
                                    Service = new BaseService { Id = service.Id, DisclosureInfoRealityObj = new DisclosureInfoRealityObj { Id = diRealityObj.Id } }
                                });
                    }

                    if (TempCompleteCodes.Contains(27) || TempCompleteCodes.Contains(28))
                    {
                        liftServCodes.AddTo(TempCompleteCodes);
                    }
                }

                var nullCodes = TempNullCodes.Where(x => !TempCompleteCodes.Contains(x)).ToList();
                tempPositionsCount -= liftServCodes.All(nullCodes.Contains) ? nullCodes.Count() - 1 : nullCodes.Count();
                tempCompletePositionsCount += liftServCodes.All(TempCompleteCodes.Contains) ? TempCompleteCodes.Count() - 1 : TempCompleteCodes.Count();

                percent = decimal.Divide(tempCompletePositionsCount, tempPositionsCount).RoundDecimal(2) * 100;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "ServicesPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 2,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion Услуги

                percent = (decimal.Divide(CompletePositionsCount, PositionsCount) * 100).RoundDecimal(2);
                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "DiRealObjPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = PositionsCount,
                    CompletePositionsCount = CompletePositionsCount,
                    ActualVersion = 2,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                DictRoPerc.Add(diRealityObj.RealObjId, new PercCalcResult
                {
                    Percent = percent.ToDecimal(),
                    CompletePositionCount = CompletePositionsCount,
                    PositionCount = PositionsCount
                });
            }
        }

        private List<ServiceProxy> CheckRequiredServices(
            IEnumerable<ServiceProxy> services,
            Dictionary<long, long> housingServices,
            Dictionary<long, long> communalServices,
            Dictionary<long, long> repairServices,
            Dictionary<long, long> serv7RealObjs,
            int? numberLifts)
        {

            var nullServiceIds = new List<long>();
            foreach (var service in services)
            {
                // Если услуга = Жилищная, в поле Тип предоставления услуги = Собственники отказались, и загружен протокол отказа, то не учитывать. 
                // Если Услуга предоставляется без участия УО и указан поставщик, то не учитывать.
                // Если в доме в поле Количество лифтов = О, не учитывать услуги
                if (service.KindServiceDi == KindServiceDi.Housing)
                {
                    if (housingServices.ContainsKey(service.Id)
                        || (liftServCodes.Contains(service.Code) && numberLifts == 0) || (service.Code == 7 && serv7RealObjs.ContainsKey(service.RealObjId)))
                    {
                        AddServiceNullPercent(service.Id);
                        TempNullCodes.Add(service.Code);
                        nullServiceIds.Add(service.Id);
                        continue;
                    }
                }

                // Если услуга = Коммунальная, тип предоставления услуги = Услуга не предоставляется,
                // или Услуга предоставляется без участия УО+указан поставщик, то не учитывать
                if (service.KindServiceDi == KindServiceDi.Communal)
                {
                    if (communalServices.ContainsKey(service.Id))
                    {
                        AddServiceNullPercent(service.Id);
                        TempNullCodes.Add(service.Code);
                        nullServiceIds.Add(service.Id);
                        continue;
                    }
                }

                // Если услуга = Ремонт, тип предоставления услуги = Услуга не предоставляется,
                // или Услуга предоставляется без участия УО+указан поставщик, то не учитывать
                if (service.KindServiceDi == KindServiceDi.Repair)
                {
                    if (repairServices.ContainsKey(service.Id))
                    {
                        AddServiceNullPercent(service.Id);
                        TempNullCodes.Add(service.Code);
                        nullServiceIds.Add(service.Id);
                    }
                }
            }

            return services.Where(x => !nullServiceIds.Contains(x.Id)).ToList();
        }
    }
}