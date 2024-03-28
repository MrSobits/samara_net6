namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.NumberValidation;

    /// <summary>
    /// Такую пустышку навсякий смлучай нужно чтобы в регионах (Там где уже заменили или отнаследовались от этого класса) непопадало и можно было бы изменять методы как сущности Disposal
    /// </summary>
    public class DisposalServiceInterceptor : DisposalServiceInterceptor<Disposal>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в DisposalServiceInterceptor<T>
    }

    /// <summary>
    /// Короче такой поворот событий делается для того чтобы в Модулях регионов  спомошью 
    /// SubClass расширять сущность Disposal + не переписывать код который регистрируется по сущности
    /// то есть в Disposal добавляеться поля, но интерцептор поскольку Generic просто наследуется  
    /// </summary>
    public class DisposalServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Disposal
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            if (!ValidateContragentState(entity))
            {
                return Failure("Организация, указанная в основании проверки, не действует");
            }
            //проставляем вид контроля/надзора в проверку
            try
            {
                var inspectionService = Container.Resolve<IRepository<InspectionGji>>();
                var inspection = inspectionService.Get(entity.Inspection.Id);
                switch (entity.KindKNDGJI)
                {
                    case KindKNDGJI.HousingSupervision:
                        {
                            inspection.ControlType = ControlType.HousingSupervision;
                            inspectionService.Update(inspection);
                        }
                        break;
                    case KindKNDGJI.LicenseControl:
                        {
                            inspection.ControlType = ControlType.LicensedControl;
                            inspectionService.Update(inspection);
                        }
                        break;
                    case KindKNDGJI.NotSet:
                        {
                            inspection.ControlType = ControlType.NotSet;
                            inspectionService.Update(inspection);
                        }
                        break;
                    case KindKNDGJI.Both:
                        {
                            inspection.ControlType = ControlType.Both;
                            inspectionService.Update(inspection);
                        }
                        break;
                }


            }
            catch
            { }

            // проверяем дату проверки
            if (entity.DateEnd < entity.DateStart)
            {
                return Failure("Дата окончания проверки должна быть больше или равна дате начала проверки");
            }

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            if (!ValidateContragentState(entity))
            {
                return Failure("Организация, указанная в основании проверки, не действует");
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var actionResult = this.ActionWithDependencies(entity);
            if (!actionResult.Success)
            {
                return actionResult;
            }
            
            var annexService = Container.Resolve<IDomainService<DisposalAnnex>>();
            var expertService = Container.Resolve<IDomainService<DisposalExpert>>();
            var provDocsService = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var typeServiceService = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var domainServiceViolation = Container.Resolve<IDomainService<DisposalViolation>>();
            try
            {
                // Удаляем все дочерние Нарушения
                var violationIds = domainServiceViolation.GetAll().Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var violId in violationIds)
                {
                    domainServiceViolation.Delete(violId);
                }
            }
            finally
            {
                this.Container.Release(domainServiceViolation);
            }

            if (entity.TypeDisposal != TypeDisposalGji.NullInspection)
            {
                var servReminders = this.Container.Resolve<IDomainService<Reminder>>();
                try
                {
                    var reminders = servReminders.GetAll().Where(x => x.DocumentGji.Id == entity.Id).Select(x => x.Id).ToList();

                    foreach (var id in reminders)
                    {
                        servReminders.Delete(id);
                    }
                }
                finally
                {
                    this.Container.Release(servReminders);
                }
            }

            return base.BeforeDeleteAction(service, entity);
        }

        /// <summary>
        /// Действия с зависимыми сущностями.
        /// На РТ реализовано удаление
        /// </summary>
        /// <returns></returns>
        protected virtual IDataResult ActionWithDependencies(T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<DisposalAnnex>>();
            var expertService = this.Container.Resolve<IDomainService<DisposalExpert>>();
            var provDocsService = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var typeServiceService = this.Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var disposalInspFoundationCheckDomain = this.Container.Resolve<IDomainService<DisposalInspFoundationCheck>>();
            var surveyObjectiveDomain = this.Container.Resolve<IDomainService<DisposalSurveyObjective>>();
            var surveyPurposeDomain = this.Container.Resolve<IDomainService<DisposalSurveyPurpose>>();
            var disposalVerificationSubjectDomain = this.Container.Resolve<IDomainService<DisposalVerificationSubject>>();

            try
            {
                var refFuncs = new List<Func<long, string>>
                               {
                                  id => annexService.GetAll().Any(x => x.Disposal.Id == id) ? "Приложения" : null,
                                  id => expertService.GetAll().Any(x => x.Disposal.Id == id) ? "Эксперты" : null,
                                  id => provDocsService.GetAll().Any(x => x.Disposal.Id == id) ? "Предоставляемые документы" : null,
                                  id => typeServiceService.GetAll().Any(x => x.Disposal.Id == id) ? "Типы обследования" : null,
                                  id => disposalInspFoundationCheckDomain.GetAll().Any(x => x.Disposal.Id == id) ? "НПА проверки" : null,
                                  id => surveyObjectiveDomain.GetAll().Any(x => x.Disposal.Id == id) ? "Задачи проверки" : null,
                                  id => surveyPurposeDomain.GetAll().Any(x => x.Disposal.Id == id) ? "Цели проверки" : null,
                                  id => disposalVerificationSubjectDomain.GetAll().Any(x => x.Disposal.Id == id) ? "Предметы проверки" : null
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                return refs.Length > 0 
                    ? this.Failure($"Существуют связанные записи в следующих таблицах: {string.Join(", ", refs)}") 
                    : new BaseDataResult();
            }
            finally
            {
                this.Container.Release(annexService);
                this.Container.Release(expertService);
                this.Container.Release(provDocsService);
                this.Container.Release(typeServiceService);
                this.Container.Release(disposalInspFoundationCheckDomain);
                this.Container.Release(surveyObjectiveDomain);
                this.Container.Release(surveyPurposeDomain);
                this.Container.Release(disposalVerificationSubjectDomain);
            }
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {

            // После удаления распоряжения нужно удалить все пустыепроверки по Отопительному сезону или по Административной деятельности
            // Пустые - это значит в которых нет ниодного документа
            var domainServiceBaseHeatSeason = Container.Resolve<IDomainService<BaseHeatSeason>>();
            var domainServiceActivityTsj = Container.Resolve<IDomainService<BaseActivityTsj>>();
            var domainServiceDocument = Container.Resolve<IDomainService<DocumentGji>>();
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();

            try
            {
                // если тип распоряжения nullInspection, то дополнительных очисток не требуется
                if (entity.TypeDisposal != TypeDisposalGji.NullInspection)
                {
                    var baseHeatSeasonIds =
                    domainServiceBaseHeatSeason.GetAll()
                                               .Where(
                                                   x =>
                                                   !domainServiceDocument.GetAll().Any(y => y.Inspection.Id == x.Id))
                                               .Select(x => x.Id)
                                               .ToList();

                    baseHeatSeasonIds.ForEach(x => domainServiceBaseHeatSeason.Delete(x));

                    var baseActivityIds =
                        domainServiceActivityTsj.GetAll()
                                                   .Where(
                                                       x =>
                                                       !domainServiceDocument.GetAll().Any(y => y.Inspection.Id == x.Id))
                                                   .Select(x => x.Id)
                                                   .ToList();

                    baseActivityIds.ForEach(x => domainServiceActivityTsj.Delete(x));
                }

                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            finally
            {
                Container.Release(domainServiceBaseHeatSeason);
                Container.Release(domainServiceActivityTsj);
                Container.Release(domainServiceDocument);
            }

            return base.AfterDeleteAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }

            return base.AfterUpdateAction(service, entity);
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Inspection", "Проверка ГЖИ" },
                { "Stage", "Этап проверки" },
                { "TypeDocumentGji", "Тип документа ГЖИ" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentSubNum", "Дополнительный номер документа (порядковый номер если документов одного типа несколько)" },
                { "State", "Статус" },
                { "TypeDisposal", "Тип распоряжения" },
                { "DateStart", "Дата начала обследования" },
                { "DateEnd", "Дата окончания обследования" },
                { "TypeAgreementProsecutor", "Согласование с прокуротурой" },
                { "DocumentNumberWithResultAgreement", "Номер документа с результатом согласования" },
                { "TypeAgreementResult", "Результат согласования" },
                { "DocumentDateWithResultAgreement", "Дата документа с результатом согласования" },
                { "IssuedDisposal", "Должностное лицо (ДЛ) вынесшее распоряжение" },
                { "ResponsibleExecution", "Ответственный за исполнение" },
                { "KindCheck", "Вид проверки" },
                { "Description", "Описание" },
                { "ObjectVisitStart", "Выезд на объект с" },
                { "ObjectVisitEnd", "Выезд на объект по" },
                { "TimeVisitStart", "Время начала визита (Время с)" },
                { "TimeVisitEnd", "Время окончания визита (Время по)" },
                { "NcNum", "Номер документа (Уведомление о проверке)" },
                { "NcDate", "Дата документа (Уведомление о проверке)" }
            };
            return result;
        }

        protected bool ValidateContragentState(T document)
        {
            var contragent = document.ReturnSafe(x => x.Inspection.Contragent);

            if (contragent != null)
            {
                if (contragent.ContragentState == ContragentState.Bankrupt
                    || contragent.ContragentState == ContragentState.Liquidated)
                {
                    return (document.DocumentDate ?? DateTime.Now.Date) <= contragent.DateTermination;
                }
            }

            return true;
        }
    }
}