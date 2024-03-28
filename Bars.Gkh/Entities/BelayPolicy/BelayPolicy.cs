namespace Bars.Gkh.Entities
{
    using System;

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Страховой полис
    /// </summary>
    public class BelayPolicy : BaseGkhEntity
    {
        /// <summary>
        /// Страхование деятельности УО
        /// </summary>
        public virtual BelayManOrgActivity BelayManOrgActivity { get; set; }

        /// <summary>
        /// Страховая организация
        /// </summary>
        public virtual BelayOrganization BelayOrganization { get; set; }

        /// <summary>
        /// Вид деятельности страховой организации
        /// </summary>
        public virtual BelayOrgKindActivity BelayOrgKindActivity { get; set; }

        /// <summary>
        /// Действие полиса
        /// </summary>
        public virtual PolicyAction PolicyAction { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата начала действия договора
        /// </summary>
        public virtual DateTime? DocumentStartDate { get; set; }

        /// <summary>
        /// Дата окончания действия договора
        /// </summary>
        public virtual DateTime? DocumentEndDate { get; set; }

        /// <summary>
        /// Лимит ответственности УО (по страховому случаю), в руб.
        /// </summary>
        public virtual decimal? LimitManOrgInsured { get; set; }

        /// <summary>
        /// Лимит ответственности УО (в отношении 1 дома), в руб.
        /// </summary>
        public virtual decimal? LimitManOrgHome { get; set; }

        /// <summary>
        /// Общий лимит гражданской ответственности (в отношении одного пострадавшего), в руб.
        /// </summary>
        public virtual decimal? LimitCivilOne { get; set; }

        /// <summary>
        /// Общий лимит гражданской ответственности (по страховому случаю), в руб.
        /// </summary>
        public virtual decimal? LimitCivilInsured { get; set; }

        /// <summary>
        /// Общий лимит гражданской ответственности, в руб.
        /// </summary>
        public virtual decimal? LimitCivil { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Cause { get; set; }

        /// <summary>
        /// Сумма страхования
        /// </summary>
        public virtual decimal? BelaySum { get; set; }
    }
}