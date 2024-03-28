namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Предоставляемые документы административного дела ГЖИ
    /// </summary>
    public class AdministrativeCaseProvidedDoc : BaseEntity
    {
        /// <summary>
        /// адм дело
        /// </summary>
        public virtual AdministrativeCase AdministrativeCase { get; set; }

        /// <summary>
        /// Предоставляемый документа
        /// </summary>
        public virtual ProvidedDocGji ProvidedDoc { get; set; }
    }
}