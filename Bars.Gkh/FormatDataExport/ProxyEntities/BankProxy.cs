namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Банки
    /// </summary>
    public class BankProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код контрагента
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long Id { get; set; }

        /// <summary>
        /// 2. БИК банка
        /// </summary>
        public string Bik { get; set; }

        /// <summary>
        /// 3. Корреспондентский счет
        /// </summary>
        public string CorrAccount { get; set; }
    }
}