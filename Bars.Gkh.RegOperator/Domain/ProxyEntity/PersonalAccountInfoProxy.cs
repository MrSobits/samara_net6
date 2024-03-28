namespace Bars.Gkh.RegOperator.Domain.ProxyEntity
{
    using System;

    public class PersonalAccountInfoProxy
    {
        /// <summary>
        /// (DOGID) Идентификатор договора (пусто)
        /// </summary>
        public string ContractId { get; set; }

        /// <summary>
        /// (UNID) Лицевой счет абонента
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// (OUTERLS) Номер Л/С во внешних системах
        /// </summary>
        public string PersAccNumExternalSystems { get; set; }

        /// <summary>
        /// (NAME1) Фамилия собственника
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// (NAME2) Имя собственника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// (NAME3) Отчество собственника
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// (ADR1) Адрес помещения
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// (STDATE) Дата начала действия записи (пусто)
        /// </summary>
        public string DateStart { get; set; }

        /// <summary>
        /// (ENDATE) Дата завершения действия записи (пусто)
        /// </summary>
        public string DateEnd { get; set; }

        /// <summary>
        /// (AREAPLACE) Площадь, относящаяся к лс
        /// </summary>
        public decimal Area { get; set; }

        /// <summary>
        /// (TARIFF) Тариф
        /// </summary>
        public decimal Tariff { get; set; }

        /// <summary>
        /// (ASHARE) Доля собственности
        /// </summary>
        public decimal AreaShare { get; set; }

        /// <summary>
        /// (ASHARE) Количество дней в периоде начисления
        /// </summary>
        public string CountDays { get; set; }

        /// <summary>
        /// (DEBT) Задолженность за прошлые периоды
        /// </summary>
        public decimal PenaltyDebt { get; set; }

        /// <summary>
        /// (DEBTDAY) Количество дней задолженности
        /// </summary>
        public string DebtCountDays { get; set; }

        /// <summary>
        /// (TYPEAB) Тип абонента (1 - физ, 2 - юр лицо)
        /// </summary>
        public string OwnerType { get; set; }

        /// <summary>
        /// (DOPEN) Дата открытия ЛС
        /// </summary>
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// (DCLOSE) Дата закрытия ЛС
        /// </summary>
        public string CloseDate { get; set; }

        /// <summary>
        /// (SUMMA) Сумма начислено
        /// </summary>
        public decimal ChargeTotal { get; set; }

        /// <summary>
        /// (BULLNUM) Уникальный идентификатор квитанции (штрих-код) (пусто)
        /// </summary>
        public string ReceiptId { get; set; }

        /// <summary>
        /// (USER1) Дополнительная информация 1 (пусто)
        /// </summary>
        public string AddInfo1 { get; set; }

        /// <summary>
        /// (USER2) Дополнительная информация 2 (пусто)
        /// </summary>
        public string AddInfo2 { get; set; }

        /// <summary>
        /// (USER3) Дополнительная информация 3 (пусто)
        /// </summary>
        public string AddInfo3 { get; set; }

        /// <summary>
        /// (USER4) Дополнительная информация 4 (пусто)
        /// </summary>
        public string AddInfo4 { get; set; }

        /// <summary>
        /// (VID_PL) Детализация вида платежа (пусто)
        /// </summary>
        public string TypePayment { get; set; }

        /// <summary>
        /// (KBK) Код бюджетной классификации платежа (пусто)
        /// </summary>
        public string ClassificationCode { get; set; }

        /// <summary>
        /// (OKATO) код окато (пусто)
        /// </summary>
        public string Okato { get; set; }

        /// <summary>
        /// Идентикатор РКЦ
        /// </summary>
        public string RkcId { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public decimal TotalArea { get; set; }

        /// <summary>
        /// Дата изменения общей площади
        /// </summary>
        public DateTime TotalAreaChangedDate { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public decimal LivingArea { get; set; }

        /// <summary>
        /// Дата изменения жилой площади
        /// </summary>
        public DateTime LivingAreaChangedDate { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public string RoomType { get; set; }

        /// <summary>
        /// Тип собственности 
        /// </summary>
        public string OwnershipType { get; set; }
        
        /// <summary>
        /// ИНН юр. лица
        /// </summary>
        public string InnLegal { get; set; }

        /// <summary>
        /// КПП юр. лица
        /// </summary>
        public string KppLegal { get; set; }

        /// <summary>
        /// Наименование юр. лица
        /// </summary>
        public string NameLegal { get; set; }

        /// <summary>
        /// Дата изменения доли собственности
        /// </summary>
        public DateTime AreaShareChangedDate { get; set; }

        /// <summary>
        /// (PERRECALC) Пересчет услуги за расчетный период
        /// </summary>
        public decimal SettlementPeriodRecalc { get; set; }

        /// <summary>
        /// (PENIASSD) Начислено пени за расчетный период
        /// </summary>
        public decimal Penalty { get; set; }
    }
}