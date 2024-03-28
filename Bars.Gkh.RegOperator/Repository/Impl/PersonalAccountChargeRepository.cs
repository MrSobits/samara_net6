namespace Bars.Gkh.RegOperator.Repository.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    public class PersonalAccountChargeRepository : IPersonalAccountChargeRepository
    {
        private readonly IRepository<PersonalAccountCharge> repository;

        public PersonalAccountChargeRepository(IRepository<PersonalAccountCharge> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Получить начисления за период, которые надо зафиксиировать
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="notActive">Исключать неактивные ЛС</param>
        public IQueryable<PersonalAccountCharge> GetNeedToBeFixedForPeriod(IPeriod period, bool notActive = true)
        {
            return this.repository.GetAll()
                .Where(x => !x.IsFixed && x.IsActive)
                .Where(x => x.ChargePeriod.Id == period.Id)
                .WhereIf(notActive, x => x.BasePersonalAccount.State.Code != "4");
        }

        /// <summary>
        /// Получить начисления за период
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="realtyObjects">Дома</param>
        public IQueryable<PersonalAccountCharge> GetChargesByPeriodAndRealties(IPeriod period, IEnumerable<RealityObject> realtyObjects)
        {
            var data = this.repository.GetAll()
                .Where(x => x.ChargePeriod.Id == period.Id)
                .Where(x => realtyObjects.Any(r => r.Id == x.BasePersonalAccount.Room.RealityObject.Id));

            return data;
        }

        /// <summary>
        /// Получить начисления по неактивным лс, которые нужно вычесть
        /// </summary>
        /// <param name="period">Период</param>
        /// <returns>Начисления</returns>
        public IQueryable<PersonalAccountCharge> GetNeedToBeUndo(IPeriod period)
        {
            return this.repository.GetAll()
                .Where(x => !x.IsFixed && x.IsActive)
                .Where(x => x.ChargePeriod.Id == period.Id)
                .Where(x => x.BasePersonalAccount.State.Code == "4");
        }
    }
}