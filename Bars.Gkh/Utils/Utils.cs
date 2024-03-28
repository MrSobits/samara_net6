namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AutoMapper;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Вспомогательные методы
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Установка параметров муниципального образования
        /// </summary>
        /// <param name="container"></param>
        /// <param name="ro"></param>
        /// <returns> Возврат ссылки на муниципальное образование </returns>
        public static Municipality GetSettlementForReality(IWindsorContainer container, RealityObject ro)
        {
            if (ro == null || ro.FiasAddress == null)
            {
                return null;
            }

            // TODO: Расскоментировать
            //октмо бывает двух видов: из 8 или из 11 цифр (тогда первые 8 - октмо мо)
            /*var oktmoList =
                container.Resolve<IDomainService<Fias>>()
                    .GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Where(x => x.AOGuid == ro.FiasAddress.PlaceGuidId)
                    .Select(x => !x.OKTMO.IsNullOrEmpty() ? x.OKTMO.Substring(0, 8).ToLong().ToString() : "0") // чтобы убрать ведущий 0
                    .ToArray();

            if (oktmoList.Length != 0)
            {
                var mo =
                    container.Resolve<IRepository<Municipality>>()
                        .GetAll()
                        .FirstOrDefault(x => oktmoList.Contains(x.Oktmo.ToString()));
                if (mo == null)
                {
                    oktmoList =
                container.Resolve<IDomainService<Fias>>()
                    .GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Where(x => x.AOGuid == ro.FiasAddress.PlaceGuidId)
                    .Select(x => !x.OKTMO.IsNullOrEmpty() ? x.OKTMO : "0") // чтобы убрать ведущий 0
                    .ToArray();
                    if (oktmoList.Length != 0)
                    {
                        mo =
                            container.Resolve<IRepository<Municipality>>()
                                .GetAll()
                                .FirstOrDefault(x => oktmoList.Contains(x.Oktmo.ToString()));
                        return mo;
                    }

                }

                return mo;
            }*/

            return null;
        }
        
        /// <summary>
        /// Проверить ИНН
        /// </summary>
        /// <param name="inn">Значение ИНН</param>
        /// <param name="juralPerson">Признак, юр. или физ. лицо</param>
        /// <returns>Результат проверки</returns>
        public static bool VerifyInn(string inn, bool juralPerson)
        {
            if (string.IsNullOrEmpty(inn))
            {
                return false;
            }

            if (juralPerson && inn.Length == 12)
            {
                return true;
            }

            if ((juralPerson && inn.Length != 10) || (!juralPerson && inn.Length != 12))
            {
                return false;
            }

            if (!IsNumber(inn))
            {
                return false;
            }

            var success = false;
            if (juralPerson)
            {
                int digit1 = inn[0].ToInt() * 2;
                int digit2 = inn[1].ToInt() * 4;
                int digit3 = inn[2].ToInt() * 10;
                int digit4 = inn[3].ToInt() * 3;
                int digit5 = inn[4].ToInt() * 5;
                int digit6 = inn[5].ToInt() * 9;
                int digit7 = inn[6].ToInt() * 4;
                int digit8 = inn[7].ToInt() * 6;
                int digit9 = inn[8].ToInt() * 8;
                int digit10 = inn[9].ToInt();

                int sum = digit1 + digit2 + digit3 + digit4 + digit5 +
                          digit6 + digit7 + digit8 + digit9;

                int mod = sum % 11;
                if (mod == 10)
                {
                    mod = 0;
                }

                if (mod == digit10)
                {
                    success = true;
                }
            }
            else
            {
                int digit1 = inn[0].ToInt() * 7;
                int digit2 = inn[1].ToInt() * 2;
                int digit3 = inn[2].ToInt() * 4;
                int digit4 = inn[3].ToInt() * 10;
                int digit5 = inn[4].ToInt() * 3;
                int digit6 = inn[5].ToInt() * 5;
                int digit7 = inn[6].ToInt() * 9;
                int digit8 = inn[7].ToInt() * 4;
                int digit9 = inn[8].ToInt() * 6;
                int digit10 = inn[9].ToInt() * 8;
                int digit11 = inn[10].ToInt();

                int sum = digit1 + digit2 + digit3 + digit4 + digit5 +
                          digit6 + digit7 + digit8 + digit9 + digit10;

                int mod = sum % 11;
                if (mod == 10)
                {
                    mod = 0;
                }

                if (mod == digit11)
                {
                    digit1 = inn[0].ToInt() * 3;
                    digit2 = inn[1].ToInt() * 7;
                    digit3 = inn[2].ToInt() * 2;
                    digit4 = inn[3].ToInt() * 4;
                    digit5 = inn[4].ToInt() * 10;
                    digit6 = inn[5].ToInt() * 3;
                    digit7 = inn[6].ToInt() * 5;
                    digit8 = inn[7].ToInt() * 9;
                    digit9 = inn[8].ToInt() * 4;
                    digit10 = inn[9].ToInt() * 6;
                    digit11 = inn[10].ToInt() * 8;
                    int digit12 = inn[11].ToInt();

                    sum = digit1 + digit2 + digit3 + digit4 + digit5 +
                          digit6 + digit7 + digit8 + digit9 + digit10 + digit11;

                    mod = sum % 11;
                    if (mod == 10)
                    {
                        mod = 0;
                    }

                    if (mod == digit12)
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Проверить огрн
        /// </summary>
        /// <param name="ogrn">Значение огрн</param>
        /// <param name="juralPerson">Признак, юр. или физ. лицо</param>
        /// <returns>Результат проверки</returns>
        public static bool VerifyOgrn(string ogrn, bool juralPerson)
        {
            if (string.IsNullOrEmpty(ogrn))
            {
                return false;
            }

            if ((juralPerson && ogrn.Length != 13) || (!juralPerson && ogrn.Length != 15))
            {
                return false;
            }

            long значение;
            if (!long.TryParse(ogrn, out значение))
            {
                return false;
            }

            var успех = false;

            if (juralPerson)
            {
                long числоБезПоследнегоЗнака;
                if (!long.TryParse(ogrn.Substring(0, 12), out числоБезПоследнегоЗнака))
                {
                    return false;
                }

                long последнийЗнакЧисла;
                if (!long.TryParse(ogrn.Substring(12, 1), out последнийЗнакЧисла))
                {
                    return false;
                }

                long числоПослеДеления = числоБезПоследнегоЗнака / 11;
                long числоПослеУмножения = числоПослеДеления * 11;
                long числоПослеВычитания = числоБезПоследнегоЗнака - числоПослеУмножения;

                числоПослеВычитания = числоПослеВычитания > 9 ? числоПослеВычитания % 10 : числоПослеВычитания;

                if (последнийЗнакЧисла == числоПослеВычитания)
                {
                    успех = true;
                }
            }
            else
            {
                long числоБезПоследнегоЗнака;
                if (!long.TryParse(ogrn.Substring(0, 14), out числоБезПоследнегоЗнака))
                {
                    return false;
                }

                long последнийЗнакЧисла;
                if (!long.TryParse(ogrn.Substring(14, 1), out последнийЗнакЧисла))
                {
                    return false;
                }

                long числоПослеДеления = числоБезПоследнегоЗнака / 13;
                long числоПослеУмножения = числоПослеДеления * 13;
                long числоПослеВычитания = числоБезПоследнегоЗнака - числоПослеУмножения;

                числоПослеВычитания = числоПослеВычитания > 9 ? числоПослеВычитания % 10 : числоПослеВычитания;

                if (последнийЗнакЧисла == числоПослеВычитания)
                {
                    успех = true;
                }
            }

            return успех;
        }

        private static bool IsNumber(string str)
        {
            var regex = new Regex(@"^\d+$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// Метод получения Min для > 2 переменных
        /// </summary>
        public static T Min<T>(params T[] vals)
        {
            return vals.Min();
        }

        /// <summary>
        /// Метод получения Max для > 2 переменных
        /// </summary>
        public static T Max<T>(params T[] vals)
        {
            return vals.Max();
        }

        /// <summary>
        /// Метод получения Муниципального образования по переданному Адресу Fias
        /// </summary>
        public static Municipality GetMunicipality(IWindsorContainer container, FiasAddress address)
        {
            if (address == null || string.IsNullOrEmpty(address.AddressGuid))
            {
                return null;
            }

            var guidMass = address.AddressGuid.Split('#');

            Municipality result = null;

            var service = container.Resolve<IRepository<Municipality>>();
            foreach (var s in guidMass)
            {
                var t = s.Split('_');

                //костыль, в данный момент до конца не ясно, нужно ли добавлять к house_guid
                //уровень ФИАС, если нужно, то нужно править исходники B4 + все использования house_guid
                //в данном случае для определения МР house_guid по-хорошему не нужен, пока используем заглушку
                var idx = t.Length == 1 ? 0 : 1;

                Guid g;

                if (Guid.TryParse(t[idx], out g) && g != Guid.Empty)
                {
                    var mcp = service.GetAll().FirstOrDefault(x => x.FiasId == g.ToString());
                    if (mcp != null)
                    {
                        result = mcp;
                    }
                }
            }

            return result != null && result.ParentMo != null ? result.ParentMo : result;
        }

        /// <summary>
        /// Метод получения Муниципального образования по переданному Адресу Fias
        /// </summary>
        public static Municipality GetMoSettlement(IWindsorContainer container, FiasAddress address)
        {
            if (address == null)
            {
                return null;
            }

            var fiasRepos = container.ResolveRepository<Fias>();
            var moRepos = container.ResolveRepository<Municipality>();
            using (container.Using(fiasRepos, moRepos))
            {
                //октмо бывает двух видов: из 8 или из 11 цифр (тогда первые 8 - октмо мо)
                var oktmoList = fiasRepos.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Where(x => x.AOGuid == address.PlaceGuidId)
                    .WhereNotEmptyString(x => x.OKTMO)
                    .Select(x => x.OKTMO.Substring(0, 8))
                    .ToList();

                if (oktmoList.Any())
                {
                    return moRepos.GetAll()
                        .FirstOrDefault(x => oktmoList.Contains(x.Oktmo));
                }

                return null;
            }
        }

        /// <summary>
        /// Установка параметров муниципального образования
        /// </summary>
        /// <param name="container"></param>
        /// <param name="ro"></param>
        /// <returns> Возврат ссылки на муниципальное образование </returns>
        public static Municipality GetSettlementByRealityObject(IWindsorContainer container, RealityObject ro)
        {
            return Utils.GetMoSettlement(container, ro?.FiasAddress);
        }

        /// <summary>
        /// Проверка электронного адреса на кореектность
        /// </summary>
        /// <param name="email">Email-адрес для проверки</param>
        /// <returns>Результат проверки</returns>
        public static bool VerifyMail(string email)
        {
            if (email.IsEmpty())
            {
                return false;
            }

            //валидация email:
            //значение до @ может иметь буквы русского и английского алфавитов, цифры, символы: [-.+_], которые не повторяются друг за другом
            //перед собакой обязательно должны быть буквы или цифры, после собаки также
            //адрес почтового сервиса может содержать буквы русского и английского алфавитов, цифры, символы: [-.+_]
            //доменное имя верхнего уровня долэно начинаться с точки и содержать только буквы и его длина не менее 2 символов и не более 9
            var pattern = @"^(([0-9a-zа-я][-.'+_]?)+)(?<=[0-9a-zа-я])@(([0-9a-zа-я]+[-.'+_]?)+)((?<=\.)[a-zа-я]{2,9})$";

            try
            {
                var regex = new Regex(pattern,
                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Сохранение ФИАС адреса
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="fiasAddress">Адрес ФИАС</param>
        /// <param name="checkDirty">Проверить на изменение</param>
        public static void SaveFiasAddress(IWindsorContainer container, FiasAddress fiasAddress)
        {
            if (fiasAddress.IsNotNull())
            {
                var fiasAddressDomain = container.Resolve<IDomainService<FiasAddress>>();

                using (container.Using(fiasAddressDomain))
                {
                    fiasAddressDomain.SaveOrUpdate(fiasAddress);
                }
            }
        }

        /// <summary>
        /// Вернуть прокси для отображения
        /// </summary>
        /// <param name="fiasAddress">Адрес ФИАС</param>
        /// <param name="container">Контейнер</param>
        /// <returns>Прокси для отображения ФИАС адреса дома</returns>
        public static FiassHouseProxy GetFiasProxy(this FiasAddress fiasAddress, IWindsorContainer container)
        {
            var mapper = container.Resolve<IMapper>();
            using (container.Using(mapper))
            {
                var result = mapper.Map<FiassHouseProxy>(fiasAddress);

                if (fiasAddress.HouseGuid != null)
                {
                    var fiasHouseDomain = container.ResolveDomain<FiasHouse>();
                    using (container.Using(fiasHouseDomain))
                    {
                        var fiasHouse = fiasHouseDomain.GetAll()
                            .OrderByDescending(x => x.EndDate ?? DateTime.MaxValue)
                            .FirstOrDefault(x => x.HouseGuid == fiasAddress.HouseGuid);

                        result.StrucNum = fiasHouse?.StrucNum;

                        var parts = new List<string>();
                        if (fiasHouse != null && fiasHouse.HouseNum.IsNotEmpty())
                        {
                            parts.Add($"д. {fiasHouse.HouseNum}");
                        }

                        if (fiasHouse != null && fiasHouse.BuildNum.IsNotEmpty())
                        {
                            parts.Add($"корпус {fiasHouse.BuildNum}");
                        }

                        if (fiasHouse != null && fiasHouse.StrucNum.IsNotEmpty())
                        {
                            var structName = fiasHouse.StructureType != FiasStructureTypeEnum.NotDefined
                                ? fiasHouse.StructureType.GetDescriptionName().ToLower()
                                : FiasStructureTypeEnum.Structure.GetDescriptionName().ToLower();

                            parts.Add($"{structName} {fiasHouse.StrucNum}");
                        }

                        result.HouseAddressName = parts.AggregateWithSeparator(" ");

                        return result;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Вычислить число дней и месяцев с указанной даты
        /// </summary>
        /// <param name="dayStart">Дата, с которой ведетя отсчет</param>
        /// <param name="months">Число месяцев</param>
        /// <returns>Число дней</returns>
        public static int CalculateDaysAndMonths(DateTime dayStart, out int months)
        {
            months = dayStart.GetMonthsUntilDate(DateTime.Today);
            return DateTime.Today.Subtract(dayStart).Days;
        }
        
        /// <summary>
        /// Получить из наследованной сущности все FiasAddress'а
        /// </summary>
        public static IEnumerable<FiasAddress> GetFiasAddressEntities<T>(T entity)
        {
            return entity
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.PropertyType == typeof(FiasAddress) && x.GetValue(entity) is FiasAddress)
                .Select(x => x.GetValue(entity) as FiasAddress) ?? Enumerable.Empty<FiasAddress>();
        }

        /// <summary>
        /// Удалить все Fias адреса относящиеся к сущности
        /// </summary>
        public static void DeleteEntityFiasAddress<T>(T entity, IWindsorContainer container)
        {
            var fiasAddresses = Utils.GetFiasAddressEntities(entity).ToList();
            if (fiasAddresses == null || !fiasAddresses.Any())
                return;
            
            var fiasAddressDomain = container.ResolveDomain<FiasAddress>();

            using (container.Using(fiasAddressDomain))
            {
                fiasAddresses.ForEach(address =>
                {
                    if (address?.Id > 0)
                    {
                        fiasAddressDomain.Delete(address.Id);
                    }
                });
            }
        }

        /// <summary>
        /// Прокси для отображения ФИАС адреса дома
        /// </summary>
        public class FiassHouseProxy
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Все гуиды адреса разделенные по уровням
            /// </summary>
            public string AddressGuid { get; set; }

            /// <summary>
            /// Полный адрес Населенный пункт+Улица+Дом+Корпус....
            /// </summary>
            public string AddressName { get; set; }

            /// <summary>
            /// Номер дома по ФИАС
            /// </summary>
            public string HouseAddressName { get; set; }

            /// <summary>
            /// Номер дома 
            /// </summary>
            public string HouseNum { get; set; }

            /// <summary>
            /// Guid дома 
            /// </summary>
            public string HouseGuid { get; set; }

            /// <summary>
            /// Номер корпуса 
            /// </summary>
            public string BuildNum { get; set; }

            /// <summary>
            /// Номер строения
            /// 
            /// </summary>
            public string StrucNum { get; set; }

            /// <summary>
            /// Литер 
            /// </summary>
            public string Letter { get; set; }

            /// <summary>
            /// Почтовый индекс 
            /// </summary>
            public string PostalCode => this.PostCode;

            /// <summary>
            /// Код населенного пункта 
            /// </summary>
            public string PlaceCode { get; set; }

            /// <summary>
            /// Почтовый индекс 
            /// </summary>
            public string PostCode { get; set; }

            /// <summary>
            /// Гуид записи ФИАСа населенного пункта 
            /// </summary>
            public string PlaceGuidId { get; set; }

            /// <summary>
            /// Наименование населенного пункта 
            /// </summary>
            public string PlaceName { get; set; }

            /// <summary>
            /// Адрес до Населенного пункта ( тоесть Улица, Дом корпус не включены) 
            /// </summary>
            public string PlaceAddressName { get; set; }

            /// <summary>
            /// Код улицы 
            /// </summary>
            public string StreetCode { get; set; }

            /// <summary>
            /// Гуид записи ФИАС улицы
            ///  </summary>
            public string StreetGuidId { get; set; }

            /// <summary>
            /// Наименование улицы 
            /// </summary>
            public string StreetName { get; set; }

            /// <summary>
            /// Дом 
            /// </summary>
            public string House { get; set; }

            /// <summary>
            /// Корпус 
            /// </summary>
            public string Housing { get; set; }

            /// <summary>
            /// Секция 
            /// </summary>
            public string Building { get; set; }

            /// <summary>
            /// Квартира 
            /// </summary>
            public string Flat { get; set; }
        }
    }
}