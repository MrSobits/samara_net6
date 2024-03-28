namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.RealityObj;

    /// <summary>
    /// Заполнение таблицы b4_fias_house из gkh_fias_house
    /// </summary>
    public class MigrateFiasHouseFromGkh : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public IDomainService<FiasHouse> FiasHouseDomain { get; set; }

        public IDomainService<GkhFiasHouse> GkhFiasHouseDomain { get; set; }

        public override string Description => "Заполнение таблицы b4_fias_house из gkh_fias_house";

        public override string Name => "Заполнение таблицы b4_fias_house из gkh_fias_house";

        public override Func<IDataResult> Action => this.Migrate;

        public BaseDataResult Migrate()
        {
            var gkhHousesQuery = this.GkhFiasHouseDomain.GetAll()
                .OrderBy(x => x.Id);

            var totalCount = gkhHousesQuery.Count();
            var take = 10000;
            for (int skip = 0; skip < totalCount; skip += take)
            {
                try
                {
                    var gkhPartHouses = gkhHousesQuery.Skip(skip).Take(take).ToArray();
                    var gkhHousesGuids = gkhPartHouses.Select(x => new Guid?(new Guid(x.HouseGuid))).ToArray();

                    var existFiasHouses = this.FiasHouseDomain.GetAll()
                        .Where(x => gkhHousesGuids.Contains(x.HouseGuid))
                        .Select(x => x.HouseGuid)
                        .AsEnumerable()
                        .GroupBy(x => x)
                        .ToDictionary(x => x.Key);

                    var fiasHouses = new List<FiasHouse>(take);
                    foreach (var gkhHouse in gkhPartHouses)
                    {
                        if (!existFiasHouses.ContainsKey(new Guid(gkhHouse.HouseGuid)))
                        {
                            fiasHouses.Add(
                                new FiasHouse
                                {
                                    PostalCode = gkhHouse.PostalCode,
                                    Okato = gkhHouse.Okato,
                                    Oktmo = gkhHouse.Oktmo,
                                    BuildNum = gkhHouse.BuildNum,
                                    StrucNum = gkhHouse.StrucNum,
                                    HouseNum = gkhHouse.HouseNum,
                                    HouseId = new Guid(gkhHouse.HouseId),
                                    HouseGuid = new Guid(gkhHouse.HouseGuid),
                                    AoGuid = new Guid(gkhHouse.AoGuid)
                                });
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(this.Container, fiasHouses);
                }
                finally
                {
                    this.SessionProvider.GetCurrentSession().Clear();
                }
            }

            return new BaseDataResult();
        }
    }
}