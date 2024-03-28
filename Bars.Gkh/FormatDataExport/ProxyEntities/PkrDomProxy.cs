namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Дома ПКР (pkrdom.csv)
    /// </summary>
    public class PkrDomProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код дома в программе
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Идентификатор дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? HouseId { get; set; }

        /// <summary>
        /// 3. Идентификатор программы
        /// </summary>
        [ProxyId(typeof(PkrProxy))]
        public long? ProgramId { get; set; }

        /// <summary>
        /// 4. Муниципальное образование (ОКТМО)
        /// </summary>
        public string Oktmo { get; set; }
    }
}