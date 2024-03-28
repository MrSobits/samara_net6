using Bars.B4.DataAccess;

namespace Bars.GkhGji.Entities.Dict
{
    /// <summary>
    /// Тип запроса на внесение изменения в реестр лицензий
    /// </summary>
    public class MKDLicTypeRequest : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        ///Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
