namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерцептор <see cref="Room"/>
    /// </summary>
    public class RoomInterceptor : EmptyDomainInterceptor<Room>
    {       
        /// <summary>
        /// Домен сервис для сущности "Жилой дом - Сведения о помещениях"
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomainService { get; set; }

        /// <summary>
        ///  Жилой дом Сведения о помещениях
        ///  проверка перед созднием 
        /// </summary>
        public override IDataResult BeforeCreateAction(IDomainService<Room> service, Room entity)
        {
            var validation = this.Validate(service, entity);

            if (!validation.Success)
            {
                return validation;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        ///  Жилой дом Сведения о помещениях
        ///  Проверка перед редактированием
        /// </summary>
        public override IDataResult BeforeUpdateAction(IDomainService<Room> service, Room entity)
        {
            var validation = this.Validate(service, entity);

            if (!validation.Success)
            {
                return validation;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private IDataResult Validate(IDomainService<Room> service, Room entity)
        {
            if (service.GetAll()
                .Any(x => x.Id != entity.Id 
                    && x.RealityObject.Id == entity.RealityObject.Id 
                    && x.RoomNum == entity.RoomNum
                    && x.ChamberNum == entity.ChamberNum))
            {
                return this.Failure("Помещение с таким номером уже существует.");
            }

            return this.Success();
        }

        /// <summary>
        /// Жилой дом Сведения о помещениях
        /// проверка перед удалением
        /// </summary>
        public override IDataResult BeforeDeleteAction(IDomainService<Room> service, Room entity)
        {
            if (PersonalAccountDomainService.GetAll().Any(x => x.Room.Id == entity.Id))
            {
                return this.Failure("Удаление невозможно. Есть привязка помещения к абонентам");
            }

            return base.BeforeDeleteAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult AfterUpdateAction(IDomainService<Room> service, Room entity)
        {
            DomainEvents.Raise(new RoomChangeEvent(entity));
            return base.AfterUpdateAction(service, entity);
        }
    }
}