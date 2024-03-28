namespace Bars.GisIntegration.Base.Entities.External.Dict.Oki
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Тип документа ОКИ
    /// </summary>
    public class OkiDocType : BaseEntity
    {
        /// <summary>
        /// Тип документа ОКИ
        /// </summary>
        public virtual string OkiDocTypeName { get; set; }
    }
}
