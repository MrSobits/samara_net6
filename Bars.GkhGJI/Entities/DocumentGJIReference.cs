namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Таблица связи документов (Для того чтобы не делать жесткие ссылки между документами ГЖИ)
    /// Данная таблица необходима для того чтобы ставить ссылки между доментами по типам
    /// В дальнейшем если в одной связке унас будет не 2 документа на несколько то мы будем расширять данную сущность
    /// </summary>
    public class DocumentGjiReference : BaseGkhEntity
    {
        /// <summary>
        /// Тип связи документов ГЖИ
        /// </summary>
        public virtual TypeDocumentReferenceGji TypeReference { get; set; }

        /// <summary>
        /// 1й документ
        /// </summary>
        public virtual DocumentGji Document1 { get; set; }

        /// <summary>
        /// 2й документ
        /// </summary>
        public virtual DocumentGji Document2 { get; set; }
    }
}