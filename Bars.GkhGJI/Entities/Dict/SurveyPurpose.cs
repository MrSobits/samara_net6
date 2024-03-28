namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Справочники - ГЖИ - Цели проверки
    /// </summary>
    public class SurveyPurpose : BaseEntity
    {
        /// <summary>
        /// Код.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public virtual string Name { get; set; }
    }
}