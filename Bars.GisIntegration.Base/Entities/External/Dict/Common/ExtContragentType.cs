namespace Bars.GisIntegration.Base.Entities.External.Dict.Common
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Типы контрагентов
    /// </summary>
    public class ExtContragentType : BaseEntity
    {
        /// <summary>
        /// Полное имя
        /// </summary>
        public virtual string ContragentTypeName { get; set; }
        /// <summary>
        /// Краткое наименование
        /// </summary>
        public virtual string ContragentTypeNameShort { get; set; }
        /// <summary>
        /// Гуид
        /// </summary>
        public virtual string GisGuid { get; set; }
        /// <summary>
        /// Код
        /// </summary>
        public virtual string DictCode { get; set; }
        /// <summary>
        /// Актуальность
        /// </summary>
        public virtual bool IsActual { get; set; }
    }
}
