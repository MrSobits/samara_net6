namespace Bars.Gkh.Enums.Decisions
{
    using B4.Utils;

    /// <summary>
    /// Владелец счета при принятии решения о владельце
    /// </summary>
    public enum AccountOwnerDecisionType
    {
        [Display("Дом")] Custom = 2,

        [Display("Региональный оператор")] RegOp = 4
    }
}