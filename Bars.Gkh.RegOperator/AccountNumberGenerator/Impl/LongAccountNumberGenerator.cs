namespace Bars.Gkh.RegOperator.AccountNumberGenerator.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Utils;
    using B4.IoC;
    using Castle.Windsor;
    using Entities;
    using Enums;

    public class LongAccountNumberGenerator : IAccountNumberGenerator
    {
        public IWindsorContainer Container { get; set; }

        public static TypeAccountNumber TypeAccountNumber = TypeAccountNumber.Long;

        public void Generate(BasePersonalAccount account)
        {
            var service = Container.ResolveDomain<BasePersonalAccount>();
            var fiasDomainService = Container.Resolve<IDomainService<Fias>>();
            var locationCodeService = Container.Resolve<IDomainService<LocationCode>>();

            using (Container.Using(service, fiasDomainService, locationCodeService))
            {
                // определяем максимальный порядковый номер собственника у квартир с одинаковым номером (например 12а, 12б и т.д.)  
                var existedAccountsCount = GetMaxOwnerNumber(service.GetAll()
                    .Where(x => x.Room.RealityObject.Id == account.Room.RealityObject.Id)
                    .Select(x => new
                    {
                        x.Room.RoomNum,
                        x.PersonalAccountNum
                    })
                    .AsEnumerable()
                    .Where(x => ParseFlatNumber(x.RoomNum) > 0)
                    .Where(x => ParseFlatNumber(x.RoomNum) == ParseFlatNumber(account.Room.RoomNum))
                    .Select(x => x.PersonalAccountNum).ToList());

                var realtyObject = account.Room.RealityObject;

                var fiasStreet = fiasDomainService.FirstOrDefault(x => x.AOGuid == realtyObject.FiasAddress.StreetGuidId);

                var locCode =
                    locationCodeService.GetAll()
                        .FirstOrDefault(x => realtyObject.FiasAddress.PlaceGuidId == x.AOGuid);

                var codeFailureStrings = new List<string>();

                int mrCode;
                if (!int.TryParse(locCode.Return(x => x.CodeLevel1), out mrCode))
                {
                    codeFailureStrings.Add("'код муниципального района или городского округа'");
                }

                int moCode;
                if (!int.TryParse(locCode.Return(x => x.CodeLevel2), out moCode))
                {
                    codeFailureStrings.Add("'код муниципального образования'");
                }

                int cityCode;
                if (!int.TryParse(locCode.Return(x => x.CodeLevel3), out cityCode))
                {
                    codeFailureStrings.Add("'код населенного пункта'");
                }

                // генерация номера лс
                if (codeFailureStrings.Any())
                {
                    throw new ValidationException(
                        string.Format(
                            "Для создания лицевого счета необходимо заполнить коды {0} из Справочника формирования ЛС",
                            string.Join(", ", codeFailureStrings)));
                }

                var streetKladrCode = fiasStreet.Return(x => x.CodeStreet);
                if (streetKladrCode != null && streetKladrCode.Length > 3)
                {
                    streetKladrCode = streetKladrCode.Substring(streetKladrCode.Length - 3);
                }

                var streetKladrCodeInt = streetKladrCode.ToInt();
                var houseStr = realtyObject.Return(x => x.FiasAddress).Return(x => x.House);
                string houseLetter; // буква из номера дома, если таковая есть
                var house = ParseHouseNumber(houseStr, out houseLetter);
                var housing = realtyObject.Return(x => x.FiasAddress).Return(x => x.Housing);
                if (housing != null && housing.Length > 1)
                {
                    housing = housing.Substring(housing.Length - 1);
                }

                var roomNum = ParseFlatNumber(account.Return(x => x.Room).Return(x => x.RoomNum));

                account.PersonalAccountNum = GetAccountNumber(mrCode, moCode, cityCode, streetKladrCodeInt, house,
                    housing, houseLetter, roomNum, existedAccountsCount);
            }
        }

        private string GetAccountNumber(int mrCode, int moCode, int cityCode, int streetKladrCodeInt, int house,
            string housing, string houseLetter, int roomNum, int existedAccountsCount)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                mrCode.ToString("D2"),
                // 1-2 знак - код муниципального района или городского округа из Справочника формирования ЛС
                moCode.ToString("D2"), // 3-4 знак - код муниципального образования из Справочника формирования ЛС
                cityCode.ToString("D2"), // 5-6 знак - код Населенного пункта из Справочника формирования ЛС
                streetKladrCodeInt > 0 ? streetKladrCodeInt.ToString("D3") : "___", // 7-8-9 знак - код улицы из КЛАДРа
                house > 0 ? house.ToString("D3") : "___", // 10-11-12 знак - номер дома из адреса МКД
                housing.Return(x => !string.IsNullOrEmpty(x) ? x : houseLetter, houseLetter),
                // 13 знак - корпус, может быть числовое или буквенное значение
                roomNum > 0 ? roomNum.ToString("D3") : "___",
                // 14-15-16 знак - номер квартиры из раздела помещений в МКД
                existedAccountsCount + 1 // 17 знак - порядковый номер собственника. принимает значение от 1 до 9.
                );
        }

        public void Generate(ICollection<BasePersonalAccount> accounts)
        {
            var service = Container.ResolveDomain<BasePersonalAccount>();
            var fiasDomainService = Container.Resolve<IDomainService<Fias>>();
            var locationCodeService = Container.Resolve<IDomainService<LocationCode>>();

            var realtyObjects = accounts.Select(account => account.Room.RealityObject).Distinct(x => x.Id).ToList();
            var streetGuidIds = realtyObjects.Select(x => x.FiasAddress.StreetGuidId).Distinct().ToList();
            var placeGuidIds = realtyObjects.Select(x => x.FiasAddress.PlaceGuidId).Distinct().ToList();


            var fiasStreets = fiasDomainService.GetAll()
                .Where(x => streetGuidIds.Contains(x.AOGuid)).ToList();

            var fiasStreetsByRoId = realtyObjects.Select(x => new
            {
                RealtyId = x.Id,
                FiasStreet = fiasStreets.FirstOrDefault(f => x.FiasAddress.StreetGuidId == f.AOGuid)
            }).GroupBy(x => x.RealtyId)
                .ToDictionary(x => x.Key,
                    x => x.Select(y => y.FiasStreet).First());

            var locCodes = locationCodeService.GetAll()
                .Where(x => placeGuidIds.Contains(x.AOGuid)).ToList();

            var locCodesByRoId = realtyObjects.Select(x => new
            {
                RealtyId = x.Id,
                LocationCode = locCodes.FirstOrDefault(f => x.FiasAddress.PlaceGuidId == f.AOGuid)
            }).GroupBy(x => x.RealtyId).ToDictionary(x => x.Key, x => x.Select(y => y.LocationCode).First());

            var existedAccsCountByRoomId = service.GetAll().GroupBy(x => x.Room.Id, acc => new
            {
                acc.Room.RoomNum,
                acc.PersonalAccountNum
            }).ToDictionary(x => x.Key,
                            y => GetMaxOwnerNumber(y.Where(x => ParseFlatNumber(x.RoomNum) > 0).Select(x => x.PersonalAccountNum).ToList()));

            var codeFailureStrings = new List<string>();

            foreach (var account in accounts)
            {
                int mrCode;
                var roId = account.Room.RealityObject.Id;
                var realtyObject = account.Room.RealityObject;

                if (!locCodesByRoId.ContainsKey(roId))
                {
                    continue;
                    //throw new ValidationException(
                    //    string.Format(
                    //        "Для создания лицевого счета необходимо заполнить коды {0} из Справочника формирования ЛС",
                    //        string.Join(", ", codeFailureStrings)));
                }

                if (!int.TryParse(locCodesByRoId[roId].Return(x => x.CodeLevel1), out mrCode))
                {
                    codeFailureStrings.Add("'код муниципального района или городского округа'");
                }

                int moCode;
                if (!int.TryParse(locCodesByRoId[roId].Return(x => x.CodeLevel2), out moCode))
                {
                    codeFailureStrings.Add("'код муниципального образования'");
                }

                int cityCode;
                if (!int.TryParse(locCodesByRoId[roId].Return(x => x.CodeLevel3), out cityCode))
                {
                    codeFailureStrings.Add("'код населенного пункта'");
                }

                // генерация номера лс
                if (codeFailureStrings.Any())
                {
                    throw new ValidationException(
                        string.Format(
                            "Для создания лицевого счета необходимо заполнить коды {0} из Справочника формирования ЛС",
                            string.Join(", ", codeFailureStrings)));
                }

                var streetKladrCode = fiasStreetsByRoId.ContainsKey(roId) ? fiasStreetsByRoId[roId].Return(x => x.CodeStreet) : null;
                if (streetKladrCode != null && streetKladrCode.Length > 3)
                {
                    streetKladrCode = streetKladrCode.Substring(streetKladrCode.Length - 3);
                }

                var streetKladrCodeInt = streetKladrCode.ToInt();

                var houseStr = realtyObject.Return(x => x.FiasAddress).Return(x => x.House);
                string houseLetter; // буква из номера дома, если таковая есть
                var house = ParseHouseNumber(houseStr, out houseLetter);
                var housing = realtyObject.Return(x => x.FiasAddress).Return(x => x.Housing);
                if (housing != null && housing.Length > 1)
                {
                    housing = housing.Substring(housing.Length - 1);
                }

                var roomNum = ParseFlatNumber(account.Return(x => x.Room).Return(x => x.RoomNum));

                account.PersonalAccountNum = GetAccountNumber(mrCode, moCode, cityCode, streetKladrCodeInt, house,
                    housing, houseLetter, roomNum, existedAccsCountByRoomId.ContainsKey(account.Room.Id) ? existedAccsCountByRoomId[account.Room.Id] : 0);
            }

        }

        private static int ParseHouseNumber(string strNum, out string letter)
        {
            var strNumber = new StringBuilder();
            int number;
            letter = "0";

            foreach (var t in strNum)
            {
                if (char.IsLetter(t))
                {
                    letter = t.ToStr().ToUpper();
                    break;
                }

                strNumber.Append(t);
            }

            int.TryParse(strNumber.ToString(), out number);

            return number;
        }

        private static int ParseFlatNumber(string strNum)
        {
            string letter;
            return ParseHouseNumber(strNum, out letter);
        }

        private static int GetMaxOwnerNumber(IEnumerable<string> accountNumbers)
        {
            var accounts = new List<int>();
            foreach (var accountNumber in accountNumbers)
            {
                if (accountNumber.Length > 16)
                {
                    int res;
                    int.TryParse(accountNumber.Substring(16), out res);
                    accounts.Add(res);
                }
            }

            return accounts.Count > 0 ? accounts.Max() : 0;
        }
    }
}