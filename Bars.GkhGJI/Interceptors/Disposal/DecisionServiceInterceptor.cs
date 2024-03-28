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
    public class DecisionServiceInterceptor : DecisionServiceInterceptor<Decision>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в DisposalServiceInterceptor<T>
    }

    /// <summary>
    /// Короче такой поворот событий делается для того чтобы в Модулях регионов  спомошью 
    /// SubClass расширять сущность Disposal + не переписывать код который регистрируется по сущности
    /// то есть в Disposal добавляеться поля, но интерцептор поскольку Generic просто наследуется  
    /// </summary>
    public class DecisionServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Decision
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

            var annexService = Container.Resolve<IDomainService<DecisionAnnex>>();
            var expertService = Container.Resolve<IDomainService<DecisionExpert>>();
            var provDocsService = Container.Resolve<IDomainService<DecisionProvidedDoc>>();
            var typeServiceAdmin = Container.Resolve<IDomainService<DecisionAdminRegulation>>();
            var typeServiceCS = Container.Resolve<IDomainService<DecisionControlSubjects>>();
            
            try
            {
                var refFuncs = new List<Func<long, string>>
                               {
                                  id => annexService.GetAll().Any(x => x.Decision.Id == id) ? "Приложения" : null,
                                  id => expertService.GetAll().Any(x => x.Decision.Id == id) ? "Эксперты" : null,
                                  id => provDocsService.GetAll().Any(x => x.Decision.Id == id) ? "Предоставляемые документы" : null,
                                  id => typeServiceAdmin.GetAll().Any(x => x.Decision.Id == id) ? "Административные регламенты" : null,
                                  id => typeServiceCS.GetAll().Any(x => x.Decision.Id == id) ? "Субъекты проверки" : null,
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

            }
            finally
            {
                Container.Release(annexService);
                Container.Release(expertService);
                Container.Release(provDocsService);
                Container.Release(typeServiceAdmin);
                Container.Release(typeServiceCS);
            }
            
            return base.BeforeDeleteAction(service, entity);
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
            return this.Success();
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
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
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
                { "IssuedDisposal", "Должностное лицо (ДЛ) вынесшее решение" },
                { "KindCheck", "Вид проверки" },
                { "Description", "Описание" },
                { "ObjectVisitStart", "Выезд на объект с" },
                { "ObjectVisitEnd", "Выезд на объект по" },
                { "TimeVisitStart", "Время начала визита (Время с)" },
                { "TimeVisitEnd", "Время окончания визита (Время по)" }
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