namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Управляющие организации, товарищества собственников жилья, кооперативы
    /// </summary>
    public class UoProxy : IHaveId
    {
        /// <summary>
        /// 1. Контрагент
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long Id { get; set; }

        /// <summary>
        /// 2. Контактный номер председателя
        /// </summary>
        public string LeaderPhone { get; set; }

        /// <summary>
        /// 3. Штатная численность административного персонала
        /// </summary>
        public int? AdministrativeStaffCount { get; set; }

        /// <summary>
        /// 4. Штатная численность инженеров
        /// </summary>
        public int? EngineersCount { get; set; }

        /// <summary>
        /// 5. Штатная численность рабочих
        /// </summary>
        public int? EmployeesCount { get; set; }

        /// <summary>
        /// 6. Доля участия субъекта Российской Федерации в уставном капитале организации
        /// </summary>
        public decimal? ShareSf { get; set; }

        /// <summary>
        /// 7. Доля участия муниципального образования в уставном капитале организации
        /// </summary>
        public decimal? ShareMo { get; set; }

        /// <summary>
        /// 8. Признак ТСЖ/кооператив
        /// </summary>
        public int IsTsj { get; set; }
    }
}
