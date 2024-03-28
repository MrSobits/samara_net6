namespace Bars.GkhGji.Regions.Tomsk.Report.DisposalGji
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using GkhGji.Entities.Dict;

    public class DisposalGjiReport : GkhGji.Report.DisposalGjiReport
    {
        protected override void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var displosalProvidedDocDateDomain = Container.Resolve<IDomainService<DisposalProvidedDocNum>>();
            var disposalProvidedDocDomain = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var disposalExpertDomain = Container.Resolve<IDomainService<DisposalExpert>>();
            var disposalTypeSurveyDomain = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var disposalVerificationSubjectDomain = Container.Resolve<IDomainService<DisposalVerificationSubject>>();
            var typeSurveyInspFoundationGjiDomain = Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>();
            var inspectionGjiDomain = Container.Resolve<IDomainService<InspectionGji>>();
            var inspectionAppealCitsDomain = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var baseJurPersonDomain = Container.Resolve<IDomainService<BaseJurPerson>>();
            var appealCitsSourceDomain = Container.Resolve<IDomainService<AppealCitsSource>>();
            var typeSurveyGjiIssueDomain = Container.Resolve<IDomainService<TypeSurveyGjiIssue>>();
            var surveySubjectDomain = Container.Resolve<IDomainService<SurveySubject>>();
            
            var disposal = (Disposal)doc;
            
            var str = string.Empty;
            var dateProvideDocuments =
                displosalProvidedDocDateDomain.GetAll()
                    .FirstOrDefault(x => x.Disposal.Id == disposal.Id);

            if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
            {
                str = string.Format("В срок не позднее 10 (десяти) рабочих дней со дня получения настоящего распоряжения {0} предоставить"
                                    + "в Департамент копии следующих документов, заверенные надлежащим образом:",
                                    reportParams.SimpleReportParams["СокрУправОрг"]);
            }
            else if (disposal.KindCheck.Code == TypeCheck.NotPlannedExit)
            {
                str = string.Format("В течении дн. {0} предоставить в Департамент следующие документы, заверенные"
                                    + " надлежащим образом:",
                                    dateProvideDocuments != null 
                                        ? dateProvideDocuments.ProvideDocumentsNum.ToString() 
                                        : string.Empty);
            } 
            
            var listDisposalProvidedDoc =
                disposalProvidedDocDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { x.ProvidedDoc.Name, x.Description })
                    .ToList();

            var section11 = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияРаздел11");

            if (listDisposalProvidedDoc.Any())
            {
                section11.ДобавитьСтроку();
                section11["ТекстПередДокументами"] = str;

                var sectionDocuments = section11.ДобавитьСекцию("СекцияДокументы");

                foreach (var item in listDisposalProvidedDoc)
                {
                    sectionDocuments.ДобавитьСтроку();
                    sectionDocuments["ПредоставляемыйДокумент"] = !string.IsNullOrEmpty(item.Description) ? item.Description : item.Name;
                }
            }

            var durationInspections = "20 рабочих дней";

            reportParams.SimpleReportParams["ПродолжительностьПроверки"] = durationInspections;

            var strControlActivities = string.Empty;

            if (disposal.KindCheck.Code == TypeCheck.NotPlannedExit)
            {
                strControlActivities = string.Format("провести визуальный осмотр дома по адресу: "
                                                     + "{0} (с {1} по {2} - {3}.)",
                                                     reportParams.SimpleReportParams["ДомаИАдреса"],
                                                     reportParams.SimpleReportParams["НачалоПериода"],
                                                     reportParams.SimpleReportParams["ОкончаниеПериода"],
                                                     durationInspections);
            }
            else if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
            {

                strControlActivities = string.Format("запросить и рассмотреть документы, необходимые для "
                                                     + "принятия объективного решения в рамках проводимой проверки "
                                                     + "(с {0} по {1} - {2}.)",
                                                     reportParams.SimpleReportParams["НачалоПериода"],
                                                     reportParams.SimpleReportParams["ОкончаниеПериода"],
                                                     durationInspections);
            }

            reportParams.SimpleReportParams["МероприятияПоКонтролю"] = strControlActivities;


            var listExperts = disposalExpertDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => x.Expert.Name)
                        .ToList();

            var sectionExperts = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияЭксперты");

            if (listExperts.Count > 0)
            {
                foreach (var expert in listExperts)
                {
                    sectionExperts.ДобавитьСтроку();
                    sectionExperts["Эксперты"] = expert;
                }
            }
            else
            {
                sectionExperts.ДобавитьСтроку();
                sectionExperts["Эксперты"] = "Не привлекаются";
            }

            var serviceTypeSurvey = disposalTypeSurveyDomain.GetAll()
                                       .Where(x => x.Disposal.Id == disposal.Id);

            var typeSurveyCodes = serviceTypeSurvey.Select(x => x.TypeSurvey.Code).ToList();

            var strRuleCode12 = "Правила предоставления коммунальных услуг собственникам и пользователям "
                                + "помещений в многоквартирных домах и жилых домов, утвержденных постановлением Правительства РФ от 06.05.2011 № 354.";

            var strRuleCode13 = "Правила и нормы технической эксплуатации жилищного фонда, "
                                + "утвержденные постановлением Госстроя РФ от 27.09.2003 № 170; "
                                + "Правила содержания общего имущества в многоквартирном доме, утвержденные постановлением Правительства РФ от 13.08.2006 № 491.";

            var strRuleCode14 = "Правила предоставления коммунальных услуг собственникам и пользователям помещений в многоквартирных домах и жилых домов, "
                                + "утвержденных постановлением Правительства РФ от 06.05.2011 № 354 и "
                                + "Правила и нормы технической эксплуатации жилищного фонда, утвержденные постановлением Госстроя РФ от 27.09.2003 № 170";

            if (typeSurveyCodes.Count == 1)
            {
                var firstCode = typeSurveyCodes.First();

                switch (firstCode)
                {
                    case "12":
                        reportParams.SimpleReportParams["Правило"] = strRuleCode12;
                        break;
                    case "13":
                        reportParams.SimpleReportParams["Правило"] = strRuleCode13;
                        break;
                    case "14":
                        reportParams.SimpleReportParams["Правило"] = strRuleCode14;
                        break;
                }
            }
            else if (typeSurveyCodes.Count > 1)
            {
                reportParams.SimpleReportParams["Правило"] = strRuleCode14;
            }

            var sectionDispVerifSub = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияПредметПроверки");
            var listDispGjiSubVerification =
                disposalVerificationSubjectDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.SurveySubject.Id)
                    .ToList();

            foreach (var subject in surveySubjectDomain.GetAll())
            {
                sectionDispVerifSub.ДобавитьСтроку();

                sectionDispVerifSub["ПредметПроверки"] = subject.Name;
                if (listDispGjiSubVerification.Count > 0)
                {
                    var tmpItem = listDispGjiSubVerification.Count == 1
                                      ? listDispGjiSubVerification.First()
                                      : listDispGjiSubVerification.Last();
                    sectionDispVerifSub["Активная"] = tmpItem == subject.Id ? "1" : string.Empty;
                }
            }

            var queryTypeSurveyId = serviceTypeSurvey.Select(x => x.TypeSurvey.Id);

            var listInspFoundations = typeSurveyInspFoundationGjiDomain.GetAll()
                .Where(x => queryTypeSurveyId.Contains(x.TypeSurvey.Id))
                .Select(x => x.NormativeDoc.Name)
                .AggregateWithSeparator("; ");

            reportParams.SimpleReportParams["ПравовоеОснование"] =
                listInspFoundations.IsEmpty()
                    ? null
                    : listInspFoundations + ".";

            var kindCheck = string.Empty;

            switch (disposal.KindCheck.Code)
            {
                case TypeCheck.PlannedExit:
                    kindCheck = "плановой выездной";
                    break;
                case TypeCheck.NotPlannedExit:
                    kindCheck = "внеплановой выездной";
                    break;
                case TypeCheck.PlannedDocumentation:
                    kindCheck = "плановой документарной";
                    break;
                case TypeCheck.NotPlannedDocumentation:
                    kindCheck = "внеплановой документарной";
                    break;
                case TypeCheck.InspectionSurvey:
                    kindCheck = "инспекционной";
                    break;
            }
            
            reportParams.SimpleReportParams["ВидПроверки"] = kindCheck;

            var inspectionId = disposal.Inspection.Id;

            var inspectionGjiData = inspectionGjiDomain.GetAll()
                .Where(x => x.Id == inspectionId)
                .Select(x => new
                {
                    x.TypeBase,
                    x.InspectionNumber,
                    x.ObjectCreateDate
                })
                .FirstOrDefault();

            if (inspectionGjiData != null && inspectionGjiData.TypeBase == TypeBase.DisposalHead)
            {
                reportParams.SimpleReportParams["НомерОснованияПроверки"] = inspectionGjiData.InspectionNumber;
                reportParams.SimpleReportParams["ДатаОснованияПроверки"] = inspectionGjiData.ObjectCreateDate;
            }

            var appealCitsData = inspectionAppealCitsDomain.GetAll()
                .Where(x => x.Inspection.Id == inspectionId)
                .Select(x => new
                {
                    x.AppealCits.Id,
                    x.AppealCits.DocumentNumber,
                    x.AppealCits.DateFrom,
                    x.AppealCits.Correspondent,
                    x.AppealCits.TypeCorrespondent
                })
                .FirstOrDefault();
            
            if (appealCitsData != null)
            {
                reportParams.SimpleReportParams["НомерОбращения"] = appealCitsData.DocumentNumber;
                reportParams.SimpleReportParams["ДатаОбращения"] =
                    appealCitsData.DateFrom.HasValue
                        ? appealCitsData.DateFrom.Value.ToShortDateString()
                        : null;
                reportParams.SimpleReportParams["Корреспондент"] = appealCitsData.Correspondent;

                //нафига??
                /*var typeCorrespondent = string.Empty;

                switch (appealCitsData.TypeCorrespondent)
                {
                    case TypeCorrespondent.CitizenHe:
                        typeCorrespondent = "Гражданин";
                        break;
                    case TypeCorrespondent.CitizenShe:
                        typeCorrespondent = "Гражданка";
                        break;
                    case TypeCorrespondent.CitizenThey:
                        typeCorrespondent = "Коллективное обращение";
                        break;
                    case TypeCorrespondent.IndividualEntrepreneur:
                        typeCorrespondent = "Индивидуальный предприниматель";
                        break;
                    case TypeCorrespondent.LegalEntity:
                        typeCorrespondent = "Юридическое лицо";
                        break;
                    case TypeCorrespondent.PublicAuthorities:
                        typeCorrespondent = "Орган государственной власти";
                        break;
                    case TypeCorrespondent.LocalAuthorities:
                        typeCorrespondent = "Орган местного самоуправления";
                        break;
                    case TypeCorrespondent.MassMedia:
                        typeCorrespondent = "Средство массовой информации";
                        break;
                   case TypeCorrespondent.Prosecutor:
                        typeCorrespondent = "Прокуратура";
                        break;
                }*/

                reportParams.SimpleReportParams["Тип корреспондента"] = appealCitsData.TypeCorrespondent.GetEnumMeta().Display;
            }

            var jurPersonPlan = baseJurPersonDomain.GetAll()
                .Where(x => x.Id == inspectionId)
                .Select(x => x.Plan.Name)
                .FirstOrDefault();

            if (jurPersonPlan != null)
            {
                int planYear = Convert.ToInt32(Regex.Replace(jurPersonPlan, @"[^\d]+", ""));
                reportParams.SimpleReportParams["ПлановыйГод"] = planYear;
            }

            var nameGenitive = appealCitsData != null
                ? appealCitsSourceDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsData.Id)
                    .Select(x => new
                    {
                        x.RevenueSource.Name,
                        x.RevenueSource.NameGenitive
                    })
                    .FirstOrDefault()
                : null;

            reportParams.SimpleReportParams["ИсточникОбращения (РП)"] = nameGenitive.ReturnSafe(x => x.NameGenitive);

            var typeSurveyId = disposalTypeSurveyDomain.GetAll()
                .Where(x => x.Disposal.Id == disposal.Id)
                .Select(x => x.TypeSurvey.Id)
                .FirstOrDefault();

            var typeSurveyGjiIssueName = typeSurveyGjiIssueDomain.GetAll()
                .Where(x => x.TypeSurvey.Id == typeSurveyId)
                .Select(x => x.Name)
                .FirstOrDefault();
            
            reportParams.SimpleReportParams["ПоВопросу"] = typeSurveyGjiIssueName;

            var strBasesurveyDisp = string.Empty;

            if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                strBasesurveyDisp = string.Format("проверка исполнения ранее выданного предписания {0} от {1}",
                                                 reportParams.SimpleReportParams.ПараметрСуществуетВСписке("НомерПредписания") 
                                                 ? reportParams.SimpleReportParams["НомерПредписания"]
                                                 : string.Empty,
                                                 reportParams.SimpleReportParams.ПараметрСуществуетВСписке("ДатаПредписания")
                                                 ? reportParams.SimpleReportParams["ДатаПредписания"]
                                                 : string.Empty);
            }
            else if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement && nameGenitive.ReturnSafe(x => x.Name.ToLower().Trim()) == "заявитель")
            {
                strBasesurveyDisp = string.Format("обращение граждан (вх. № {0} от {1})"
                                                  + " по вопросу {2} в доме, расположенному по адресу: {3}.",
                                                  reportParams.SimpleReportParams["НомерОбращения"],
                                                  reportParams.SimpleReportParams["ДатаОбращения"],
                                                  reportParams.SimpleReportParams["ПоВопросу"],
                                                  reportParams.SimpleReportParams["ДомаИАдреса"]);
            }
            else if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement && nameGenitive != null && nameGenitive.ReturnSafe(x => x.Name.ToLower().Trim()) != "заявитель")
            {
                strBasesurveyDisp = string.Format("обращение граждан, поступившее в Департамент из {0} (вх. № {1} от {2}) по вопросу {3}"
                                                  + " в доме, расположенному по адресу: {4}.",
                                                  reportParams.SimpleReportParams["ИсточникОбращения (РП)"],
                                                  reportParams.SimpleReportParams["НомерОбращения"],
                                                  reportParams.SimpleReportParams["ДатаОбращения"],
                                                  reportParams.SimpleReportParams["ПоВопросу"],
                                                  reportParams.SimpleReportParams["ДомаИАдреса"]);
            }
            else if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement && nameGenitive == null)
            {
                strBasesurveyDisp = string.Format("обращение граждан, поступившее в Департамент (вх. № {0} от {1}) по вопросу {2}"
                                                  + " в доме, расположенному по адресу: {3}.",
                                                  reportParams.SimpleReportParams["НомерОбращения"],
                                                  reportParams.SimpleReportParams["ДатаОбращения"],
                                                  reportParams.SimpleReportParams["ПоВопросу"],
                                                  reportParams.SimpleReportParams["ДомаИАдреса"]);
            }
            else if (disposal.Inspection.TypeBase == TypeBase.DisposalHead)
            {
                strBasesurveyDisp = string.Format("поручение руководителя (вх. № {0} от {1}) по вопросу {2} в доме, расположенному по адресу: {3}",
                                                  reportParams.SimpleReportParams["НомерОснованияПроверки"],
                                                  reportParams.SimpleReportParams["ДатаОснованияПроверки"],
                                                  reportParams.SimpleReportParams["ПоВопросу"],
                                                  reportParams.SimpleReportParams["ДомаИАдреса"]);
            }
            else if (disposal.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
            {
                strBasesurveyDisp = string.Format("требование {0} по вопросу {1} в доме, расположенному по адресу: {2}",
                                                  reportParams.SimpleReportParams["ИсточникОбращения (РП)"],
                                                  reportParams.SimpleReportParams["ПоВопросу"],
                                                  reportParams.SimpleReportParams["ДомаИАдреса"]);
            }
            else if (disposal.Inspection.TypeBase == TypeBase.PlanJuridicalPerson)
            {
                strBasesurveyDisp = string.Format("план проведения плановых проверок на {0} год", reportParams.SimpleReportParams["ПлановыйГод"]);
            }

            reportParams.SimpleReportParams["ОснованиеОбследованияПриказа"] = strBasesurveyDisp;

            Container.Release(displosalProvidedDocDateDomain);
            Container.Release(disposalProvidedDocDomain);
            Container.Release(disposalExpertDomain);
            Container.Release(disposalTypeSurveyDomain);
            Container.Release(disposalVerificationSubjectDomain);
            Container.Release(typeSurveyInspFoundationGjiDomain);
            Container.Release(inspectionGjiDomain);
            Container.Release(inspectionAppealCitsDomain);
            Container.Release(baseJurPersonDomain);
            Container.Release(appealCitsSourceDomain);
            Container.Release(typeSurveyGjiIssueDomain);
            Container.Release(surveySubjectDomain);
        }
    }
}