namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Прокси для Паспорт дома
    /// </summary>
    public class HouseDocProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код записи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный код дома в системе отправителя
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long HouseId { get; set; }

        /// <summary>
        /// 3. Код параметра
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 4. Значение Параметра (Текст)
        /// </summary>
        public string TextValue { get; set; }

        /// <summary>
        /// 5. Значение Параметра (Число)
        /// </summary>
        public decimal? DecimalValue { get; set; }

        /// <summary>
        /// 6. Значение Параметра (Дата)
        /// </summary>
        public DateTime? DateValue { get; set; }

        /// <summary>
        /// 7. Значение Параметра (Целое)
        /// </summary>
        public int? IntValue { get; set; }

        /// <summary>
        /// 8. Значение Параметра (Логическое Да / Нет)
        /// </summary>
        public bool? BoolValue { get; set; }

        /// <summary>
        /// 9. Значение Параметра (Справочное)
        /// </summary>
        public string DictValue { get; set; }

        /// <summary>
        /// 10. Значение Параметра (Файл)
        /// </summary>
        public long? FileValue { get; set; }

    }
}