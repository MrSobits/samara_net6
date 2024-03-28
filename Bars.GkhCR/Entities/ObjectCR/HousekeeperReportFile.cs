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
    using Bars.B4.Modules.FileStorage;
    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид работы КР
    /// </summary>
    public class HousekeeperReportFile : BaseEntity
    {
        /// <summary>
        /// Отчет стршего по дому
        /// </summary>
        public virtual HousekeeperReport HousekeeperReport { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
        
    }
}
