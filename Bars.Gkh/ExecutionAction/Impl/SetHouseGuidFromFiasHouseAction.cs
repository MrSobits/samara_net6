namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Заполнение атрибута HOUSEGUID домов
    /// </summary>
    public class SetHouseGuidFromFiasHouseAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public IDomainService<FiasHouse> FiasHouseDomain { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        public override string Description => "Заполнить HouseGuid жилого дома на основании сопоставления с таблицей b4_fias_house";

        public override string Name => "Заполнение атрибута HOUSEGUID домов";

        public override Func<IDataResult> Action => this.SetHouseGuid;

        public BaseDataResult SetHouseGuid()
        {
            try
            {
                var realityRepo = this.Container.Resolve<IRepository<RealityObject>>();
                var realityQuery = realityRepo.GetAll()
                    .Where(x=> x.TypeHouse == Enums.TypeHouse.ManyApartments)
                    .Where(x=> x.MoSettlement == null).ToList();
                foreach (RealityObject ro in realityQuery)
                {
                    ro.MoSettlement = Utils.GetSettlementForReality(this.Container, ro);
                    if (ro.MoSettlement != null)
                    {
                        realityRepo.Update(ro);
                    }
                }

            }
            catch
            { }
            var fiasAddressesQuery = this.FiasAddressDomain.GetAll()
                .OrderBy(x => x.PlaceGuidId)
                .ThenBy(x => x.StreetGuidId);

            var totalCount = fiasAddressesQuery.Count();

            var take = 10000;

            var regEx = new Regex(@"^(\d+?)\W+(\D*)$", RegexOptions.Compiled | RegexOptions.Singleline);

            for (int skip = 0; skip < totalCount; skip += take)
            {
                try
                {
                    var fiasAddressPart =
                        fiasAddressesQuery.Skip(skip)
                            .Take(take)
                            .AsEnumerable()
                            .GroupBy(
                                x =>
                                    $"{x.StreetGuidId ?? x.PlaceGuidId}#{this.SafeReplace(regEx, x.House)}{x.Letter}#{x.Housing}#{x.Building}".ToLower())
                            .ToDictionary(
                                x => x.Key,
                                x => x.Select(
                                    y =>
                                        new FiasAddress
                                        {
                                            Id = y.Id,
                                            AddressName = y.AddressName,
                                            AddressGuid = y.AddressGuid,
                                            Building = y.Building,
                                            Letter = y.Letter,
                                            House = this.SafeReplace(regEx, y.House),
                                            Housing = y.Housing,
                                            HouseGuid = y.HouseGuid,
                                            StreetCode = y.StreetCode,
                                            StreetName = y.StreetName,
                                            StreetGuidId = y.StreetGuidId,
                                            PlaceGuidId = y.PlaceGuidId,
                                            PostCode = y.PostCode,
                                            Coordinate = y.Coordinate,
                                            Flat = y.Flat,
                                            ObjectCreateDate = y.ObjectCreateDate,
                                            ObjectEditDate = y.ObjectEditDate,
                                            ObjectVersion = y.ObjectVersion,
                                            PlaceAddressName = y.PlaceAddressName,
                                            PlaceCode = y.PlaceCode,
                                            PlaceName = y.PlaceName
                                        }).ToArray());

                    var aoGuids =
                        fiasAddressPart.Values.SelectMany(x => x)
                            .Where(x => x.StreetGuidId.IsNotEmpty() || x.PlaceGuidId.IsNotEmpty())
                            .Select(x => new Guid?(new Guid(x.StreetGuidId ?? x.PlaceGuidId)))
                            .Distinct()
                            .ToArray();

                    var existFiasHouses = this.FiasHouseDomain.GetAll()
                        .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Today)
                        .WhereContainsBulked(x => x.AoGuid, aoGuids)
                        .Select(
                            x =>
                                new
                                {
                                    AoGuid = x.AoGuid.ToString().ToLower(),
                                    HouseNum = x.HouseNum.ToLower(),
                                    BuildNum = x.BuildNum.ToLower(),
                                    StrucNum = x.StrucNum.ToLower(),
                                    x.HouseGuid,
                                    x.PostalCode
                                })
                        .AsEnumerable()
                        .GroupBy(x => $"{x.AoGuid}#{x.HouseNum}#{x.BuildNum}#{x.StrucNum}", x => Tuple.Create(x.HouseGuid, x.PostalCode))
                        .ToDictionary(x => x.Key, x => x.First());

                    var fiasAddressesToUpdate = new List<FiasAddress>(take);

                    foreach (var existFiasHouse in existFiasHouses)
                    {
                        FiasAddress[] fiasAddresses;

                        if (!fiasAddressPart.TryGetValue(existFiasHouse.Key, out fiasAddresses))
                        {
                            continue;
                        }

                        foreach (var fiasAddress in fiasAddresses)
                        {
                            fiasAddress.HouseGuid = existFiasHouse.Value.Item1;
                            fiasAddress.PostCode = existFiasHouse.Value.Item2;
                            fiasAddressesToUpdate.Add(fiasAddress);
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(this.Container, fiasAddressesToUpdate, useStatelessSession: true);
                }
                finally
                {
                    this.SessionProvider.GetCurrentSession().Clear();
                }
            }
            return new BaseDataResult();
        }

        private string SafeReplace(Regex regex, string input)
        {
            return input == null ? null : regex.Replace(input, "$1$2");
        }
    }
}