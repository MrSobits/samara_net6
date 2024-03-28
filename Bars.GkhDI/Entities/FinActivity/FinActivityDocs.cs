namespace Bars.GkhDi.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Документы фин деятельности
    /// </summary>
    public class FinActivityDocs : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Бухгалтерский баланс
        /// </summary>
        public virtual FileInfo BookkepingBalance { get; set; }

        /// <summary>
        /// Приложение к бухгалтерскому балансу
        /// </summary>
        public virtual FileInfo BookkepingBalanceAnnex { get; set; }
    }
}
