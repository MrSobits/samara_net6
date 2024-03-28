namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Дом акта без взаимодействия
    /// Данная таблица хранит всебе все дома которые необходимо проверить
    /// </summary>
    public class ActIsolatedRealObj : BaseEntity
    {
        /// <summary>
        /// Акт без взаимодействия
        /// </summary>
        public virtual ActIsolated ActIsolated { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Выявлено нарушение
        /// </summary>
        public virtual YesNoNotSet HaveViolation { get; set; }
    }
}