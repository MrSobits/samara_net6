namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Акты выполненных работ по договору на выполнение работ по капитальному ремонту
    /// </summary>
    public class ActWorkDogovProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы/услуги
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Код договора на выполнение работ по капитальному ремонту
        /// </summary>
        [ProxyId(typeof(DogovorPkrProxy))]
        public long? DogovorPkrId { get; set; }

        /// <summary>
        /// 3. Статус
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 4. Наименование акта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 5. Номер акта
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 6. Дата акта
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 7. Сумма акта
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// 8. Сумма штрафных санкций Исполнителю
        /// </summary>
        public decimal ExecutantPenaltySum { get; set; }

        /// <summary>
        /// 9. Сумма штрафных санкций Заказчику
        /// </summary>
        public decimal CustomerPenaltySum { get; set; }

        /// <summary>
        /// 10. Акт подписан представителем собственников
        /// </summary>
        public int? IsSigned { get; set; }

        /// <summary>
        /// 11. Фамилия представителя собственников
        /// </summary>
        public string AgentSurname { get; set; }

        /// <summary>
        /// 12. Имя представителя собственников
        /// </summary>
        public string AgentName { get; set; }

        /// <summary>
        /// 13. Отчество представителя собственников
        /// </summary>
        public string AgentPatronymic { get; set; }

        /// <summary>
        /// 14. Рассрочка по оплате выполненных работ
        /// </summary>
        public int? IsInstallments { get; set; }

        #region ACTWORK
        /// <summary>
        /// ACTWORK 2. Уникальный идентификатор работы КПР
        /// </summary>
        [ProxyId(typeof(WorkDogovProxy))]
        public long? WorkDogovId { get; set; }

        /// <summary>
        /// ACTWORK 3. Стоимость работ
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// ACTWORK 4. Объем работ
        /// </summary>
        public decimal? Volum { get; set; }

        /// <summary>
        /// ACTWORK 5. Принята в эксплуатацию
        /// </summary>
        public int ExploitationAccepted { get; set; }

        /// <summary>
        /// ACTWORK 6. Дата начала гарантийного срока
        /// </summary>
        public DateTime? WarrantyStartDate { get; set; }

        /// <summary>
        /// ACTWORK 7. Дата окончания гарантийного срока
        /// </summary>
        public DateTime? WarrantyEndDate { get; set; }
        #endregion

        #region ACTWORKDOGOVFILES
        /// <summary>
        /// ACTWORK 1. Уникальный идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// ACTWORK 3. Тип файла
        /// </summary>
        public int Type { get; set; }
        #endregion
    }
}