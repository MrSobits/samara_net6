namespace Bars.Gkh.Regions.Tyumen.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    //using Bars.B4.Modules.FIAS.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Заполнение атрибута HOUSEGUID домов
    /// </summary>
    public class GkhTyumenSetHouseGuidFromFiasHouseAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public IDomainService<FiasHouse> FiasHouseDomain { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        public override string Description => "Заполнить HouseGuid жилого дома на основании сопоставления с таблицей b4_fias_house (Тюмень)";

        public override string Name => "Заполнение атрибута HOUSEGUID домов (Тюмень)";

        public override Func<IDataResult> Action => this.SetHouseGuid;

        Regex regEx = new Regex(@"^(\d+?)\W+(\D*)$", RegexOptions.Compiled | RegexOptions.Singleline);

        public BaseDataResult SetHouseGuid()
        {
            var fiasAddressesQuery = this.FiasAddressDomain.GetAll()
                .OrderBy(x => x.Id);

            var totalCount = fiasAddressesQuery.Count();

            var take = 10000;
            

            for (int skip = 0; skip < totalCount; skip += take)
            {
                try
                {
                    //создаем словарь упоротый ключ - фиас адрес
                    var fiasAddressPart =
                        fiasAddressesQuery.Skip(skip)
                            .Take(take)
                            .AsEnumerable()
                            .GroupBy(x => GetKey(x))
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
                    //кэш GUID родительских проектов
                    var aoGuids =
                        fiasAddressPart.Values.SelectMany(x => x)
                            .Where(x => x.StreetGuidId.IsNotEmpty() || x.PlaceGuidId.IsNotEmpty())
                            .Select(x => new Guid?(new Guid(x.StreetGuidId ?? x.PlaceGuidId)))
                            .Distinct()
                            .ToArray();
                    //кэш полного номера дома
                    var houses =
                        fiasAddressPart.Values.SelectMany(x => x)
                            .Select(x => x.House.ToStr()/*.ToLower()*/)
                            .Union(
                                fiasAddressPart.Values.SelectMany(x => x)
                                    .Where(x => x.Housing.IsNotEmpty())
                                    .Select(x => $"{x.House}{x.Housing}"/*.ToLower()*/))
                            .Union(
                                fiasAddressPart.Values.SelectMany(x => x)
                                    .Where(x => x.Letter.IsNotEmpty())
                                    .Select(x => $"{x.House}{x.Building}".ToLower()))
                            .Union(
                                fiasAddressPart.Values.SelectMany(x => x)
                                    .Where(x => x.Letter.IsNotEmpty())
                                    .Select(x => $"{x.House}{x.Letter}{x.Building}"/*.ToLower()*/))
                            .Distinct()
                            .ToArray();
                    //кэш корпусов
                    var housings = fiasAddressPart.Values.SelectMany(x => x).Select(x => x.Housing.ToStr().ToLower()).Distinct().ToArray();

                    
                    /*var existFiasHouses = this.FiasHouseDomain.GetAll()
                        .WhereContainsBulked(x => x.AoGuid, aoGuids)
                        .WhereContainsBulked(x => x.HouseNumToLower() + x.StructureType.ToString().ToLower() + x.StrucNum.ToLower(), houses)
                        .Where(x => x.BuildNum == null || housings.Contains(x.BuildNum.ToLower()))
                        .Select(x => new
                                {
                                    x.EstimateStatus,
                                    AoGuid = x.AoGuid.ToString().ToLower(),
                                    HouseNum = x.HouseNum.ToLower(),
                                    BuildNum = x.BuildNum.ToLower(),
                                    StrucNum = x.StrucNum.ToLower(),
                                    x.StructureType,
                                    x.HouseGuid,
                                    x.PostalCode
                                })
                        .AsEnumerable()
                        .GroupBy(x => $"{x.AoGuid}#{x.HouseNum}#{x.BuildNum}#{x.StructureType.ToInt()}#{x.StrucNum}", x => Tuple.Create(x.HouseGuid, x.PostalCode, x.EstimateStatus))
                        .ToDictionary(x => x.Key, x => x.First());*/

                    var fiasAddressesToUpdate = new List<FiasAddress>(take);

                    // TODO: Расскоментировать после перевода FIAS
                /*    foreach (var existFiasHouse in existFiasHouses)
                    {
                        FiasAddress[] fiasAddresses;

                        if (!fiasAddressPart.TryGetValue(existFiasHouse.Key, out fiasAddresses))
                        {
                            continue;
                        }

                        foreach (var fiasAddress in fiasAddresses)
                        {
                            if (!CheckEstimate(existFiasHouse.Value.Item3, fiasAddress.EstimateStatus))
                                continue;

                            fiasAddress.HouseGuid = existFiasHouse.Value.Item1;
                            fiasAddress.PostCode = existFiasHouse.Value.Item2;
                            fiasAddressesToUpdate.Add(fiasAddress);
                        }
                    }*/

                    TransactionHelper.InsertInManyTransactions(this.Container, fiasAddressesToUpdate, useStatelessSession: true);
                }
                finally
                {
                    SessionProvider.GetCurrentSession().Clear();
                }
            }
            return new BaseDataResult();
        }

        /// <summary>
        /// Сравнение типов владений
        /// </summary>
        /// <returns>true, если соответствует</returns>
        
        // TODO: Расскоментировать после перевода FIAS
       /* private bool CheckEstimate(FiasEstimateStatusEnum estimateFromFias, FiasEstimateStatusEnum estimateFromRO)
        {
            if (estimateFromFias == FiasEstimateStatusEnum.NotDefined || estimateFromRO == FiasEstimateStatusEnum.NotDefined)
                return true;

            return (estimateFromFias == estimateFromRO);
        }*/

        private string GetKey(FiasAddress x)
        {
            return $"{x.StreetGuidId ?? x.PlaceGuidId}#{this.SafeReplace(regEx, x.House)}#{x.Housing}#{x.Letter}#{x.Building}"/*.ToLower()*/;
        }

        private string SafeReplace(Regex regex, string input)
        {
            return input == null ? null : regex.Replace(input, "$1$2");
        }
    }
}