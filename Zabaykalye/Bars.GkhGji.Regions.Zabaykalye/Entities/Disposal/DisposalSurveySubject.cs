namespace Bars.GkhGji.Regions.Zabaykalye.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Zabaykalye.Enums;
    using Gkh.Entities;

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