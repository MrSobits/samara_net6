namespace Bars.Gkh.RegOperator.AccountNumberGenerator.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class ShortAccountNumberGenerator : IAccountNumberGenerator
    {
        public static TypeAccountNumber TypeAccountNumber = TypeAccountNumber.Short;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="locationCodeDomain">DomainService кодов муниципальных образований</param>
        /// <param name="sessionProvider">Провайдер сессий</param>
        public ShortAccountNumberGenerator(
            IDomainService<LocationCode> locationCodeDomain,
            ISessionProvider sessionProvider)
        {
            _locationCodeDomain = locationCodeDomain;
            _sessionProvider = sessionProvider;
        }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        private readonly IDomainService<LocationCode> _locationCodeDomain;
        private readonly ISessionProvider _sessionProvider;

        private static readonly int BatchSize = 7;
        private static int _batchStart;
        private int _current;

        private Dictionary<long, string> _muCodes;

        public Dictionary<long, string> MuCodes
        {
            get
            {
                return _muCodes ?? (_muCodes = _locationCodeDomain.GetAll()
                    .GroupBy(x => x.FiasLevel1.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.CodeLevel1).First()));
            }
        }

        private Dictionary<long, long> _roMuDict;

        private Dictionary<long, long> RoMuDict
        {
            get
            {
                return _roMuDict ?? (_roMuDict = RealityObjectDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        MuId = x.Municipality.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.MuId).First()));
            }
        }


        /// <summary>
        ///     Генерация номера для одного лицевого счета
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        public void Generate(BasePersonalAccount account)
        {
            var muId = RoMuDict.Get(account.Room.RealityObject.Id);

            var muCode = MuCodes.Get(muId);

            if (string.IsNullOrEmpty(muCode))
            {
                if (account.Room.RealityObject.Municipality != null)
                {
                    muCode = MuCodes.Get(account.Room.RealityObject.Municipality.Id);
                }

                if (string.IsNullOrWhiteSpace(muCode) && account.Room.RealityObject.MoSettlement != null)
                {
                    muCode = MuCodes.Get(account.Room.RealityObject.MoSettlement.Id);
                }

                if (string.IsNullOrWhiteSpace(muCode))
                {
                    throw new ValidationException(
                        "Для генерации лицевого счета необходимо заполнить справочник кодов населенных пунктов. Не найден код расположения {0}".FormatUsing(muId));
                }
            }

            SetNumber(account, muCode);
        }

        /// <summary>
        ///     Генерация номеров для коллекции лицевых счетов
        /// </summary>
        /// <param name="accounts">Коллекция лицевых счетов</param>
        public void Generate(ICollection<BasePersonalAccount> accounts)
        {
            foreach (var account in accounts)
            {
                var muCode = MuCodes.Get(account.Room.RealityObject.Municipality.Id);

                if (string.IsNullOrEmpty(muCode))
                {
                    if (account.Room.RealityObject.MoSettlement != null)
                    {
                        muCode = MuCodes.Get(account.Room.RealityObject.MoSettlement.Id);
                    }

                    if (string.IsNullOrWhiteSpace(muCode))
                    {
                        throw new ValidationException(
                            "Для генерации лицевого счета необходимо заполнить справочник кодов населенных пунктов. Не найден код расположения {0}".FormatUsing(
                                account.Room.RealityObject.Municipality.Id));
                    }
                }

                SetNumber(account, muCode);
            }
        }

        /// <summary>
        ///     Установить значение номера лицевого счета
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="muCode">Код муниципального образования</param>
        private void SetNumber(BasePersonalAccount account, string muCode)
        {
            var number = GetNextNumber();

            account.PersonalAccountNum =
                string.Format("{0}{1}",
                    muCode.PadLeft(2, '0'),
                    number.ToString(CultureInfo.InvariantCulture).PadLeft(7, '0'));
        }

        /// <summary>
        /// Получить следующий номер лс
        /// </summary>
        /// <returns>Следующий номер лс</returns>
        private int GetNextNumber()
        {
            //если дошли до конца пакета номеров, то запрашиваем новый пакет
            if (_current == 0 || _batchStart + BatchSize <= _current)
            {
                _batchStart = GetNextBatch(_sessionProvider);
                _current = _batchStart;
            }
            //иначе используем существующий
            else
            {
                _current++;
            }

            return _current;
        }

        /// <summary>
        ///     Получить начало следующего пакета номеров
        /// </summary>
        /// <returns>Номер с которого начинается следующая пачка номеров</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static int GetNextBatch(ISessionProvider sessionProvider)
        {
            int value;

            using(var session = sessionProvider.OpenStatelessSession())
            {
                using(var tr = session.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var hiValue = session.QueryOver<PersonalAccountNumberHiValue>().SingleOrDefault();

                        if (hiValue == null)
                        {
                            var maxNumber = (session.QueryOver<BasePersonalAccount>()
                                .Where(x => x.PersonalAccountNum != null)
                                .Select(x => x.PersonalAccountNum)
                                .List<string>()
                                .Where(x => x.Length == 9)
                                .Select(x => (int?) x.Substring(2).ToInt())
                                .Max() ?? 0) + 1;

                            value = maxNumber;
                            hiValue = new PersonalAccountNumberHiValue { Value = value };
                            session.Insert(hiValue);
                        }
                        else
                        {
                            hiValue.Value += BatchSize + 1;
                            value = hiValue.Value;
                            session.Update(hiValue);
                        }

                        tr.Commit();
                    }
                    catch
                    {
                        if (tr.IsActive)
                            tr.Rollback();
                        throw;
                    }
                }
            }

            return value;
        }
    }
}