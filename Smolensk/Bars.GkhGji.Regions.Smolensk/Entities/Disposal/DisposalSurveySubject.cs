namespace Bars.GkhGji.Regions.Smolensk.Entities.Disposal
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using GkhGji.Entities.Dict;

    /// <summary>
    /// Предмет проверки для приказа 
    /// </summary>
    public class DisposalSurveySubject : BaseEntity
    {
        /// <summary>
        /// Приказ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Предмет проверки
        /// </summary>
        public virtual SurveySubject SurveySubject { get; set; }
    }
}