namespace Bars.GkhGji.Regions.Chelyabinsk
{
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Entities;
    using Enums;
    using System;
    using System.Linq;

    class CourtPracticeInterceptor : EmptyDomainInterceptor<CourtPractice>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<Prescription> PrescriptionDomail { get; set; }
        public IDomainService<Resolution> ResolutionDomail { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<CourtPractice> service, CourtPractice entity)
        {
           

            try
            { 
                var servStateProvider = Container.Resolve<IStateProvider>();

                if (entity.FormatHour.HasValue)
                {
                    if (entity.FormatMinute.HasValue)
                    {
                        entity.CourtMeetingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.FormatHour.Value, entity.FormatMinute.Value, DateTime.Now.Second);
                    }
                    else
                    {
                        entity.CourtMeetingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.FormatHour.Value, 0, 0);
                    }
                }

                try
                {
                    servStateProvider.SetDefaultState(entity);                  
                }
                finally
                {
                    Container.Release(servStateProvider);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<CourtPractice>: {e.Message}");
            }
        }
        public override IDataResult BeforeUpdateAction(IDomainService<CourtPractice> service, CourtPractice entity)
        {
            var stateDomain = this.Container.Resolve<IDomainService<State>>();
            var courtPracticePrescriptionDomain = this.Container.Resolve<IDomainService<CourtPracticePrescription>>();
            var courtPracticenspectorDomain = this.Container.Resolve<IDomainService<CourtPracticeInspector>>();
            var courtPracticeDisputeHistoryDomain = this.Container.Resolve<IDomainService<CourtPracticeDisputeHistory>>();
            var courtPracticeRealityObjectDomain = this.Container.Resolve<IDomainService<CourtPracticeRealityObject>>();
            var documentInspectorDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var inspectionGjiRealityObjectDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var documentRepo = this.Container.Resolve<IRepository<DocumentGji>>();

           

            //пробуем минимизировать ссылочные свойства ентити, так как сохранение всего этого добра приводит к ошибке
            if (entity.Admonition != null)
            {
                entity.Admonition = new AppealCitsAdmonition { Id = entity.Admonition.Id };
            }
            if (entity.ContragentDefendant != null)
            {
                entity.ContragentDefendant = new Contragent { Id = entity.ContragentDefendant.Id };
            }
            if (entity.ContragentPlaintiff != null)
            {
                entity.ContragentPlaintiff = new Contragent { Id = entity.ContragentPlaintiff.Id };
            }
            if (entity.DifferentContragent != null)
            {
                entity.DifferentContragent = new Contragent { Id = entity.DifferentContragent.Id };
            }
            if (entity.DocumentGji != null)
            {
                entity.DocumentGji = new DocumentGji { Id = entity.DocumentGji.Id };
            }

            try
            {
                var docs = courtPracticePrescriptionDomain.GetAll()
                    .Where(x => x.CourtPractice == entity)
                    .Select(x => x.DocumentGji.Id).Distinct().ToList();
                var inspections = courtPracticePrescriptionDomain.GetAll()
                    .Where(x => x.CourtPractice == entity)
                     .Select(x => x.DocumentGji.Inspection.Id).Distinct().ToList();
                // поучаем инспекторов из документов
                var inspectors = documentInspectorDomain.GetAll()
                    .Where(x => docs.Contains(x.DocumentGji.Id))
                    .Select(x => x.Inspector).Distinct().ToList();
                var exists = courtPracticenspectorDomain.GetAll()
                    .Where(x => x.CourtPractice == entity)
                    .Where(x=> x.LawyerInspector == LawyerInspector.Inspector)
                    .Select(x => x.Inspector.Id).ToList();
                foreach (Inspector insp in inspectors)
                {
                    if (!exists.Contains(insp.Id))
                    {
                        CourtPracticeInspector cpinsp = new CourtPracticeInspector
                        {
                            CourtPractice = entity,
                            Inspector = insp,
                            LawyerInspector = LawyerInspector.Inspector,
                            Discription = "Перенесен из документа ГЖИ"
                        };
                        courtPracticenspectorDomain.Save(cpinsp);
                    }
                }
                //заполняем место возникновения проблемы
                var ros = inspectionGjiRealityObjectDomain.GetAll()
                     .Where(x => inspections.Contains(x.Inspection.Id))
                   .Select(x => x.RealityObject).Distinct().ToList();

                var existsRo = courtPracticeRealityObjectDomain.GetAll()
                   .Where(x => x.CourtPractice == entity)               
                   .Select(x => x.RealityObject.Id).ToList();

                foreach (RealityObject ro in ros)
                {
                    if (!existsRo.Contains(ro.Id))
                    {
                        CourtPracticeRealityObject cpro = new CourtPracticeRealityObject
                        {
                            CourtPractice = entity,
                            RealityObject = ro
                        };
                        courtPracticeRealityObjectDomain.Save(cpro);
                    }
                }

                var documentss = courtPracticePrescriptionDomain.GetAll()
                   .Where(x => x.CourtPractice == entity)
                   .Select(x => x.DocumentGji).Distinct().ToList();

                foreach (DocumentGji cpdoc in documentss)
                {
                    if (entity.CourtMeetingResult == CourtMeetingResult.CompletelySatisfied && cpdoc.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription)
                    {
                        var prescription = PrescriptionDomail.Get(cpdoc.Id);
                        prescription.PrescriptionState = GkhGji.Enums.PrescriptionState.CancelledByCourt;
                        PrescriptionDomail.Update(prescription);
                    }

                    if (entity.InterimMeasures && cpdoc.State.Code != "3")
                    {
                        switch (cpdoc.TypeDocumentGji)
                        {
                            case GkhGji.Enums.TypeDocumentGji.Prescription:
                                {
                                    var state = stateDomain.GetAll()
                                        .FirstOrDefault(x => x.TypeId == "gji_document_prescr" && x.Code == "3");
                                    if (state != null)
                                    {
                                        cpdoc.State = state;
                                        documentRepo.Update(cpdoc);
                                    }
                                }
                                break;

                            case GkhGji.Enums.TypeDocumentGji.Resolution:
                                {
                                    var state = stateDomain.GetAll()
                                        .FirstOrDefault(x => x.TypeId == "gji_document_resol" && x.Code == "3");
                                    if (state != null)
                                    {
                                        cpdoc.State = state;
                                        documentRepo.Update(cpdoc);
                                    }
                                }
                                break;

                        }
                    }
                    else if (!entity.InterimMeasures)
                    {
                        switch (cpdoc.TypeDocumentGji)
                        {
                            case GkhGji.Enums.TypeDocumentGji.Prescription:
                                {
                                    var state = stateDomain.GetAll()
                                        .FirstOrDefault(x => x.TypeId == "gji_document_prescr" && x.Code == "2");
                                    if (state != null)
                                    {
                                        cpdoc.State = state;
                                        documentRepo.Update(cpdoc);
                                    }
                                }
                                break;

                            case GkhGji.Enums.TypeDocumentGji.Resolution:
                                {
                                    var state = stateDomain.GetAll()
                                        .FirstOrDefault(x => x.Name == "Оспаривание" && x.TypeId == "gji_document_resol");
                                    if (state != null)
                                    {
                                        cpdoc.State = state;
                                        documentRepo.Update(cpdoc);
                                    }
                                }
                                break;

                        }
                    }
                }

                if (entity.FormatHour.HasValue)
                {
                    if (entity.FormatMinute.HasValue)
                    {
                        entity.CourtMeetingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.FormatHour.Value, entity.FormatMinute.Value, DateTime.Now.Second);
                    }
                    else
                    {
                        entity.CourtMeetingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.FormatHour.Value, 0, 0);
                    }
                }
                else if (entity.FormatMinute.HasValue && entity.CourtMeetingTime.HasValue)
                {
                    entity.CourtMeetingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.CourtMeetingTime.Value.Hour, entity.FormatMinute.Value, DateTime.Now.Second);
                }
                //создаем историю рассмотрения
                try
                {
                    if (entity.InstanceGji != null)
                    {
                        
                        var existsCpHist = courtPracticeDisputeHistoryDomain.GetAll()
                            .Where(x => x.CourtPractice.Id == entity.Id && x.InstanceGji == entity.InstanceGji && x.CourtPractice.DisputeResult == x.CourtMeetingResult).FirstOrDefault();
                        if (entity.SaveHistory)
                        {
                            var newRecHist = new CourtPracticeDisputeHistory
                            {
                                CourtPractice = new CourtPractice { Id = entity.Id },
                                FileInfo = entity.FileInfo,
                                CourtCosts = entity.CourtCosts,
                                CourtCostsFact = entity.CourtCostsFact,
                                CourtCostsPlan = entity.CourtCostsPlan,
                                CourtMeetingResult = entity.DisputeResult,
                                CourtMeetingTime = entity.CourtMeetingTime,
                                CourtPracticeState = entity.CourtPracticeState,
                                DateCourtMeeting = entity.DateCourtMeeting,
                                Discription = entity.Discription,
                                Dispute = entity.Dispute,
                                FormatHour = entity.FormatHour,
                                FormatMinute = entity.FormatMinute,
                                InLaw = entity.InLaw,
                                InLawDate = entity.InLawDate,
                                InstanceGji = entity.InstanceGji,
                                InterimMeasures = entity.InterimMeasures,
                                InterimMeasuresDate = entity.InterimMeasuresDate,
                                JurInstitution = entity.JurInstitution,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                ObjectVersion = 0,
                                PausedComment = entity.PausedComment,
                                PerformanceList = entity.PerformanceList,
                                PerformanceProceeding = entity.PerformanceProceeding
                            };
                            courtPracticeDisputeHistoryDomain.Save(newRecHist);
                        }
                        else if (existsCpHist != null)
                        {
                            //update history
                            existsCpHist.FileInfo = entity.FileInfo;
                            existsCpHist.CourtCosts = entity.CourtCosts;
                            existsCpHist.CourtCostsFact = entity.CourtCostsFact;
                            existsCpHist.CourtCostsPlan = entity.CourtCostsPlan;
                            existsCpHist.CourtMeetingResult = entity.DisputeResult;
                            existsCpHist.CourtMeetingTime = entity.CourtMeetingTime;
                            existsCpHist.CourtPracticeState = entity.CourtPracticeState;
                            existsCpHist.DateCourtMeeting = entity.DateCourtMeeting;
                            existsCpHist.Discription = entity.Discription;
                            existsCpHist.Dispute = entity.Dispute;
                            existsCpHist.FormatHour = entity.FormatHour;
                            existsCpHist.FormatMinute = entity.FormatMinute;
                            existsCpHist.InLaw = entity.InLaw;
                            existsCpHist.InterimMeasures = entity.InterimMeasures;
                            existsCpHist.InterimMeasuresDate = entity.InterimMeasuresDate;
                            existsCpHist.JurInstitution = entity.JurInstitution;
                            existsCpHist.PausedComment = entity.PausedComment;
                            existsCpHist.PerformanceList = entity.PerformanceList;
                            existsCpHist.PerformanceProceeding = entity.PerformanceProceeding;                           
                            courtPracticeDisputeHistoryDomain.Update(existsCpHist);
                        }
                        else
                        {
                            //create history instance
                            var newRecHist = new CourtPracticeDisputeHistory
                            {
                                CourtPractice = new CourtPractice { Id = entity.Id },
                                FileInfo = entity.FileInfo,
                                CourtCosts = entity.CourtCosts,
                                CourtCostsFact = entity.CourtCostsFact,
                                CourtCostsPlan = entity.CourtCostsPlan,
                                CourtMeetingResult = entity.DisputeResult,
                                CourtMeetingTime = entity.CourtMeetingTime,
                                CourtPracticeState = entity.CourtPracticeState,
                                DateCourtMeeting = entity.DateCourtMeeting,
                                Discription = entity.Discription,
                                Dispute = entity.Dispute,
                                FormatHour = entity.FormatHour,
                                FormatMinute = entity.FormatMinute,
                                InLaw = entity.InLaw,
                                InLawDate = entity.InLawDate,
                                InstanceGji = entity.InstanceGji,
                                InterimMeasures = entity.InterimMeasures,
                                InterimMeasuresDate = entity.InterimMeasuresDate,
                                JurInstitution = entity.JurInstitution,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                ObjectVersion = 0,
                                PausedComment = entity.PausedComment,
                                PerformanceList = entity.PerformanceList,
                                PerformanceProceeding = entity.PerformanceProceeding
                            };
                            courtPracticeDisputeHistoryDomain.Save(newRecHist);
                        }
                    }
                }
                catch
                { }
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<CourtPractice>: {e.Message}");
            }
            finally
            {
                Container.Release(courtPracticePrescriptionDomain);                
                Container.Release(stateDomain);
                Container.Release(courtPracticeDisputeHistoryDomain);
                Container.Release(documentRepo);
                Container.Release(courtPracticenspectorDomain);
                Container.Release(documentInspectorDomain);
                Container.Release(inspectionGjiRealityObjectDomain);
                Container.Release(courtPracticeRealityObjectDomain);
            }
        }
    }
}
