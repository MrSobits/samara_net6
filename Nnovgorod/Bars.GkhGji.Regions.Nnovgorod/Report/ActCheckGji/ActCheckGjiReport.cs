namespace Bars.GkhGji.Regions.Nnovgorod.Report.ActCheckGji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;

    public class ActCheckGjiReport : GkhGji.Report.ActCheckGjiReport
    {
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>()
                       {
                           new TemplateInfo()
                               {
                                   Code = "BlockGJI_ActSurvey_5001",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки на 1 дом, Коды видов проверок 1,2,3,4,7,9 и типы обследования 5001, 5002, 5003, 5004, 5005, 5006, 5007, 5009, 5011, 5012, 5015, 5019, 5020 ",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_5001
                               },
                           new TemplateInfo()
                               {
                                   Code = "BlockGJI_ActSurvey_5001_all",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки общий, Коды видов проверок 1,2,3,4,7,9 и типы обследования 5001, 5002, 5003, 5004, 5005, 5006, 5007, 5009, 5011, 5012, 5015, 5019, 5020",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_5001_all
                               },
                           new TemplateInfo()
                               {
                                   Code = "BlockGJI_ActSurvey_5002",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки на 1 дом, Коды видов проверок 5,6 и типы обследования 5008, 5010, 5013, 5017",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_5002
                               },
                           new TemplateInfo()
                               {
                                   Code = "BlockGJI_ActSurvey_5002_all",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки общий, Коды видов проверок 5,6 и типы обследования 5008, 5010, 5013, 5017",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_5002_all
                               },
                           new TemplateInfo()
                               {
                                   Code = "BlockGJI_ActSurvey_5001_3",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки на 1 дом, Коды видов проверок 3,4,7,9 и типы обследования 5014, 5016",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_5001_3
                               }
                       };
        }

        protected override void GetCodeTemplate(ActCheck act, Disposal disposal, TypeSurveyGji[] types)
        {
            CodeTemplate = "BlockGJI_ActSurvey_5001";

            var listTypeSurvType1_9 = new List<string> { "5001", "5002", "5003", "5004", "5005", "5006", "5007", "5009", "5011", "5012", "5015", "5019", "5020" };
            var listTypeSurvType5_6 = new List<string> { "5008", "5010", "5013", "5017" };

            if (types.Any(x => listTypeSurvType1_9.Contains(x.Code)) && disposal.KindCheck != null)
            {
                switch (disposal.KindCheck.Code)
                {
                    case TypeCheck.PlannedDocumentation:
                    case TypeCheck.NotPlannedDocumentation:
                    case TypeCheck.PlannedDocumentationExit:
                    case TypeCheck.NotPlannedDocumentationExit:
                    case TypeCheck.PlannedExit:
                    case TypeCheck.NotPlannedExit:

                        if (act.TypeActCheck == TypeActCheckGji.ActCheckIndividual)
                        {
                            CodeTemplate = "BlockGJI_ActSurvey_5001";
                        }
                        if (act.TypeActCheck == TypeActCheckGji.ActCheckGeneral)
                        {
                            CodeTemplate = "BlockGJI_ActSurvey_5001_all";
                        }
                        return;
                }
            }

            if (types.Any(x => listTypeSurvType5_6.Contains(x.Code)))
            {
                switch (disposal.KindCheck.Code)
                {
                    case TypeCheck.InspectionSurvey:
                    case TypeCheck.Monitoring:
                        if (act.TypeActCheck == TypeActCheckGji.ActCheckIndividual)
                        {
                            CodeTemplate = "BlockGJI_ActSurvey_5002";
                        }
                        if (act.TypeActCheck == TypeActCheckGji.ActCheckGeneral)
                        {
                            CodeTemplate = "BlockGJI_ActSurvey_5002_all";
                        }
                        return;
                }
            }

            if (types.Any(x => x.Code == "5014" || x.Code == "5016"))
            {
                switch (disposal.KindCheck.Code)
                {
                    case TypeCheck.PlannedDocumentationExit:
                    case TypeCheck.NotPlannedDocumentationExit:
                    case TypeCheck.PlannedExit:
                    case TypeCheck.NotPlannedExit:
                        if (act.TypeActCheck == TypeActCheckGji.ActCheckIndividual)
                        {
                            CodeTemplate = "BlockGJI_ActSurvey_5001_3";
                        }
                        return;
                }
            }
        }

        protected override void FillSectionViolations(ReportParams reportParams, ActCheck act, long disposalId = 0)
        {
            var serviceActCheckViolation = Container.Resolve<IDomainService<ActCheckViolation>>().GetAll();
            var queryActViols = serviceActCheckViolation.Where(x => x.Document.Id == act.Id);
            var queryActInspViol = queryActViols.Select(x => x.InspectionViolation.Id);
            var actViols = queryActViols.Select(x => new
                                                         {
                                                             roId = x.ActObject.RealityObject.Id,
                                                             inspId = x.InspectionViolation.Id,
                                                             violId = x.InspectionViolation.Violation.Id,
                                                             violName = x.InspectionViolation.Violation.Name,
                                                             violCodePin = x.InspectionViolation.Violation.CodePin
                                                         })
                                     .ToList()
                                     .GroupBy(x => x.roId)
                                     .ToDictionary(x => x.Key, v => v.Select(x => new { x.inspId, x.violId, x.violName, x.violCodePin}).ToList());

            var listRoId = actViols.Keys.Distinct().ToList();

            var dictRo = Container.Resolve<IDomainService<RealityObject>>()
                                    .GetAll()
                                    .Where(x => listRoId.Contains(x.Id))
                                    .Select(x => new
                                                     {
                                                         x.Id, 
                                                         fiasAddressName = x.FiasAddress.AddressName,
                                                         x.WallMaterial,
                                                         x.AreaMkd,
                                                         x.Floors,
                                                         x.DateCommissioning,
                                                         x.RoofingMaterial,
                                                         x.HavingBasement,
                                                         x.NumberEntrances
                                                     })
                                    .ToList()
                                    .GroupBy(x => x.Id)
                                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            var disposalProvidedDocs = Container.Resolve<IDomainService<DisposalProvidedDoc>>()
                                                .GetAll()
                                                .Where(x => x.Disposal.Id == disposalId)
                                                .Select(x => x.ProvidedDoc.Name)
                                                .AsEnumerable()
                                                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

            var dictInspViolWording = Container.Resolve<IDomainService<InspectionGjiViolWording>>().GetAll()
                                                .Where(x => queryActInspViol.Contains(x.InspectionViolation.Id))
                                                .Select(x => new { x.Wording, inspViolsId = x.InspectionViolation.Id })
                                                .ToArray()
                                                .GroupBy(x => x.inspViolsId)
                                                .ToDictionary(x => x.Key, v => string.Join("; ", v.Select(y => y.Wording)));

            var actCheckRealtyObjects = Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                                               .Where(x => x.ActCheck.Id == act.Id)
                                               .Select(x => new
                                               {
                                                   x.RealityObject.Id,
                                                   x.NotRevealedViolations
                                               })
                                               .ToArray();
            #region Секция нарушений
            if (actViols.Count > 0)
            {
                var sectionHouse = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияПодому");
                foreach (var item in actViols)
                {
                    var ro = dictRo[item.Key];
                    if (ro != null)
                    {
                        sectionHouse.ДобавитьСтроку();
                        var sectionViolation = sectionHouse.ДобавитьСекцию("СекцияНарушений");

                        sectionHouse["ДомАдрес"] = ro.fiasAddressName;

                        sectionHouse["МатериалСтен"] = ro.WallMaterial != null ? ro.WallMaterial.Name : string.Empty;
                        sectionHouse["ОбщаяПлощадь"] = ro.AreaMkd.HasValue ? ro.AreaMkd.Value.ToStr() : string.Empty;
                        sectionHouse["ПроверПлощадь"] = act.Area.HasValue ? act.Area.Value.ToStr() : string.Empty;
                        sectionHouse["Этажей"] = ro.Floors.HasValue ? ro.Floors.Value.ToStr() : string.Empty;
                        sectionHouse["ГодСдачи"] = ro.DateCommissioning.HasValue ? ro.DateCommissioning.Value.ToShortDateString() : string.Empty;
                        sectionHouse["Кровля"] = ro.RoofingMaterial != null ? ro.RoofingMaterial.Name : string.Empty;
                        sectionHouse["Подвал"] = ro.HavingBasement;
                        sectionHouse["Секций"] = ro.NumberEntrances.ToInt();

                        sectionHouse["ПредоставляемыеДокументы"] = disposalProvidedDocs;                        

                        var addedViols = new List<long>();

                        int i = 1;
                        foreach (var viol in item.Value.Where(viol => !addedViols.Contains(viol.violId)))
                        {
                            addedViols.Add(viol.violId);

                            sectionViolation.ДобавитьСтроку();
                            sectionViolation["Номер1"] = i++;
                            sectionViolation["Пункт"] = viol.violCodePin;
                            sectionViolation["ТекстНарушения"] = viol.violName;
                            sectionViolation["ФормулировкаНарушения"] = dictInspViolWording.ContainsKey(viol.inspId)
                                                                            ? dictInspViolWording[viol.inspId]
                                                                            : string.Empty;
                        }

                        sectionHouse["НевыявленныеНарушения"] = actCheckRealtyObjects.Where(x => x.Id == ro.Id).Select(x => x.NotRevealedViolations).FirstOrDefault();
                    }
                }
            #endregion            
            }

            var sectionViolationsNN = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушенийНН");
            var inspViols = queryActViols
                .Select(x => new
                            {
                                inspId = x.InspectionViolation.Id,
                                CodePIN = x.InspectionViolation.Violation.CodePin
                            })
                .ToArray();

            var ii = 1;
            foreach (var inspViol in inspViols.Distinct())
            {
                sectionViolationsNN.ДобавитьСтроку();
                sectionViolationsNN["Номер1"] = ii++;
                sectionViolationsNN["ФормулировкаНарушения"] = dictInspViolWording.ContainsKey(inspViol.inspId)
                                                                    ? dictInspViolWording[inspViol.inspId]
                                                                    : string.Empty;
                sectionViolationsNN["Пункт"] = inspViol.CodePIN;
            }

            reportParams.SimpleReportParams["ФормулировкаНарушения"] = string.Join("; ", dictInspViolWording.Values);
        }

        protected override void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var act = (ActCheck)doc;

            var firstActCheckPeriod = Container.Resolve<IDomainService<ActCheckPeriod>>().GetAll()
                                                .Where(x => x.ActCheck.Id == act.Id)
                                                .Select(x => x.DateEnd)
                                                .OrderByDescending(x => x.Value)
                                                .FirstOrDefault();

            reportParams.SimpleReportParams["ВремяСоставленияАкта"] = firstActCheckPeriod != null ? 
                                                                                firstActCheckPeriod.Value.ToShortTimeString() 
                                                                                : string.Empty;

            var listInspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                            .Where(x => x.DocumentGji.Id == act.Id)
                                            .Select(x => x.Inspector.ShortFio)
                                            .ToList();

            reportParams.SimpleReportParams["ИнспекторФИОСокр"] = string.Join(", ", listInspectors);

            if ((act.Inspection.TypeBase == TypeBase.PlanJuridicalPerson) && (act.TypeActCheck != TypeActCheckGji.ActCheckDocumentGji))
            {
                var countDays = this.Container.Resolve<IDomainService<BaseJurPerson>>().GetAll()
                    .Where(x => x.Id == act.Inspection.Id)
                    .Select(x => x.CountDays)
                    .FirstOrDefault();
                reportParams.SimpleReportParams["СрокПроверки"] = countDays != null ? countDays.ToString() : string.Empty;
            }
            else
            {
                reportParams.SimpleReportParams["СрокПроверки"] = "1 рабочий день";
            }
        }
    }
}