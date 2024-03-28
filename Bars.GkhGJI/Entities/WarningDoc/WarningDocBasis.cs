namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Предостережение ГЖИ - основание для предостережения
    /// </summary>
    public class WarningDocBasis : BaseEntity
    {
        /// <summary>
        /// Предостережение ГЖИ
        /// </summary>
        public virtual WarningDoc WarningDoc { get; set; }

        /// <summary>
        /// Основание для предостережения
        /// </summary>
        public virtual WarningBasis WarningBasis { get; set; }
    }
}