namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using B4.Modules.States;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Расчет коэффициента
    /// </summary>
    public class ROMCategory : BaseGkhEntity, IStatefulEntity

    {
        /// <summary>
        /// Тип КНД
        /// </summary>
        public virtual KindKND KindKND { get; set; }

        /// <summary>
        /// Год расчета
        /// </summary>
        public virtual YearEnums YearEnums { get; set; }

        /// <summary>
        ///Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        ///Vп
        /// </summary>
        public virtual Int32 Vp { get; set; }

        /// <summary>
        ///Vн
        /// </summary>
        public virtual Int32 Vn { get; set; }

        /// <summary>
        ///Vпр
        /// </summary>
        public virtual Int32 Vpr { get; set; }

        /// <summary>
        ///количество месяцев управления МКД
        /// </summary>
        public virtual Int32 MonthCount { get; set; }

        /// <summary>
        ///Площадь домов в управлении
        /// </summary>
        public virtual decimal MkdAreaTotal { get; set; }

        /// <summary>
        ///Коэффициент
        /// </summary>
        public virtual decimal Result { get; set; }

        /// <summary>
        ///Категория риска
        /// </summary>
        public virtual RiskCategory RiskCategory { get; set; }

        /// <summary>
        /// Дата расчета
        /// </summary>
        public virtual DateTime? CalcDate { get; set; }

        /// <summary>
        /// Инспектор, проводивший расчет
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }


    }
}