namespace Bars.Gkh.RegOperator.Entities
{
    using System.Collections.Generic;
    using Bars.Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Строка импорта для импорта платежных агентов
    /// </summary>
    public class FsGorodImportInfo : BaseImportableEntity
    {
        private readonly IList<FsGorodMapItem> _mapItems;

        /// <summary>
        /// Мапинг для импорта платежных агентов
        /// </summary>
        public FsGorodImportInfo()
        {
            _mapItems = new List<FsGorodMapItem>();
        }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Индекс, с которого читается информация
        /// </summary>
        public virtual int DataHeadIndex { get; set; }

        /// <summary>
        /// Разделитель
        /// </summary>
        public virtual string Delimiter { get; set; }

        /// <summary>
        /// Мапинг для импорта платежных агентов
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<FsGorodMapItem> MapItems
        {
            get { return _mapItems; }
        }
    }
}