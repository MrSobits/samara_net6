namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип собственника расчетного счета
    /// </summary>
    public enum TypeOwnerCalcAccount
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        [Display("Управляющая организация")]
        Manorg = 10,

        /// <summary>
        /// Региональный оператор
        /// </summary>
        [Display("Региональный оператор")]
        Regoperator = 20
    }
}