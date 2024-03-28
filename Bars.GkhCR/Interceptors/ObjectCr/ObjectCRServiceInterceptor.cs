namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Entities;

    public class ObjectCrServiceInterceptor : EmptyDomainInterceptor<ObjectCr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ObjectCr> service, ObjectCr entity)
        {
            //проверяем, есть ли объект кр с таким домом и такой программой
            if (service.GetAll().Any(x => x.ProgramCr.Id == entity.ProgramCr.Id && x.RealityObject.Id == entity.RealityObject.Id))
            {
                return Failure("Объект КР с таким жилым домом и программой уже существует!");
            }

            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ObjectCr> service, ObjectCr entity)
        {
            //проверяем, есть ли объект кр с таким домом и такой программой
            if (service.GetAll().Any(x => x.ProgramCr.Id == entity.ProgramCr.Id && x.RealityObject.Id == entity.RealityObject.Id && x.Id != entity.Id))
            {
                return Failure("Объект КР с таким жилым домом и программой уже существует!");
            }

            return Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<ObjectCr> service, ObjectCr entity)
        {
            // После сохранения создаем Мониторинг СМР со ссылкой на этот Oбъект КР
            var serviceObj = Container.Resolve<IDomainService<MonitoringSmr>>();
            var newObj = new MonitoringSmr {ObjectCr = entity};

            serviceObj.Save(newObj);

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ObjectCr> service, ObjectCr entity)
        {
            // После обновления создаем Мониторинг СМР, если у этого объекта не было Мониторинга СМР
            var serviceObj = Container.Resolve<IDomainService<MonitoringSmr>>();
            if (!serviceObj.GetAll().Any(x => x.ObjectCr.Id == entity.Id))
            {
                var newObj = new MonitoringSmr {ObjectCr = entity};
                serviceObj.Save(newObj);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ObjectCr> service, ObjectCr entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => Container.Resolve<IDomainService<BankStatement>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Банковские выписки" : null,
                                   id => Container.Resolve<IDomainService<EstimateCalculation>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Cметный расчет по работе" : null,
                                   id => Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Акт выполненных работ" : null,
                                   id => Container.Resolve<IDomainService<Qualification>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Участники квалификационного отбора" : null,
                                   id => Container.Resolve<IDomainService<BuildContract>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Договоры подряда объекта КР" : null,
                                   id => Container.Resolve<IDomainService<ContractCr>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Договоры объекта КР" : null,
                                   id => Container.Resolve<IDomainService<DefectList>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Дефектная ведомость объекта КР" : null,
                                   id => Container.Resolve<IDomainService<DocumentWorkCr>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Документы объекта КР" : null,
                                   id => Container.Resolve<IDomainService<FinanceSourceResource>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Средства источника финансирования объекта КР" : null,
                                   id => Container.Resolve<IDomainService<PersonalAccount>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Лицевые счета объекта КР" : null,
                                   id => Container.Resolve<IDomainService<ProtocolCr>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Протоколы, акты объекта КР" : null,
                                   id => Container.Resolve<IDomainService<TypeWorkCr>>().GetAll().Any(x => x.ObjectCr.Id == id) ? "Виды работ объекта КР" : null
                               };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return Failure(message);
            }

            var monitoringSmrService = Container.Resolve<IDomainService<MonitoringSmr>>();
            var monitoringSmrList = monitoringSmrService.GetAll().Where(x => x.ObjectCr.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in monitoringSmrList)
            {
                monitoringSmrService.Delete(value);
            }

            return Success();
        }
    }
}