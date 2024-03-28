namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// Справочник единиц измерения
    /// </summary>
    public class DictMeasureProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код единицы измерения
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 3. Сокращенное обозначение
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 4. Код по ОКЕИ
        /// </summary>
        public string OkeiCode { get; set; }

        /// <summary>
        /// 5. Код базовой единицы измерения
        /// </summary>
        public string BaseCode { get; set; }
    }
}