namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дом Предостережения
    /// Данная таблица хранит всебе все дома которые необходимо проверить
    /// </summary>
    public class WarningDocRealObj : BaseEntity
    {
        /// <summary>
        /// Предостережение
        /// </summary>
        public virtual WarningDoc WarningDoc { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}