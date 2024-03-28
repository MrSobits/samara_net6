namespace Bars.GkhDi.Regions.Tatarstan.PercentCalculationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Calculating;
    using Bars.GkhDi.Calculating.Configs;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Реализация калькулятора Раскрытия информации по форме 988 для Татарстана по новому регламенту от ГЖИ
    /// </summary>
    public class PercentCalculationGji988 : PercentCalculation988
    {
        /// <summary>
        /// Домен-серв <see cref="InfoAboutPaymentCommunal"/>
        /// </summary>
        public IDomainService<InfoAboutPaymentCommunal> InfoAboutPaymentCommunalDomain { get; set; }

        protected Dictionary<long, InfoAboutPaymentCommunal> InfoAboutPaymentCommunalDict;
        protected HashSet<long> EnergyEfficiency;

        /// <inheritdoc />
        public override bool CheckByPeriod(PeriodDi periodDi)
        {
            return periodDi.DateStart >= new DateTime(2016, 1, 1);
        }

        /// <inheritdoc />
        protected override ICalcPercentAlgoritm InitCalcAlgoritmInternal()
        {
            var configs = new BlockPercentConfigs();

            // Настройки для УО
            configs
                .SetPercent("GeneralDataPercent", 0.1m)
                .SetPercent("TerminateContractPercent", 0.1m)
                .SetPercent("MembershipUnionsPercent", 0.05m)
                .SetPercent("DocumentsPercent", 0.05m)
                .SetPercent(ManorgType.Uk, "FinActivityPercent", 0.35m)
                .SetPercent(ManorgType.Uk, "LicensePercent", 0.15m)
                .SetPercent(ManorgType.Uk, "AdminResponsibilityPercent", 0.2m)
                .SetPercent(ManorgType.TsjJsk, "FinActivityPercent", 0.3m)
                .SetPercent(ManorgType.TsjJsk, "LicensePercent", 0.1m)
                .SetPercent(ManorgType.TsjJsk, "AdminResponsibilityPercent", 0.15m)
                .SetPercent(ManorgType.TsjJsk, "InfoOnContrPercent", 0.1m)
                .SetPercent(ManorgType.TsjJsk, "FundsInfoPercent", 0.05m);

            // Настройки для домов
            configs
                .SetPercent("RealityObjectGeneralDataPercent", 0.01m)
                .SetPercent("LiftsPercent", 0.01m)
                .SetPercent("RealtyObjectDevicesPercent", 0.01m)
                .SetPercent("StructElementsPercent", 0.01m)
                .SetPercent("EngineerSystemsPercent", 0.01m)
                .SetPercent("HouseManagingPercent", 0.05m)
                .SetPercent("ServicesPercent", 0.3m)
                .SetPercent("PlanWorkServiceRepairPercent", 0.05m)
                .SetPercent("PlaceGeneralUsePercent", 0.1m)
                .SetPercent("InfoAboutReductionPaymentPercent", 0.05m)
                .SetPercent("NonResidentialPlacementPercent", 0.1m)
                .SetPercent("CommunalServicesPaymentPercent", 0.1m)
                .SetPercent("FinActivityHousePercent", 0.15m)
                .SetPercent("DocumentsDiRealObjPercent", 0.05m);

            return new CoefCalcAlgoritm(configs, this.GetManagingOrganizationType);
        }

        /// <summary>
        /// Приготовить кэш для домов
        /// </summary>
        /// <param name="diRoQuery">Дома в период раскрытия информации</param>
        protected override void PrepareRealityObjectCache(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            base.PrepareRealityObjectCache(diRoQuery);

            this.InfoAboutPaymentCommunalDict = this.InfoAboutPaymentCommunalDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(x => x.Key, x => x.First());
            
            this.EnergyEfficiency = this.TehPassportValueDomain.GetAll()
                .Where(x => x.Value != null)
                .Where(x => x.FormCode == "Form_6_1_1")
                .Where(x => x.CellCode == "1:1")
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
                .Select(x => x.TehPassport.RealityObject.Id)
                .ToHashSet();
        }

        /// <summary>
        /// Считает раскрытие информации по административной ответственности
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateAdminResponsibilityPercent(DisclosureInfo disclosureInfo)
        {
            int tempPositionsCount = 1, 
                tempCompletePositionsCount = 0;
            decimal? percent = 0;

            if (disclosureInfo.AdminResponsibility == YesNoNotSet.No)
            {
                percent = 100;
                tempCompletePositionsCount = 1;
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
                                tempPositionsCount += 9;

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

                                // Предмет административного нарушения
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

                                // Количество выявленных нарушений
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

                                // Дата документа о применении мер административного воздействия
                                if (adminResp.DateFrom.HasValue)
                                {
                                    tempCompletePositionsCount++;
                                }
                                else
                                {
                                    this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                                    {
                                        FieldName = "Дата документа о применении мер административного воздействия",
                                        PathId = DiFieldPathType.Path20,
                                        ManOrg = disclosureInfo
                                    });
                                }

                                // Мероприятия, проведенные для устранения выявленных нарушений и результаты административного воздействия
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
                                FieldName = "Тип лица, привлеченного к административной ответственности",
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
        /// Считает раскрытие информации по финансовой деятельности
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateFinActivityPercent(DisclosureInfo disclosureInfo)
        {
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
                tempPositionsCount++;
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
                    tempPositionsCount++;
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
                    tempPositionsCount += 2;
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
                                PathId = DiFieldPathType.Path31,
                                ManOrg = disclosureInfo
                            });
                        }

                        // Отчет о выполнении сметы доходов и расходов за текущий год
                        if (isNotEmpty && finActivityDocs.Any(x => x.Year == periodYear && x.TypeDocByYearDi == TypeDocByYearDi.ReportEstimateIncome))
                        {
                            tempCompletePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoEmptyFields()
                            {
                                FieldName = "Отчет о выполнении сметы доходов и расходов за текущий год",
                                PathId = DiFieldPathType.Path31,
                                ManOrg = disclosureInfo
                            });
                        }
                    }
                }
            }

            var percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

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
        /// Считает раскрытие информации по общим сведениям об УО
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DisclosureInfoPercent CalculateGeneralDataPercent(DisclosureInfo disclosureInfo)
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
                    percent = 100;
                    positionsCount = 1;
                    completePositionsCount++;
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
        /// Считает раскрытие информации по планам мер по снижению расходов
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculatePlanReductionExpensePercent(DisclosureInfoRealityObjProxy realObj)
        {
            return null;
        }

        /// <summary>
        /// Считает раскрытие информации по планам работ по сведениям об использовании мест общего пользования
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculatePlaceGeneralUsePercent(DisclosureInfoRealityObjProxy realObj)
        {
            int positionsCount = 1,
                completePositionsCount = 0;

            if (realObj.PlaceGeneralUse == YesNoNotSet.No)
            {
                completePositionsCount++;
            }
            else if (realObj.PlaceGeneralUse == YesNoNotSet.Yes)
            {
                var infoAboutUseCommonFacilities = this.InfoAboutUseCommonFacilitiesDict.Get(realObj.Id);
                if (infoAboutUseCommonFacilities.IsNotEmpty())
                {
                    completePositionsCount++;

                    foreach (var info in infoAboutUseCommonFacilities)
                    {
                        positionsCount += 7;

                        if (info.AppointmentCommonFacilities.IsNotEmpty())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Назначение общего имущества",
                                PathId = DiFieldPathType.Path16,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        if (info.AreaOfCommonFacilities.HasValue)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Площадь общего имущества",
                                PathId = DiFieldPathType.Path16,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        // ИНН и ОГРН у юр. лиц не проверяем.  
                        if (info.LesseeTypeDi.Equals(LesseeTypeDi.Legal))
                        {
                            //ИНН владельца (пользователя)
                            if (info.Inn.IsNotEmpty())
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(
                                    new DisclosureInfoRealityObjEmptyFields()
                                    {
                                        FieldName = "ИНН владельца (пользователя)",
                                        PathId = DiFieldPathType.Path16,
                                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                    });
                            }

                            //ОГРН владельца (пользователя)
                            if (info.Ogrn.IsNotEmpty())
                            {
                                completePositionsCount++;
                            }
                            else
                            {
                                this.EmptyFieldsLogger.Add(
                                    new DisclosureInfoRealityObjEmptyFields()
                                    {
                                        FieldName = "ОГРН владельца (пользователя)",
                                        PathId = DiFieldPathType.Path16,
                                        RealityObj = this.GetRealityObjectProxy(realObj.Id)
                                    });
                            }
                        }
                        else
                        {
                            completePositionsCount += 2;
                        }

                        //Номер договора
                        if (info.ContractNumber.IsNotEmpty())
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Номер договора",
                                PathId = DiFieldPathType.Path16,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        //Дата договора
                        if (info.ContractDate.HasValue)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Дата договора",
                                PathId = DiFieldPathType.Path16,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }

                        //Стоимость по договору в месяц
                        if (info.CostByContractInMonth.HasValue)
                        {
                            completePositionsCount++;
                        }
                        else
                        {
                            this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                            {
                                FieldName = "Стоимость по договору в месяц",
                                PathId = DiFieldPathType.Path16,
                                RealityObj = this.GetRealityObjectProxy(realObj.Id)
                            });
                        }
                    }
                }
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Договоры на использование мест общего пользования имеются",
                    PathId = DiFieldPathType.Path16,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            return new DiRealObjPercent
            {
                Code = "PlaceGeneralUsePercent",
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
        /// Считает раскрытие информации по сведениям об оплатах коммунальных услуг
        /// </summary>
        /// <param name="realityObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculateCommunalServicesPaymentPercent(DisclosureInfoRealityObjProxy realityObj)
        {
            int positionsCount = 1,
                completePositionsCount = 0;

            var services = this.ServicesDict.Get(realityObj.Id)?
                    .Where(x => x.KindServiceDi == KindServiceDi.Communal)
                    .Where(x => this.CommunalServices.ContainsKey(x.Id) && this.CommunalServices.Get(x.Id).ProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo)
                    .ToList();

            if (services.IsEmpty())
            {
                //если нет комунальных услуг через УО, то не считаем блок и меняем настройки у других блоков
                this.PercentAlgoritm.ForceBlockCoef("FinActivityHousePercent", 0.2m);
                this.PercentAlgoritm.ForceBlockCoef("DocumentsDiRealObjPercent", 0.1m);
                this.PercentAlgoritm.ForceBlockCoef("CommunalServicesPaymentPercent", 0m);

                return new DiRealObjPercent
                {
                    Code = "CommunalServicesPaymentPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = null,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = 0,
                    CompletePositionsCount = 0,
                    ActualVersion = 1,
                    DiRealityObject = this.GetDisclosureInfoRealityObj(realityObj)
                }.AddForcePercent(this.PercentAlgoritm);
            }
            else
            {
                this.PercentAlgoritm.ForceBlockCoef("FinActivityHousePercent", 0.15m);
                this.PercentAlgoritm.ForceBlockCoef("DocumentsDiRealObjPercent", 0.05m);
                this.PercentAlgoritm.ForceBlockCoef("CommunalServicesPaymentPercent", 0.1m);

                positionsCount = 0;

                foreach (var service in services)
                {
                    var paymentInfo = this.InfoAboutPaymentCommunalDict.Get(service.Id);

                    positionsCount += 8;

                    if (paymentInfo != null)
                    {
                        completePositionsCount += this.ValidateInfoPaymentCommunal(paymentInfo);
                    }
                    else
                    {
                        this.EmptyFieldsLogger.Add(
                            new DisclosureInfoRealityObjEmptyFields
                            {
                                FieldName = $"Сведения об оплатах коммунальных услуг({service.Name})",
                                PathId = DiFieldPathType.Path40,
                                RealityObj = this.GetRealityObjectProxy(realityObj.Id)
                            });
                    }
                }
            }
            return new DiRealObjPercent
            {
                Code = "CommunalServicesPaymentPercent",
                TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                Percent = (decimal.Divide(completePositionsCount, positionsCount) * 100).RoundDecimal(2),
                CalcDate = DateTime.Now.Date,
                PositionsCount = positionsCount,
                CompletePositionsCount = completePositionsCount,
                ActualVersion = 1,
                DiRealityObject = this.GetDisclosureInfoRealityObj(realityObj)
            };
        }

        /// <summary>
        /// Считает раскрытие информации по общим сведениям о доме
        /// </summary>
        /// <param name="realObj">Дом в периоде раскрытия информации</param>
        /// <returns>Процент блока</returns>
        protected override DiRealObjPercent CalculateRealityObjectGeneralDataPercent(DisclosureInfoRealityObjProxy realObj)
        {
            var diPercent = base.CalculateRealityObjectGeneralDataPercent(realObj);
            diPercent.PositionsCount++;

            if (this.EnergyEfficiency.Contains(realObj.RoId))
            {
                diPercent.CompletePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields()
                {
                    FieldName = "Класс энергетической эффективности",
                    PathId = DiFieldPathType.Path11,
                    RealityObj = this.GetRealityObjectProxy(realObj.Id)
                });
            }

            diPercent.Percent = (decimal.Divide(diPercent.CompletePositionsCount, diPercent.PositionsCount) * 100).RoundDecimal(2);
            return diPercent;
        }

        private int ValidateInfoPaymentCommunal(InfoAboutPaymentCommunal info)
        {
            int completePositionsCount = 0;

            if (info.TotalConsumption.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                {
                    FieldName = "Общий объем потребления",
                    PathId = DiFieldPathType.Path40,
                    RealityObj = this.GetRealityObjectProxy(info.DisclosureInfoRealityObj.Id)
                });
            }

            if (info.Accrual.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                {
                    FieldName = "Начислено потребителями",
                    PathId = DiFieldPathType.Path40,
                    RealityObj = this.GetRealityObjectProxy(info.DisclosureInfoRealityObj.Id)
                });
            }

            if (info.Payed.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                {
                    FieldName = "Оплачено потребителями",
                    PathId = DiFieldPathType.Path40,
                    RealityObj = this.GetRealityObjectProxy(info.DisclosureInfoRealityObj.Id)
                });
            }

            if (info.Debt.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                {
                    FieldName = "Задолженность потребителей",
                    PathId = DiFieldPathType.Path40,
                    RealityObj = this.GetRealityObjectProxy(info.DisclosureInfoRealityObj.Id)
                });
            }

            if (info.AccrualByProvider.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                {
                    FieldName = "Начислено поставщиком (поставщиками) коммунального ресурса",
                    PathId = DiFieldPathType.Path40,
                    RealityObj = this.GetRealityObjectProxy(info.DisclosureInfoRealityObj.Id)
                });
            }

            if (info.PayedToProvider.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                {
                    FieldName = "Оплачено поставщику (поставщикам) коммунального ресурса",
                    PathId = DiFieldPathType.Path40,
                    RealityObj = this.GetRealityObjectProxy(info.DisclosureInfoRealityObj.Id)
                });
            }

            if (info.DebtToProvider.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                {
                    FieldName = "Задолженность перед поставщиком (поставщиками) коммунального ресурса",
                    PathId = DiFieldPathType.Path40,
                    RealityObj = this.GetRealityObjectProxy(info.DisclosureInfoRealityObj.Id)
                });
            }

            if (info.ReceivedPenaltySum.HasValue)
            {
                completePositionsCount++;
            }
            else
            {
                this.EmptyFieldsLogger.Add(new DisclosureInfoRealityObjEmptyFields
                {
                    FieldName = "Сумма пени и штрафов, полученных от потребителей",
                    PathId = DiFieldPathType.Path40,
                    RealityObj = this.GetRealityObjectProxy(info.DisclosureInfoRealityObj.Id)
                });
            }

            return completePositionsCount;
        }

        private ManorgType GetManagingOrganizationType(BasePercent percent)
        {
            var diPercent = percent as DisclosureInfoPercent;
            if (diPercent != null)
            {
                return diPercent.DisclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK
                    ? ManorgType.Uk
                    : ManorgType.TsjJsk;
            }

            var roPercent = percent as DiRealObjPercent;
            if (roPercent != null)
            {
                var manorg = this.ManorgContracts.Get(roPercent.DiRealityObject.RealityObject.Id)?.ManagingOrganization.TypeManagement;
                if (manorg != null)
                {
                    return manorg == TypeManagementManOrg.UK
                    ? ManorgType.Uk
                    : ManorgType.TsjJsk;
                }
            }

            return ManorgType.NotManorg;
        }
    }
}