namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using B4.Utils;

    /// <summary>
    /// тип погашения задолженности
    /// </summary>
    public enum LawsuitCollectionDebtType
    {
        /// <summary>
        /// Не погашено
        /// </summary>
        [Display("Не погашено")]
        NotRepaiment = 0,

        /// <summary>
        /// Погашено частично
        /// </summary>
        [Display("Погашено частично")]
        PartiallyRepaiment = 10,

        /// <summary>
        /// Погашено полностью
        /// </summary>
        [Display("Погашено полностью")]
        FullRepaid = 20
    }
}