namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Предоставленная информация
    /// </summary>
    public class VisitSheetInfo: BaseEntity
    {
        /// <summary>
        /// Лист визита
        /// </summary>
        public virtual VisitSheet VisitSheet { get; set; }

        /// <summary>
        /// Сведения
        /// </summary>
        public virtual string Info { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }

    }
}
