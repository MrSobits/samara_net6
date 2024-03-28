namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Предоставляемый документ Типа обследования
    /// </summary>
    public class TypeSurveyProvidedDocumentGji : BaseEntity
    {
        /// <summary>
        /// Тип обследования
        /// </summary>
        public virtual TypeSurveyGji TypeSurvey { get; set; }

        /// <summary>
        /// Предоставляемый документ ГЖИ
        /// </summary>
        public virtual ProvidedDocGji ProvidedDocGji { get; set; }
    }
}