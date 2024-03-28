namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Сущность должностей инспекторов в видах контроля
    /// </summary>
    public class ControlTypeInspectorPositions : BaseEntity
    {
        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }

        /// <summary>
        /// Должность инспектора
        /// </summary>
        public virtual InspectorPositions InspectorPosition { get; set; }

        /// <summary>
        /// Вынесший документ
        /// </summary>
        public virtual bool IsIssuer { get; set; }

        /// <summary>
        /// Участвующий в мероприятии
        /// </summary>
        public virtual bool IsMember { get; set; }

        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public virtual string ErvkId { get; set; }
    }
}