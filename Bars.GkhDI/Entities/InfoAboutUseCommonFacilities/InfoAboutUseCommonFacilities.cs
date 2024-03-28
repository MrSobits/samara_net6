namespace Bars.GkhDi.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Enums;
    using System;

    /// <summary>
    /// Сведения об использовании мест общего пользования
    /// </summary>
    public class InfoAboutUseCommonFacilities : BaseGkhEntity
    {
        #warning Оставить ссылку только на дом
        /// <summary>
        /// Объект в управление
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Вид общего имущества
        /// </summary>
        public virtual string KindCommomFacilities { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? From { get; set; }

        /// <summary>
        /// Файл протокола
        /// </summary>
        public virtual FileInfo ProtocolFile { get; set; }

        /// <summary>
        /// Тип арендатора
        /// </summary>
        public virtual LesseeTypeDi LesseeType { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

		/// <summary>
		/// Отчество
		/// </summary>
		public virtual string Patronymic { get; set; }

		/// <summary>
		/// Пол
		/// </summary>
		public virtual Gender Gender { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string Snils { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Наименование арендатора (пользователя)
        /// </summary>
        public virtual string Lessee { get; set; }
        
        /// <summary>
        /// Тип договора
        /// </summary>
        public virtual TypeContractDi TypeContract { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Сумма договора
        /// </summary>
        public virtual decimal? CostContract { get; set; }

        /// <summary>
        /// Скан документа
        /// </summary>
        public virtual FileInfo ContractFile { get; set; }

        /// <summary>
        /// Назначение общего имущества
        /// </summary>
        public virtual string AppointmentCommonFacilities { get; set; }

        /// <summary>
        /// Площадь общего имущества (заполняется в отношении помещений и земельных участков) (кв.м)
        /// </summary>
        public virtual decimal? AreaOfCommonFacilities { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string ContractNumber { get; set; }

        /// <summary>
        /// Предмет договора
        /// </summary>
        public virtual string ContractSubject { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? ContractDate { get; set; }

        /// <summary>
        /// Стоимость по договору в месяц (руб.)
        /// </summary>
        public virtual decimal? CostByContractInMonth { get; set; }

		/// <summary>
		/// Комментарий
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		/// Дата подписания договора
		/// </summary>
		public virtual DateTime? SigningContractDate { get; set; }

        #region Период внесения платы по договору

        /// <summary>
        /// День месяца начало периода
        /// </summary>
        public virtual int? DayMonthPeriodIn { get; set; }

        /// <summary>
        /// День месяца окончания периода
        /// </summary>
        public virtual int? DayMonthPeriodOut { get; set; }

        /// <summary>
        /// Последний день месяца начало периода
        /// </summary>
        public virtual bool? IsLastDayMonthPeriodIn { get; set; }

        /// <summary>
        /// Последний день месяца окончания периода
        /// </summary>
        public virtual bool? IsLastDayMonthPeriodOut { get; set; }

        /// <summary>
        /// День следующего месяца начало периода
        /// </summary>
        public virtual bool? IsNextMonthPeriodIn { get; set; }

        /// <summary>
        /// День следующего месяца окончания периода
        /// </summary>
        public virtual bool? IsNextMonthPeriodOut { get; set; }

        #endregion
    }
}
