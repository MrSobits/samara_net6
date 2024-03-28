namespace Bars.GkhGji.Entities.Dict
{
    using B4.DataAccess;

    /// <summary>
    /// Справочники - ГЖИ - Цель проведения проверки
    /// </summary>
    public class AuditPurposeSurveySubjectGji : BaseEntity
    {
        /// <summary>
        /// Цель проведения проверки
        /// </summary>
        public virtual AuditPurposeGji AuditPurpose { get; set; }

        /// <summary>
        /// Предметы проверки
        /// </summary>
        public virtual SurveySubject SurveySubject { get; set; }
    }
}