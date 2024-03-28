namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Enums;
    using System;

    public class SMEVNDFLAnswer : BaseEntity
    {
        /// <summary>
        /// Запись
        /// </summary>
        public virtual SMEVNDFL SMEVNDFL { get; set; }

        /// <summary>
        /// ИНН
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
    }
}
