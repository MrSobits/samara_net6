namespace Bars.GkhCr.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Договор КР на услуги
    /// </summary>
    public class ContractCr : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public virtual FinanceSource FinanceSource { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public virtual Contragent Customer { get; set; }

        /// <summary>
        /// Тип договора объекта КР
        /// </summary>
        public virtual MultipurposeGlossaryItem TypeContractObject { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual TypeWorkCr TypeWork { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Сумма договора
        /// </summary>
        public virtual decimal? SumContract { get; set; }

        /// <summary>
        /// Бюджет МО
        /// </summary>
        public virtual decimal? BudgetMo { get; set; }

        /// <summary>
        /// Бюджет субъекта
        /// </summary>
        public virtual decimal? BudgetSubject { get; set; }

        /// <summary>
        /// Средства собственников
        /// </summary>
        public virtual decimal? OwnerMeans { get; set; }

        /// <summary>
        /// Средства фонда
        /// </summary>
        public virtual decimal? FundMeans { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }
    }
}