namespace Bars.KP60.Protocol.DomainService.Impl
{
    /// <summary>
    /// Протокол расчета: поисковые данные лицевого счета
    /// </summary>
    public class AccountDataChd
    {
        /// <summary>
        /// ID лицевого счета
        /// </summary>
        public long PersonalAccountId { get; set; }

        /// <summary>
        /// № лицевого счета
        /// </summary>
        public long PersonalAccountNumber { get; set; }

        /// <summary>
        /// ID дома
        /// </summary>
        public long HouseId { get; set; }



        /// <summary>
        /// Расчетный год и месяц
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Расчетный год и месяц
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// Договор ЖКУ
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// Признак блока данных ЦХД
        /// </summary>
        public bool IsGis { get; set; }



    }
}
