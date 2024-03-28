namespace Bars.Gkh.Overhaul.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип счета
    /// </summary>
    public enum AccountType
    {
        [Display("Счет оплат")]
        PaymentAccount = 10,

        [Display("Специальный")]
        Special = 20,

        [Display("Счет начислений")]
        Accruals = 30,

        [Display("Расчетный")]
        //счет рег оператора
        CalcAccount = 40
    }
}