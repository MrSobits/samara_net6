namespace Bars.GkhDi.Regions.Tatarstan.PercentCalculationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    /// <summary>
    /// Реализация калькулятора Раскрытия информации по форме 988 для Татарстана
    /// </summary>
    public class PercentCalculation988 : GkhDi.PercentCalculationProvider.PercentCalculation988
    {
        /// <summary>
        /// Домен-серв <see cref="WorkRepairListTat"/>
        /// </summary>
        public IDomainService<WorkRepairListTat> WorkRepairListTatDomain { get; set; }

        protected HashSet<long> HasFundsInfoDisInfos;
        protected HashSet<long> InformationOnContracts;
        protected HashSet<long> HasAnyWorkRepairListTat;
        protected HashSet<long> RepairWorkHasAnyTarrifDict;
        protected HashSet<long> HasWorkRepairTechServ;
        
        /// <summary>
        /// Дома, у которых в разделе 3.9 Мусоропроводы в поле Мусоропроводы стоит значение "Нет", не учитываются
        /// </summary>
        protected HashSet<long> RoServ7ToExclude;

        protected Dictionary<long, Documents> DocumentsDisInfo;
        protected Dictionary<long, IEnumerable<PlanWorkServiceRepairWorks>> PlanWorkServiceRepairWorksDict;
        protected Dictionary<long, IEnumerable<PlanWorkServiceRepair>> PlanWorkServiceRepairDict;
        protected Dictionary<long, RepairService> RepairServiceDict;
        protected Dictionary<long, bool> AnyWorkRepairTechServDict;

        protected Dictionary<long, CrFundFormationType> RealiTypeOfFormingCrDict;

        protected Dictionary<long, IEnumerable<long>> PlanReductionExpenseDict;
        protected Dictionary<long, bool> AnyInfoAboutReductionPaymentDict;
        protected Dictionary<long, bool> AnyNonResidentialPlacementDict;
        protected Dictionary<long, bool> AnyPlanReductionExpenseWorks;

        protected Dictionary<long, IEnumerable<TerminateContractProxy>> TerminateContractDict;
        protected Dictionary<long, IEnumerable<MembershipUnionsProxy>> MembershipUnionsDict;

        /// <inheritdoc />
        public override bool CheckByPeriod(PeriodDi periodDi)
        {
            return periodDi.DateStart >= new DateTime(2015, 1, 1) && periodDi.DateStart < new DateTime(2016, 01, 01);
        }

        /// <summary>
        /// Приготовить кэш для УО
        /// </summary>
        /// <param name="diQuery">УО в период раскрытия информации</param>
        protected override void PrepareManOrgCacheCache(IQueryable<DisclosureInfo> diQuery)
        {
            base.PrepareManOrgCacheCache(diQuery);

            this.HasFundsInfoDisInfos = this.FundsInfoDomain
                .GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .Select(x => x.DisclosureInfo.Id)
                .Distinct()
                .ToHashSet();

            this.InformationOnContracts = this.InformationOnContractsDomain
                .GetAll()
                .Where(
                    x => diQuery.Any(y => y.ManagingOrganization.Id == x.DisclosureInfo.ManagingOrganization.Id) &&
                        ((x.DateStart.HasValue && x.DateStart >= this.period.DateStart.Value || !x.DateStart.HasValue)
                        && x.DateStart.HasValue && this.period.DateEnd.Value >= x.DateStart
                        || (x.DateStart.HasValue && this.period.DateStart.Value >= x.DateStart || !x.DateStart.HasValue)
                            && (x.DateEnd.HasValue && x.DateEnd >= this.period.DateStart.Value || !x.DateEnd.HasValue || x.DateEnd <= DateTime.MinValue)))
                .Select(x => x.DisclosureInfo.ManagingOrganization.Id)
                .ToHashSet();

            this.TerminateContractDict = this.ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.ManOrgContract.EndDate >= this.period.DateStart.Value.AddYears(-1))
                .Where(x => x.ManOrgContract.EndDate < this.period.DateEnd.Value.AddYears(-1))
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManOrgContract.ManagingOrganization.Id))
                .AsEnumerable()
                .Select(x => new
                {
                    x.ManOrgContract.ManagingOrganization.Id,
                    Address = x.RealityObject.FiasAddress?.AddressName,
                    TerminateReason = x.ManOrgContract.TerminateReason
                })
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(z => new TerminateContractProxy
                    {
                        Address = z.Address,
                        TerminateReason = z.TerminateReason
                    }));

            this.MembershipUnionsDict = this.ManagingOrgMembershipDomain
                .GetAll()
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                .Where(
                    x =>
                        (x.DateStart.Value >= this.period.DateStart.Value && this.period.DateEnd.Value >= x.DateStart.Value)
                            || (this.period.DateStart.Value >= x.DateStart.Value
                                && (x.DateEnd.HasValue && x.DateEnd.Value >= this.period.DateStart.Value || !x.DateEnd.HasValue)))
                .AsEnumerable()
                .Select(x => new
                {
                    Id = x.ManagingOrganization.Id,
                    x.Name,
                    x.Address,
                    x.OfficialSite
                })
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(z => new MembershipUnionsProxy
                    {
                        Name = z.Name,
                        Address = z.Address,
                        OfficialSite = z.OfficialSite
                    }));

            this.DocumentsDisInfo = this.DocumentsDomain
                .GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .ToDictionary(x => x.DisclosureInfo.Id);
        }

        /// <summary>
        /// Приготовить кэш для домов
        /// </summary>
        /// <param name="diRoQuery">Дома в период раскрытия информации</param>
        protected override void PrepareRealityObjectCache(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            base.PrepareRealityObjectCache(diRoQuery);

            this.RoServ7ToExclude = this.TehPassportValueDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id)
                            && x.FormCode == "Form_3_7" && x.CellCode == "1:3" && x.Value == "0")
                .Select(x => x.TehPassport.RealityObject.Id).Distinct()
                .ToHashSet();

            this.RepairWorkHasAnyTarrifDict = this.TariffForConsumersDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => x.BaseService.Id)
                .ToHashSet();

            this.HasWorkRepairTechServ = this.WorkRepairTechServDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => x.BaseService.Id)
                .ToHashSet();

            this.HasAnyWorkRepairListTat = this.WorkRepairListTatDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => x.BaseService.Id)
                .ToHashSet();

            var planWorksQuery = this.PlanWorkServiceRepairDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id));

            this.RepairServiceDict = this.RepairServiceDomain.GetAll()
                .Where(x => planWorksQuery.Any(y => y.BaseService.Id == x.Id))
                .ToDictionary(x => x.Id);

            this.PlanWorkServiceRepairDict = planWorksQuery
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.AsEnumerable());

            this.PlanWorkServiceRepairWorksDict = this.PlanWorkServiceRepairWorksDomain.GetAll()
                .Where(x => planWorksQuery.Any(y => y.Id == x.PlanWorkServiceRepair.Id))
                .GroupBy(x => x.PlanWorkServiceRepair.Id)
                .ToDictionary(x => x.Key, y => y.AsEnumerable());

            this.AnyWorkRepairTechServDict = this.WorkRepairTechServDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(x => x.Key, y => y.Any());

            var planReduceQuery = this.PlanReductionExpenseDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id));

            this.PlanReductionExpenseDict = planReduceQuery
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id));

            this.AnyPlanReductionExpenseWorks = this.PlanReductionExpenseWorksDomain.GetAll()
                .Where(x => planReduceQuery.Any(y => y.Id == x.PlanReductionExpense.Id))
                .GroupBy(x => x.PlanReductionExpense.Id)
                .ToDictionary(x => x.Key, y => y.Any());

            this.AnyInfoAboutReductionPaymentDict = this.InfoAboutReductionPaymentDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, x => x.Any());

            this.AnyNonResidentialPlacementDict = this.NonResidentialPlacementDomain.GetAll()
                 .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                 .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                 .ToDictionary(x => x.Key, x => x.Any());

            this.RealiTypeOfFormingCrDict = this.TypeOfFormingCrProvider.GetTypeOfFormingCr(diRoQuery.Select(x => x.RealityObject));
        }

        /// <summary>
        /// Считает раскрытие информации по общим сведениям об УО (дополнение)
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateGeneralDataPercent(DisclosureInfo disclosureInfo)
        {
            var diPercent = base.CalculateGeneralDataPercent(disclosureInfo);

            int tempPositionsCount = 0,
                tempCompletePositionsCount = 0;

            var manOrg = disclosureInfo.ManagingOrganization;

            // Дата регистрации
            tempPositionsCount++;
            if (manOrg.Contragent.DateRegistration.HasValue)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Дата регистрации",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // Орган, принявший решение о регистрации
            tempPositionsCount++;
            if (manOrg.Contragent.OgrnRegistration.IsNotEmpty())
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Орган, принявший решение о регистрации",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // E-mail
            tempPositionsCount++;
            if (!string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.Email))
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Электронный адрес",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            tempPositionsCount++;
            // официальный сайт
            if (disclosureInfo.ManagingOrganization.Contragent.OfficialWebsite.IsNotEmpty())
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Официальный сайт",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ)
            {
                // Члены правления
                tempPositionsCount++;
                if (this.DisclosureInfoService.GetPositionByCode(
                        disclosureInfo.ManagingOrganization.Contragent.Id,
                        disclosureInfo.PeriodDi,
                        new List<string> { "5" }).IsNotEmpty())
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Члены правления",
                        PathId = DiFieldPathType.Path24,
                        ManOrg = disclosureInfo
                    });
                }

                // Члены ревизионной комисии
                tempPositionsCount++;
                if (this.DisclosureInfoService.GetPositionByCode(
                        disclosureInfo.ManagingOrganization.Contragent.Id,
                        disclosureInfo.PeriodDi,
                        new List<string> { "6" }).IsNotEmpty())
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Члены ревизионной комисии",
                        PathId = DiFieldPathType.Path24,
                        ManOrg = disclosureInfo
                    });
                }
            }

            diPercent.PositionsCount += tempPositionsCount;
            diPercent.CompletePositionsCount += tempCompletePositionsCount;
            diPercent.Percent = (decimal.Divide(diPercent.CompletePositionsCount, diPercent.PositionsCount) * 100).RoundDecimal(2);

            return diPercent;
        }

        /// <summary>
        /// Считает раскрытие информации по общим сведениям о расторгнутых договорах
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateTerminateContractPercent(DisclosureInfo disclosureInfo)
        {
            decimal? percent = null;
            int tempPositionsCount = 0;
            int tempCompletePositionsCount = 0;

            // Есть случаи расторжения договоров
            tempPositionsCount++;
            if (disclosureInfo.TerminateContract == YesNoNotSet.Yes)
            {
                tempCompletePositionsCount++;

                if (this.TerminateContractDict.ContainsKey(disclosureInfo.ManagingOrganization.Id))
                {
                    var terminateContractProxies = this.TerminateContractDict[disclosureInfo.ManagingOrganization.Id];
                    tempPositionsCount += 2 * terminateContractProxies.Count();
                    foreach (var terminateContractProxy in terminateContractProxies)
                    {
                        if (terminateContractProxy.Address.IsNotEmpty())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Адрес",
                                PathId = DiFieldPathType.Path29,
                                ManOrg = disclosureInfo
                            });
                        }

                        if (terminateContractProxy.TerminateReason.IsNotEmpty())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Основание расторжения",
                                PathId = DiFieldPathType.Path29,
                                ManOrg = disclosureInfo
                            });
                        }
                    }
                }
            }
            else if (disclosureInfo.TerminateContract == YesNoNotSet.No)
            {
                tempPositionsCount = 1;
                tempCompletePositionsCount = 1;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Есть случаи расторжения договоров",
                    PathId = DiFieldPathType.Path29,
                    ManOrg = disclosureInfo
                });
            }

            percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

            return new DisclosureInfoPercent
            {
                Code = "TerminateContractPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = tempPositionsCount,
                CompletePositionsCount = tempCompletePositionsCount,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            };
        }

        /// <summary>
        /// Считает раскрытие информации по сведениям о членстве в объединениях
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateMembershipUnionsPercent(DisclosureInfo disclosureInfo)
        {
            decimal? percent = null;
            int tempPositionsCount = 0;
            int tempCompletePositionsCount = 0;

            // В данном отчетном периоде организация состояла в объединениях
            tempPositionsCount++;
            if (disclosureInfo.MembershipUnions == YesNoNotSet.Yes)
            {
                tempCompletePositionsCount++;

                if (this.MembershipUnionsDict.ContainsKey(disclosureInfo.ManagingOrganization.Id))
                {
                    var membershipUnionsProxies = this.MembershipUnionsDict[disclosureInfo.ManagingOrganization.Id];
                    tempPositionsCount += 3 * membershipUnionsProxies.Count();
                    foreach (var membershipUnionsProxy in membershipUnionsProxies)
                    {
                        if (membershipUnionsProxy.Name.IsNotEmpty())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Наименование",
                                PathId = DiFieldPathType.Path35,
                                ManOrg = disclosureInfo
                            });
                        }

                        if (membershipUnionsProxy.Address.IsNotEmpty())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Адрес",
                                PathId = DiFieldPathType.Path35,
                                ManOrg = disclosureInfo
                            });
                        }

                        if (membershipUnionsProxy.OfficialSite.IsNotEmpty())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Официальный сайт",
                                PathId = DiFieldPathType.Path35,
                                ManOrg = disclosureInfo
                            });
                        }
                    }
                }
            } else if (disclosureInfo.MembershipUnions == YesNoNotSet.No)
            {
                tempPositionsCount = 1;
                tempCompletePositionsCount = 1;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "В данном отчетном периоде организация состояла в объединениях",
                    PathId = DiFieldPathType.Path35,
                    ManOrg = disclosureInfo
                });
            }

            percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

            return new DisclosureInfoPercent
            {
                Code = "MembershipUnionsPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = tempPositionsCount,
                CompletePositionsCount = tempCompletePositionsCount,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            };
        }

        /// <summary>
        /// Считает раскрытие информации по сведениям о фондах
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateFundsInfoPercent(DisclosureInfo disclosureInfo)
        {
            int positionsCount = 3,
                completePositionsCount = 0;
            decimal? percent = 0;

            if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ || disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK)
            {

                if (disclosureInfo.FundsInfo == YesNoNotSet.No && disclosureInfo.DocumentWithoutFunds != null)
                {
                    percent = null;
                    positionsCount = 0;
                }
                else if (disclosureInfo.FundsInfo == YesNoNotSet.Yes)
                {
                    if (disclosureInfo.SizePayments != null)
                    {
                        completePositionsCount++;
                    }

                    if (this.HasFundsInfoDisInfos.Contains(disclosureInfo.Id))
                    {
                        completePositionsCount++;
                    }

                    // если заполнено хотя бы одно из двух, то добавляем +1 от отметки
                    if (completePositionsCount > 0)
                    {
                        completePositionsCount++;
                    }

                    percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = @"Фонды товарищества \ кооперативов существуют",
                        PathId = DiFieldPathType.Path30,
                        ManOrg = disclosureInfo
                    });
                }
            }
            else
            {
                positionsCount = 0;
                percent = null;
            }

            return new DisclosureInfoPercent
            {
                Code = "FundsInfoPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            };
        }

        /// <summary>
        /// Считает раскрытие информации по документам УО
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateDocumentsPercent(DisclosureInfo disclosureInfo)
        {
            int positionsCount = disclosureInfo.ManagingOrganization.TypeManagement != TypeManagementManOrg.JSK ? 3 : 2,
                completePositionsCount = 0;

            decimal? percent = 0;

            var docs = this.DocumentsDisInfo.Get(disclosureInfo.Id);

            if (docs.IsNotNull())
            {
                // Проект договора управления
                if (disclosureInfo.ManagingOrganization.TypeManagement != TypeManagementManOrg.JSK)
                {
                    if (docs.NotAvailable && docs.FileProjectContract.IsNull() || docs.FileProjectContract.IsNotNull() && !docs.NotAvailable)
                    {
                        completePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Проект договора управления",
                            PathId = DiFieldPathType.Path21,
                            ManOrg = disclosureInfo
                        });
                    }
                }

                // Перечень и качество коммунальных услуг
                if (docs.FileCommunalService.IsNotNull())
                {
                    completePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Перечень и качество коммунальных услуг",
                        PathId = DiFieldPathType.Path21,
                        ManOrg = disclosureInfo
                    });
                }

                // Базовый перечень показателей качества
                if (docs.FileServiceApartment.IsNotNull())
                {
                    completePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Базовый перечень показателей качества",
                        PathId = DiFieldPathType.Path21,
                        ManOrg = disclosureInfo
                    });
                }

                percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Документы",
                    PathId = DiFieldPathType.Path21,
                    ManOrg = disclosureInfo
                });
            }

            return new DisclosureInfoPercent
            {
                Code = "DocumentsPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            };
        }

        /// <summary>
        /// Считает раскрытие информации по сведениям о договорах
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateInfoOnContrPercent(DisclosureInfo disclosureInfo)
        {
            int tempPositionsCount = 2,
                tempCompletePositionsCount = 0;
            decimal? percent = 0;

            if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ
                || disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK)
            {

                if (disclosureInfo.ContractsAvailability == YesNoNotSet.No)
                {
                    percent = 100;
                }
                else if (disclosureInfo.ContractsAvailability == YesNoNotSet.Yes)
                {
                    var anyContracts = this.InformationOnContracts.Contains(disclosureInfo.ManagingOrganization.Id);
                    percent = anyContracts ? 100 : 0;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = @"Действующие договоры за отчетный период имеются",
                        PathId = DiFieldPathType.Path28,
                        ManOrg = disclosureInfo
                    });
                }

                tempCompletePositionsCount = percent == 100 ? 2 : 0;
            }
            else
            {
                tempPositionsCount = 0;
                percent = null;
            }

            return new DisclosureInfoPercent
            {
                Code = "InfoOnContrPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = tempPositionsCount,
                CompletePositionsCount = tempCompletePositionsCount,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            };
        }

        /// <summary>
        /// Считает раскрытие информации по  планам работ по содержанию и ремонту
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculatePlanWorkServiceRepairPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int completePositionsCount = 0;

            var repairServices = this.ServicesDict.Get(realObj.Id)?
                .Where(x => x.KindServiceDi == KindServiceDi.Repair)
                .Where(x => this.HousingServiceProvision.Get(x.Id) == TypeOfProvisionServiceDi.ServiceProvidedMo)
                .ToDictionary(x => x.Id, x => false);

            // сколько услуг по ремонту, столько же должно быть планов
            var positionsCount = 1 + (repairServices?.Count ?? 0);

            var planWorks = this.PlanWorkServiceRepairDict.Get(realObj.Id);
            if (planWorks.IsNotEmpty())
            {
                positionsCount--;

                foreach (var planWork in planWorks)
                {
                    positionsCount += 5;

                    // если услуга не предоставляется через УО, то по плану 100%
                    if (this.HousingServiceProvision.Get(planWork.BaseService.Id) != TypeOfProvisionServiceDi.ServiceProvidedMo)
                    {
                        completePositionsCount += 5;
                        continue;
                    }
                    if (repairServices != null)
                    {
                        if (repairServices.ContainsKey(planWork.BaseService.Id))
                        {
                            repairServices[planWork.BaseService.Id] = true;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "План по услуге",
                                PathId = DiFieldPathType.Path13,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }
                    }
                    

                    var repairService = this.RepairServiceDict.Get(planWork.BaseService.Id);

                    if (repairService.IsNotNull() && repairService.SumWorkTo.HasValue)
                    {
                        completePositionsCount++;
                    }

                    var planWorkServiceRepairWorks = this.PlanWorkServiceRepairWorksDict.Get(planWork.Id);
                    if (repairService.IsNotNull() && repairService.ScheduledPreventiveMaintanance == YesNoNotSet.No)
                    {
                        completePositionsCount += 3;
                    }
                    else if (planWorkServiceRepairWorks.IsNotEmpty())
                    {
                        // т.к. учитываем заполненность по каждому эл-ту, то вычитаем -3 поля, т.к. они были добавлены
                        // ранее для верного подсчёта общего процента в текущем блоке
                        positionsCount -= 3;

                        foreach (var work in planWorkServiceRepairWorks)
                        {
                            positionsCount += 3;
                            if (work.Cost.HasValue)
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Плановая стоимость",
                                    PathId = DiFieldPathType.Path13,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }

                            if (work.DateStart.HasValue)
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Дата начала",
                                    PathId = DiFieldPathType.Path13,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }

                            if (work.DateEnd.HasValue)
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Дата окончания",
                                    PathId = DiFieldPathType.Path13,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }
                        }
                    }

                    if (this.AnyWorkRepairTechServDict.Get(planWork.BaseService.Id))
                    {
                        completePositionsCount++;
                    }
                }

                if (repairServices != null)
                {
                    // считаем, все ли добавлены планы
                    completePositionsCount += repairServices.Count(x => x.Value);
                }
            }

            return new DiRealObjPercent
            {
                Code = "PlanWorkServiceRepairPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2),
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по документам дома
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculateDocumentsDiRealObjPercent(DisclosureInfoRealityObjProxy realObj)
        {
            var diPercent = base.CalculateDocumentsDiRealObjPercent(realObj);

            diPercent.PositionsCount += 3;
            var docs = this.DocumentsRealityObjDiProxyDict.Get(realObj.Id);
            if (docs.IsNotNull())
            {
                if (docs.HasFileActState)
                {
                    diPercent.CompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Акт состояния общего имущества",
                        PathId = DiFieldPathType.Path3,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (docs.HasFileCatalogRepair)
                {
                    diPercent.CompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Перечень работ по содержанию и ремонту",
                        PathId = DiFieldPathType.Path3,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }

                if (docs.HasFileReportPlanRepair)
                {
                    diPercent.CompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Отчет о выполнении годового плана",
                        PathId = DiFieldPathType.Path3,
                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                    });
                }
            }

            diPercent.Percent = (decimal.Divide(diPercent.CompletePositionsCount, diPercent.PositionsCount) * 100).RoundDecimal(2);
            return diPercent;
        }

        /// <summary>
        /// Считает раскрытие информации по планам мер по снижению расходов
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculatePlanReductionExpensePercent(DisclosureInfoRealityObjProxy realObj)
        {
            var planReduction = this.PlanReductionExpenseDict.Get(realObj.Id);

            int positionsCount = planReduction.ReturnSafe(x => x.Count()),
                completePositionsCount = 0;

            decimal percent;
            if (positionsCount == 0)
            {
                percent = 0;
                positionsCount = 1;
            }

            if (planReduction.IsNotEmpty())
            {
                completePositionsCount = planReduction.Count(x => this.AnyPlanReductionExpenseWorks.Get(x));
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Мера по снижению расходов",
                    PathId = DiFieldPathType.Path12,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);

            return new DiRealObjPercent
            {
                Code = "PlanReductionExpensePercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по планам мер по сведениям о случаях снижения платы
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculateInfoAboutReductionPaymentPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int positionsCount = 1,
                completePositionsCount = 0;

            if (realObj.ReductionPayment == YesNoNotSet.No)
            {
                completePositionsCount++;
            }
            else if (realObj.ReductionPayment == YesNoNotSet.Yes)
            {
                if (this.AnyInfoAboutReductionPaymentDict.Get(realObj.Id))
                {
                    completePositionsCount++;
                }
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Отметка о случаях",
                    PathId = DiFieldPathType.Path15,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            var percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);

            return new DiRealObjPercent
            {
                Code = "InfoAboutReductionPaymentPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по планам мер по сведениям об использовании нежилых помещений
        /// </summary>
        /// <param name="realityObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculateNonResidentialPlacementPercent(DisclosureInfoRealityObjProxy realityObj)
        {
            int positionsCount = 1,
                completePositionsCount = 0;

            if (realityObj.NonResidentialPlacement == YesNoNotSet.No)
            {
                completePositionsCount++;
            }
            else if (realityObj.NonResidentialPlacement == YesNoNotSet.Yes)
            {
                if (this.AnyNonResidentialPlacementDict.Get(realityObj.Id))
                {
                    completePositionsCount++;
                }
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Отметка о случаях",
                    PathId = DiFieldPathType.Path17,
                    RealityObj = this.GetRealityObjectProxy(realityObj.Id)
                });
            }

            var percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);

            return new DiRealObjPercent
            {
                Code = "NonResidentialPlacementPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realityObj)
            };
        }

        /// <summary>
        /// Проверяет жилищную услугу на необходимость в проведении расчётов
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <returns>Необходимость в расчётах</returns>
        protected override bool IsHousingServiceCalculable(ServiceProxy service)
        {
            var isCalculable = base.IsHousingServiceCalculable(service);

            if (isCalculable && service.Code == 7 && this.RoServ7ToExclude.Contains(service.RealObjId))
            {
                isCalculable = false;
            }

            if (isCalculable && (service.Code == 27 || service.Code == 28) && this.LiftsCountDict.Get(service.RealObjId) == 0)
            {
                isCalculable = false;
            }

            return isCalculable;
        }

        /// <summary>
        /// Возвращает количество обязательных услуг, которые не нужно считать по каким-либо условиям
        /// </summary>
        /// <param name="services">Услуги дома</param>
        /// <param name="realityObj">Дом</param>
        /// <returns>Количество</returns>
        protected override int CountNotRequiredHousingServices(List<ServiceProxy988> services, DisclosureInfoRealityObjProxy realityObj)
        {
            var count = base.CountNotRequiredHousingServices(services, realityObj);

            if (services.All(x => x.Code != 7) && this.RoServ7ToExclude.Contains(realityObj.RoId))
            {
                count++;
            }

            if (this.LiftsCountDict.Get(realityObj.RoId) == 0)
            {
                if (services.All(x => x.Code != 27))
                {
                    count++;
                }
                if (services.All(x => x.Code != 28))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Проверка раскрытия процентов по жилищной услуге
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <returns>Блок процента</returns>
        protected override ServicePercent ValidateHousingService(ServiceProxy988 service)
        {
            var diPercent = base.ValidateHousingService(service);
            var typeProvision = this.HousingServiceProvision.Get(service.Id);

            // учитывать 100%, если по жилищным услугам тип предоставления=сосбтвенники отказались от предоставления услуги, 
            // а для всех остальных услуг тип предоставления = услуга не предоставляется
            if (service.KindServiceDi == KindServiceDi.Housing && typeProvision == TypeOfProvisionServiceDi.OwnersRefused || 
                service.KindServiceDi != KindServiceDi.Housing && typeProvision == TypeOfProvisionServiceDi.ServiceNotAvailable)
            {
                return diPercent;
            }

            diPercent.PositionsCount++;
            if (service.HasProvider)
            {
                diPercent.CompletePositionsCount++;
            }
            else
            {
                // Тип предоставления услуги = 0 из 1
                diPercent.CompletePositionsCount--;
            }

            if (service.KindServiceDi == KindServiceDi.Repair)
            {
                diPercent.PositionsCount++;
                if (this.RepairWorkHasAnyTarrifDict.Contains(service.Id))
                {
                    diPercent.CompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Тарифы для потребителей",
                        PathId = DiFieldPathType.Path24,
                        RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                    });
                }

                diPercent.PositionsCount++;
                if (this.HasWorkRepairTechServ.Contains(service.Id))
                {
                    diPercent.CompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "Работа по ТО",
                        PathId = DiFieldPathType.Path24,
                        RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                    });
                }

                diPercent.PositionsCount++;
                if (service.ScheduledPreventiveMaintanance == YesNoNotSet.No)
                {
                    diPercent.CompletePositionsCount++;
                }
                else if (this.HasAnyWorkRepairListTat.Contains(service.Id))
                {
                    diPercent.CompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                    {
                        FieldName = "ППР",
                        PathId = DiFieldPathType.Path24,
                        RealityObj = this.GetRealityObjectProxy(service.DiRealObjId)
                    });
                }
            }

            diPercent.Percent = (decimal.Divide(diPercent.CompletePositionsCount, diPercent.PositionsCount) * 100).RoundDecimal(2);
            return diPercent;
        }

        /// <summary>
        /// Акцессор для получения способа формирования фонда
        /// </summary>
        protected override CrFundFormationType GetAccountFormationType(DisclosureInfoRealityObjProxy diRo)
        {
            CrFundFormationType result;
            if (this.RealiTypeOfFormingCrDict.TryGetValue(diRo.RoId, out result))
            {
                return result;
            }

            return CrFundFormationType.Unknown;
        }

        /// <summary>
        /// Считает раскрытие информации по общим сведениям о доме
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculateRealityObjectGeneralDataPercent(DisclosureInfoRealityObjProxy realObj)
        {
            int positionsCount = 18,
                completePositionsCount = 0;

            if (realObj.BuildYear.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Год постройки",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.DateCommissioning.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Дата ввода в эксплуатацию",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.FiasAddress.IsNotNull() && realObj.FiasAddress.AddressName.IsNotEmpty())
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Адрес МКД",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.TypeProject.IsNotEmpty())
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Серия, тип постройки здания",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.TypeHouse != TypeHouse.NotSet)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Тип дома",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.Floors.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во этажей наименьшее",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.MaximumFloors.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во этажей наибольшее",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.NumberEntrances.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во подъездов",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.NumberLifts.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во лифтов",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AllNumberOfPremises.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во помещений всего",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.NumberApartments.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во жилых помещений",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.NumberNonResidentialPremises.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Кол-во нежилых помещений",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AreaMkd.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Общая площадь дома",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AreaLiving.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Общая площадь жилых помещений",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AreaNotLivingPremises.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Общая площадь нежилых помещений",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (realObj.AreaOfAllNotLivingPremises.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Общая площадь помещений, входящих в состав общего имущества",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (this.ChildrenAreaTp.Contains(realObj.RoId))
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Детская площадка",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            if (this.SportAreaTp.Contains(realObj.RoId))
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Спортивная площадка",
                    PathId = DiFieldPathType.Path28,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            // Сведения о фонде КР, если есть модуль регфонд (иначе словарь будет null)
            if (this.RealityObjectDecisionProtocolDict.IsNotNull())
            {
                var type = this.GetAccountFormationType(realObj);
                if (type != CrFundFormationType.Unknown && type != CrFundFormationType.NotSelected)
                {
                    positionsCount += 3;
                    var bothProtocolProxy = this.RealityObjectDecisionProtocolDict.Get(realObj.RoId);
                    if (bothProtocolProxy.IsNotNull())
                    {
                        if (bothProtocolProxy.ProtocolDate.IsValid())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Дата протокола общего собрания собственников помещения",
                                PathId = DiFieldPathType.Path1,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (bothProtocolProxy.ProtocolNumber.IsNotEmpty())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Номер протокола общего собрания собственников помещения",
                                PathId = DiFieldPathType.Path1,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (bothProtocolProxy.CrPaySize != null)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Размер взноса на капитальный ремонт",
                                PathId = DiFieldPathType.Path1,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        // Если спец. счет
                        if (type != CrFundFormationType.RegOpAccount)
                        {
                            positionsCount += 2;
                            if (bothProtocolProxy.RegOpContragentInn.IsNotEmpty())
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "ИНН владельца",
                                    PathId = DiFieldPathType.Path1,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }

                            if (bothProtocolProxy.RegOpContragentName.IsNotEmpty())
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Владелец специального счета",
                                    PathId = DiFieldPathType.Path1,
                                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                });
                            }
                        }
                    }
                }else if (type == CrFundFormationType.NotSelected && realObj.ConditionHouse == ConditionHouse.Emergency)
                {
                    positionsCount += 3;
                    completePositionsCount += 3;
                }
            }

            var percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);

            return new DiRealObjPercent
            {
                Code = "RealityObjectGeneralDataPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = percent,
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realObj)
            };
        }

        protected class TerminateContractProxy
        {
            public virtual string Address { get; set; }
            public virtual string TerminateReason { get; set; }
        }

        protected class MembershipUnionsProxy
        {
            public virtual string Name { get; set; }
            public virtual string Address { get; set; }
            public virtual string OfficialSite { get; set; }
        }
    }
}