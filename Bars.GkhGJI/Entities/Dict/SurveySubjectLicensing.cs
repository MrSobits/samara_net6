namespace Bars.GkhGji.Entities.Dict
{
    using B4.DataAccess;
    using GkhGji.Enums;

    /// <summary>
    /// Предметы проверки Лицензирование
    /// </summary>
    public class SurveySubjectLicensing : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}