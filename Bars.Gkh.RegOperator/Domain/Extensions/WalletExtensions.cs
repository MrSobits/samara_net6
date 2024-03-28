namespace Bars.Gkh.RegOperator.Domain.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Wallet;

    using Castle.Windsor;

    /// <summary>
    ///     Методы расширения для кошельков
    /// </summary>
    public static class WalletExtensions
    {
        /// <summary>
        ///     Получить строковое название кошелька
        /// </summary>
        public static string GetWalletName(
            this Wallet wallet, 
            IWindsorContainer container, 
            BasePersonalAccount account,
            bool useDefaultName = false,
            string notFoundValue = "Неизвестный источник")
        {
            ArgumentChecker.NotNull(wallet, "wallet");
            ArgumentChecker.NotNull(account, "account");

            var glossaryDomain = container.ResolveDomain<MultipurposeGlossary>();
            try
            {
                var glossary = glossaryDomain.GetAll().FirstOrDefault(x => x.Code == "distribution_type");
                var items = glossary != null ? glossary.Items : new List<MultipurposeGlossaryItem>();

                MultipurposeGlossaryItem item = null;
                string defaultValue;
                if (account.BaseTariffWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "TransferCrDistribution");
                    defaultValue = "Средства собственников по минимальному тарифу";
                }
                else if (account.DecisionTariffWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "TransferCrDistribution");
                    defaultValue = "Средства собственников по тарифу решения";
                }
                else if (account.PenaltyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "ComissionForAccountServiceDistribution");
                    defaultValue = "Пени";
                }
                else if (account.AccumulatedFundWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "AccumulatedFundsDistribution");
                    defaultValue = "Ранее накопленные средства";
                }
                else if (account.RestructAmicableAgreementWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "RestructAmicableAgreementWallet");
                    defaultValue = "Реструктуризация по мировому соглашению";
                }
                else if (account.PreviosWorkPaymentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "PreviousWorkPaymentDistribution");
                    defaultValue = "Средства за ранее выполненные работы";
                }
                else if (account.RentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "RentPaymentDistribution");
                    defaultValue = "Средства за оплату аренды";
                }
                else if (account.SocialSupportWallet.Id == wallet.Id)
                {
                    defaultValue = "Средства соц. поддержки";
                }
                else
                {
                    defaultValue = notFoundValue;
                }

                return (useDefaultName || item == null) ? defaultValue : item.Value;
            }
            finally
            {
                container.Release(glossaryDomain);
            }
        }

        /// <summary>
        ///     Получить строковое название кошелька
        /// </summary>
        public static string GetWalletName(
            this Wallet wallet, 
            IWindsorContainer container, 
            RealityObjectPaymentAccount account,
            bool useDefaultName = false,
            string notFoundValue = "Неизвестный источник")
        {
            ArgumentChecker.NotNull(wallet, "wallet");
            ArgumentChecker.NotNull(account, "account");

            var glossaryDomain = container.ResolveDomain<MultipurposeGlossary>();
            try
            {
                var glossary = glossaryDomain.GetAll().FirstOrDefault(x => x.Code == "distribution_type");
                var items = glossary != null ? glossary.Items : new List<MultipurposeGlossaryItem>();

                MultipurposeGlossaryItem item = null;
                string defaultValue;
                if (account.BaseTariffPaymentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "TransferCrDistribution");
                    defaultValue = "Средства собственников по минимальному тарифу";
                }
                else if (account.DecisionPaymentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "TransferCrDistribution");
                    defaultValue = "Средства собственников по тарифу решения";
                }
                else if (account.PenaltyPaymentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "ComissionForAccountServiceDistribution");
                    defaultValue = "Пени";
                }
                else if (account.AccumulatedFundWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "AccumulatedFundsDistribution");
                    defaultValue = "Ранее накопленные средства";
                }
                else if (account.PreviosWorkPaymentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "PreviousWorkPaymentDistribution");
                    defaultValue = "Средства за ранее выполненные работы";
                }
                else if (account.RentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "RentPaymentDistribution");
                    defaultValue = "Средства за оплату аренды";
                }
                else if (account.SocialSupportWallet.Id == wallet.Id)
                {
                    defaultValue = "Средства соц. поддержки";
                }
                else if (account.FundSubsidyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "FundSubsidyDistribution");
                    defaultValue = "Субсидия фонда";
                }
                else if (account.RegionalSubsidyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "RegionalSubsidyDistribution");
                    defaultValue = "Региональная субсидия";
                }
                else if (account.StimulateSubsidyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "StimulateSubsidyDistribution");
                    defaultValue = "Стимулирующая субсидия";
                }
                else if (account.TargetSubsidyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "TargetSubsidyDistribution");
                    defaultValue = "Целевая субсидия";
                }
                else if (account.BankPercentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "BankPercentDistribution");
                    defaultValue = "Поступление процентов банка";
                }
                else if (account.RestructAmicableAgreementWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "RestructAmicableAgreementDistribution");
                    defaultValue = "Средства за оплату по мировому соглашению";
                }
                else if (account.OtherSourcesWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "OtherSourcesDistribution");
                    defaultValue = "Иные поступления";
                }
                else
                {
                    defaultValue = notFoundValue;
                }

                return (useDefaultName || item == null) ? defaultValue : item.Value;
            }
            finally
            {
                container.Release(glossaryDomain);
            }
        }

        /// <summary>
        ///     Получить строковое название кошелька, различающее оплаты по базовому тарифу и тарифу решений
        /// </summary>
        public static string GetWalletFullName(
            this Wallet wallet,
            IWindsorContainer container,
            RealityObjectPaymentAccount account,
            bool useDefaultName = false,
            string notFoundValue = "Неизвестный источник")
        {
            ArgumentChecker.NotNull(wallet, "wallet");
            ArgumentChecker.NotNull(account, "account");

            var glossaryDomain = container.ResolveDomain<MultipurposeGlossary>();
            try
            {
                var glossary = glossaryDomain.GetAll().FirstOrDefault(x => x.Code == "distribution_type");
                var items = glossary != null ? glossary.Items : new List<MultipurposeGlossaryItem>();

                MultipurposeGlossaryItem item = null;
                string defaultValue;
                
                //универсальный справочник не различает средства собственников по мин тарифу и тарифу решений
                if (account.BaseTariffPaymentWallet.Id == wallet.Id)
                {
                    defaultValue = "Средства собственников по минимальному тарифу";
                }
                else if (account.DecisionPaymentWallet.Id == wallet.Id)
                {
                    defaultValue = "Средства собственников по тарифу решения";
                }
                else if (account.PenaltyPaymentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "ComissionForAccountServiceDistribution");
                    defaultValue = "Пени";
                }
                else if (account.AccumulatedFundWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "AccumulatedFundsDistribution");
                    defaultValue = "Ранее накопленные средства";
                }
                else if (account.PreviosWorkPaymentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "PreviousWorkPaymentDistribution");
                    defaultValue = "Средства за ранее выполненные работы";
                }
                else if (account.RentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "RentPaymentDistribution");
                    defaultValue = "Средства за оплату аренды";
                }
                else if (account.SocialSupportWallet.Id == wallet.Id)
                {
                    defaultValue = "Средства соц. поддержки";
                }
                else if (account.FundSubsidyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "FundSubsidyDistribution");
                    defaultValue = "Субсидия фонда";
                }
                else if (account.RegionalSubsidyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "RegionalSubsidyDistribution");
                    defaultValue = "Региональная субсидия";
                }
                else if (account.StimulateSubsidyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "StimulateSubsidyDistribution");
                    defaultValue = "Стимулирующая субсидия";
                }
                else if (account.TargetSubsidyWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "TargetSubsidyDistribution");
                    defaultValue = "Целевая субсидия";
                }
                else if (account.BankPercentWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "BankPercentDistribution");
                    defaultValue = "Поступление процентов банка";
                }
                else if (account.RestructAmicableAgreementWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "RestructAmicableAgreementDistribution");
                    defaultValue = "Оплата по мировому соглашению";
                }
                else if (account.OtherSourcesWallet.Id == wallet.Id)
                {
                    item = items.FirstOrDefault(x => x.Key == "OtherSourcesDistribution");
                    defaultValue = "Иные поступления";
                }
                else
                {
                    defaultValue = notFoundValue;
                }

                return (useDefaultName || item == null) ? defaultValue : item.Value;
            }
            finally
            {
                container.Release(glossaryDomain);
            }
        }
    }
}