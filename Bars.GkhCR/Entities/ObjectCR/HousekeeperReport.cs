namespace Bars.GkhCr.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.States;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;

    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид работы КР
    /// </summary>
    public class HousekeeperReport : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Старший по дому
        /// </summary>
        public virtual RealityObjectHousekeeper RealityObjectHousekeeper { get; set; }       

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата отчета
        /// </summary>
        public virtual DateTime ReportDate { get; set; }

        /// <summary>
        /// Устранена
        /// </summary>
        public virtual bool IsArranged { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual string ReportNumber { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Дата отчета
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual string CheckTime { get; set; }

    }
}
