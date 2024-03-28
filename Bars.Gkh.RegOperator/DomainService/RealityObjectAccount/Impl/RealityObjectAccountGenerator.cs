namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Utils;
    using Bars.Gkh.RegOperator.Exceptions;
    using Castle.Windsor;
    using RealityObjectAccount;
    using Entities;
    using Enums;
    using Gkh.Entities;

    public class RealityObjectAccountGenerator : IRealityObjectAccountGenerator
    {
        public RealityObjectAccountGenerator(
            IDomainService<RealityObjectChargeAccount> chargeAccountDomain,
            IDomainService<RealityObjectPaymentAccount> paymentAccountDomain,
            IDomainService<RealityObjectSupplierAccount> suppAccDomain,
            IDomainService<RealityObjectSubsidyAccount> subsidyAccDomain,
            IDomainService<LocationCode> locCodeDomain,
            IDomainService<Fias> fiasDomain,
            IWindsorContainer container
            )
        {
            _chargeAccountDomain = chargeAccountDomain;
            _paymentAccountDomain = paymentAccountDomain;
            _suppAccDomain = suppAccDomain;
            _subsidyAccDomain = subsidyAccDomain;
            _locCodeDomain = locCodeDomain;
            _fiasDomain = fiasDomain;
            _container = container;
        }

        public void GenerateChargeAccounts(IQueryable<RealityObject> collection)
        {
            InLock<RealityObjectChargeAccount>(() => collection
                .Where(x => !_chargeAccountDomain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                .Select(x => new RealityObject { Id = x.Id }).ToList().ForEach(x =>
                {
                    var chargeAccount = new RealityObjectChargeAccount
                    {
                        RealityObject = x,
                        AccountNumber = GenerateAccountNumber<RealityObjectChargeAccount>(x)
                    };

                    _chargeAccountDomain.Save(chargeAccount);
                }));
        }

        public void GenerateChargeAccount(RealityObject realityObject)
        {
            InLock<RealityObjectChargeAccount>(() =>
            {
                var chargeAccount = new RealityObjectChargeAccount
                {
                    RealityObject = realityObject,
                    AccountNumber = GenerateAccountNumber<RealityObjectChargeAccount>(realityObject, true)
                };

                _chargeAccountDomain.Save(chargeAccount);
            });
        }

        public void GeneratePaymentAccounts(IQueryable<RealityObject> collection)
        {
            InLock<RealityObjectPaymentAccount>(() => collection
                .Where(x => !_paymentAccountDomain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                .Select(x => new RealityObject { Id = x.Id }).ToList().ForEach(x =>
                {
                    var newAcc = new RealityObjectPaymentAccount(x)
                    {
                        AccountType = RealityObjectPaymentAccountType.Common,
                        DateOpen = DateTime.Now,
                        AccountNumber = GenerateAccountNumber<RealityObjectPaymentAccount>(x)
                    };

                    _paymentAccountDomain.Save(newAcc);
                }));
        }

        public void GeneratePaymentAccount(RealityObject realityObject)
        {
            InLock<RealityObjectPaymentAccount>(() =>
            {
                if (_paymentAccountDomain.GetAll().Any(x => x.RealityObject.Id == realityObject.Id))
                {
                    throw new RealityObjectAccountGenerationException(
                        string.Format("PaymentAccount for realty object {{Id={0}, Address={1}}} already exists",
                            realityObject.Id, realityObject.Address));
                }
                var newAcc = new RealityObjectPaymentAccount(realityObject)
                {
                    AccountType = RealityObjectPaymentAccountType.Common,
                    DateOpen = DateTime.Now,
                    AccountNumber = GenerateAccountNumber<RealityObjectPaymentAccount>(realityObject, true)
                };

                _paymentAccountDomain.Save(newAcc);
            });
        }

        public void GenerateSupplierAccounts(IQueryable<RealityObject> collection)
        {
            InLock<RealityObjectSupplierAccount>(() => collection
                .Where(x => !_suppAccDomain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                .Select(x => new RealityObject { Id = x.Id }).ToList().ForEach(x =>
                {
                    var acc = new RealityObjectSupplierAccount
                    {
                        RealityObject = x,
                        OpenDate = DateTime.Now,
                        AccountNumber = GenerateAccountNumber<RealityObjectSupplierAccount>(x)
                    };

                    _suppAccDomain.Save(acc);
                }));
        }

        public void GenerateSupplierAccount(RealityObject realityObject)
        {
            InLock<RealityObjectSupplierAccount>(() =>
            {
                var acc = new RealityObjectSupplierAccount
                {
                    RealityObject = realityObject,
                    OpenDate = DateTime.Now,
                    AccountNumber = GenerateAccountNumber<RealityObjectSupplierAccount>(realityObject, true)
                };

                _suppAccDomain.Save(acc);
            });
        }

        public void GenerateSubsidyAccounts(IQueryable<RealityObject> collection)
        {
            InLock<RealityObjectSubsidyAccount>(() => collection
                .Where(x => !_subsidyAccDomain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                .Select(x => new RealityObject { Id = x.Id }).ToList().ForEach(x =>
                {
                    var newAcc = new RealityObjectSubsidyAccount
                    {
                        RealityObject = x,
                        DateOpen = DateTime.Now,
                        AccountNumber = GenerateAccountNumber<RealityObjectSubsidyAccount>(x)
                    };

                    _subsidyAccDomain.Save(newAcc);
                }));
        }

        public void GenerateSubsidyAccount(RealityObject realityObject)
        {
            InLock<RealityObjectSubsidyAccount>(() =>
            {
                var newAcc = new RealityObjectSubsidyAccount
                {
                    RealityObject = realityObject,
                    DateOpen = DateTime.Now,
                    AccountNumber = GenerateAccountNumber<RealityObjectSubsidyAccount>(realityObject, true)
                };

                _subsidyAccDomain.Save(newAcc);
            });
        }

        public string GenerateAccountNumber<T>(RealityObject ro, bool onlyForThisRo = false) where T : PersistentObject, IRealityObjectAccount
        {
            _locCodeCache = GetLocationCodes();

            _houseNumSb.Clear();
            _houseLetterSb.Clear();

            var accountProxy = GetAccount<T>(ro, onlyForThisRo);

            if (accountProxy == null)
            {
                return string.Empty;
            }

            var addressGuid = accountProxy.AddressGuid;
            var locCode = _locCodeCache.FirstOrDefault(x => addressGuid.Contains(x.AOGuid));

            var mrCode = locCode.Return(x => x.CodeLevel1).ToInt();
            var moCode = locCode.Return(x => x.CodeLevel2).ToInt();
            var cityCode = locCode.Return(x => x.CodeLevel3).ToInt();

            var streetKladrCode = accountProxy.CodeStreet;

            if (streetKladrCode != null && streetKladrCode.Length > 3)
            {
                streetKladrCode = streetKladrCode.Substring(streetKladrCode.Length - 3);
            }

            var streetKladrCodeInt = streetKladrCode.ToInt();

            var houseNum = _houseNumSb.Append(accountProxy.House.Where(char.IsDigit).ToArray()).ToInt();
            var houseLetter = _houseLetterSb.Append(accountProxy.House.Where(char.IsLetter).ToArray()).ToString();
            // буква из номера дома, если таковая есть

            if (houseLetter.Length > 1)
            {
                houseLetter = houseLetter.Substring(houseLetter.Length - 1);
            }
            else if (houseLetter.Length == 0)
            {
                houseLetter = "0";
            }

            var housing = accountProxy.Housing;
            if (housing != null && housing.Length > 1)
            {
                housing = housing.Substring(housing.Length - 1);
            }

            return string.Format(
                "{0}{1}{2}{3}{4}{5}",
                mrCode.ToString("D2"),
                // 1-2 знак - код муниципального района или городского округа из Справочника формирования ЛС
                moCode.ToString("D2"), // 3-4 знак - код муниципального образования из Справочника формирования ЛС
                cityCode.ToString("D2"), // 5-6 знак - код Населенного пункта из Справочника формирования ЛС
                streetKladrCodeInt > 0 ? streetKladrCodeInt.ToString("D3") : "___", // 7-8-9 знак - код улицы из КЛАДРа
                houseNum > 0 ? houseNum.ToString("D3") : "___", // 10-11-12 знак - номер дома из адреса МКД
                string.IsNullOrWhiteSpace(housing) ? houseLetter.ToUpper() : housing.ToUpper()
                // 13 знак - корпус, может быть числовое или буквенное значение
                );
        }

        #region private account number helper methods
        private AccountProxy GetAccount<T>(RealityObject ro, bool onlyForThisRo) where T : PersistentObject, IRealityObjectAccount
        {
            var accs = GetAccounts<T>(onlyForThisRo ? ro : null);

            AccountProxy proxy;
            if (accs != null)
            {
                accs.TryGetValue(ro.Id, out proxy);
                return proxy;
            }

            return null;
        }

        private IEnumerable<LocationCodeProxy> GetLocationCodes()
        {
            _locCodeCache = _locCodeCache ?? _locCodeDomain.GetAll()
                .Select(x => new LocationCodeProxy
                {
                    CodeLevel1 = x.CodeLevel1,
                    CodeLevel2 = x.CodeLevel2,
                    CodeLevel3 = x.CodeLevel3,
                    AOGuid = x.AOGuid
                })
                .Where(x => x.AOGuid != null && x.AOGuid != string.Empty)
                .ToList();

            return _locCodeCache;
        }

        private Dictionary<long, AccountProxy> GetAccounts<T>(RealityObject ro) where T : PersistentObject, IRealityObjectAccount
        {
            _accountCache = GetAccountCache<T>(ro);

            Dictionary<long, AccountProxy> accounts;
            _accountCache.TryGetValue(typeof(T), out accounts);

            return accounts;
        }

        private Dictionary<Type, Dictionary<long, AccountProxy>> GetAccountCache<T>(RealityObject ro) where T : PersistentObject, IRealityObjectAccount
        {
            _accountCache = _accountCache ?? new Dictionary<Type, Dictionary<long, AccountProxy>>();

            if (_accountCache.ContainsKey(typeof(T)))
            {
                return _accountCache;
            }

            // Когда передается объект - это признак того что он идет из действия интерцептора, а значит в базе его еще нет
            if (ro != null)
            {
                var fias = _fiasDomain.GetAll()
                    .FirstOrDefault(x => x.AOGuid == ro.FiasAddress.StreetGuidId);
                if (fias == null)
                {
                    return _accountCache;
                }

                var roPayAccDict = new Dictionary<long, AccountProxy>
                {
                    {
                        ro.Id, 
                        new AccountProxy
                        {
                            RoId = ro.Id,
                            AccountNumber = "",
                            AddressGuid = ro.FiasAddress.AddressGuid,
                            House = ro.FiasAddress.House,
                            Housing = ro.FiasAddress.Housing,
                            CodeStreet = fias.CodeStreet
                        }
                    }
                };
                _accountCache.Add(typeof(T), roPayAccDict);

                return _accountCache;
            }

            _accountCache.Add(typeof(T),
                _container.ResolveDomain<T>().GetAll()
                    .Join(
                        _fiasDomain.GetAll(),
                        x => x.RealityObject.FiasAddress.StreetGuidId,
                        y => y.AOGuid,
                        (a, b) => new AccountProxy
                        {
                            RoId = a.RealityObject.Id,
                            AccountNumber = a.AccountNumber,
                            AddressGuid = a.RealityObject.FiasAddress.AddressGuid,
                            House = a.RealityObject.FiasAddress.House,
                            Housing = a.RealityObject.FiasAddress.Housing,
                            CodeStreet = b.CodeStreet
                        })
                    .Where(x => x.AccountNumber == null || x.AccountNumber == "")
                    .Where(x => x.AddressGuid != null)
                    .ToList()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, x => x.First()));

            return _accountCache;
        }
        #endregion

        private object GetLocker<T>()
        {
            object syncRoot;
            if (_syncTree.TryGetValue(typeof(T), out syncRoot))
            {
                return syncRoot;
            }

            syncRoot = new object();

            while (_syncTree.TryAdd(typeof(T), syncRoot))
            {
                return syncRoot;
            }

            throw new InvalidOperationException();
        }

        private void InLock<T>(Action action)
        {
            var syncRoot = GetLocker<T>();
            var canMonitorExit = true;
            try
            {
                if (Monitor.TryEnter(syncRoot))
                {
                    action();
                }
                else
                {
                    canMonitorExit = false;
                }
            }
            finally
            {
                if (canMonitorExit)
                {
                    Monitor.Exit(syncRoot);
                }
                else
                {
                    throw new Exception("Задача уже выполняется!");
                }
            }
        }

        private readonly IDomainService<RealityObjectChargeAccount> _chargeAccountDomain;
        private readonly IDomainService<RealityObjectPaymentAccount> _paymentAccountDomain;
        private readonly IDomainService<RealityObjectSupplierAccount> _suppAccDomain;
        private readonly IDomainService<RealityObjectSubsidyAccount> _subsidyAccDomain;
        private readonly IDomainService<LocationCode> _locCodeDomain;
        private readonly IDomainService<Fias> _fiasDomain;
        private readonly IWindsorContainer _container;

        private IEnumerable<LocationCodeProxy> _locCodeCache;
        private Dictionary<Type, Dictionary<long, AccountProxy>> _accountCache;
        readonly StringBuilder _houseNumSb = new StringBuilder();
        readonly StringBuilder _houseLetterSb = new StringBuilder();

        static readonly ConcurrentDictionary<Type, object> _syncTree = new ConcurrentDictionary<Type, object>();

        private class LocationCodeProxy
        {
            public string CodeLevel1 { get; set; }
            public string CodeLevel2 { get; set; }
            public string CodeLevel3 { get; set; }
            public string AOGuid { get; set; }
        }

        private class AccountProxy
        {
            public long RoId { get; set; }
            public string AccountNumber { get; set; }
            public string AddressGuid { get; set; }
            public string House { get; set; }
            public string Housing { get; set; }
            public string CodeStreet { get; set; }
        }
    }
}