namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    public class PhysicalPersonDocType : BaseEntity
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
