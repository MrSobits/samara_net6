namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;

    public class GISGMPPayerStatus : BaseEntity
    {
        /// <summary>
        /// Код статуса
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        ///Наименование статуса
        /// </summary>
        public virtual string Name { get; set; }

    }
}
