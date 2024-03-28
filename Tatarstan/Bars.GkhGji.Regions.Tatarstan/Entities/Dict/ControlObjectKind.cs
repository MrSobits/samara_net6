namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Справочник. Вид объекта контроля
    /// </summary>
    public class ControlObjectKind: BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }

        /// <summary>
        /// Тип объекта контроля
        /// </summary>
        public virtual ControlObjectType ControlObjectType { get; set; }

        /// <summary>
        /// Идентификатор в ЕРВК
        /// </summary>
        public virtual string ErvkId { get; set; }
    }
}