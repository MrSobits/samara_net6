﻿namespace Bars.Gkh.ConfigSections.RegOperator
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Расчет вести для домов, у которых способ формирования фонда
    /// </summary>
    public class HouseCalculationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Счет регионального оператора
        /// </summary>
        [GkhConfigProperty(DisplayName = "Счет регионального оператора")]
        public virtual bool RegopCalcAccount { get; set; }

        /// <summary>
        /// Специальный счет регионального оператора
        /// </summary>
        [GkhConfigProperty(DisplayName = "Специальный счет регионального оператора")]
        public virtual bool RegopSpecialCalcAccount { get; set; }

        /// <summary>
        /// Специальный счет
        /// </summary>
        [GkhConfigProperty(DisplayName = "Специальный счет")]
        public virtual bool SpecialCalcAccount { get; set; }

        /// <summary>
        /// Не выбран
        /// </summary>
        [GkhConfigProperty(DisplayName = "Не выбран")]
        public virtual bool Unknown { get; set; }
    }
}