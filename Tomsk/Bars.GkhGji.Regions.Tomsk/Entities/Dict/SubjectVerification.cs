namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Предмет проверки
    /// </summary>
    public class SubjectVerification : BaseEntity
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
