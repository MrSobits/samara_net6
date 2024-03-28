namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using B4;
    using B4.Utils.Annotations;
    using Entities;
    using Gkh.Entities;

    using B4.Application;
    using B4.Utils;
    using Gkh.Domain.CollectionExtensions;
    using Castle.Windsor;

    public class RoomAreaShareSpecification : IRoomAreaShareSpecification
    {
        private IWindsorContainer Container
        {
            get { return ApplicationContext.Current.Container; }
        }

        /// <summary>
        /// Проверка поля areaShare  от 0 до 1, и чтобы сумма с другими полями этого же помещения была не больше 1 за период времени, начиная с указанной даты 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="room"></param>
        /// <param name="areaShare"></param>
        /// <param name="dateActual"></param>
        /// <returns></returns>
        public bool ValidateAreaShare(BasePersonalAccount account, Room room, decimal areaShare, DateTime? dateActual = null)
        {
            var accountDomain = Container.Resolve<IDomainService<BasePersonalAccount>>();
            var entityLogLightDomain = Container.Resolve<IDomainService<EntityLogLight>>();
            try
            {
                ArgumentChecker.NotNull(room, "room");
                ArgumentChecker.IsBetween(areaShare, 0, 1, "areaShare");


                var existsAreaShare =
                    accountDomain.GetAll()
                        .Where(x => x.Room.Id == room.Id)
                        .Where(x => x.Id != account.Id)
                        .Select(x => (decimal?) x.AreaShare)
                        .Sum() ?? 0M;


                if (existsAreaShare + areaShare > 1)
                {
                    return false;
                }

                if (!dateActual.HasValue)
                {
                    throw new ArgumentException("Не проставлена Дата начала действия значения");
                }
                
                var allIds = accountDomain.GetAll()
                    .Where(x => x.Room.Id == room.Id && x.Id != account.Id)
                    .Select(x => x.Id).ToArray();

                var allHistory = entityLogLightDomain.GetAll()
                    .Where( x => x.ClassName == "BasePersonalAccount" && x.ParameterName == "area_share")
                    .WhereContains(x => x.EntityId, allIds)
                    .ToList();
                    
                //исключаем более ранние изменения с той же датой начала действия
                var filteredHistory = allHistory
                    .GroupBy(x => new { x.EntityId, x.DateActualChange })
                    .ToDictionary(x => x.Key)
                    .Select(x => x.Value.OrderByDescending(u => u.DateApplied).FirstOrDefault())
                    .ToList()
                    .GroupBy(x => x.EntityId)
                    .ToDictionary(x => x.Key);

                var firstNeeded =
                    filteredHistory.Select(
                            z => z.Value
                                .OrderByDescending(p => p.DateApplied)
                                .FirstOrDefault(p => p.DateActualChange <= dateActual.Value))
                        .Where(p => p != null )
                        .Select(z => new {z.EntityId, z.DateApplied})
                        .ToDictionary(z => z.EntityId);

                var actualHistory = filteredHistory
                    .Select(x => x.Value.OrderByDescending(y => y.DateApplied)
                        .Where(y => !firstNeeded.ContainsKey(y.EntityId) ||
                                    y.DateApplied >= firstNeeded[y.EntityId].DateApplied)
                        .ToList()
                    ).ToList();

                //проверка на общую площадь помещения за период времени с требуемой даты установки
                List<DateTime> allDates = new List<DateTime>();
                foreach (var entityLogs in actualHistory)
                {
                    allDates.AddRange(entityLogs.Select(x => x.DateActualChange));
                }
                allDates = allDates.Distinct().ToList();

                foreach (var period in allDates)
                {
                    decimal sumFromHistory = 0;
                    foreach (var entityLogs in actualHistory)
                    {
                        var log = entityLogs.Where(x => x.DateActualChange <= period)
                            .OrderByDescending(x => x.DateApplied)
                            .FirstOrDefault();
                        if (log != null)
                        {
                            sumFromHistory += log.PropertyValue.ToDecimal();
                        }
                    }
                    if (sumFromHistory + areaShare > 1)
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                Container.Release(accountDomain);
                Container.Release(entityLogLightDomain);
            }
        }
    }
}