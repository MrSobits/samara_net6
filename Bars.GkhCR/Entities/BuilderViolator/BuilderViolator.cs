using System;
using Bars.B4.DataAccess;
using Bars.GkhCr.Modules.ClaimWork.Enums;

namespace Bars.GkhCr.Entities
{

    /// <summary>
    /// Основание претензионно исковой работы для Договоров Подряда
    /// </summary>
    public class BuilderViolator : BaseEntity
    {
        /// <summary>
        /// Договор подряда
        /// </summary>
        public virtual BuildContract BuildContract { get; set; }

        /// <summary>
        /// тип создания записи
        /// </summary>
        public virtual BuildContractCreationType CreationType { get; set; }

        /// <summary>
        /// Дата начала отсчета
        /// </summary>
        public virtual DateTime? StartingDate { get; set; }

        /// <summary>
        /// количество дней просрочки
        /// </summary>
        public virtual int? CountDaysDelay { get; set; }
    }
}