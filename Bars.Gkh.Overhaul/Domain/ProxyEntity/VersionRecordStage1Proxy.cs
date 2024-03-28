namespace Bars.Gkh.Overhaul.Domain.ProxyEntity
{
    /// <summary>
    /// Прокси для версии 1 этапа из из дпкр
    /// </summary>
    public class VersionRecordStage1Proxy
    {
        /// <summary>
        /// Идентификатор 1го этапа ДПКР
        /// </summary>
        public long Stage1Id { get; set; }

        /// <summary>
        /// Идентификатор 1го этапа ДПКР
        /// </summary>
        public long Stage2Id { get; set; }

        /// <summary>
        /// Скорректированный год
        /// </summary>
        public int CorrectionYear { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public long RealityObjectId { get; set; }

        /// <summary>
        /// КЭ Id
        /// </summary>
        public long StructElementId { get; set; }

        /// <summary>
        /// КЭ Наименование
        /// </summary>
        public string StructElementName { get; set; }

        /// <summary>
        /// ООИ Id
        /// </summary>
        public long CeoId { get; set; }

        /// <summary>
        /// ООИ Наименование
        /// </summary>
        public string CeoName { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public decimal Volume { get; set; }
    }
}