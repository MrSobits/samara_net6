namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    public class ControlTypeRiskIndicators: BaseEntity
    {
        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }
        
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Идентификатор ЕРВК
        /// </summary>
        public virtual string ErvkId { get; set; }
    }
}