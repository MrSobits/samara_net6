namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Предметы проверки ГЖИ
    /// </summary>
    public class DecisionVerificationSubject: BaseEntity
    {
        /// <summary>
        /// Приказ 
        /// </summary>
        public virtual Decision Decision { get; set; }

        /// <summary>
        /// Предмет проверки
        /// </summary>
        public virtual SurveySubject SurveySubject { get; set; }
    }
}
