namespace Bars.Gkh.Entities.Dicts
{
    /// <summary>
    /// Справочник "Документы подрядных организаций"
    /// </summary>
    public class BuilderDocumentType : BaseGkhEntity
    {
        /// <summary>
        /// Пустой конструктор.
        /// <remarks>Code = -1 для того, чтобы отловить это в интерцепторе</remarks>
        /// </summary>
        public BuilderDocumentType()
        {
            Code = -1;
        }

        /// <summary>
        /// Код
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// Наименование типа документа
        /// </summary>
        public virtual string Name { get; set; }
    }
}
