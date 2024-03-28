namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System;

    public class SMEVNDFL : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// ИдЗапрос
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// год отчетного периода
        /// </summary>
        public virtual string PeriodYear { get; set; }

        /// <summary>
        /// Код (идентификатор) государственной услуги
        /// </summary>
        public virtual string ServiceCode { get; set; }

        /// <summary>
        /// Ф
        /// </summary>
        public virtual string FamilyName { get; set; }

        /// <summary>
        /// И
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// О
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime BirthDate { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string SNILS { get; set; }

        /// <summary>
        /// Номер заявления физического лица
        /// </summary>
        public virtual string RegNumber { get; set; }

        /// <summary>
        /// Дата заявления
        /// </summary>
        public virtual DateTime RegDate { get; set; }

        /// <summary>
        /// Код документа
        /// </summary>
        public virtual FLDocType DocumentCode { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        public virtual string SeriesNumber { get; set; }

        //ответ
        /// <summary>
        /// ИННЮЛ
        /// </summary>
        public virtual string INNUL { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public virtual string KPP { get; set; }

        /// <summary>
        /// Наим. орг.
        /// </summary>
        public virtual string OrgName { get; set; }

        /// <summary>
        /// Ставка
        /// </summary>
        public virtual int? Rate { get; set; }

        /// <summary>
        /// КодДоход
        /// </summary>
        public virtual string RevenueCode { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual string Month { get; set; }

        /// <summary>
        /// СумДоход
        /// </summary>
        public virtual decimal? RevenueSum { get; set; }

        /// <summary>
        /// КодВычет
        /// </summary>
        public virtual string RecoupmentCode { get; set; }

        /// <summary>
        /// СумВычет
        /// </summary>
        public virtual decimal? RecoupmentSum { get; set; }

        /// <summary>
        /// НалБаза
        /// </summary>
        public virtual decimal? DutyBase { get; set; }

        /// <summary>
        /// Сумма налога исчисленная
        /// </summary>
        public virtual decimal? DutySum { get; set; }

        /// <summary>
        /// Сумма налога, не удержанная налоговым агентом
        /// </summary>
        public virtual decimal? UnretentionSum { get; set; }

        /// <summary>
        /// СумДохОбщ
        /// </summary>
        public virtual decimal? RevenueTotalSum { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }
    }
}
