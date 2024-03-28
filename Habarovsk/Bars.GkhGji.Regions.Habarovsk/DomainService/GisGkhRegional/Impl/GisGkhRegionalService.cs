namespace Bars.GkhGji.Regions.Habarovsk.DomainService.GisGkhRegional.Impl
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk;
    using Bars.GkhGji.Regions.Habarovsk.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    /// <summary>
    /// Сервис для получения региональных ранных при работе с ГИС ЖКХ
    /// </summary>
    public class GisGkhRegionalService : Bars.GkhGji.DomainService.GisGkhRegional.Impl.GisGkhRegionalService
    {
        //public IDomainService<BaseLicenseReissuance> BaseLicenseReissuanceDomain { get; set; }
        //public IDomainService<ResolutionArtLaw> ResolutionArtLawDomain { get; set; }
        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }
        public IDomainService<AppealCitsExecutant> AppealCitsExecutantDomain { get; set; }
        public IDomainService<DisposalControlMeasures> DisposalControlMeasuresDomain { get; set; }
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public IDomainService<BaseJurPerson> BaseJurPersonDomain { get; set; }

        public IDomainService<Protocol197ArticleLaw> Protocol197ArticleLawDomain { get; set; }
        public IDomainService<InspectionAppealCits> InspectionAppealCitsDomain { get; set; }
     //   public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }
        public IDomainService<ActRemovalAnnex> ActRemovalAnnexRepo { get; set; }
        public IDomainService<ActRemovalWitness> ActRemovalWitnessDomain { get; set; }
        public IDomainService<ActRemovalPeriod> ActRemovalPeriodRepo { get; set; }

        //public override string GetObjectiveLicReissuance(Disposal disposal)
        //{
        //    if (disposal != null)
        //    {
        //        var request = BaseLicenseReissuanceDomain.GetAll()
        //            .FirstOrDefault(x => x.Id == disposal.Inspection.Id);

        //        if (request != null && request.LicenseReissuance != null)
        //        {
        //            return $"Проверка соответствия лицензиата установленным лицензионным требованиям на основании обращения (заявления) №{ request.LicenseReissuance.RegisterNumber} от {request.LicenseReissuance.ReissuanceDate.ToDateString()}";
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else return null;
        //}

        //public override List<ArticleLawGji> GetResolutionArtLaw(Resolution resolution)
        //{
        //    return ResolutionArtLawDomain.GetAll()
        //        .Where(x => x.Resolution == resolution)
        //        .Select(x => x.ArticleLawGji).ToList();
        //}


        public virtual bool UserIsCr()
        {

            return true;
        }

        /// <summary>
		/// Получить инспектора-исполнителя
		/// </summary>
		/// <param name="appeal">Обращение</param>
		/// <returns>Инспектор</returns>
        public override Inspector GetAppealPerformerForGisGkh(AppealCits appeal)
        {
            return AppealCitsExecutantDomain.GetAll().Where(x => x.AppealCits == appeal).Where(x => x.Executant != null && x.IsResponsible).Select(x => x.Executant).FirstOrDefault();
        }

        /// <summary>
		/// Получить тип закрытия обращения
		/// </summary>
		/// <param name="appeal">Обращение</param>
		/// <returns>Инспектор</returns>
        public override AppealAnswerType GetAppealAnswerTypeForGisGkh(AppealCits appeal)
        {
            var answers = AppealCitsAnswerDomain.GetAll()
                .Where(x => x.AppealCits == appeal).ToList();
            bool Answer = false;
            bool NotNeedAnswer = false;
            bool Redirect = false;
            foreach (var answer in answers)
            {
                if (answer.AnswerContent.Name == "Перенаправлено")
                {
                    Redirect = true;
                    break;
                    //return AppealAnswerType.Redirect;
                }
                else if (answer.AnswerContent.Name == "Отказ от переписки")
                {
                    NotNeedAnswer = true;
                    break;
                    //return AppealAnswerType.NotNeedAnswer;
                }
                else if (answer.AnswerContent.Name == "Даны разъяснения")
                {
                    Answer = true;
                    break;
                    //return AppealAnswerType.Answer;
                }
                else 
                {
                    Answer = true;
                    break;
                    //return AppealAnswerType.Answer;
                }
            }
            if (NotNeedAnswer && (Redirect || Answer))
            {
                return AppealAnswerType.NotSet;
            }
            else if (NotNeedAnswer)
            {
                return AppealAnswerType.NotNeedAnswer;
            }
            if (Redirect && Answer)
            {
                return AppealAnswerType.RedirectAndAnswer;
            }
            else if (Redirect)
            {
                return AppealAnswerType.Redirect;
            }
            else if (Answer)
            {
                return AppealAnswerType.Answer;
            }
            return AppealAnswerType.NotSet;
        }

        public override List<AppealCitsAnswer> GetAppCitAnswersForGisGkh(AppealCits appeal, AppealAnswerType closeType)
        {
            switch (closeType)
            {
                case AppealAnswerType.Answer:
                case AppealAnswerType.RedirectAndAnswer:
                    return AppealCitsAnswerDomain.GetAll()
                        .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Даны разъяснения").ToList();
                case AppealAnswerType.NotNeedAnswer:
                    return AppealCitsAnswerDomain.GetAll()
                        .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Отказ от переписки").ToList();
                //case AppealAnswerType.Redirect:
                //    return AppealCitsAnswerDomain.GetAll()
                //        .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Перенаправлено").ToList();
                default:
                    return null;
            }
        }

        public override List<AppealCitsAnswer> GetAppCitRollOverAnswersForGisGkh(AppealCits appeal)
        {
            return AppealCitsAnswerDomain.GetAll()
                        .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Продлен срок рассмотрения").ToList();
        }

        public override List<AppealCitsAnswer> GetAppCitRedirectAnswersForGisGkh(AppealCits appeal)
        {
            return AppealCitsAnswerDomain.GetAll()
                        .Where(x => x.AppealCits == appeal && x.AnswerContent.Name == "Перенаправлено").ToList();
        }

        public override DateTime GetDateOfAppointment(AppealCits appeal)
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

        public override TypeExecutantForGisGkh GetTypeExecutant(Resolution resolution)
        {
            switch (resolution.Executant.Code)
            {
                case "0":
                case "2":
                case "4":
                case "10":
                case "12":
                case "6":
                case "7":
                case "15":
                case "21":
                    return TypeExecutantForGisGkh.UL;
                case "1":
                case "3":
                case "5":
                case "11":
                case "13":
                case "16":
                case "18":
                case "19":
                    return TypeExecutantForGisGkh.DL;
                case "8":
                case "9":
                case "14":
                    return TypeExecutantForGisGkh.FL;
            }
            return TypeExecutantForGisGkh.NotSet;
        }

        public override List<string> GetDisposalControlMeasures(Disposal disposal)
        {
            return DisposalControlMeasuresDomain.GetAll().Where(x => x.Disposal == disposal).OrderBy(x => x.DateStart).Select(x => x.Description).ToList();
        }

        public override string GetFunctionRegistryNumber(bool licensecontrol)
        {
            if (licensecontrol)
            {
                return "3640000010000306797";
            }
            else
            {
                return "3640000010000026981";
            }
        }

        public override void FindActRemovalAnnexes(DocumentGji docChildAct, out List<long> ids, out List<FileInfo> files, out List<string> gisGkhGuids)
        {
            files = new List<FileInfo>();
            gisGkhGuids = new List<string>();
            ids = new List<long>();
            var actAnnexes = ActRemovalAnnexRepo.GetAll()                
                            .Where(x => x.ActRemoval == docChildAct)
                             .Where(x => x.File.Extention.ToLower() == "pdf").ToList();
            foreach (var annex in actAnnexes)
            {
                ids.Add(annex.Id);
                files.Add(annex.File);
                gisGkhGuids.Add(annex.GisGkhAttachmentGuid);
            }
        }

        public override void SaveActRemovalAnnex(long id, string fileGuid)
        {
            var annex = ActRemovalAnnexRepo.Get(id);
            annex.GisGkhAttachmentGuid = fileGuid;
            ActRemovalAnnexRepo.Update(annex);
        }

        public override int FindActRemovalWitness(ActRemoval actRemoval, out string witnessesStr, out bool NotFamiliarize, out string FamiliarizedPerson)
        {
            // лица, присутствовавшие
            var witnesses = ActRemovalWitnessDomain.GetAll()
                .Where(x => x.ActRemoval == actRemoval).ToList();
            // лица, присутствовавшие и ознакомленные
            witnessesStr = ""; // присутствовали
            NotFamiliarize = true; // никто не ознакомлен
            FamiliarizedPerson = ""; // кто ознакомлен
            foreach (var witness in witnesses)
            {
                if (!string.IsNullOrEmpty(witnessesStr))
                {
                    witnessesStr += "; ";
                }
                witnessesStr += $"{witness.Fio}, {witness.Position}";
                if (witness.IsFamiliar)
                {
                    NotFamiliarize = false;
                    if (!string.IsNullOrEmpty(FamiliarizedPerson))
                    {
                        FamiliarizedPerson += "; ";
                    }
                    FamiliarizedPerson += $"{witness.Fio}";
                }
            }
            return witnesses.Count;
        }

        public override void GetActRemovalPeriods(ActRemoval actRemoval, out List<DateTime?> ActDatesStart, out List<DateTime?> ActDatesEnd)
        {
            ActDatesStart = new List<DateTime?>();
            ActDatesEnd = new List<DateTime?>();
            var actRemovalPeriods = ActRemovalPeriodRepo.GetAll()
                               .Where(x => x.ActRemoval == actRemoval).ToList();
            foreach (var actRemovalPeriod in actRemovalPeriods)
            {
                ActDatesStart.Add(actRemovalPeriod.DateStart);
                ActDatesEnd.Add(actRemovalPeriod.DateEnd);
            }
        }

        public override List<ArticleLawGji> GetResolutionArtLaw(Resolution resolution)
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
                        case TypeDocumentGji.Protocol197:
                            artLaws.AddRange(Protocol197ArticleLawDomain.GetAll()
                                .Where(y => y.Protocol197.Id == x.Parent.Id)
                                .Select(y => y.ArticleLaw));
                            break;
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
    }
}