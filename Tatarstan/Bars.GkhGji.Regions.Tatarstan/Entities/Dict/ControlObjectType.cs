namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Справочник. Типы объекта контроля
    /// </summary>
    public class ControlObjectType : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Идентификатор в ЕРВК
        /// </summary>
        public virtual string ErvkId { get; set; }
    }
}