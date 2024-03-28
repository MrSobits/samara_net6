namespace Bars.GkhGji.Entities.Dict
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Основание создания проверки />
    /// </summary>
    public class InspectionBasis : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}