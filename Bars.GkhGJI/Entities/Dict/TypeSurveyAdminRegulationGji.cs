namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Административные регламенты
    /// </summary>
    public class TypeSurveyAdminRegulationGji : BaseEntity
    {

        /// <summary>
        /// Тип обследования
        /// </summary>
        public virtual TypeSurveyGji TypeSurvey { get; set; }

        /// <summary>
        /// Нормативный документ
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }
    }
}