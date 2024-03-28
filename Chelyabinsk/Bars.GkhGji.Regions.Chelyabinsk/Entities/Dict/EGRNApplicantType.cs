namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Справочник "Категория заявителя ЕГРН"
    /// </summary>
    public class EGRNApplicantType : BaseEntity
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
