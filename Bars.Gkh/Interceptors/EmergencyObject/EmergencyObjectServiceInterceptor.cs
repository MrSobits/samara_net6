namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using B4;
    using B4.Modules.States;
    using Entities;

    public class EmergencyObjectServiceInterceptor : EmptyDomainInterceptor<EmergencyObject>
    {
        public override IDataResult BeforeCreateAction(IDomainService<EmergencyObject> service, EmergencyObject entity)
        {
            // перед сохранением проверяем существует ли аварийный дом с таким жилым домом
            if (service.GetAll().Any(x => x.RealityObject.Id == entity.RealityObject.Id))
            {
                return this.Failure("Аварийный дом уже создан");
            }

            // Перед сохранением проставляем начальный статус
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<EmergencyObject> service, EmergencyObject entity)
        {
            // перед сохранением проверяем существует ли аварийный дом с таким жилым домом
            if (service.GetAll().Any(x => x.RealityObject.Id == entity.RealityObject.Id && x.Id != entity.Id))
            {
                return this.Failure("Аварийный дом уже создан");
            }

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<EmergencyObject> service, EmergencyObject entity)
        {
            if (this.Container.Resolve<IDomainService<EmerObjResettlementProgram>>().GetAll().Any(x => x.EmergencyObject.Id == entity.Id))
            {
                return this.Failure("Существуют связанные записи в следующих таблицах: Разрезы финансирования по программе переселения;");
            }

            return this.Success();
        }
    }
}
