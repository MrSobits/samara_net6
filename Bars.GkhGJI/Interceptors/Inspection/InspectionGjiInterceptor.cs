namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    // Пустышка нужна чтобы регистрировать и в регионах где могли наследоваться от этого типа чтобы осталось все в рабочем состоянии
    /// <inheritdoc />
    public class InspectionGjiInterceptor : InspectionGjiInterceptor<InspectionGji>
    {
        //Внимание!!! не удалять поскольку в регионах могли наследвоатся от этой сущности
    }

    // Делаю Generic чтобы лучше наследоватся и заменять в других модулях
    /// <inheritdoc />
    public class InspectionGjiInterceptor<T> : EmptyDomainInterceptor<T>
        where T: InspectionGji
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            try
            {
                stateProvider.SetDefaultState(entity);

                return this.Success();
            }
            finally
            {
                this.Container.Release(stateProvider);
            }
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var domainServiceDocument = this.Container.Resolve<IDomainService<DocumentGji>>();
            var domainServiceViolation = this.Container.Resolve<IDomainService<InspectionGjiViol>>();
            var domainServiceStage = this.Container.Resolve<IDomainService<InspectionGjiStage>>();
            var domainServiceObject = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var domainServiceInspector = this.Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var domainServiceDocRef = this.Container.Resolve<IDomainService<InspectionDocGjiReference>>();
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();
            var appealCitsServ = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var zonalInspection = this.Container.Resolve<IDomainService<InspectionGjiZonalInspection>>();
            var inspectionRisk = this.Container.Resolve<IDomainService<InspectionRisk>>();
            var inspectionContragent = this.Container.Resolve<IDomainService<InspectionBaseContragent>>();

            using (this.Container.Using(domainServiceDocument,
                domainServiceViolation,
                domainServiceStage,
                domainServiceObject,
                domainServiceInspector,
                domainServiceDocRef,
                servReminder,
                appealCitsServ,
                zonalInspection,
                inspectionRisk,
                inspectionContragent))
            {
                // Удаляем все дочерние документы у постановления
                domainServiceDocument.GetAll().Where(x => x.Inspection.Id == entity.Id).ForEach(
                    x =>
                    {
                        var type = x.GetType();
                        var domainServiceType = typeof(IDomainService<>).MakeGenericType(type);
                        var domainServiceImpl = (IDomainService) this.Container.Resolve(domainServiceType);
                        try
                        {
                            domainServiceImpl.Delete(x.Id);
                        }
                        finally
                        {
                            this.Container.Release(domainServiceImpl);
                        }
                    });

                // Удаляем все дочерние Нарушения
                domainServiceViolation.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceViolation.Delete(x));

                // Удаляем все дочерние Этапы проверки
                domainServiceStage.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceStage.Delete(x));

                // Удаляем все дочерние Проверяемые дома
                domainServiceObject.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceObject.Delete(x));

                // Удаляем всех дочерних инспекторов
                domainServiceInspector.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceInspector.Delete(x));

                // Удаляем напоминания по проверке
                servReminder.GetAll().Where(x => x.InspectionGji.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => servReminder.Delete(x));

                // удаляем Неявные ссылки (Если такие имеются)
                domainServiceDocRef.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceDocRef.Delete(x));

                appealCitsServ.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => appealCitsServ.Delete(x));

                // Удаляем отделы в проверке
                zonalInspection.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => zonalInspection.Delete(x));

                // Удаляем категории риска
                inspectionRisk.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => inspectionRisk.Delete(x));

                // Удаляем органы совместной проверки
                inspectionContragent.GetAll().Where(x => x.InspectionGji.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => inspectionContragent.Delete(x));

                return this.Success();
            }
        }

        /// <inheritdoc />
        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            var result = this.SaveInspectionRisk(entity);
            if (!result.Success)
            {
                return result;
            }

            this.CreateReminders(entity);

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            this.CreateReminders(entity);
        //    this.SendRequests(entity);

            return this.Success();
        }

        private void CreateReminders(T entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var servReminderRule = this.Container.ResolveAll<IReminderRule>();

            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "InspectionReminderRule");
                if (rule != null)
                {
                    rule.Create(entity);
                }
            }
            finally
            {
                this.Container.Release(servReminderRule);
            }
        }

        /// <summary>
        /// Сохранить категории рисков проверки ГЖИ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDataResult SaveInspectionRisk(T entity)
        {
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();
            using (this.Container.Using(inspectionRiskDomain))
            {
                var actualRisk = inspectionRiskDomain.GetAll()
                    .Where(x => x.Inspection == entity)
                    .FirstOrDefault(x => !x.EndDate.HasValue);

                if (entity.RiskCategory == null && actualRisk != null)
                {
                    inspectionRiskDomain.Delete(actualRisk.Id);
                    return this.Success();
                }

                if (actualRisk == null)
                {
                    var maxEndDate = inspectionRiskDomain.GetAll()
                        .Where(x => x.Inspection == entity)
                        .Where(x => x.EndDate.HasValue)
                        .Select(x => x.EndDate)
                        .Max();
                    
                    if (entity.RiskCategoryStartDate <= maxEndDate)
                    {
                        return this.Failure("Неверно указана дата начала присвоения категории риска. Дата начала " +
                            "не должна пересекаться с периодами предыдущих категорий. Измените дату начала или удалите " +
                            "соответствующий период в предыдущих категориях риска.");
                    }

                    if (entity.RiskCategory == null && !entity.RiskCategoryStartDate.HasValue)
                    {
                        return this.Success();
                    }

                    actualRisk = new InspectionRisk
                    {
                        Inspection = entity,
                        RiskCategory = entity.RiskCategory,
                        StartDate = entity.RiskCategoryStartDate.GetValueOrDefault()
                    };

                    inspectionRiskDomain.Save(actualRisk);
                    return this.Success();
                }

                // Если было изменено только поле «Категория», 
                if (actualRisk.RiskCategory != entity.RiskCategory && actualRisk.StartDate == entity.RiskCategoryStartDate)
                {
                    actualRisk.RiskCategory = entity.RiskCategory;
                    inspectionRiskDomain.Update(actualRisk);
                }
                // Если было изменено только поле «Дата начала»
                else if (actualRisk.RiskCategory == entity.RiskCategory && actualRisk.StartDate != entity.RiskCategoryStartDate)
                {
                    var maxEndDate = inspectionRiskDomain.GetAll()
                        .Where(x => x.Inspection == entity)
                        .Where(x => x.EndDate.HasValue)
                        .Select(x => x.EndDate)
                        .Max();

                    if (entity.RiskCategoryStartDate <= maxEndDate)
                    {
                        return this.Failure("Неверно указана дата начала присвоения категории риска. Дата начала " +
                            "не должна пересекаться с периодами предыдущих категорий. Измените дату начала или удалите " +
                            "соответствующий период в предыдущих категориях риска.");
                    }

                    actualRisk.StartDate = entity.RiskCategoryStartDate.GetValueOrDefault();
                    inspectionRiskDomain.Update(actualRisk);
                }
                // Если были изменены поля «Категория» и «Дата начала»
                else if (actualRisk.RiskCategory != entity.RiskCategory && actualRisk.StartDate != entity.RiskCategoryStartDate)
                {
                    var maxEndDate = inspectionRiskDomain.GetAll()
                        .Where(x => x.Inspection == entity)
                        .Where(x => x.EndDate.HasValue)
                        .Select(x => x.EndDate)
                        .Max();

                    if (entity.RiskCategoryStartDate <= maxEndDate)
                    {
                        return this.Failure("Неверно указана дата начала присвоения категории риска. Дата начала " +
                            "не должна пересекаться с периодами предыдущих категорий. Измените дату начала или удалите " +
                            "соответствующий период в предыдущих категориях риска.");
                    }

                    if (entity.RiskCategoryStartDate < actualRisk.StartDate)
                    {
                        return this.Failure($"Новая дата начала присвоения категории риска не может быть меньше чем предыдущая {actualRisk.StartDate:d}");
                    }

                    var newRisk = new InspectionRisk
                    {
                        Inspection = entity,
                        RiskCategory = entity.RiskCategory,
                        StartDate = entity.RiskCategoryStartDate.GetValueOrDefault()
                    };

                    actualRisk.EndDate = newRisk.StartDate.AddDays(-1);

                    inspectionRiskDomain.Save(newRisk);
                    inspectionRiskDomain.Update(actualRisk);
                }
            }

            return this.Success();
        }

        private void SendRequests(T entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var smevRule = Container.ResolveAll<ISMEVRule>();

            try
            {
                var rule = smevRule.FirstOrDefault(x => x.Id == "BaseSMEVInspectionRule");
                if (rule != null)
                {
                    rule.SendRequests(entity);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(smevRule);
            }
        }
    }
}
