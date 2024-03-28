namespace Bars.Gkh.Repair.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Repair.Entities;

    public class RepairObjectServiceInterceptor : EmptyDomainInterceptor<RepairObject>
    {
        public IStateProvider StateProvider { get; set; }

        public IDomainService<RepairWork> RepairWorkDomain { get; set; }
        
        public override IDataResult BeforeCreateAction(IDomainService<RepairObject> service, RepairObject entity)
        {
            //проверяем, есть ли объект текущего ремонта с таким домом и такой программой
            if (service.GetAll().Any(x => x.RepairProgram.Id == entity.RepairProgram.Id && x.RealityObject.Id == entity.RealityObject.Id))
            {
                return Failure("Объект текущего ремонта с таким жилым домом и программой уже существует!");
            }

            // Перед сохранением проставляем начальный статус
            this.StateProvider.SetDefaultState(entity);

            if (entity.State == null)
            {
                return Failure("Для объекта текущего ремонта не задан начальный статус!");
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RepairObject> service, RepairObject entity)
        {
            //проверяем, есть ли объект текущего ремонта с таким домом и такой программой
            if (service.GetAll().Any(x => x.RepairProgram.Id == entity.RepairProgram.Id && x.RealityObject.Id == entity.RealityObject.Id && x.Id != entity.Id))
            {
                return Failure("Объект текущего ремонта с таким жилым домом и программой уже существует!");
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RepairObject> service, RepairObject entity)
        {
            //проверяем, есть ли у объекта текущего ремонта виды работ
            if (RepairWorkDomain.GetAll().Any(x => x.RepairObject.Id == entity.Id))
            {
                return Failure("У объекта текущего ремонта существуют виды работ!");
            }

            return Success();
        }
    }
}