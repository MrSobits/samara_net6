namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Деятельности ТСЖ (Не путать с проверкой по дейтельности ТСЖ)
    /// </summary>
    public class ActivityTsj : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }
    }
}