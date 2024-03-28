namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Предоставляемые документы рапоряжения ГЖИ
    /// </summary>
    public class DecisionProvidedDoc : BaseEntity
    {
        /// <summary>
        /// Предоставляемый документа
        /// </summary>
        public virtual ProvidedDocGji ProvidedDoc { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Decision Decision { get; set; }
    }
}