namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Вид проверок
    /// </summary>
    public class TypeSurveyKindInspGji : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Тип обследования
        /// </summary>
        public virtual TypeSurveyGji TypeSurvey { get; set; }

        /// <summary>
        /// Вид обследования
        /// </summary>
        public virtual KindCheckGji KindCheck { get; set; }
    }
}