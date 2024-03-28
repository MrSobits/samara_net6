namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// ТИпы исполнения обращения
    /// </summary>
    public class AppealCitsExecutionType : BaseGkhEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Тип исполнения
        /// </summary>
        public virtual AppealExecutionType AppealExecutionType { get; set; }        
    }
}