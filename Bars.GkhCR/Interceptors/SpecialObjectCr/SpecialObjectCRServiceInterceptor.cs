namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Entities;

    public class SpecialObjectCrServiceInterceptor : EmptyDomainInterceptor<SpecialObjectCr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialObjectCr> service, SpecialObjectCr entity)
        {
            //проверяем, есть ли объект кр с таким домом и такой программой
            if (service.GetAll().Any(x => x.ProgramCr.Id == entity.ProgramCr.Id && x.RealityObject.Id == entity.RealityObject.Id))
            {
                return this.Failure("Объект КР с таким жилым домом и программой уже существует!");
            }

            // Перед сохранением проставляем начальный статус
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SpecialObjectCr> service, SpecialObjectCr entity)
        {
            //проверяем, есть ли объект кр с таким домом и такой программой
            if (service.GetAll().Any(x => x.ProgramCr.Id == entity.ProgramCr.Id && x.RealityObject.Id == entity.RealityObject.Id && x.Id != entity.Id))
            {
                return this.Failure("Объект КР с таким жилым домом и программой уже существует!");
            }

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<SpecialObjectCr> service, SpecialObjectCr entity)
        {
            // После сохранения создаем Мониторинг СМР со ссылкой на этот Oбъект КР
            var serviceObj = this.Container.Resolve<IDomainService<SpecialMonitoringSmr>>();
            var newObj = new SpecialMonitoringSmr { ObjectCr = entity };

            serviceObj.Save(newObj);

            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<SpecialObjectCr> service, SpecialObjectCr entity)
        {
            // После обновления создаем Мониторинг СМР, если у этого объекта не было Мониторинга СМР
            var serviceObj = this.Container.Resolve<IDomainService<SpecialMonitoringSmr>>();
            if (!serviceObj.GetAll().Any(x => x.ObjectCr.Id == entity.Id))
            {
                var newObj = new SpecialMonitoringSmr { ObjectCr = entity };
                serviceObj.Save(newObj);
            }

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SpecialObjectCr> service, SpecialObjectCr entity)
        {
            var refFuncs = new List<Func<long, string>>
            {
                id => this.Container.Resolve<IDomainService<SpecialEstimateCalculation>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Cметный расчет по работе" : null,
                id => this.Container.Resolve<IDomainService<SpecialPerformedWorkAct>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Акт выполненных работ" : null,
                id => this.Container.Resolve<IDomainService<SpecialQualification>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Участники квалификационного отбора" : null,
                id => this.Container.Resolve<IDomainService<SpecialBuildContract>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Договоры подряда объекта КР" : null,
                id => this.Container.Resolve<IDomainService<SpecialContractCr>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Договоры объекта КР" : null,
                id => this.Container.Resolve<IDomainService<SpecialDefectList>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Дефектная ведомость объекта КР" : null,
                id => this.Container.Resolve<IDomainService<SpecialDocumentWorkCr>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Документы объекта КР" : null,
                id => this.Container.Resolve<IDomainService<SpecialFinanceSourceResource>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Средства источника финансирования объекта КР" : null,
                id => this.Container.Resolve<IDomainService<SpecialPersonalAccount>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Лицевые счета объекта КР" : null,
                id => this.Container.Resolve<IDomainService<SpecialProtocolCr>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Протоколы, акты объекта КР" : null,
                id => this.Container.Resolve<IDomainService<SpecialTypeWorkCr>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Виды работ объекта КР" : null
            };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + $" {str}; ");
                message = $"Существуют связанные записи в следующих таблицах: {message}";
                return this.Failure(message);
            }

            var monitoringSmrService = this.Container.Resolve<IDomainService<MonitoringSmr>>();
            var monitoringSmrList = monitoringSmrService.GetAll().Where(x => x.ObjectCr.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in monitoringSmrList)
            {
                monitoringSmrService.Delete(value);
            }

            return this.Success();
        }
    }
}