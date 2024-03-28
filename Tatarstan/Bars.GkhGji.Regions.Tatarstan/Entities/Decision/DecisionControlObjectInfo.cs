namespace Bars.GkhGji.Regions.Tatarstan.Entities.Decision
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    /// <summary>
    /// Сведения об объектах контроля
    /// </summary>
    public class DecisionControlObjectInfo : BaseEntity
    {
        /// <summary>
        /// Решение
        /// </summary>
        public virtual Decision Decision { get; set; }
        
        /// <summary>
        /// Проверяемые дома в инспекционной проверки ГЖИ
        /// </summary>
        public virtual InspectionGjiRealityObject InspGjiRealityObject { get; set; }

        /// <summary>
        /// Вид объекта контроля
        /// </summary>
        public virtual ControlObjectKind ControlObjectKind { get; set; }
    }
}