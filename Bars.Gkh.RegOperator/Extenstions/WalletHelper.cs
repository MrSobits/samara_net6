namespace Bars.Gkh.RegOperator.Extenstions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Помощник по работе с кошельками
    /// </summary>
    public static class WalletHelper
    {
        private static readonly IList<IWalletInfo> mapping = new List<IWalletInfo>();

        static WalletHelper()
        {
            var realtyObjectMap = new WalletHolder<RealityObjectPaymentAccount>();
            var accountMap = new WalletHolder<BasePersonalAccount>();

            WalletHelper.mapping.Add(realtyObjectMap);
            WalletHelper.mapping.Add(accountMap);

            realtyObjectMap
                .Map(x => x.BaseTariffPaymentWallet, WalletType.BaseTariffWallet)
                .Map(x => x.DecisionPaymentWallet, WalletType.DecisionTariffWallet)
                .Map(x => x.RentWallet, WalletType.RentWallet)
                .Map(x => x.PenaltyPaymentWallet, WalletType.PenaltyWallet)
                .Map(x => x.SocialSupportWallet, WalletType.SocialSupportWallet)
                .Map(x => x.PreviosWorkPaymentWallet, WalletType.PreviosWorkPaymentWallet)
                .Map(x => x.AccumulatedFundWallet, WalletType.AccumulatedFundWallet)
                .Map(x => x.RestructAmicableAgreementWallet, WalletType.RestructAmicableAgreementWallet)
                .Map(x => x.TargetSubsidyWallet, WalletType.TargetSubsidyWallet)
                .Map(x => x.FundSubsidyWallet, WalletType.FundSubsidyWallet)
                .Map(x => x.RegionalSubsidyWallet, WalletType.RegionalSubsidyWallet)
                .Map(x => x.StimulateSubsidyWallet, WalletType.StimulateSubsidyWallet)
                .Map(x => x.OtherSourcesWallet, WalletType.OtherSourcesWallet)
                .Map(x => x.BankPercentWallet, WalletType.BankPercentWallet);

            accountMap
                .Map(x => x.BaseTariffWallet, WalletType.BaseTariffWallet)
                .Map(x => x.DecisionTariffWallet, WalletType.DecisionTariffWallet)
                .Map(x => x.PenaltyWallet, WalletType.PenaltyWallet)
                .Map(x => x.RentWallet, WalletType.RentWallet)
                .Map(x => x.SocialSupportWallet, WalletType.SocialSupportWallet)
                .Map(x => x.PreviosWorkPaymentWallet, WalletType.PreviosWorkPaymentWallet)
                .Map(x => x.AccumulatedFundWallet, WalletType.AccumulatedFundWallet)
                .Map(x => x.RestructAmicableAgreementWallet, WalletType.RestructAmicableAgreementWallet);
        }

        /// <summary>
        /// Метод возвращает имя свойства кошелька
        /// </summary>
        /// <typeparam name="TOwner">Тип владельца</typeparam>
        /// <param name="type">Тип кошелька</param>
        /// <returns>Имя свойства</returns>
        public static string GetWalletPropertyName<TOwner>(this WalletType type) where TOwner : class, ITransferOwner
        {
            var holder = WalletHelper.mapping.FirstOrDefault(x => x is WalletHolder<TOwner>).To<WalletHolder<TOwner>>();
            return holder.GetWalletPropertyName(type);
        }

        /// <summary>
        /// Метод возвращает имя свойства кошелька
        /// </summary>
        /// <typeparam name="TOwner">Тип владельца</typeparam>
        /// <param name="type">Тип кошелька</param>
        /// <param name="owner">Владелец кошелька</param>
        /// <returns>Имя свойства</returns>
        public static string GetWalletPropertyName<TOwner>(this WalletType type, TOwner owner) where TOwner : class, ITransferOwner
        {
            return type.GetWalletPropertyName<TOwner>();
        }

        /// <summary>
        /// Метод возвращает имя свойства кошелька
        /// </summary>
        /// <param name="type">Тип кошелька</param>
        /// <returns>Имя свойства</returns>
        public static string GetWalletPropertyName(this WalletType type)
        {
            var name = WalletHelper.mapping.Select(x => x.GetWalletPropertyName(type)).FirstOrDefault(x => x != null);
            return name;
        }

        /// <summary>
        /// Метод возвращает тип кошелька по имени свойства
        /// </summary>
        /// <param name="name">Имя свойства</param>
        /// <returns>Тип кошелька</returns>
        public static WalletType GetWalletTypeByPropertyName(string name)
        {
            return WalletHelper.mapping.Select(x => x.GetWalletType(name)).Where(x => x.HasValue).Select(x => x.Value).FirstOrDefault();
        }

        /// <summary>
        /// Метод возвращает кошелек по типу
        /// </summary>
        /// <param name="owner">Владелец</param>
        /// <param name="type">Тип кошелька</param>
        /// <returns>Кошелек</returns>
        public static Wallet GetWalletByType(this ITransferOwner owner, WalletType type)
        {
            return WalletHelper.mapping.Select(x => x.GetWallet(owner, type)).FirstOrDefault(x => x != null);
        }

        /// <summary>
        /// Метод возвращает кошелек по типу
        /// </summary>
        /// <param name="owner">Владелец</param>
        /// <param name="type">Тип кошелька</param>
        /// <typeparam name="TOwner">Владелец</typeparam>
        /// <returns>Кошелек</returns>
        public static Wallet GetWalletByType<TOwner>(this TOwner owner, WalletType type) where TOwner : class, ITransferOwner
        {
            var holder = WalletHelper.mapping.FirstOrDefault(x => x is WalletHolder<TOwner>).To<WalletHolder<TOwner>>();
            return holder?.GetWallet(owner, type);
        }

        #region Interface
        private interface IWalletInfo
        {
            string GetWalletPropertyName(WalletType type);

            WalletType? GetWalletType(string name);

            Wallet GetWallet(ITransferOwner owner, WalletType type);
        }

        private class WalletHolder<TOwner> : IWalletInfo
            where TOwner : class, ITransferOwner
        {
            private readonly HashSet<Tuple<string, Func<TOwner, Wallet>, WalletType>> walletMap 
                = new HashSet<Tuple<string, Func<TOwner, Wallet>, WalletType>>();

            /// <inheritdoc />
            public string GetWalletPropertyName(WalletType type)
            {
                return this.walletMap.FirstOrDefault(x => x.Item3 == type)?.Item1;
            }

            /// <inheritdoc />
            public WalletType? GetWalletType(string name)
            {
                return this.walletMap.FirstOrDefault(x => x.Item1 == name)?.Item3;
            }

            /// <summary>
            /// Метод возвращает кошелек по типу
            /// </summary>
            /// <param name="owner">Владелец</param>
            /// <param name="type">Тип кошелька</param>
            /// <returns></returns>
            public Wallet GetWallet(ITransferOwner owner, WalletType type)
            {
                var typedOwner = owner as TOwner;
                if (typedOwner == null)
                {
                    return null;
                }

                return this.walletMap
                    .Where(x => x.Item3 == type)
                    .Select(x => x.Item2(typedOwner))
                    .FirstOrDefault();
            }

            public WalletHolder<TOwner> Map(Expression<Func<TOwner, Wallet>> walletSelector, WalletType walletType)
            {
                var member = (PropertyInfo)((MemberExpression)walletSelector.Body).Member;

                this.walletMap.Add(Tuple.Create(member.Name, walletSelector.Compile(), walletType));

                return this;
            }
        }
        #endregion
    }
}