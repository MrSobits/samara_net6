namespace Bars.GisIntegration.Base.Entities.External.Dict.Oki
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Dict.Common;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    /// <summary>
    /// Вид коммунальной инфраструктуры (121?)
    /// </summary>
    public class SkiType : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string DictCode { get; set; }
        /// <summary>
        /// ГУИД из ГИС
        /// </summary>
        public virtual string GisGuid { get; set; }
        /// <summary>
        /// ОКЕИ
        /// </summary>
        public virtual Okei Okei { get; set; }
        /// <summary>
        ///  Вид коммунального ресурса (4)
        /// </summary>
        public virtual CommunalSource CommunalSource { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string SkiTypeName { get; set; }
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public virtual string SkiTypeShortName { get; set; }
        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
