using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh.Gis.Entities.CalcVerification
{
    /// <summary>
    /// Таблицы заполняемые в проверочном расчете
    /// </summary>
    [Flags]
    public enum CalcVerificationTables
    {
        /// <summary>
        /// gil_xx - таблица с данными о кол-во проживающих
        /// </summary>
        TenantTable = 1,
        /// <summary>
        /// chd_counters_xx - таблица с данными о расходах ИПУ, норм., ОДН и т.д.
        /// </summary>
        ConsumptionsTable = 2,
        /// <summary>
        /// chd_calc_gku_xx - таблица с данными о расходах 
        /// по услугам после применения формул расчета
        /// </summary>
        CalcServicesTable = 4,
        /// <summary>
        /// chd_charge_xx - таблица с данными о начислениях по услугам 
        /// </summary>
        ChargesTable = 8
    }
}
