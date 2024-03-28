namespace Bars.Gkh.DataProviders.Meta
{
    using Bars.Gkh.Entities.Licensing;

    /// <summary>
    /// Описание <see cref="GovernmenServiceDetail"/> для отчёта
    /// </summary>
    public class GovernmenServiceDetailMeta
    {
        /// <summary>
        /// Отображаемое значение
        /// </summary>
        public string DisplayValue { get; set; }

        /// <summary>
        /// Номер строки (может быть пустым)
        /// </summary>
        public int? RowNumber { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public decimal? Value { get; set; }

    }
}