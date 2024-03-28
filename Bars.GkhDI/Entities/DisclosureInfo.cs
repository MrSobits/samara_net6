namespace Bars.GkhDi.Entities
{
    using System;

    using Bars.B4.Application;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Utils;

    using Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Деятельность управляющей организации в периоде раскрытия информации
    /// </summary>
    public class DisclosureInfo : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Период раскрытия информации
        /// </summary>
        public virtual PeriodDi PeriodDi { get; set; }

        /// <summary>
        /// Признак расчета процентов
        /// </summary>
        public virtual bool IsCalculation { get; set; }

        /// <summary>
        /// Признак, обозначающий, что выполняется расчет процентов, но не более 1 часа (проверка выполняется только из веб приложения)
        /// </summary>
        [JsonIgnore]
        public virtual bool InCalculation => this.IsCalculation && this.ObjectEditDate > DateTime.Now.AddHours(-1) && ApplicationContext.Current.GetContextType() == ApplicationContextType.WebApplication;

        #region Количество персонала

        /// <summary>
        /// Административный персонал
        /// </summary>
        public virtual int? AdminPersonnel { get; set; }

        /// <summary>
        /// Инженеры
        /// </summary>
        public virtual int? Engineer { get; set; }

        /// <summary>
        /// Рабочие
        /// </summary>
        public virtual int? Work { get; set; }

        /// <summary>
        /// Административный персонал уволено
        /// </summary>
        public virtual int? DismissedAdminPersonnel { get; set; }

        /// <summary>
        /// Инженеры уволено
        /// </summary>
        public virtual int? DismissedEngineer { get; set; }

        /// <summary>
        /// Рабочие уволено
        /// </summary>
        public virtual int? DismissedWork { get; set; }

        /// <summary>
        /// Число несчастных случаев
        /// </summary>
        public virtual int? UnhappyEventCount { get; set; }
        #endregion

        #region Единичные поля над гридами разделов

        /// <summary>
        /// Случаи расторжения договора в данном отчетном периоде
        /// </summary>
        public virtual YesNoNotSet TerminateContract { get; set; }

        /// <summary>
        /// Членство в объединениях
        /// </summary>
        public virtual YesNoNotSet MembershipUnions { get; set; }

        /// <summary>
        /// Сведения о фондах
        /// </summary>
        public virtual YesNoNotSet FundsInfo { get; set; }

        /// <summary>
        /// Документ, подтверждающий отсутствие фондов у организации
        /// </summary>
        public virtual FileInfo DocumentWithoutFunds { get; set; }

        /// <summary>
        /// Административная ответственность
        /// </summary>
        public virtual YesNoNotSet AdminResponsibility { get; set; }

        /// <summary>
        /// Размер обязательных платежей и взносов
        /// </summary>
        public virtual decimal? SizePayments { get; set; }

        /// <summary>
        /// Действующие договоры за отчетный период
        /// </summary>
        public virtual YesNoNotSet ContractsAvailability { get; set; }

        /// <summary>
        /// Количество заключенных договоров (Сведения о договорах)
        /// </summary>
        public virtual int? NumberContracts { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Имеется ли лицензия на осуществление деятельности по управлению многоквартирными домами
        /// </summary>
        public virtual YesNoNotSet HasLicense { get; set; }

        #endregion
    }
}
