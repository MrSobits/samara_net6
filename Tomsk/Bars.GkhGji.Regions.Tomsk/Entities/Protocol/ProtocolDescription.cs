namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ProtocolDescription : BaseEntity
    {
        public virtual Protocol Protocol { get; set; }

        /// <summary>
        /// Подробнее
        /// </summary>
        public virtual byte[] Description { get; set; }

        /// <summary>
        /// Установил
        /// </summary>
        public virtual byte[] DescriptionSet { get; set; }
    }
}