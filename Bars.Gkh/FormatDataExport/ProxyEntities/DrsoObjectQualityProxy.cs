namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// Показатели качества предметов договора ресурсоснабжения
    /// </summary>
    public class DrsoObjectQualityProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Предмет договора ресурсоснабжения
        /// </summary>
        public long? DrsoId { get; set; }

        /// <summary>
        /// 3. Показатель качества коммунальных ресурсов
        /// </summary>
        public int? QualityType { get; set; }

        /// <summary>
        /// 4. Значение показателя качества
        /// </summary>
        public decimal? QualityValue { get; set; }

        /// <summary>
        /// 5. Начало диапазона значения
        /// </summary>
        public decimal? StartValue { get; set; }

        /// <summary>
        /// 6. Конец диапазона значения
        /// </summary>
        public decimal? EndValue { get; set; }

        /// <summary>
        /// 7. Код ОКЕИ
        /// </summary>
        public string OkeiCode { get; set; }

        /// <summary>
        /// DRSOOBJECTOTHERQUALITY 3. Наименование показателя качества
        /// </summary>
        public string QualityName { get; set; }
    }
}