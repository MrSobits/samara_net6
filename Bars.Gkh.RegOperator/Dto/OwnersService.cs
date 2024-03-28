namespace Bars.Gkh.RegOperator.Dto
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;
    using Bars.Gkh.DomainService;
    
    /// <summary>
    /// Сервис для получение количество собственников
    /// </summary>
    public class OwnersService : IOwnersService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен сервис для Лицевой счет
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>
        /// Домен сервис для Помещение
        /// </summary>
        public IDomainService<Room> Domain { get; set; }

        /// <summary>
        /// Получение количество собственников
        /// </summary>
        /// <param name="ids">Id жилых домов</param>
        /// <returns></returns>
        public Dictionary<long, int> GetOwnersCount(long[] ids)
        {
            var query = this.Domain.GetAll()
                      .Where(x => ids.Contains(x.RealityObject.Id))
                      .Select(x => x.Id);

            var accountsPerRoom = this.BasePersonalAccountDomain.GetAll()
                .Where(x => query.Any(y => y == x.Room.Id))
                .Where(x => !x.State.FinalState)
                .GroupBy(x => x.Room.RealityObject.Id)
                .Select(x => new
                {
                    x.Key,
                    Count = x.Count(),
                })
                .ToDictionary(x => x.Key, x => x.Count);

            return accountsPerRoom;
        }
    }
}