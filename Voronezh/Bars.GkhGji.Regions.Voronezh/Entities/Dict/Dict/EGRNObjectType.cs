namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Справочник "Объект запроса ЕГРН"
    /// </summary>
    public class EGRNObjectType : BaseEntity
    {
        /// <summary>
        /// Классификационный код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        ///Наименование 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///Комментарий
        /// </summary>
        public virtual string Description { get; set; }


    }
}
