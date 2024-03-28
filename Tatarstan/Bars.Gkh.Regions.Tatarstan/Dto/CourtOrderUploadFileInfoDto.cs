namespace Bars.Gkh.Regions.Tatarstan.Dto
{
    using System;

    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    /// <summary>
    /// Сущность для выгружаемого файла информации о делопроизводствах
    /// </summary>
    public class CourtOrderUploadFileInfoDto
    {
        /// <summary>
        /// Номер строки
        /// </summary>
        public int RowNum { get; set; }

        /// <summary>
        /// id ИП
        /// </summary>
        public long OuterId { get; set; }

        /// <summary>
        /// Подразделение ОСП
        /// </summary>
        public string JurInstitution { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Регистрационный номер ИП
        /// </summary>
        public string RegNumber { get; set; }

        /// <summary>
        /// Должник
        /// </summary>
        public string Debtor { get; set; }


        /// <summary>
        /// Адрес должника
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Адрес ПГМУ
        /// </summary>
        public PgmuAddress PgmuAddress { get; set; }

        /// <summary>
        /// Дата последнего платежа
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// Сумма последнего платежа
        /// </summary>
        public decimal? PaySum { get; set; }
        
        /// <summary>
        /// Дата создания ИП
        /// </summary>
        public DateTime? EntrepreneurCreateDate { get; set; }
        
        /// <summary>
        /// Сумма задолженности ИП
        /// </summary>
        public decimal? EntrepreneurDebtSum { get; set; }
    }
}