namespace Bars.Gkh.RegOperator.Enums
{
    /// <summary>
    /// Источник тарифа на КР
    /// </summary>
    public enum TariffSourceType
    {
        /// <summary>
        /// Отсутсвует
        /// </summary>
        None = 0,

        /// <summary>
        /// Исключения по подъездам
        /// </summary>
        EntranceSize = 1,

        /// <summary>
        /// Исключения по типу дома
        /// </summary>
        PaysizeByType = 2,

        /// <summary>
        /// Справочник рамеров взносов на КР
        /// </summary>
        PaysizeByMu = 3,

        /// <summary>
        /// Протокол решения на доме
        /// </summary>
        PaySizeByProtocol = 4
    }
}