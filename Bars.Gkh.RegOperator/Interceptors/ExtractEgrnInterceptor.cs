namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;
    using Bars.Gkh.RegOperator.Entities;
    using Sobits.RosReg.Entities;

    /// <summary>
    /// Интерцептор <see cref="ExtractEgrn"/>
    /// </summary>
    public class ExtractEgrnInterceptor : EmptyDomainInterceptor<ExtractEgrn>
    {       
        /// <summary>
        /// Домен сервис для сущности "Жилой дом - Сведения о помещениях"
        /// </summary>
        public IDomainService<Room> RoomDomainService { get; set; }

        /// <summary>
        ///  Жилой дом Сведения о помещениях
        ///  проверка перед созднием 
        /// </summary>
        public override IDataResult BeforeCreateAction(IDomainService<ExtractEgrn> service, ExtractEgrn entity)
        {
            if (entity.Room_id != null)
            {
               string fullAddress = entity.Room_id > 0 ? RoomDomainService.Get(entity.Room_id).RealityObject.Address + ", пом. " + RoomDomainService.Get(entity.RoomId.Id).RoomNum : "";
                entity.FullAddress = fullAddress;
            }
            else if (entity.RoomId != null)
            {
                string fullAddress = entity.Room_id > 0 ? RoomDomainService.Get(entity.RoomId.Id).RealityObject.Address + ", пом. " + RoomDomainService.Get(entity.RoomId.Id).RoomNum : "";
                entity.FullAddress = fullAddress;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        ///  Жилой дом Сведения о помещениях
        ///  Проверка перед редактированием
        /// </summary>
        public override IDataResult BeforeUpdateAction(IDomainService<ExtractEgrn> service, ExtractEgrn entity)
        {

            if (entity.Room_id >0)
            {
                var room = RoomDomainService.Get(entity.Room_id);
                string fullAddress = room.Id > 0 ? room.RealityObject.Address + ", пом. " + room.RoomNum : "";
                entity.FullAddress = fullAddress;
                entity.RoomId = room;
            }
            return Success();
        }

     
    }
}