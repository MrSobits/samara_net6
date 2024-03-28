namespace Bars.Gkh.RegOperator.Entities
{
    /// <summary>
    /// Класс Dto для <see cref="BasePersonalAccount"/> с кошельками
    /// </summary>
    public class PersonalAccountWalletDto : PersonalAccountDto
    {
        /// <summary>
        /// Гуид кошелька по базовому тарифу
        /// </summary>
        public string BaseTariffWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька по тарифу решения
        /// </summary>
        public string DecisionTariffWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька по пени
        /// </summary>
        public string PenaltyWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька по соц. поддержке
        /// </summary>
        public string SocialSupportWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька ранее накопленных средств
        /// </summary>
        public string AccumulatedFundWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька за ранее проделанные работы
        /// </summary>
        public string PreviosWorkPaymentWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька оплат по мировому соглашению
        /// </summary>
        public string RestructAmicableAgreementWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька оплат аренды
        /// </summary>
        public string RentWallet { get; set; }

    }
}