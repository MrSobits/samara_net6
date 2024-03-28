namespace Bars.GkhDi.PercentCalculationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Calculating;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Методы для подсчёта раскрытия информации по УО
    /// </summary>
    public partial class PercentCalculation988
    {
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
        public IDomainService<Actions> ActionsDomain { get; set; }
        public IDisclosureInfoService DisclosureInfoService { get; set; }

        /// <summary>
        /// Домен сервис "Коммунальные услуги"
        /// </summary>
        public IDomainService<FinActivityCommunalService> FinActivityCommunalServiceDomain { get; set; }

        /// <summary>
        /// Домен-сервис "Раскрытыие информации о лицензиях управляющей организаци (УО), согласно 988"
        /// </summary>
        public IDomainService<DisclosureInfoLicense> DisclosureInfoLicenseDomain { get; set; }

        /// <summary>
        /// Код ОКОПФ для ИП
        /// </summary>
        protected const string OkopfCodeForIp = "5 01 02";

        /// <summary>
        /// Коммунальные услуги, задолженность по которым должна быть заполнена
        /// </summary>
        protected TypeServiceDi[] RequiredCommunalFinActivityServices =
        {
            TypeServiceDi.Heating,
            TypeServiceDi.ColdWater,
            TypeServiceDi.HotWater,
            TypeServiceDi.Wastewater,
            TypeServiceDi.Gas,
            TypeServiceDi.Electrical,
            TypeServiceDi.OtherSource
        };

        protected HashSet<long> AnyActions; 

        protected Dictionary<long, WorkModeProxy> WorkModeService;
        protected Dictionary<long, FinActivityProxy> FinActivityDisInfo;
        protected Dictionary<long, FinActivityDocsProxy> FinActivityDocsDisInfo;

        protected Dictionary<long, List<FinActivityAuditDocsProxy>> FinActivityAuditYearsDi;
        protected Dictionary<long, List<FinActivityRealityObjectProxy>> FinActivityRealityObject;
        protected Dictionary<long, List<FinActivityCommunalServiceProxy>> FinActivityCommunalService;
        protected Dictionary<long, List<FinActivityDocsByYearProxy>> FinActivityDocsByYearDi;
        protected Dictionary<long, List<AdminResp>> AdminRespDisInfo;
        protected Dictionary<long, List<DisclosureInfoLicense>> DisclosureInfoLicense;
        protected Dictionary<long, int> CountManagingHousesDict; 

        /// <summary>
        /// Приготовить кэш
        /// </summary>
        /// <param name="diQuery">УО в период раскрытия информации</param>
        protected virtual void PrepareManOrgCacheCache(IQueryable<DisclosureInfo> diQuery)
        {
            this.AdminRespDisInfo = this.AdminRespDomain
                .GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .GroupBy(x => x.DisclosureInfo.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            this.AnyActions = this.ActionsDomain.GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.AdminResp.DisclosureInfo.Id))
                .Select(x => x.AdminResp.Id)
                .ToHashSet();

            this.CountManagingHousesDict = this.moRoQuery
                .Select(x => new
                {
                    ManorgId = x.ManOrgContract.ManagingOrganization.Id,
                    RoId = x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.ManorgId)
                .ToDictionary(x => x.Key, y => y.Distinct().Count());

            this.FinActivityDisInfo = this.FinActivityDomain
                .GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .ToDictionary(
                    x => x.DisclosureInfo.Id,
                    y => new FinActivityProxy
                    {
                        TaxSystem = y.TaxSystem,
                        MoTypeManagement = y.DisclosureInfo.ManagingOrganization.TypeManagement
                    });

            this.FinActivityAuditYearsDi = this.FinActivityAuditDomain
                .GetAll()
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                .GroupBy(y => y.ManagingOrganization.Id)
                .ToDictionary(
                    y => y.Key,
                    x => x.Select(
                        z => new FinActivityAuditDocsProxy
                        {
                            Year = z.Year,
                            TypeAuditStateDi = z.TypeAuditStateDi,
                            File = z.File
                        }).ToList());

            this.FinActivityDocsDisInfo = this.FinActivityDocsDomain
                .GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .ToDictionary(
                    x => x.DisclosureInfo.Id,
                    y => new FinActivityDocsProxy
                    {
                        BookkepingBalance = y.BookkepingBalance,
                        BookkepingBalanceAnnex = y.BookkepingBalanceAnnex
                    });

            this.FinActivityDocsByYearDi = this.FinActivityDocByYearDomain
                .GetAll()
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                .GroupBy(y => y.ManagingOrganization.Id)
                .ToDictionary(
                    y => y.Key,
                    x => x.Select(
                        y => new FinActivityDocsByYearProxy
                        {
                            Year = y.Year,
                            TypeDocByYearDi = y.TypeDocByYearDi
                        }).ToList());

            this.FinActivityRealityObject = this.FinActivityManagRealityObjDomain.GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .Where(x => this.moRoQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                .GroupBy(x => x.DisclosureInfo.Id)
                .ToDictionary(
                    y => y.Key,
                    x => x.Select(
                        y => new FinActivityRealityObjectProxy
                        {
                            SumDebt = y.SumDebt,
                            SumFactExpense = y.SumFactExpense,
                            SumIncomeManage = y.SumIncomeManage,
                            RoId = y.RealityObject.Id
                        }).ToList());

            this.WorkModeService = this.ManagingOrgWorkModeDomain.GetAll()
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                .GroupBy(x => x.ManagingOrganization.Id)
                .ToDictionary(
                    x => x.Key,
                    y => new WorkModeProxy
                    {
                        DispatcherWork = y.Any(z => z.TypeMode == TypeMode.DispatcherWork),
                        ReceptionCitizens = y.Any(z => z.TypeMode == TypeMode.ReceptionCitizens),
                        WorkMode = y.Any(z => z.TypeMode == TypeMode.WorkMode)
                    });

            this.FinActivityCommunalService = this.FinActivityCommunalServiceDomain.GetAll()
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.DisclosureInfo.ManagingOrganization.Id))
                .GroupBy(x => x.DisclosureInfo.Id)
                .ToDictionary(
                    x => x.Key,
                    x =>
                        x.GroupBy(y => y.TypeServiceDi)
                        .ToDictionary(y => y.Key, y => y.First())
                        .Select(
                            y => 
                            new FinActivityCommunalServiceProxy
                            {
                                TypeServiceDi = y.Key,
                                DebtManOrgCommunalService = y.Value.DebtManOrgCommunalService
                            })
                         .ToList());

            this.DisclosureInfoLicense = this.DisclosureInfoLicenseDomain.GetAll()
                .Where(x => diQuery.Any(y => y.ManagingOrganization.Id == x.DisclosureInfo.ManagingOrganization.Id))
                .GroupBy(x => x.DisclosureInfo.Id)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        /// <summary>
        /// Считает раскрытие информации по общим сведениям о расторгнутых договорах
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DisclosureInfoPercent CalculateTerminateContractPercent(DisclosureInfo disclosureInfo)
        {
            return new DisclosureInfoPercent
            {
                Code = "TerminateContractPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            }.AddForcePercent(this.PercentAlgoritm);
        }

        /// <summary>
        /// Считает раскрытие информации по общим сведениям об УО
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DisclosureInfoPercent CalculateGeneralDataPercent(DisclosureInfo disclosureInfo)
        {
            int tempPositionsCount = 0,
                tempCompletePositionsCount = 0;

            var manOrg = disclosureInfo.ManagingOrganization;
            if (manOrg.Contragent.OrganizationForm.IsNotNull())
            {
                if (manOrg.Contragent.OrganizationForm.OkopfCode != PercentCalculation988.OkopfCodeForIp)
                {
                    // Наименование юр. лица
                    tempPositionsCount++;
                    if (disclosureInfo.ManagingOrganization.Contragent.Name.IsNotEmpty())
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Наименование юр. лица",
                            PathId = DiFieldPathType.Path24,
                            ManOrg = disclosureInfo
                        });
                    }

                    // Краткое наименование юр. лица
                    tempPositionsCount++;
                    if (disclosureInfo.ManagingOrganization.Contragent.ShortName.IsNotEmpty())
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Краткое наименование юр. лица",
                            PathId = DiFieldPathType.Path36,
                            ManOrg = disclosureInfo
                        });
                    }
                }
            }

            // ФИО руководителя
            tempPositionsCount++;
            if (!string.IsNullOrEmpty(this.DisclosureInfoService.GetPositionByCode(manOrg.Contragent.Id, disclosureInfo.PeriodDi, new List<string> { "1", "4" })))
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "ФИО руководителя",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // Юридический адрес
            tempPositionsCount++;
            if (!string.IsNullOrEmpty(manOrg.Contragent.FiasJuridicalAddress?.AddressName))
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Юридический адрес",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // Почтовый адрес
            tempPositionsCount++;
            if (!string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.FiasMailingAddress?.AddressName))
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Почтовый адрес",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // Фактический адрес
            tempPositionsCount++;
            if (!string.IsNullOrEmpty(disclosureInfo.ManagingOrganization.Contragent.FiasFactAddress?.AddressName))
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Фактический адрес",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // Устав товарищества собственников жилья или кооператива
            if (manOrg.TypeManagement == TypeManagementManOrg.TSJ || manOrg.TypeManagement == TypeManagementManOrg.JSK)
            {
                tempPositionsCount++;
                if (manOrg.DispatchFile != null)
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Устав товарищества собственников жилья или кооператива",
                        PathId = DiFieldPathType.Path24,
                        ManOrg = disclosureInfo
                    });
                }
            }

            // ОГРН
            tempPositionsCount++;
            if (manOrg.Contragent.Ogrn.IsNotEmpty())
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "ОГРН",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // ИНН
            tempPositionsCount++;
            if (manOrg.Contragent.Inn.IsNotEmpty())
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "ИНН",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // Телефон
            tempPositionsCount++;
            if (manOrg.Contragent.Phone.IsNotEmpty())
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Телефон",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            // Факс
            tempPositionsCount++;
            if (manOrg.Contragent.Fax.IsNotEmpty())
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Факс",
                    PathId = DiFieldPathType.Path24,
                    ManOrg = disclosureInfo
                });
            }

            tempPositionsCount += disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK ? 3 : 2;
            if (this.WorkModeService.ContainsKey(disclosureInfo.ManagingOrganization.Id))
            {
                // Режим Работы
                if (this.WorkModeService[disclosureInfo.ManagingOrganization.Id].WorkMode)
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Режим работы",
                        PathId = DiFieldPathType.Path27,
                        ManOrg = disclosureInfo
                    });
                }

                // Прием граждан
                if (this.WorkModeService[disclosureInfo.ManagingOrganization.Id].ReceptionCitizens)
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Прием граждан",
                        PathId = DiFieldPathType.Path25,
                        ManOrg = disclosureInfo
                    });
                }

                // Режим работы диспетчерских служб
                if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK)
                {
                    if (this.WorkModeService[disclosureInfo.ManagingOrganization.Id].DispatcherWork)
                    {
                        tempCompletePositionsCount++;
                    }
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Режим работы диспетчерских служб",
                        PathId = DiFieldPathType.Path26,
                        ManOrg = disclosureInfo
                    });
                }
            }

            // Доля участия субъекта Российской Федерации в уставном капитале организации
            tempPositionsCount++;
            if (manOrg.ShareSf.HasValue)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Доля участия субъекта Российской Федерации в уставном капитале организации",
                    PathId = DiFieldPathType.Path38,
                    ManOrg = disclosureInfo
                });
            }

            // Доля участия муниципального образования в уставном капитале организации
            tempPositionsCount++;
            if (manOrg.ShareMo.HasValue)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Доля участия муниципального образования в уставном капитале организации",
                    PathId = DiFieldPathType.Path39,
                    ManOrg = disclosureInfo
                });
            }

            tempPositionsCount += 4;

            // Штатная численность, всего
            if (manOrg.NumberEmployees.HasValue)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Штатная численность, всего",
                    PathId = DiFieldPathType.Path23,
                    ManOrg = disclosureInfo
                });
            }

            // Штатная численность административного персонала
            if (disclosureInfo.AdminPersonnel.HasValue)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Штатная численность административного персонала",
                    PathId = DiFieldPathType.Path23,
                    ManOrg = disclosureInfo
                });
            }

            // Штатная численность инженеров
            if (disclosureInfo.Engineer.HasValue)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Штатная численность инженеров",
                    PathId = DiFieldPathType.Path23,
                    ManOrg = disclosureInfo
                });
            }

            // Штатная численность рабочих
            if (disclosureInfo.Work.HasValue)
            {
                tempCompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                {
                    FieldName = "Штатная численность рабочих",
                    PathId = DiFieldPathType.Path23,
                    ManOrg = disclosureInfo
                });
            }

            var percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);
            return new DisclosureInfoPercent
            {
                Code = "GeneralDataPercent",
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
        protected virtual DisclosureInfoPercent CalculateMembershipUnionsPercent(DisclosureInfo disclosureInfo)
        {
            return new DisclosureInfoPercent
            {
                Code = "MembershipUnionsPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            }.AddForcePercent(this.PercentAlgoritm);
        }

        /// <summary>
        /// Считает раскрытие информации по финансовой деятельности
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DisclosureInfoPercent CalculateFinActivityPercent(DisclosureInfo disclosureInfo)
        {
            decimal? percent = 0;
            int tempPositionsCount = 0,
                tempCompletePositionsCount = 0;

            var periodYear = 0;
            if (this.period.DateStart != null)
            {
                periodYear = this.period.DateStart.Value.Year;
            }

            var finActivity = this.FinActivityDisInfo.Get(disclosureInfo.Id);
            if (finActivity.IsNotNull())
            {
                // Система налогообложения
                tempPositionsCount++;
                if (finActivity.TaxSystem.IsNotNull())
                {
                    tempCompletePositionsCount++;
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Система налогообложения",
                        PathId = DiFieldPathType.Path33,
                        ManOrg = disclosureInfo
                    });
                }

                // Документы аудиторской проверки
                tempPositionsCount += 3;
                {
                    var isNotEmpty = this.FinActivityAuditYearsDi.ContainsKey(disclosureInfo.ManagingOrganization.Id);

                    // Документ аудиторской проверки за отчетный год
                    if (isNotEmpty && this.FinActivityAuditYearsDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear &&
                        (x.TypeAuditStateDi == TypeAuditStateDi.NotInspect || x.File != null)))
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Документ аудиторской проверки за отчетный год",
                            PathId = DiFieldPathType.Path33,
                            ManOrg = disclosureInfo
                        });
                    }

                    // Документ аудиторской проверки за год, предшествующий отчетному
                    if (isNotEmpty && this.FinActivityAuditYearsDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear - 1 &&
                        (x.TypeAuditStateDi == TypeAuditStateDi.NotInspect || x.File != null)))
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Документ аудиторской проверки за год, предшествующий отчетному",
                            PathId = DiFieldPathType.Path33,
                            ManOrg = disclosureInfo
                        });
                    }

                    // Документ аудиторской проверки за 2 года, предшествующих отчетному
                    if (isNotEmpty && this.FinActivityAuditYearsDi[disclosureInfo.ManagingOrganization.Id].Any(x => x.Year == periodYear - 2 &&
                        (x.TypeAuditStateDi == TypeAuditStateDi.NotInspect || x.File != null)))
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Документ аудиторской проверки за 2 года, предшествующих отчетному",
                            PathId = DiFieldPathType.Path33,
                            ManOrg = disclosureInfo
                        });
                    }
                }

                tempPositionsCount += 2;
                var docs = this.FinActivityDocsDisInfo.Get(disclosureInfo.Id);
                {
                    var isNotEmpty = docs.IsNotNull();

                    // Бухгалтерский баланс или налоговая декларация для УСНО
                    if (isNotEmpty && docs.BookkepingBalance.IsNotNull())
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Бухгалтерский баланс",
                            PathId = DiFieldPathType.Path31,
                            ManOrg = disclosureInfo
                        });
                    }

                    // Приложение к бухгалтерскому балансу
                    if (isNotEmpty && docs.BookkepingBalanceAnnex.IsNotNull())
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Приложение к бухгалтерскому балансу",
                            PathId = DiFieldPathType.Path31,
                            ManOrg = disclosureInfo
                        });
                    }
                }

                var finActivityDocs = this.FinActivityDocsByYearDi.Get(disclosureInfo.ManagingOrganization.Id);
                // Заключения ревизионной комиссии
                if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ)
                {
                    tempPositionsCount += 3;
                    {
                        var isNotEmpty = finActivityDocs.IsNotNull();

                        // Заключение ревизионной комиссии за текущий год
                        if (isNotEmpty && finActivityDocs.Any(x => x.Year == periodYear && x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory))
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Заключение ревизионной комиссии за текущий год",
                                PathId = DiFieldPathType.Path31,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Заключение ревизионной комиссии за год, предшествующий текущему году
                        if (isNotEmpty && finActivityDocs.Any(x => x.Year == periodYear - 1 && x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory))
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Заключение ревизионной комиссии за год, предшествующий текущему году",
                                PathId = DiFieldPathType.Path31,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Заключение ревизионной комиссии за 2 года, предшествующих текущему году
                        if (isNotEmpty && finActivityDocs.Any(x => x.Year == periodYear - 2 && x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory))
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Заключение ревизионной комиссии за 2 года, предшествующих текущему году",
                                PathId = DiFieldPathType.Path31,
                                ManOrg = disclosureInfo
                            });
                        }
                    }
                }

                // Управление по домам
                var finActivityRealityObjects = this.FinActivityRealityObject.Get(disclosureInfo.Id);
                var managingHousesCount = 3 * this.CountManagingHousesDict.Get(disclosureInfo.ManagingOrganization.Id);
                tempPositionsCount += managingHousesCount;
                if (managingHousesCount > 0)
                {
                    if (finActivityRealityObjects.IsNotNull())
                    {
                        foreach (var activityRealityObject in finActivityRealityObjects)
                        {
                            // Сумма дохода от управления
                            if (activityRealityObject.SumIncomeManage.HasValue)
                            {
                                tempCompletePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Сумма задолженности",
                                    PathId = DiFieldPathType.Path34,
                                    RealityObj = this.DisclosureInfoService.GetDiRoProxyByDi(disclosureInfo.Id, activityRealityObject.RoId.Value)
                                });
                            }

                            // Сумма факт расходов
                            if (activityRealityObject.SumFactExpense.HasValue)
                            {
                                tempCompletePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Сумма фактических расходов",
                                    PathId = DiFieldPathType.Path34,
                                    RealityObj = this.DisclosureInfoService.GetDiRoProxyByDi(disclosureInfo.Id, activityRealityObject.RoId.Value)
                                });
                            }

                            // Сумма задолженности
                            if (activityRealityObject.SumDebt.HasValue)
                            {
                                tempCompletePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                                {
                                    FieldName = "Общая задолженность управляющей организации",
                                    PathId = DiFieldPathType.Path34,
                                    RealityObj = this.DisclosureInfoService.GetDiRoProxyByDi(disclosureInfo.Id, activityRealityObject.RoId.Value)
                                });
                            }
                        }
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Сумма задолженности",
                            PathId = DiFieldPathType.Path34,
                            ManOrg = disclosureInfo
                        });
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Сумма фактических расходов",
                            PathId = DiFieldPathType.Path34,
                            ManOrg = disclosureInfo
                        });
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = "Общая задолженность управляющей организации",
                            PathId = DiFieldPathType.Path34,
                            ManOrg = disclosureInfo
                        });
                    }
                }

                // Задолженность по коммунальным услугам
                var finActivityCommunal = this.FinActivityCommunalService.Get(disclosureInfo.Id)
                    ?.Where(x => this.RequiredCommunalFinActivityServices.Contains(x.TypeServiceDi));

                tempPositionsCount += this.RequiredCommunalFinActivityServices.Length;

                foreach (var proxy in finActivityCommunal.AllOrEmpty())
                {
                    if (proxy.DebtManOrgCommunalService.HasValue)
                    {
                        tempCompletePositionsCount++;
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                        {
                            FieldName = $"Общая задолженность ({proxy.TypeServiceDi.GetDisplayName()})",
                            PathId = DiFieldPathType.Path32,
                            ManOrg = disclosureInfo
                        });
                    }
                }

                if (disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ
                    || disclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK)
                {
                    tempPositionsCount += 3;

                    {
                        var isNotEmpty = finActivityDocs.IsNotNull();

                        // Смета доходов и расходов за текущий год
                        if (isNotEmpty && finActivityDocs.Any(x => x.Year == periodYear && x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome))
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Смета доходов и расходов за текущий год",
                                PathId = DiFieldPathType.Path32,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Смета доходов и расходов за предшествующий год
                        if (isNotEmpty && finActivityDocs.Any(x => x.Year == periodYear - 1 && x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome))
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Смета доходов и расходов за предшествующий год",
                                PathId = DiFieldPathType.Path32,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Отчет о выполнении сметы доходов и расходов за предшествующий год
                        if (isNotEmpty && finActivityDocs.Any(x => x.Year == periodYear - 1 && x.TypeDocByYearDi == TypeDocByYearDi.ReportEstimateIncome))
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Отчет о выполнении сметы доходов и расходов за предшествующий год",
                                PathId = DiFieldPathType.Path32,
                                ManOrg = disclosureInfo
                            });
                        }
                    }
                }
            }

            percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);
            return new DisclosureInfoPercent
            {
                Code = "FinActivityPercent",
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
        protected virtual DisclosureInfoPercent CalculateFundsInfoPercent(DisclosureInfo disclosureInfo)
        {
            return new DisclosureInfoPercent
            {
                Code = "FundsInfoPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            }.AddForcePercent(this.PercentAlgoritm);
        }

        /// <summary>
        /// Считает раскрытие информации по административной ответственности
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DisclosureInfoPercent CalculateAdminResponsibilityPercent(DisclosureInfo disclosureInfo)
        {
            int tempPositionsCount = 11, tempCompletePositionsCount = 0;
            decimal? percent = 0;

            if (disclosureInfo.AdminResponsibility == YesNoNotSet.No)
            {
                percent = 100;
                tempCompletePositionsCount = 11;
            }
            else if (disclosureInfo.AdminResponsibility == YesNoNotSet.Yes)
            {
                // Документы административной ответственности
                var docs = this.AdminRespDisInfo.Get(disclosureInfo.Id);
                if (docs.IsNotEmpty())
                {
                    // отметка +1, если есть документы, иначе 0%
                    tempPositionsCount = 1;
                    tempCompletePositionsCount = 1;

                    foreach (var adminResp in docs)
                    {
                        // Дата привлечения к административной ответственности
                        tempPositionsCount++;
                        if (adminResp.DateImpositionPenalty.HasValue)
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Дата привлечения к административной ответственности",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Тип лица, привлеченного к административной ответственности
                        tempPositionsCount++;
                        if (adminResp.TypePerson != TypePersonAdminResp.NotSet)
                        {
                            tempCompletePositionsCount++;

                            if (adminResp.TypePerson == TypePersonAdminResp.PhisicalPerson)
                            {
                                tempPositionsCount += 2;

                                // ФИО должностного лица
                                if (adminResp.Fio.IsNotEmpty())
                                {
                                    tempCompletePositionsCount++;
                                }
                                else
                                {
                                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                                    {
                                        FieldName = "ФИО должностного лица",
                                        PathId = DiFieldPathType.Path20,
                                        ManOrg = disclosureInfo
                                    });
                                }

                                // Должность лица, привлеченного к административной ответственности
                                if (adminResp.Position.IsNotEmpty())
                                {
                                    tempCompletePositionsCount++;
                                }
                                else
                                {
                                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                                    {
                                        FieldName = "Должность лица, привлеченного к административной ответственности",
                                        PathId = DiFieldPathType.Path20,
                                        ManOrg = disclosureInfo
                                    });
                                }
                            }
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Тип лица, привлеченного к административной ответственности",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Предмет административного нарушения
                        tempPositionsCount++;
                        if (adminResp.TypeViolation.IsNotEmpty())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Предмет административного нарушения",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Наименование контрольного органа или судебного органа
                        tempPositionsCount++;
                        if (adminResp.SupervisoryOrg.IsNotNull())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Наименование контрольного органа или судебного органа",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Количество выявленных нарушений
                        tempPositionsCount++;
                        if (adminResp.AmountViolation.HasValue)
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Количество выявленных нарушений",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Размер штрафа
                        tempPositionsCount++;
                        if (adminResp.SumPenalty.HasValue)
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Размер штрафа",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Наименование документа о применении мер административного воздействия
                        tempPositionsCount++;
                        if (adminResp.DocumentName.IsNotEmpty())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Наименование документа о применении мер административного воздействия",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Номер документа о применении мер административного воздействия
                        tempPositionsCount++;
                        if (adminResp.DocumentNum.IsNotEmpty())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Номер документа о применении мер административного воздействия",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Документ о применении мер административного воздействия
                        tempPositionsCount++;
                        if (adminResp.File.IsNotNull())
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Документ о применении мер административного воздействия",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Мероприятия, проведенные для устранения выявленных нарушений и результаты административного воздействия
                        tempPositionsCount++;
                        if (this.AnyActions.Contains(adminResp.Id))
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Мероприятия, проведенные для устранения выявленных нарушений и результаты административного воздействия",
                                PathId = DiFieldPathType.Path20,
                                ManOrg = disclosureInfo
                            });
                        }
                    }
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Есть случаи привлечения к административной ответственности в данном отчетном периоде",
                        PathId = DiFieldPathType.Path20,
                        ManOrg = disclosureInfo
                    });
                }

                percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);
            }

            return new DisclosureInfoPercent
            {
                Code = "AdminResponsibilityPercent",
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
        /// Считает раскрытие информации по лицензиям УО
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DisclosureInfoPercent CalculateLicensePercent(DisclosureInfo disclosureInfo)
        {
            int positionsCount = 5,
                completePositionsCount = 0;
            decimal? percent = 0;

            if (disclosureInfo.HasLicense == YesNoNotSet.No)
            {
                percent = 100;
                completePositionsCount = 5;
            }

            if (disclosureInfo.HasLicense == YesNoNotSet.Yes)
            {
                var licensies = this.DisclosureInfoLicense.Get(disclosureInfo.Id);

                if (licensies.IsNotEmpty())
                {
                    // отметка +1, если есть документы, иначе 0%
                    completePositionsCount = 1;

                    var maxPositionsCount = 0;

                    foreach (var license in licensies)
                    {
                        var curPositionsCount = 0;

                        if (license.LicenseNumber.IsNotEmpty())
                        {
                            curPositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Номер лицензии",
                                PathId = DiFieldPathType.Path22,
                                ManOrg = disclosureInfo
                            });
                        }

                        if (license.DateReceived > DateTime.MinValue && license.DateReceived < DateTime.MaxValue)
                        {
                            curPositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Дата получения лицензии",
                                PathId = DiFieldPathType.Path22,
                                ManOrg = disclosureInfo
                            });
                        }

                        if (license.LicenseOrg.IsNotEmpty())
                        {
                            curPositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Орган, выдавший лицензию",
                                PathId = DiFieldPathType.Path22,
                                ManOrg = disclosureInfo
                            });
                        }

                        if (license.LicenseDoc != null)
                        {
                            curPositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Документ лицензии",
                                PathId = DiFieldPathType.Path22,
                                ManOrg = disclosureInfo
                            });
                        }

                        if (curPositionsCount > maxPositionsCount)
                        {
                            maxPositionsCount = curPositionsCount;
                        }
                    }

                    completePositionsCount += maxPositionsCount;
                    percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2);
                }
                else
                {
                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                    {
                        FieldName = "Имеется ли лицензия на осуществление деятельности по управлению многоквартирными домами",
                        PathId = DiFieldPathType.Path22,
                        ManOrg = disclosureInfo
                    });
                }
            }

            return new DisclosureInfoPercent
            {
                Code = "LicensePercent",
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
        protected virtual DisclosureInfoPercent CalculateDocumentsPercent(DisclosureInfo disclosureInfo)
        {
            return new DisclosureInfoPercent
            {
                Code = "DocumentsPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            }.AddForcePercent(this.PercentAlgoritm);
        }

        /// <summary>
        /// Считает раскрытие информации по сведениям о договорах
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected virtual DisclosureInfoPercent CalculateInfoOnContrPercent(DisclosureInfo disclosureInfo)
        {
            return new DisclosureInfoPercent
            {
                Code = "InfoOnContrPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                DisclosureInfo = disclosureInfo
            }.AddForcePercent(this.PercentAlgoritm);
        }
    }
}
