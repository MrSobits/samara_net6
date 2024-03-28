namespace Bars.GkhGji.Regions.Nso.Report.DisposalGji
{
    using System;
    using System.Linq;

    using B4;
    using Bars.B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Utils;
    using GkhCalendar.DomainService;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using Entities;
    using Entities.Disposal;

    public class DisposalGjiReport : GkhGji.Report.DisposalGjiReport
    {
        protected override void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var serviceNsoDisposal = Container.Resolve<IDomainService<NsoDisposal>>();
            var serviceDisposalSubj = Container.Resolve<IDomainService<DisposalVerificationSubject>>();
            var serviceInspectionRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var serviceTypeSurvey = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var serviceTypeSurveyTaskInsp = Container.Resolve<IDomainService<TypeSurveyTaskInspGji>>();
            var serviceTypeSurveyInspFound = Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>();
            var serviceProvDocs = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var serviceBaseJurPers = Container.Resolve<IDomainService<BaseJurPerson>>();

            try
            {
                var disposal = (Disposal)doc;

                var nsoDisposal = serviceNsoDisposal.GetAll().FirstOrDefault(x => x.Id == disposal.Id);
                if (nsoDisposal != null)
                {
                    reportParams.SimpleReportParams["ВремяНачалаМероприятий"] = nsoDisposal.TimeVisitStart.ToTimeString();
                    reportParams.SimpleReportParams["ВремяОкончанияМероприятий"] = nsoDisposal.TimeVisitEnd.ToTimeString();
                }

                // Секция предметы проверки 
                var sectionDispVerifSub = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияПредметыПроверки");
                
                var listDispGjiSubVerification = serviceDisposalSubj.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => x.SurveySubject)
                        .ToList();

                foreach (var type in listDispGjiSubVerification)
                {
                    sectionDispVerifSub.ДобавитьСтроку();

                    sectionDispVerifSub["ПредметыПроверки"] = type.Name;
                }

                // Секция дома
                var roList = serviceInspectionRo.GetAll()
                    .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                    .Select(x => new
                    {
                        region = x.RealityObject.Municipality.Name,
                        mo = x.RealityObject.MoSettlement.Name,
                        place = x.RealityObject.FiasAddress.PlaceName,
                        street = x.RealityObject.FiasAddress.StreetName,
                        house = x.RealityObject.FiasAddress.House,
                        housing = x.RealityObject.FiasAddress.Housing
                    })
                    .ToList();

                var sectionRo = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияДома");
                foreach (var ro in roList)
                {
                    sectionRo.ДобавитьСтроку();
                    sectionRo["МуниципальныйРайон"] = ro.region;
                    sectionRo["МуниципальноеОбразование"] = ro.mo;
                    sectionRo["НаселенныйПункт"] = ro.place;
                    sectionRo["Улица"] = ro.street;
                    sectionRo["НомерДома"] = ro.house;
                    if (!string.IsNullOrEmpty(ro.housing))
                    {
                        sectionRo["НомерДома"] = ro.house + ", корп" + ro.housing;
                    }
                }

                // СекцияДокументы
                var sectionDisposalProvidedDoc = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияДокументы");
                var listDisposalProvidedDoc = serviceProvDocs.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.ProvidedDoc.Name)
                    .ToList();

                var i = 0;
                foreach (var item in listDisposalProvidedDoc)
                {
                    sectionDisposalProvidedDoc.ДобавитьСтроку();
                    sectionDisposalProvidedDoc["ПредоставляемыеДокументы"] = item + (i < listDisposalProvidedDoc.Count() - 1 ? ";" : ".");
                    i++;
                }

                var dateStartDisposal = disposal.DateStart ?? DateTime.Today;
                var dateEndDisposal = disposal.DateEnd ?? DateTime.Today;
                var days = (dateEndDisposal - dateStartDisposal).Days;
                reportParams.SimpleReportParams["ПродолжительностьПроверки"] = days != 0 ? (days + 1).ToString() : string.Empty;

                // СекцияЗадача
                var sectionTask = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияЗадача");
                var dataTasks = serviceTypeSurveyTaskInsp.GetAll()
                    .Where(x => serviceTypeSurvey.GetAll()
                        .Where(y => y.Disposal.Id == disposal.Id)
                        .Any(y => y.TypeSurvey.Id == x.TypeSurvey.Id))
                    .Select(x => x.SurveyObjective.Name)
                    .ToList();

                i = 0;
                foreach (var task in dataTasks)
                {
                    sectionTask.ДобавитьСтроку();
                    sectionTask["Задача"] = task + (i < dataTasks.Count() - 1 ? ";" : ".");
                    i++;
                }

                // ПравовоеОснование
                var sectionFound = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияПравовоеОснование");
                var dataFound = serviceTypeSurveyInspFound.GetAll()
                    .Where(x => serviceTypeSurvey.GetAll()
                        .Where(y => y.Disposal.Id == disposal.Id)
                        .Any(y => y.TypeSurvey.Id == x.TypeSurvey.Id))
                    .OrderBy(x => x.Code)
                    .Select(x => x.NormativeDoc.Name)
                    .ToList();

                i = 0;
                foreach (var found in dataFound)
                {
                    sectionFound.ДобавитьСтроку();
                    sectionFound["ПравовоеОснование"] = found + (i < dataFound.Count() - 1 ? ";" : ".");
                    i++;
                }

                if (disposal.Inspection.TypeBase == TypeBase.PlanJuridicalPerson)
                {
                    var baseInspection = serviceBaseJurPers.GetAll().Where(x => x.Id == disposal.Inspection.Id)
                        .Select(x => new { x.CountDays, x.CountHours, x.Plan.DateDisposal, x.Plan.NumberDisposal})
                        .FirstOrDefault();

                    if (baseInspection != null)
                    {
                        reportParams.SimpleReportParams["СрокПроверкиЧасов"] = baseInspection.CountHours.HasValue ? baseInspection.CountHours.Value.ToString() : string.Empty;
                        reportParams.SimpleReportParams["СрокПроверкиДней"] = baseInspection.CountDays.HasValue ? baseInspection.CountDays.Value.ToString() : string.Empty;

                        reportParams.SimpleReportParams["НомерПриказаПлана"] = baseInspection.NumberDisposal;
                        reportParams.SimpleReportParams["ДатаПриказаПлана"] = baseInspection.DateDisposal.ToDateString();
                    }
                }

                if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
                    {
                        var calendarService = Container.Resolve<IIndustrialCalendarService>();
                        reportParams.SimpleReportParams["СрокПроверки"] = 
                            calendarService.GetWorkDaysCount(disposal.DateStart.Value.AddDays(-1), disposal.DateEnd.Value);
                    }
                    else
                    {
                        reportParams.SimpleReportParams["СрокПроверки"] = null;
                    }
                }
            }
            finally 
            {
                Container.Release(serviceNsoDisposal);
                Container.Release(serviceDisposalSubj);
                Container.Release(serviceInspectionRo);
                Container.Release(serviceTypeSurvey);
                Container.Release(serviceTypeSurveyInspFound);
                Container.Release(serviceTypeSurveyTaskInsp);
                Container.Release(serviceProvDocs);
                Container.Release(serviceBaseJurPers);
            }
        }
    }
}