namespace Bars.GkhCr.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Report.ComparePrograms;

    using Castle.Windsor;

    /// <summary>
    /// Отчет по сверке программ КР
    /// </summary>
    public class CompareProgramsReport : BasePrintForm
    {
        private long programCrOneId;

        private long programCrTwoId;

        private long[] municipalityIds;

        private long[] financialSourceIds;

        public CompareProgramsReport()
            : base(new ReportTemplateBinary(Properties.Resources.ComparePrograms))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Отчет по сверке программ КР"; }
        }

        public override string Desciption
        {
            get { return "Отчет по сверке программ КР"; }
        }

        public override string GroupName
        {
            get { return "Ход капремонта"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CompareProgramsReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.ComparePrograms"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrOneId = baseParams.Params["ProgramCrOne"].ToInt();
            programCrTwoId = baseParams.Params["ProgramCrTwo"].ToInt();
            var m = baseParams.Params["Municipalities"].ToStr();
            this.municipalityIds = !string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];

            var financialSourceIdStr = baseParams.Params["FinanceSource"].ToStr();
            this.financialSourceIds = string.IsNullOrEmpty(financialSourceIdStr) ? new long[0] : financialSourceIdStr.Split(',').Select(x => x.ToLong()).ToArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var servProgramCr = Container.Resolve<IDomainService<ProgramCr>>();
            var programCrOne = servProgramCr.Get(programCrOneId);
            var programCrTwo = servProgramCr.Get(programCrTwoId);
            var compareProgramsData = new CompareProgramsData();
            var data = compareProgramsData.GetData(Container, programCrOne, programCrTwo, municipalityIds, financialSourceIds);

            reportParams.SimpleReportParams["Год1"] = programCrOne.Period.DateStart.Year;
            reportParams.SimpleReportParams["Год2"] = programCrTwo.Period.DateStart.Year;
            reportParams.SimpleReportParams["Программа1"] = programCrTwo.Name;
            reportParams.SimpleReportParams["Программа2"] = programCrTwo.Name;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияГруппа");
           
            Section sectionMunicipality = null;
            foreach (var recordForGroup in data)
            {
                section.ДобавитьСтроку();
                if (!string.IsNullOrEmpty(recordForGroup.Group))
                {
                    var sectionGroupName = section.ДобавитьСекцию("СекцияНазваниеГруппы");
                    sectionGroupName.ДобавитьСтроку();
                    sectionGroupName["НазваниеГруппы"] = recordForGroup.Group;
                }

                foreach (var recordMunicipality in recordForGroup.Municipalities)
                {
                    var number = 0;
                    sectionMunicipality = section.ДобавитьСекцию("СекцияРайон");
                    sectionMunicipality.ДобавитьСтроку();
                    sectionMunicipality["НазваниеРайона"] = recordMunicipality.Value.Municipality;

                    var overLimit = recordMunicipality.Value.OverLimit();

                    if (overLimit[1] > overLimit[0])
                    {
                        sectionMunicipality["ЛимитПревышен"] = string.Format("ЛИМИТ ФИНАНСИРОВАНИЯ ПО РАЙОНУ ПРЕВЫШЕН! (ЛИМИТ ПРОГРАММЫ КАПРЕМОНТА {0}, ЛИМИТ ТЕКУШЕЙ ПРОГРАММЫ {1}, РАЗНИЦА {2})", overLimit[0], overLimit[1], overLimit[1] - overLimit[0]);
                    }

                    var sectionRealtyObjects = sectionMunicipality.ДобавитьСекцию("СекцияОбъект");
                    foreach (var realtyObject in recordMunicipality.Value.RealtyObjects)
                    {
                        ++number;
                        sectionRealtyObjects.ДобавитьСтроку();
                        sectionRealtyObjects["Номер"] = number;
                        var sectionGroupFinances = sectionRealtyObjects.ДобавитьСекцию("СекцияГруппыФинансирования");
                        foreach (var recGroupFinances in realtyObject.Value.GroupFinances)
                        {
                            sectionGroupFinances.ДобавитьСтроку();
                            sectionGroupFinances["Номер"] = number;
                            var sectionSources = sectionGroupFinances.ДобавитьСекцию("СекцияИсточникФинансирования");
                            foreach (var recSource in recGroupFinances.Value.Sources)
                            {
                                sectionSources.ДобавитьСтроку();
                                sectionSources["Номер"] = number;
                                FillRealtyObject(sectionSources, realtyObject.Value, recSource.Value.Source);

                                FillWorks(sectionSources, recSource.Value.WorksOne, "1", recSource.Value);
                                FillWorks(sectionSources, recSource.Value.WorksTwo, "2", recSource.Value);

                                FillBudget(sectionSources, "1", recSource.Value.BudgetOne);
                                FillBudget(sectionSources, "2", recSource.Value.BudgetTwo);

                                FillDifferencesForWork(sectionSources, recSource.Value, recGroupFinances.Value);
                                FillDifferencesForSource(sectionSources, recSource.Value);
                            }

                            FillTotalGroupFinance(sectionGroupFinances, recGroupFinances.Value);
                        }

                        FillTotalRealtyObject(sectionRealtyObjects, realtyObject.Value);
                    }

                    var sectionTotal = sectionMunicipality.ДобавитьСекцию("СекцияИтоги");
                    FillTotal(sectionTotal, "Итого по району", recordMunicipality.Value.Total);
                }

                if (!string.IsNullOrEmpty(recordForGroup.Group) && sectionMunicipality != null)
                {
                    var sectionTotalGroup = sectionMunicipality.ДобавитьСекцию("СекцияИтоги");
                    FillTotal(sectionTotalGroup, "Итого по группе", recordForGroup.Grand);
                }
            }

            if (sectionMunicipality != null)
            {
                var sectionTotal = sectionMunicipality.ДобавитьСекцию("СекцияИтоги");
                FillTotal(sectionTotal, "Итого", compareProgramsData.GrandTotal);
            }
        }

        private void FillTotal(Section section, string name, RecordRealtyObject total)
        {
            section.ДобавитьСтроку();
            section["НазваниеИтогов"] = name;
            var sectionGroupFinances = section.ДобавитьСекцию("СекцияИтогиГруппыФинансирования");
            foreach (var recGroupFinances in total.GroupFinances)
            {
                sectionGroupFinances.ДобавитьСтроку();
                var sectionSources = sectionGroupFinances.ДобавитьСекцию("СекцияИтогиИсточникФинансирования");
                foreach (var recSource in recGroupFinances.Value.Sources)
                {
                    sectionSources.ДобавитьСтроку();

                    sectionSources["ИсточникФинансирования"] = recSource.Value.Source;
                    sectionSources["ОбщаяПл1"] = total.RecordOne.AreaMkd;
                    sectionSources["ЖилаяПл1"] = total.RecordOne.AreaLiving;
                    sectionSources["ПлЖилИНеЖил1"] = total.RecordOne.AreaLivingNotLivingMkd;
                    sectionSources["ЖилаяПлГр1"] = total.RecordOne.AreaLivingOwned;
                    sectionSources["КолКвартир1"] = total.RecordOne.NumberApartments;
                    sectionSources["КолПроживающих1"] = total.RecordOne.NumberLiving;

                    sectionSources["ОбщаяПл2"] = total.RecordTwo.AreaMkd;
                    sectionSources["ЖилаяПл2"] = total.RecordTwo.AreaLiving;
                    sectionSources["ПлЖилИНеЖил2"] = total.RecordTwo.AreaLivingNotLivingMkd;
                    sectionSources["ЖилаяПлГр2"] = total.RecordTwo.AreaLivingOwned;
                    sectionSources["КолКвартир2"] = total.RecordTwo.NumberApartments;
                    sectionSources["КолПроживающих2"] = total.RecordTwo.NumberLiving;

                    FillWorks(sectionSources, recSource.Value.WorksOne, "1", recSource.Value);
                    FillWorks(sectionSources, recSource.Value.WorksTwo, "2", recSource.Value);

                    FillBudget(sectionSources, "1", recSource.Value.BudgetOne);
                    FillBudget(sectionSources, "2", recSource.Value.BudgetTwo);

                    FillDifferencesForWork(sectionSources, recSource.Value, recGroupFinances.Value);
                    FillDifferencesForSource(sectionSources, recSource.Value);
                }

                FillTotalGroupFinance(sectionGroupFinances, recGroupFinances.Value);
            }

            FillTotalRealtyObject(section, total);
        }

        private void FillRealtyObject(Section sectionSources, RecordRealtyObject recordRealtyObject, string source)
        {
            sectionSources["ИсточникФинансирования"] = source;
            var objectCrOne = recordRealtyObject.RecordOne;
            if (objectCrOne != null)
            {
                sectionSources["Адрес1"] = objectCrOne.Address;
                sectionSources["ТипУправления1"] = objectCrOne.TypeManagement.GetEnumMeta().Display;
                sectionSources["УпрОрг1"] = objectCrOne.ManagementOrganization;
                sectionSources["НасПунктУпрОрг1"] = objectCrOne.PlaceAddressManOrg;
                sectionSources["Индекс1"] = objectCrOne.PostCode;
                sectionSources["Улица1"] = objectCrOne.Street;
                sectionSources["Дом1"] = objectCrOne.House;
                sectionSources["Квартира1"] = objectCrOne.Flat;
                sectionSources["ИНН1"] = objectCrOne.Inn;
                sectionSources["КПП1"] = objectCrOne.Kpp;
                sectionSources["ФИО1"] = objectCrOne.Leader;
                sectionSources["Телефон1"] = objectCrOne.Phone;
                sectionSources["ЭлПочта1"] = objectCrOne.Email;
                sectionSources["Этажность1"] = objectCrOne.MaximumFloors;
                sectionSources["Серия1"] = objectCrOne.SeriesHome;
                sectionSources["ГруппаКапитальности1"] = objectCrOne.CapitalGroup;
                sectionSources["МатериалСтен1"] = objectCrOne.WallMaterial;
                sectionSources["МатериалКровли1"] = objectCrOne.RoofingMaterial;
                sectionSources["ГодСдачиВЭкспл1"] = objectCrOne.YearCommissioning;
                sectionSources["ФизичИзнос1"] = objectCrOne.PhysicalWear;
                sectionSources["ГодПоследнегоКапРем1"] = objectCrOne.YearLastOverhaul;

                sectionSources["ОбщаяПл1"] = objectCrOne.AreaMkd;
                sectionSources["ЖилаяПл1"] = objectCrOne.AreaLiving;
                sectionSources["ПлЖилИНеЖил1"] = objectCrOne.AreaLivingNotLivingMkd;
                sectionSources["ЖилаяПлГр1"] = objectCrOne.AreaLivingOwned;
                sectionSources["КолКвартир1"] = objectCrOne.NumberApartments;
                sectionSources["КолПроживающих1"] = objectCrOne.NumberLiving;
            }

            var objectCrTwo = recordRealtyObject.RecordTwo;
            if (objectCrTwo != null)
            {
                sectionSources["Адрес2"] = objectCrTwo.Address;
                sectionSources["ТипУправления2"] = objectCrTwo.TypeManagement.GetEnumMeta().Display;
                sectionSources["УпрОрг2"] = objectCrTwo.ManagementOrganization;
                sectionSources["НасПунктУпрОрг2"] = objectCrTwo.PlaceAddressManOrg;
                sectionSources["Индекс2"] = objectCrTwo.PostCode;
                sectionSources["Улица2"] = objectCrTwo.Street;
                sectionSources["Дом2"] = objectCrTwo.House;
                sectionSources["Квартира2"] = objectCrTwo.Flat;
                sectionSources["ИНН2"] = objectCrTwo.Inn;
                sectionSources["КПП2"] = objectCrTwo.Kpp;
                sectionSources["ФИО2"] = objectCrTwo.Leader;
                sectionSources["Телефон2"] = objectCrTwo.Phone;
                sectionSources["ЭлПочта2"] = objectCrTwo.Email;
                sectionSources["Этажность2"] = objectCrTwo.MaximumFloors;
                sectionSources["Серия2"] = objectCrTwo.SeriesHome;
                sectionSources["ГруппаКапитальности2"] = objectCrTwo.CapitalGroup;
                sectionSources["МатериалСтен2"] = objectCrTwo.WallMaterial;
                sectionSources["МатериалКровли2"] = objectCrTwo.RoofingMaterial;
                sectionSources["ГодСдачиВЭкспл2"] = objectCrTwo.YearCommissioning;
                sectionSources["ФизичИзнос2"] = objectCrTwo.PhysicalWear;
                sectionSources["ГодПоследнегоКапРем2"] = objectCrTwo.YearLastOverhaul;

                sectionSources["ОбщаяПл2"] = objectCrTwo.AreaMkd;
                sectionSources["ЖилаяПл2"] = objectCrTwo.AreaLiving;
                sectionSources["ПлЖилИНеЖил2"] = objectCrTwo.AreaLivingNotLivingMkd;
                sectionSources["ЖилаяПлГр2"] = objectCrTwo.AreaLivingOwned;
                sectionSources["КолКвартир2"] = objectCrTwo.NumberApartments;
                sectionSources["КолПроживающих2"] = objectCrTwo.NumberLiving;
            }
        }

        private void FillWorks(Section sectionSources, Dictionary<int, RecordWorks> works, string number, RecordForSource recordForSource)
        {
            var psd = new RecordWorks();
            var tehInspection = new RecordWorks();

            foreach (var rec in works)
            {
                if (rec.Key == 1018 || rec.Key == 1019)
                {
                    psd += rec.Value;
                    continue;
                }

                if (rec.Key == 1020)
                {
                    tehInspection += rec.Value;
                    continue;
                }

                if (rec.Value.Volume != 0)
                {
                    sectionSources[string.Format("Объем{0}{1}", number, rec.Key)] = rec.Value.Volume;
                }

                if (rec.Value.Sum != 0)
                {
                    sectionSources[string.Format("Сумма{0}{1}", number, rec.Key)] = rec.Value.Sum;
                }
            }

            if (!psd.IsZero())
            {
                sectionSources[string.Format("СуммаНаРазИЭкспПСД{0}", number)] = psd.Sum;
            }

            if (!tehInspection.IsZero())
            {
                sectionSources[string.Format("СуммаНаТехНадзор{0}", number)] = tehInspection.Sum;
            }

            sectionSources[string.Format("СуммаПоВидам{0}", number)] = number == "1" ? recordForSource.SumWorksOne : recordForSource.SumWorksTwo;
        }

        private void FillBudget(Section sectionSources, string number, Budget budget)
        {
            sectionSources[string.Format("СредстваФонда{0}", number)] = budget.BudgetRf;
            sectionSources[string.Format("БюджетРТ{0}", number)] = budget.BudgetRt;
            sectionSources[string.Format("БюджетМО{0}", number)] = budget.BudgetMu;
            sectionSources[string.Format("СредстваСобственников{0}", number)] = budget.BudgetOwners;
        }

        private void FillDifferencesForWork(Section sectionSources, RecordForSource recForSource, RecordGroupFinance recGroupFinance)
        {
            var psd = new RecordWorks();
            var tehInspection = new RecordWorks();

            foreach (var rec in recForSource.WorksOne)
            {
                if (rec.Key == 1018 || rec.Key == 1019)
                {
                    psd -= rec.Value;

                    if (recForSource.WorksTwo.ContainsKey(rec.Key))
                    {
                        psd += recForSource.WorksTwo[rec.Key];
                    }

                    continue;
                }

                if (rec.Key == 1020)
                {
                    tehInspection -= rec.Value;

                    if (recForSource.WorksTwo.ContainsKey(rec.Key))
                    {
                        tehInspection += recForSource.WorksTwo[rec.Key];
                    }

                    continue;
                }

                if (recForSource.WorksTwo.ContainsKey(rec.Key))
                {
                    var difference = recForSource.WorksTwo[rec.Key] - rec.Value;

                    sectionSources[string.Format("Объем{0}ИтогоПоИсточнику", rec.Key)] = difference.Volume;
                    sectionSources[string.Format("Сумма{0}ИтогоПоИсточнику", rec.Key)] = difference.Sum;

                    recGroupFinance.Total.AddGrandWork(rec.Key, difference);
                }
                else
                {
                    sectionSources[string.Format("Объем{0}ИтогоПоИсточнику", rec.Key)] = -rec.Value.Volume;
                    sectionSources[string.Format("Сумма{0}ИтогоПоИсточнику", rec.Key)] = -rec.Value.Sum;

                    recGroupFinance.Total.AddGrandWork(rec.Key, new RecordWorks() - rec.Value);
                }

                sectionSources["СуммаПоВидамИтогоПоИсточнику"] = recForSource.SumWorksTwo - recForSource.SumWorksOne;
            }

            foreach (var rec in recForSource.WorksTwo.Where(x => !recForSource.WorksOne.ContainsKey(x.Key)))
            {
                if (rec.Key == 1018 || rec.Key == 1019)
                {
                    psd += rec.Value;
                    continue;
                }

                if (rec.Key == 1020)
                {
                    tehInspection += rec.Value;
                    continue;
                }

                sectionSources[string.Format("Объем{0}ИтогоПоИсточнику", rec.Key)] = rec.Value.Volume;
                sectionSources[string.Format("Сумма{0}ИтогоПоИсточнику", rec.Key)] = rec.Value.Sum;

                recGroupFinance.Total.AddGrandWork(rec.Key, rec.Value);
            }

            if (!psd.IsZero())
            {
                sectionSources["СуммаНаРазИЭкспПСДИтогоПоИсточнику"] = psd.Sum;
                recGroupFinance.Total.Psd += psd.Sum;
            }

            if (!tehInspection.IsZero())
            {
                sectionSources["СуммаНаТехНадзорИтогоПоИсточнику"] = tehInspection.Sum;
                recGroupFinance.Total.TehInspection += tehInspection.Sum;
            }

            recGroupFinance.Total.Budget.Add(recForSource.GetDifferenceForFunds());
        }

        private void FillDifferencesForSource(Section sectionSources, RecordForSource recForSource)
        {
            sectionSources["СредстваФондаИтогоПоИсточнику"] = recForSource.BudgetTwo.BudgetRf - recForSource.BudgetOne.BudgetRf;
            sectionSources["БюджетРТИтогоПоИсточнику"] = recForSource.BudgetTwo.BudgetRt - recForSource.BudgetOne.BudgetRt;
            sectionSources["БюджетМОИтогоПоИсточнику"] = recForSource.BudgetTwo.BudgetMu - recForSource.BudgetOne.BudgetMu;
            sectionSources["СредстваСобственниковИтогоПоИсточнику"] = recForSource.BudgetTwo.BudgetOwners - recForSource.BudgetOne.BudgetOwners;
        }

        /// <summary>
        ///  Заполнение итогов по группе источника финансирования
        /// </summary>
        /// <param name="sectionGroupFinances"></param>
        /// <param name="recGroupFinance"></param>
        private void FillTotalGroupFinance(Section sectionGroupFinances, RecordGroupFinance recGroupFinance)
        {
            var total = recGroupFinance.Total;

            sectionGroupFinances["НазваниеИтоговПоГруппеИсточников"] = string.Format("Итоги по группе {0}", recGroupFinance.GroupSource);

            foreach (var rec in recGroupFinance.Total.Works)
            {
                if (rec.Value.Volume != 0)
                {
                    sectionGroupFinances[string.Format("Объем{0}ИтогоПоГруппеИсточников", rec.Key)] = rec.Value.Volume;
                }

                if (rec.Value.Sum != 0)
                {
                    sectionGroupFinances[string.Format("Сумма{0}ИтогоПоГруппеИсточников", rec.Key)] = rec.Value.Sum;
                }
            }

            sectionGroupFinances["СуммаНаРазИЭкспПСДИтогоПоГруппеИсточников"] = total.Psd;
            sectionGroupFinances["СуммаНаТехНадзорИтогоПоГруппеИсточников"] = total.TehInspection;

            var totalWors = total.Works.Sum(x => x.Value.Sum) + total.Psd + total.TehInspection;
            sectionGroupFinances["СуммаПоВидамИтогоПоГруппеИсточников"] = totalWors;
            sectionGroupFinances["СредстваФондаИтогоПоГруппеИсточников"] = total.Budget.BudgetRf;
            sectionGroupFinances["БюджетРТИтогоПоГруппеИсточников"] = total.Budget.BudgetRt;
            sectionGroupFinances["БюджетМОИтогоПоГруппеИсточников"] = total.Budget.BudgetMu;
            sectionGroupFinances["СредстваСобственниковИтогоПоГруппеИсточников"] = total.Budget.BudgetOwners;
        }

        private void FillTotalRealtyObject(Section sectionRealtyObjects, RecordRealtyObject recRealtyObject)
        {
            var total = recRealtyObject.GetTotals();

            foreach (var rec in total.Works)
            {
                if (rec.Value.Volume != 0)
                {
                    sectionRealtyObjects[string.Format("Объем{0}ИтогоПоОбъекту", rec.Key)] = rec.Value.Volume;
                }

                if (rec.Value.Sum != 0)
                {
                    sectionRealtyObjects[string.Format("Сумма{0}ИтогоПоОбъекту", rec.Key)] = rec.Value.Sum;
                }
            }

            sectionRealtyObjects["СуммаНаРазИЭкспПСДИтогоПоОбъекту"] = total.Psd;
            sectionRealtyObjects["СуммаНаТехНадзорИтогоПоОбъекту"] = total.TehInspection;

            var grandsForWors = total.Works.Sum(x => x.Value.Sum) + total.Psd + total.TehInspection;
            sectionRealtyObjects["СуммаПоВидамИтогоПоОбъекту"] = grandsForWors;
            sectionRealtyObjects["СредстваФондаИтогоПоОбъекту"] = total.Budget.BudgetRf;
            sectionRealtyObjects["БюджетРТИтогоПоОбъекту"] = total.Budget.BudgetRt;
            sectionRealtyObjects["БюджетМОИтогоПоОбъекту"] = total.Budget.BudgetMu;
            sectionRealtyObjects["СредстваСобственниковИтогоПоОбъекту"] = total.Budget.BudgetOwners;
        }
    }
}
