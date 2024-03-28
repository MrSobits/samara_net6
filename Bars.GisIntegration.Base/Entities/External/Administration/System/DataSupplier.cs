namespace Bars.GisIntegration.Base.Entities.External.Administration.System
{
    using B4.DataAccess;

    using Entities.External.Dict.Common;

    /// <summary>
    /// Поставщик информации
    /// </summary>
    public class DataSupplier : BaseEntity
    {
        /// <summary>
        /// Идентификатор схемы
        /// </summary>
        public virtual int TableSchemaId { get; set; }

        /// <summary>
        /// Наименование поставщика инфомрации
        /// </summary>
        public virtual string DataSupplierName { get; set; }
        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }
        /// <summary>
        /// КПП
        /// </summary>
        public virtual string Kpp { get; set; }
        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// Тип контрагента
        /// </summary>
        public virtual ExtContragentType ContragentType { get; set; }
    }
}
