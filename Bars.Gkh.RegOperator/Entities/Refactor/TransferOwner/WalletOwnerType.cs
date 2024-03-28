namespace Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип владельца кошелька
    /// </summary>
    public enum WalletOwnerType
    {
        /// <summary>
        /// Лицевой счёт
        /// </summary>
        [Display("Лицевой счёт")]
        BasePersonalAccount = 10,

        /// <summary>
        /// Счёт оплат жилого дома
        /// </summary>
        [Display("Счёт оплат жилого дома")]
        RealityObjectPaymentAccount = 20
    }
}