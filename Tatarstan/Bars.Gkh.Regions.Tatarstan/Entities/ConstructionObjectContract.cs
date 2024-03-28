namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Договор для объекта строительства
    /// </summary>
    public class ConstructionObjectContract : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект строительства
        /// </summary>
        public virtual ConstructionObject ConstructionObject { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Тип договора
        /// </summary>
        public virtual ConstructionObjectContractType Type { get; set; }

        /// <summary>
        /// Наименование договора
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual int? Number { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Сумма договора
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Дата начала действия договора
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия договора
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }
    }
}
