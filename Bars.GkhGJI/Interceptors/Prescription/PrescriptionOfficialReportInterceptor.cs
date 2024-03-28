namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;

    using Entities;

    /// <summary>
    /// Интерцептор сущности "Этап указания к устранению нарушения в предписании" (см. <see cref="PrescriptionOfficialReport"/>)
    /// </summary>
    public class PrescriptionOfficialReportInterceptor : EmptyDomainInterceptor<PrescriptionOfficialReport>
    {

        /// <summary>
        /// Домент-сервис "PrescriptionOfficialReportViolation"
        /// </summary>
        public IDomainService<PrescriptionOfficialReportViolation> PrescriptionOfficialReportViolationDomain { get; set; }

        /// <summary>
        /// Домент-сервис "PrescriptionViol"
        /// </summary>
        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<Reminder> ReminderDomain { get; set; }

        public IDomainService<ActRemoval> ActRemovalDomain { get; set; }

        public IDomainService<ActRemovalViolation> ActRemovalViolationDomain { get; set; }

        /// <summary>
        /// Домент-сервис "PrescriptionViol"
        /// </summary>
        public IDomainService<InspectionGjiViol> InspectionGjiViolDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<PrescriptionOfficialReport> service, PrescriptionOfficialReport entity)
        {
            try
            {
                if (entity.OfficialReportType == OfficialReportType.Extension)
                {
                    entity.YesNo = Gkh.Enums.YesNo.No;
                }
                    var stateProvider = Container.Resolve<IStateProvider>();
                try
                {
                    stateProvider.SetDefaultState(entity);

                }
                catch
                {
                    return Failure("Для расчета не задан начальный статус");
                }
                finally
                {
                    Container.Release(stateProvider);
                }


                return Success();
            }
            catch (Exception e)
            {
                return Failure("Ошибка интерцептора BeforeCreateAction");
            }

        }

        /// <summary>
        /// Действие, выполняемое после обновления сущности
        /// </summary>
        /// <param name="service">Домен сервис сущности <see cref="PrescriptionOfficialReport"/></param>
        /// <param name="entity">Сущность <see cref="PrescriptionOfficialReport"/></param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterUpdateAction(IDomainService<PrescriptionOfficialReport> service, PrescriptionOfficialReport entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            if (entity.OfficialReportType == OfficialReportType.Extension)
            {
                entity.YesNo = Gkh.Enums.YesNo.No;
            }

            if (entity.OfficialReportType == OfficialReportType.Removal)
            {
                if (entity.State.FinalState && entity.YesNo == Gkh.Enums.YesNo.Yes)
                {
                    var prescrViolations = PrescriptionOfficialReportViolationDomain.GetAll()
                        .Where(x => x.PrescriptionOfficialReport == entity)
                        .Select(x => x.PrescriptionViol).ToList();

            
                    var inspectionViolations = PrescriptionOfficialReportViolationDomain.GetAll()
                        .Where(x => x.PrescriptionOfficialReport == entity)
                        .Select(x => x.PrescriptionViol.InspectionViolation).ToList();

          //          var dateDateFactRemovalStages = Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
          //.Where(x => x.InspectionViolation.Inspection.Id == entity.InspectionViolation.Id)
          //.Max(x => x.DateFactRemoval);
                    try
                    {
                        //удаляем задачи
                        var reminders = ReminderDomain.GetAll()
                            .Where(x => x.Actuality && x.TypeReminder == Contracts.Enums.TypeReminder.Prescription)
                            .Where(x => x.DocumentGji != null && x.DocumentGji.Id == entity.Prescription.Id)
                            .Select(x => x.Id).ToList();
                        foreach (long id in reminders)
                        {
                            var rem = ReminderDomain.Get(id);
                            rem.Actuality = false;
                            ReminderDomain.Update(rem);
                        }
                        foreach (InspectionGjiViol iv in inspectionViolations)
                        {
                            iv.DateFactRemoval = entity.ViolationDate;
                            InspectionGjiViolDomain.Update(iv);
                        }

                        foreach (PrescriptionViol pv in prescrViolations)
                        {
                            pv.DateFactRemoval = entity.ViolationDate;
                            PrescriptionViolDomain.Update(pv);
                        }
                    }
                    catch
                    {
                        return this.Failure("Ошибка обновления фактических дат устранения нарушений");
                    }
                }
                else if(entity.State.StartState)
                {
                    var actRemoval = ActRemovalDomain.GetAll()
                        .Where(x=> x.TypeRemoval == Gkh.Enums.YesNoNotSet.Yes)
                        .Where(x => x.Inspection.Id == entity.Prescription.Inspection.Id)
                        .Where(x => x.Id > entity.Prescription.Id).Select(x=> x.Id).ToList();
                    if (actRemoval.Count == 0)
                    {
                        var prescrViolations = PrescriptionOfficialReportViolationDomain.GetAll()
                           .Where(x => x.PrescriptionOfficialReport == entity)
                           .Select(x => x.PrescriptionViol).ToList();

                        var inspectionViolations = PrescriptionOfficialReportViolationDomain.GetAll()
                            .Where(x => x.PrescriptionOfficialReport == entity)
                            .Select(x => x.PrescriptionViol.InspectionViolation).ToList();
                        try
                        {
                            //восстанавливаем задачи
                            var reminders = ReminderDomain.GetAll()
                                .Where(x => !x.Actuality && x.TypeReminder == Contracts.Enums.TypeReminder.Prescription)
                                .Where(x => x.DocumentGji != null && x.DocumentGji.Id == entity.Prescription.Id)
                                .Select(x => x.Id).ToList();
                            foreach (long id in reminders)
                            {
                                var rem = ReminderDomain.Get(id);
                                rem.Actuality = true;
                                ReminderDomain.Update(rem);
                            }
                            foreach (InspectionGjiViol iv in inspectionViolations)
                            {
                                iv.DateFactRemoval = null;
                                InspectionGjiViolDomain.Update(iv);
                            }

                            foreach (PrescriptionViol pv in prescrViolations)
                            {
                                pv.DateFactRemoval = null;
                                PrescriptionViolDomain.Update(pv);
                            }
                        }
                        catch
                        {
                            return this.Failure("Ошибка обновления фактических дат устранения нарушений");
                        }
                    }
                    else
                    {
                        var prescrViolations = PrescriptionOfficialReportViolationDomain.GetAll()
                          .Where(x => x.PrescriptionOfficialReport == entity)
                          .Select(x => x.PrescriptionViol).ToList();

                        var inspectionViolations = PrescriptionOfficialReportViolationDomain.GetAll()
                            .Where(x => x.PrescriptionOfficialReport == entity)
                            .Select(x => x.PrescriptionViol.InspectionViolation).AsQueryable();

                        var actnotRemoval = ActRemovalDomain.GetAll()
                        .Where(x => x.TypeRemoval == Gkh.Enums.YesNoNotSet.No)
                        .Where(x => x.Inspection.Id == entity.Prescription.Inspection.Id)
                        .Where(x => x.Id > entity.Prescription.Id).Select(x => x.Id).ToList();

                        var actRemovalViolations = ActRemovalViolationDomain.GetAll()
                            .Where(x => actnotRemoval.Contains(x.Document.Id))
                            .Where(x => inspectionViolations.Any(y => y.Id == x.InspectionViolation.Id))
                            .Select(x => x.InspectionViolation).ToList();
                        if (actRemovalViolations.Count > 0)
                        {
                            foreach (InspectionGjiViol iv in actRemovalViolations)
                            {
                                iv.DateFactRemoval = null;
                                InspectionGjiViolDomain.Update(iv);
                            }

                            foreach (PrescriptionViol pv in prescrViolations)
                            {
                                pv.DateFactRemoval = null;
                                PrescriptionViolDomain.Update(pv);
                            }
                        }
                    }
                }
            }
            else
            {
                if (entity.State.FinalState && entity.ExtensionViolationDate.HasValue)
                {
                    var prescrViolations = PrescriptionOfficialReportViolationDomain.GetAll()
                        .Where(x => x.PrescriptionOfficialReport == entity)
                        .Select(x => x.PrescriptionViol).ToList();

                    var inspectionViolations = PrescriptionOfficialReportViolationDomain.GetAll()
                        .Where(x => x.PrescriptionOfficialReport == entity)
                        .Select(x => x.PrescriptionViol.InspectionViolation).ToList();
                    try
                    {
                        //обновляем задачи
                        var reminders = ReminderDomain.GetAll()
                            .Where(x => x.Actuality && x.TypeReminder == Contracts.Enums.TypeReminder.Prescription)
                            .Where(x => x.DocumentGji != null && x.DocumentGji.Id == entity.Prescription.Id)
                            .Select(x => x.Id).ToList();
                        foreach (long id in reminders)
                        {
                            var rem = ReminderDomain.Get(id);
                            rem.CheckDate = entity.ExtensionViolationDate.Value;
                            ReminderDomain.Update(rem);
                        }
                        foreach (InspectionGjiViol iv in inspectionViolations)
                        {
                            iv.DatePlanRemoval = entity.ExtensionViolationDate;
                            InspectionGjiViolDomain.Update(iv);
                        }

                        foreach (PrescriptionViol pv in prescrViolations)
                        {
                            pv.DatePlanExtension = entity.ExtensionViolationDate;
                            PrescriptionViolDomain.Update(pv);
                        }

                        Prescription pr = PrescriptionDomain.Get(entity.Prescription.Id);
                        pr.RenewalApplicationNumber = entity.DocumentNumber;
                        pr.RenewalApplicationDate = entity.DocumentDate;
                        PrescriptionDomain.Update(pr);
                    }
                    catch
                    {
                        return this.Failure("Ошибка продления дат устранения нарушений");
                    }
                }
            }
            logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.PrescriptionOfficialReport, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.DocumentNumber);


            return this.Success();
        }
        public override IDataResult BeforeDeleteAction(IDomainService<PrescriptionOfficialReport> service, PrescriptionOfficialReport entity)
        {
            //InspectionGjiViolDomain
            var porViolIds = PrescriptionOfficialReportViolationDomain.GetAll()
                .Where(x => x.PrescriptionOfficialReport == entity)
                .Select(x => x.Id).ToList();

            foreach (long vid in porViolIds)
            {
                PrescriptionOfficialReportViolationDomain.Delete(vid);
            }
            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<PrescriptionOfficialReport> service, PrescriptionOfficialReport entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.PrescriptionOfficialReport, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<PrescriptionOfficialReport> service, PrescriptionOfficialReport entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.PrescriptionOfficialReport, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Prescription", "Предписание" },
                { "OfficialReportType", "Тип служебной записки" },
                { "DocumentDate", "Дата документа" },
                { "ViolationDate", "Дата устранения нарушений" },
                { "ExtensionViolationDate", "Дата продления нарушений" },
                { "Name", "Наименование" },
                { "DocumentNumber", "Номер документа" },
                { "Description", "Описание" },
                { "File", "Файл" },
                { "Inspector", "Инспектор" },
                { "State", "Статус" }
            };
            return result;
        }
    }
}