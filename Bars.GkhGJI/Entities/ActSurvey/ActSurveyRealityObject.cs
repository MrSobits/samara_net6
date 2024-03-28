namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дом акта обследования
    /// Данная таблица хранит всебе все дома акта обследования
    /// </summary>
    public class ActSurveyRealityObject : BaseEntity
    {
        /// <summary>
        /// Акт обследования
        /// </summary>
        public virtual ActSurvey ActSurvey { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}