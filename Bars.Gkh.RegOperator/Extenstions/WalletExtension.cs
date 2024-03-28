namespace Bars.Gkh.RegOperator.Extenstions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;

    using FastMember;

    /// <summary>
    /// Расширение для работы с
    /// </summary>
    public static class WalletExtension
    {
        private static Dictionary<string, List<PropertyInfo>> walletPropertiesByTypeMap = new Dictionary<string, List<PropertyInfo>>();

        /// <summary>
        /// Метод проверяет хозяин ли кошелька
        /// </summary>
        /// <returns> List guid кошельков </returns>
        public static bool IsOwner(this RealityObjectPaymentAccount realityObjectPaymentAccount, IWallet wallet)
        {
            return WalletExtension.IsOwner((object)realityObjectPaymentAccount, wallet);
        }

        /// <summary>
        /// Метод возвращает список кошельков
        /// </summary>
        /// <returns> List кошельков </returns>
        public static List<Wallet> GetWallets(this RealityObjectPaymentAccount realityObjectPaymentAccount)
        {
            return WalletExtension.GetWallets((object)realityObjectPaymentAccount);
        }

        /// <summary>
        /// Метод возвращает список кошельков
        /// </summary>
        /// <returns> List кошельков </returns>
        public static List<Wallet> GetWallets(this BasePersonalAccount basePersonalAccount)
        {
            return WalletExtension.GetWallets((object)basePersonalAccount);
        }

        /// <summary>
        /// Метод возвращает список основных кошельков (по базовому тарифу, тарифу решений, пени)
        /// </summary>
        /// <returns> List кошельков </returns>
        public static List<Wallet> GetMainWallets(this BasePersonalAccount basePersonalAccount)
        {
            var wallets = new List<Wallet>
            {
                basePersonalAccount.BaseTariffWallet,
                basePersonalAccount.DecisionTariffWallet,
                basePersonalAccount.PenaltyWallet,
                basePersonalAccount.SocialSupportWallet
            };

            return wallets;
        }

        /// <summary>
        /// Получить кошелёк по типу
        /// </summary>
        /// <param name="realityObjectPaymentAccount">Счёт на оплату дома</param>
        /// <param name="loanSourceType">Источник займа</param>
        /// <returns>Кошелёк</returns>
        public static Wallet GetWallet(this RealityObjectPaymentAccount realityObjectPaymentAccount, TypeSourceLoan loanSourceType)
        {
            switch (loanSourceType)
            {
                case TypeSourceLoan.FundSubsidy:
                    return realityObjectPaymentAccount.FundSubsidyWallet;

                case TypeSourceLoan.TargetSubsidy:
                    return realityObjectPaymentAccount.TargetSubsidyWallet;

                case TypeSourceLoan.PaymentByDesicionTariff:
                    return realityObjectPaymentAccount.DecisionPaymentWallet;

                case TypeSourceLoan.Penalty:
                    return realityObjectPaymentAccount.PenaltyPaymentWallet;

                case TypeSourceLoan.RegionalSubsidy:
                    return realityObjectPaymentAccount.RegionalSubsidyWallet;

                case TypeSourceLoan.StimulateSubsidy:
                    return realityObjectPaymentAccount.StimulateSubsidyWallet;

                default:
                    return realityObjectPaymentAccount.BaseTariffPaymentWallet;
            }
        }


        /// <summary>
        /// Общий метод получения кошельков
        /// </summary>
        /// <param name="walletHolder">Хранитель кошельков</param>
        /// <returns>Список кошельков</returns>
        public static List<Wallet> GetWallets(object walletHolder)
        {
            var type = walletHolder.GetType();

            List<PropertyInfo> wallets;
            if (!WalletExtension.walletPropertiesByTypeMap.TryGetValue(type.FullName, out wallets))
            {
                wallets = type.GetProperties().Where(x => x.PropertyType == typeof(Wallet)).ToList();
                WalletExtension.walletPropertiesByTypeMap[type.FullName] = wallets;
            }

            var acessor = ObjectAccessor.Create(walletHolder);

            var walletList = new List<Wallet>();

            foreach (var walletProp in wallets)
            {
                var wallet = (Wallet)acessor[walletProp.Name];
                if (wallet != null)
                {
                    walletList.Add(wallet);
                }
            }

            return walletList;
        }

        /// <summary>
        /// Хозяин ли кошелька
        /// </summary>
        /// <param name="walletHolder">Хранитель</param>
        /// <param name="wallet">Кошелёк</param>
        /// <returns>true - хозяин, false - нет</returns>
        public static bool IsOwner(object walletHolder, IWallet wallet)
        {
            var type = walletHolder.GetType();

            List<PropertyInfo> wallets;
            if (!WalletExtension.walletPropertiesByTypeMap.TryGetValue(type.FullName, out wallets))
            {
                wallets = type.GetProperties().Where(x => x.PropertyType == typeof(Wallet)).ToList();
                WalletExtension.walletPropertiesByTypeMap[type.FullName] = wallets;
            }

            var acessor = ObjectAccessor.Create(walletHolder);

            var result = false;
            foreach (var walletProp in wallets)
            {
                var value = (IWallet)acessor[walletProp.Name];
                if (value != null && value.WalletGuid == wallet.WalletGuid)
                {
                    result = true;
                    break;

                }
            }

            return result;
        }

        /// <summary>
        /// Метод подгружает входящие трансферы за период для указанного списка кошельков.
        /// </summary>
        /// <remarks>Не подгружает трансферы начислений, т.к. их нет</remarks>
        /// <param name="wallets">Кошельки</param>
        /// <param name="periods">Периоды, за которые необходимы трансферы</param>
        public static void FetchInTransfers(this IEnumerable<Wallet> wallets, params ChargePeriod[] periods)
        {
            foreach (var walletGroup in wallets.GroupBy(x => x.OwnerType))
            {
                switch (walletGroup.Key)
                {
                    case WalletOwnerType.BasePersonalAccount:
                        walletGroup.FetchTransfers<PersonalAccountPaymentTransfer>(periods, x => x.InTransfersDict, x => x.TargetGuid);
                        break;

                    case WalletOwnerType.RealityObjectPaymentAccount:
                        walletGroup.FetchTransfers<RealityObjectTransfer>(periods, x => x.InTransfersDict, x => x.TargetGuid);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Метод подгружает исходящие трансферы за период для указанного списка кошельков
        /// </summary>
        /// <remarks>Не подгружает трансферы начислений</remarks>
        /// <param name="wallets">Кошельки</param>
        /// <param name="periods">Периоды, за которые необходимы трансферы</param>
        public static void FetchOutTransfers(this IEnumerable<Wallet> wallets, params ChargePeriod[] periods)
        {
            foreach (var walletGroup in wallets.GroupBy(x => x.OwnerType))
            {
                switch (walletGroup.Key)
                {
                    case WalletOwnerType.BasePersonalAccount:
                        walletGroup.FetchTransfers<PersonalAccountPaymentTransfer>(periods, x => x.OutTransfersDict, x => x.SourceGuid);
                        walletGroup.FetchTransfers<PersonalAccountChargeTransfer>(periods, x => x.OutTransfersDict, x => x.SourceGuid);
                        break;

                    case WalletOwnerType.RealityObjectPaymentAccount:
                        walletGroup.FetchTransfers<RealityObjectTransfer>(periods, x => x.OutTransfersDict, x => x.SourceGuid);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void FetchTransfers<TTransfer>(
            this IEnumerable<Wallet> wallets,
            ChargePeriod[] periods,
            Func<Wallet, IDictionary<ChargePeriod, Transfer[]>> dictSelector,
            Expression<Func<TTransfer, string>> guidSelector) where TTransfer : Transfer
        {
            ArgumentChecker.NotNullOrEmpty(periods, nameof(periods));

            var container = ApplicationContext.Current.Container;
            var transferDomain = container.ResolveDomain<TTransfer>();
            var periodIds = periods.Select(x => x.Id).ToArray();

            var parameter = guidSelector.Parameters[0];
            var listMethod = typeof(List<string>).GetMethod("Contains");

            using (container.Using(transferDomain))
            {
                var groupFunc = guidSelector.Compile();

                foreach (var walletPortion in wallets.Section(6000))
                {
                    var guids = walletPortion.Select(x => x.WalletGuid).ToList();
                    var containsMethod = Expression.Call(Expression.Constant(guids), listMethod, guidSelector.Body);

                    var transfers =
                        transferDomain.GetAll()
                            .Where(x => periodIds.Contains(x.ChargePeriod.Id))
                            .Where(Expression.Lambda<Func<TTransfer, bool>>(containsMethod, parameter))
                            .AsEnumerable()
                            .GroupBy(x => x.ChargePeriod)
                            .ToDictionary(
                                x => x.Key,
                                y => y.GroupBy(groupFunc).ToDictionary(
                                    x => x.Key,
                                    x => x.ToArray()));

                    foreach (var wallet in walletPortion)
                    {
                        var transferDict = dictSelector(wallet);

                        foreach (var chargePeriod in periods)
                        {
                            var existsTransfers = transferDict.Get(chargePeriod).AllOrEmpty();
                            transferDict[chargePeriod] = (transfers.Get(chargePeriod)?.Get(wallet.WalletGuid)).AllOrEmpty()
                                .Union(existsTransfers)
                                .ToArray();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавить в кэш созданный трансфер
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="transfer">Созданный трансфер для кошелька</param>
        public static void AddOperationTransfer(this IWallet wallet, Transfer transfer)
        {
            var container = ApplicationContext.Current.Container;
            var walletRepo = container.Resolve<IWalletOperationHistoryRepository>();

            walletRepo.AddTransfer(wallet, transfer);
        }

        /// <summary>
        /// Метод проверяет, была ли уже првоедена отмена данного трансфера в текущей сессии
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="transfer">Отменяемый трансфер</param>
        /// <returns>Результат проверки</returns>
        public static bool IsTransferCancelled(this IWallet wallet, Transfer transfer)
        {
            var container = ApplicationContext.Current.Container;
            var walletRepo = container.Resolve<IWalletOperationHistoryRepository>();

            return walletRepo.IsTransferCancelled(wallet, transfer);
        }

        /// <summary>
        /// Метод возвращает строкове название столбца в базе данных ссылки на указанный тип кошелька
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <returns>Название столбца</returns>
        public static string GetWalletColumnName(this IWallet wallet)
        {
            return wallet.WalletType.GetWalletColumnName();
        }

        /// <summary>
        /// Метод возвращает строкове название столбца в базе данных ссылки на указанный тип кошелька
        /// </summary>
        /// <param name="walletType">Тип кошелька</param>
        /// <returns>Название столбца</returns>
        public static string GetWalletColumnName(this WalletType walletType)
        {
            switch (walletType)
            {
                case WalletType.BaseTariffWallet:
                    return "bt_wallet_id";

                case WalletType.DecisionTariffWallet:
                    return "dt_wallet_id";

                case WalletType.PenaltyWallet:
                    return "p_wallet_id";

                case WalletType.RentWallet:
                    return "r_wallet_id";

                case WalletType.SocialSupportWallet:
                    return "ss_wallet_id";

                case WalletType.PreviosWorkPaymentWallet:
                    return "pwp_wallet_id";

                case WalletType.AccumulatedFundWallet:
                    return "af_wallet_id";

                case WalletType.RestructAmicableAgreementWallet:
                    return "raa_wallet_id";

                case WalletType.TargetSubsidyWallet:
                    return "tsu_wallet_id";

                case WalletType.FundSubsidyWallet:
                    return "fsu_wallet_id";

                case WalletType.RegionalSubsidyWallet:
                    return "rsu_wallet_id";

                case WalletType.StimulateSubsidyWallet:
                    return "ssu_wallet_id";

                case WalletType.OtherSourcesWallet:
                    return "os_wallet_id";

                case WalletType.BankPercentWallet:
                    return "bp_wallet_id";

                default:
                    throw new ArgumentOutOfRangeException(nameof(walletType), walletType, null);
            }
        }

        /// <summary>
        /// Метод возвращает строкове название таблицы в базе данных ссылки владельца
        /// </summary>
        /// <param name="owner">Владелец кошелька/трансфера</param>
        /// <returns>Название таблицы</returns>
        public static string GetOwnerTableName(this ITransferOwner owner)
        {
            return owner.TransferOwnerType.GetOwnerTableName();
        }

        /// <summary>
        /// Метод возвращает строкове название таблицы в базе данных ссылки владельца
        /// </summary>
        /// <param name="ownerType">Тип владельца кошелька/трансфера</param>
        /// <returns>Название таблицы</returns>
        public static string GetOwnerTableName(this WalletOwnerType ownerType)
        {
            switch (ownerType)
            {
                case WalletOwnerType.BasePersonalAccount:
                    return "regop_pers_acc";

                case WalletOwnerType.RealityObjectPaymentAccount:
                    return "regop_ro_payment_account";

                default:
                    throw new ArgumentOutOfRangeException(nameof(ownerType), ownerType, null);
            }
        }
    }
}