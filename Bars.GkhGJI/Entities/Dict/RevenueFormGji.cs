namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities.Base;

    /// <summary>
    /// Форма поступления
    /// </summary>
    public class RevenueFormGji : BaseGkhEntity, IEntityWithName
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}