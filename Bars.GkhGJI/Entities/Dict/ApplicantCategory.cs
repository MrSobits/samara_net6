using Bars.B4.DataAccess;

namespace Bars.GkhGji.Entities.Dict
{
    /// <summary>
    /// Категории заявителя
    /// </summary>
    public class ApplicantCategory : BaseEntity
    {
        /// <summary>
        /// Наименование категории
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код записи
        /// </summary>
        public virtual string Code { get; set; }
    }
}
