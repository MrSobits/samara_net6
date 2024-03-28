namespace Bars.GkhGji.DomainService.GisGkhRegional.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService.GisGkhRegional;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Сервис для получения региональных ранных при работе с ГИС ЖКХ
    /// </summary>
    public class GisGkhRegionalService : IGisGkhRegionalService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<PrescriptionArticleLaw> PrescriptionArticleLawDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<ProtocolMhcArticleLaw> ProtocolMhcArticleLawDomain { get; set; }
        public IDomainService<ProtocolMvdArticleLaw> ProtocolMvdArticleLawDomain { get; set; }
        public IDomainService<ProtocolRSOArticleLaw> ProtocolRSOArticleLawDomain { get; set; }
        public IDomainService<ResolProsArticleLaw> ResolProsArticleLawDomain { get; set; }
        public IDomainService<ResolutionRospotrebnadzorArticleLaw> ResolutionRospotrebnadzorArticleLawDomain { get; set; }
        public IDomainService<DisposalSurveyPurpose> DisposalSurveyPurposeDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }
        public IRepository<InspectionGjiRealityObject> InspectionGjiRealityObjectRepo { get; set; }

        public virtual string GetObjectiveLicReissuance(Disposal disposal)
        {
            return $"Проверка соответствия лицензиата установленным лицензионным требованиям";
        }

        public virtual List<ArticleLawGji> GetResolutionArtLaw(Resolution resolution)
        {
            List<ArticleLawGji> artLaws = new List<ArticleLawGji>();

            DocumentGjiChildrenDomain.GetAll()
                .Where(x => x.Children == resolution)
                .ForEach(x =>
                {
                    switch (x.Parent.TypeDocumentGji)
                    {
                        case TypeDocumentGji.Prescription:
                            artLaws.AddRange(PrescriptionArticleLawDomain.GetAll()
                                .Where(y => y.Prescription.Id == x.Parent.Id)
                                .Select(y => y.ArticleLaw));
                            break;
                        case TypeDocumentGji.Protocol:
                            artLaws.AddRange(ProtocolArticleLawDomain.GetAll()
                                .Where(y => y.Protocol.Id == x.Parent.Id)
                                .Select(y => y.ArticleLaw));
                            break;
                                                //case TypeDocumentGji.ProtocolGZHI:
                                                //    artLaws.AddRange(ProtocolGZHIArticleLawDomain.GetAll()
                                                //        .Where(y => y.ProtocolGZHI.Id == x.Parent.Id)
                                                //        .Select(y => y.ArticleLaw));
                                                //    break;
                                                case TypeDocumentGji.ProtocolMhc:
                            artLaws.AddRange(ProtocolMhcArticleLawDomain.GetAll()
                                .Where(y => y.ProtocolMhc.Id == x.Parent.Id)
                                .Select(y => y.ArticleLaw));
                            break;
                        case TypeDocumentGji.ProtocolMvd:
                            artLaws.AddRange(ProtocolMvdArticleLawDomain.GetAll()
                                .Where(y => y.ProtocolMvd.Id == x.Parent.Id)
                                .Select(y => y.ArticleLaw));
                            break;
                                                //case TypeDocumentGji.ProtocolProsecutor:
                                                //    artLaws.AddRange(ProtocolProsecutorArticleLawDomain.GetAll()
                                                //        .Where(y => y.ProtocolProsecutor.Id == x.Parent.Id)
                                                //        .Select(y => y.ArticleLaw));
                                                //    break;
                                                case TypeDocumentGji.ProtocolRSO:
                            artLaws.AddRange(ProtocolRSOArticleLawDomain.GetAll()
                                .Where(y => y.ProtocolRSO.Id == x.Parent.Id)
                                .Select(y => y.ArticleLaw));
                            break;
                        case TypeDocumentGji.ResolutionProsecutor:
                            artLaws.AddRange(ResolProsArticleLawDomain.GetAll()
                                .Where(y => y.ResolPros.Id == x.Parent.Id)
                                .Select(y => y.ArticleLaw));
                            break;
                        case TypeDocumentGji.ResolutionRospotrebnadzor:
                            artLaws.AddRange(ResolutionRospotrebnadzorArticleLawDomain.GetAll()
                                .Where(y => y.Resolution.Id == x.Parent.Id)
                                .Select(y => y.ArticleLaw));
                            break;
                    }
                });
            return artLaws;
        }

        /// <summary>
        /// Получить инспектора-исполнителя
        /// </summary>
        /// <param name="appeal">Обращение</param>
        /// <returns>Инспектор</returns>
        public virtual Inspector GetAppealPerformerForGisGkh(AppealCits appeal)
        {
            return appeal.Executant;
        }

        /// <summary>
		/// Получить тип закрытия обращения
		/// </summary>
		/// <param name="appeal">Обращение</param>
		/// <returns>Инспектор</returns>
        public virtual AppealAnswerType GetAppealAnswerTypeForGisGkh(AppealCits appeal)
        {
            //var answers = AppealCitsAnswerDomain.GetAll()
            //    .Where(x => x.AppealCits == appeal).ToList();
            //foreach (var answer in answers)
            //{
            //    if (answer.AnswerContent.Name == "Перенаправлено по подведомственности")
            //    {
            //        return AppealAnswerType.Redirect;
            //    }
            //    else if (answer.AnswerContent.Name == "Отказ от переписки")
            //    {
            //        return AppealAnswerType.NotNeedAnswer;
            //    }
            //    else if (answer.AnswerContent.Name == "Даны разъяснения")
            //    {
            //        return AppealAnswerType.Answer;
            //    }
            //}
            return AppealAnswerType.NotSet;
        }

        public virtual List<AppealCitsAnswer> GetAppCitAnswersForGisGkh(AppealCits appeal, AppealAnswerType closeType)
        {
            //switch (closeType)
            //{
            //    case AppealAnswerType.Answer:
            //        return AppealCitsAnswerDomain.GetAll()
            //            .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Даны разъяснения").ToList();
            //    case AppealAnswerType.NotNeedAnswer:
            //        return AppealCitsAnswerDomain.GetAll()
            //            .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Отказ от переписки").ToList();
            //    case AppealAnswerType.Redirect:
            //        return AppealCitsAnswerDomain.GetAll()
            //            .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Перенаправлено по подведомственности").ToList();
            //    default:
            return null;
            //}
        }

        public virtual List<AppealCitsAnswer> GetAppCitRollOverAnswersForGisGkh(AppealCits appeal)
        {
            //return AppealCitsAnswerDomain.GetAll()
            //            .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Продлён срок рассмотрения").ToList();
            return new List<AppealCitsAnswer>();
        }

        public virtual List<AppealCitsAnswer> GetAppCitRedirectAnswersForGisGkh(AppealCits appeal)
        {
            //return AppealCitsAnswerDomain.GetAll()
            //            .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Продлён срок рассмотрения").ToList();
            return new List<AppealCitsAnswer>();
        }

        public virtual DateTime GetDateOfAppointment(AppealCits appeal)
        {
            if (appeal.CheckTime.HasValue)
            {
                return appeal.CheckTime.Value;
            }
            else
            {
                throw new Exception("Не указан контрольный срок");
            }
        }

        public virtual TypeExecutantForGisGkh GetTypeExecutant(Resolution resolution)
        {
            return TypeExecutantForGisGkh.NotSet;
        }

        public virtual List<string> GetDisposalControlMeasures(Disposal disposal)
        {
            List<string> Events = new List<string>();
            //int EventNo = 1;
            switch (disposal.KindCheck.Code)
            {
                case TypeCheck.PlannedExit:
                    {
                        Events.Add(@"Запросить соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.NotPlannedExit:
                    {
                        Events.Add(@"Провести обследование жилого дома (домов) и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.PlannedDocumentation:
                    {
                        Events.Add(@"Запросить соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.NotPlannedDocumentation:
                    {
                        Events.Add(@"Запросить соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                    }
                    break;
                case TypeCheck.InspectionSurvey:
                    {
                        Events.Add(@"Провести обследование жилого дома(домов) и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.Monitoring:
                    {
                        Events.Add(@"Запросить соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.PlannedDocumentationExit:
                    {
                        Events.Add(@"Провести обследование жилого дома(домов)");
                        Events.Add(@"Запросить  соответствующие целям и задачам проверки документы; рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.VisualSurvey:
                    {
                        Events.Add(@"Провести обследование жилого дома(домов)");
                        Events.Add(@"Составить акт осмотра");
                    }
                    break;
                case TypeCheck.NotPlannedDocumentationExit:
                    {
                        Events.Add(@"Запросить  соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                    }
                    break;
            }
            return Events;
        }

        public virtual List<string> GetDecisionControlMeasures(Decision decision)
        {
            List<string> Events = new List<string>();
            switch (decision.KindCheck.Code)
            {
                case TypeCheck.PlannedExit:
                    {
                        Events.Add(@"Запросить соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.NotPlannedExit:
                    {
                        Events.Add(@"Провести обследование жилого дома (домов) и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.PlannedDocumentation:
                    {
                        Events.Add(@"Запросить соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.NotPlannedDocumentation:
                    {
                        Events.Add(@"Запросить соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                    }
                    break;
                case TypeCheck.InspectionSurvey:
                    {
                        Events.Add(@"Провести обследование жилого дома(домов) и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.Monitoring:
                    {
                        Events.Add(@"Запросить соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.PlannedDocumentationExit:
                    {
                        Events.Add(@"Провести обследование жилого дома(домов)");
                        Events.Add(@"Запросить  соответствующие целям и задачам проверки документы; рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
                case TypeCheck.VisualSurvey:
                    {
                        Events.Add(@"Провести обследование жилого дома(домов)");
                        Events.Add(@"Составить акт осмотра");
                    }
                    break;
                case TypeCheck.NotPlannedDocumentationExit:
                    {
                        Events.Add(@"Запросить  соответствующие целям и задачам проверки документы");
                        Events.Add(@"Рассмотреть предоставленные документы, провести проверку и составить акт проверки");
                    }
                    break;
                case TypeCheck.InspectVisit:
                    {
                        Events.Add(@"Провести обследование жилого дома(домов) и составить акт проверки");
                        Events.Add(@"В случае выявления при проведении проверки нарушений законодательства принять меры к устранению выявленных нарушений");
                    }
                    break;
            }
            return Events;
        }

        public virtual string GetFunctionRegistryNumber(bool licensecontrol)
        {
            return "0000000000000000000";
        }

        public virtual string FindExaminationPlace(InspectionGji inspection)
        {
            var place = "";
            if (inspection != null)
            {
                try
                {
                    var ROs = new List<InspectionGjiRealityObject>();
                    if (InspectionGjiRealityObjectDomain == null)
                    {
                        ROs = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                                            .Where(x => x.Inspection == inspection).ToList();
                    }
                    else
                    {
                        ROs = InspectionGjiRealityObjectDomain.GetAll()
                                            .Where(x => x.Inspection == inspection).ToList();
                    }

                    foreach (var RO in ROs)
                    {
                        if (RO.RealityObject.Address != null)
                        {
                            if (!string.IsNullOrEmpty(place))
                            {
                                place += ", ";
                            }
                            place += RO.RealityObject.Address;
                        }
                    }
                }
                catch
                {
                    var ROs = new List<InspectionGjiRealityObject>();
                    if (InspectionGjiRealityObjectDomain == null)
                    {
                        ROs = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                                            .Where(x => x.Inspection == inspection).ToList();
                    }
                    else
                    {
                        ROs = InspectionGjiRealityObjectDomain.GetAll()
                                            .Where(x => x.Inspection == inspection).ToList();
                    }

                    foreach (var RO in ROs)
                    {
                        if (RO.RealityObject.Address != null)
                        {
                            if (!string.IsNullOrEmpty(place))
                            {
                                place += ", ";
                            }
                            place += RO.RealityObject.Address;
                        }
                    }
                }
            }
            return place;
        }

        public virtual void FindActRemovalAnnexes(DocumentGji docChildAct, out List<long> ids, out List<FileInfo> files, out List<string> gisGkhGuids)
        {
            files = new List<FileInfo>();
            gisGkhGuids = new List<string>();
            ids = new List<long>();
        }

        public virtual void SaveActRemovalAnnex(long id, string fileGuid)
        {

        }

        public virtual int FindActRemovalWitness(ActRemoval actRemoval, out string witnessesStr, out bool NotFamiliarize, out string FamiliarizedPerson)
        {
            witnessesStr = string.Empty;
            NotFamiliarize = new bool();
            FamiliarizedPerson = string.Empty;
            return 0;
        }

        public virtual void GetActRemovalPeriods(ActRemoval actRemoval, out List<DateTime?> ActDatesStart, out List<DateTime?> ActDatesEnd)
        {
            ActDatesStart = new List<DateTime?>();
            ActDatesEnd = new List<DateTime?>();
        }
    }
}