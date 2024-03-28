namespace Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// Дополнительный поставщик информации (addcontragent.csv)
    /// </summary>
    public class AddContragentProxy : IHaveId
    {
        /// <summary>
        /// Уникальный код контрагента
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип поставщика информации
        /// </summary>
        public string InformationProviderType { get; set; }
    }
}