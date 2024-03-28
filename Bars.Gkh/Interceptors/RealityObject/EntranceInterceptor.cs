namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;

    public class EntranceInterceptor : EmptyDomainInterceptor<Entrance>
    {
        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<Entrance> service, Entrance entity)
        {
            var roomRepo = Container.ResolveRepository<Room>();

            var rooms = roomRepo.GetAll()
                .Where(x => x.Entrance.Id == entity.Id)
                .ToArray();

            foreach (var room in rooms)
            {
                room.Entrance = null;
                roomRepo.Update(room);
            }

            return Success();
        }
    }
}