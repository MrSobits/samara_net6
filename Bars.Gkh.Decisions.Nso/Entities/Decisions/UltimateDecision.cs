namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System;
    
    using Bars.Gkh.Entities;

    /// <summary>
    ///     Протокол решения собственников жилья
    /// </summary>
    public class UltimateDecision : BaseImportableEntity
    {
        /// <summary>
        ///     Дата протокола
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        ///     Проверен ли
        /// </summary>
        public virtual bool IsChecked { get; set; }

        /// <summary>
        ///     Решения по дому
        /// </summary>
        public virtual RealityObjectDecisionProtocol Protocol { get; set; }
    }
}