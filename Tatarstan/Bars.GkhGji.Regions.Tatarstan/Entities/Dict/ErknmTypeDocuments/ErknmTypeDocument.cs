namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Справочник "Тип документов ЕРКНМ"
    /// </summary>
    public class ErknmTypeDocument : BaseEntity
    {
        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual string DocumentType { get; set; }

        /// <summary>
        /// Код в ЕРКНМ
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Используется в "Основании проведения КНМ"
        /// </summary>
        public virtual bool IsBasisKnm { get; set; }
    }
}