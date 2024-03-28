namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Виды КНД
    /// </summary>
    public class EffectiveKNDIndex : BaseGkhEntity
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
        /// Код показателя в отчете
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование показателя в отчете
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Целевой показатель
        /// </summary>
        public virtual Decimal TargetIndex { get; set; }

        /// <summary>
        /// Текущий показатель
        /// </summary>
        public virtual Decimal CurrentIndex { get; set; }
       
    }
}