namespace Bars.GisIntegration.Base.Entities.External.Dict.House
{
    using System;

    using Bars.B4.DataAccess;
    using Entities.External.Dict.Common;

    /// <summary>
    /// НСИ 2. Вид коммунального ресурса
    /// </summary>
    public class CommunalSource : BaseEntity
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
        /// Наименование 
        /// </summary>
        public virtual string CommunalSourceName { get; set; }
        /// <summary>
        /// Сокращенное наименование 
        /// </summary>
        public virtual string CommunalSourceShortName { get; set; }
        /// <summary>
        /// Возможность установки связи с прибором учета
        /// </summary>
        public virtual bool IsPuLink { get; set; }
        /// <summary>
        /// ОКЕИ
        /// </summary>
        public virtual Okei Okei { get; set; }
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
