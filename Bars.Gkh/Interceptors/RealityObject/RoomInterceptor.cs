using Bars.B4;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Interceptors
{
    public class RoomInterceptor : EmptyDomainInterceptor<Room>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Room> service, Room entity)
        {
            return CheckArea(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Room> service, Room entity)
        {
            return CheckArea(entity);
        }

        private IDataResult CheckArea(Room entity)
        {
            if (entity.LivingArea > entity.Area)
            {
                return Failure("Жилая площадь не может быть больше общей площади");
            }

            return Success();
        }
    }
}
