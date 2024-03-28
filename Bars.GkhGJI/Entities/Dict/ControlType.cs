namespace Bars.GkhGji.Entities.Dict
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Base;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Вид контроля.
    /// </summary>
    public class ControlType : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Уникальный идентификатор ТОР.
        /// </summary>
        public virtual Guid? TorId { get; set; }

        /// <summary>
        /// Наименование вида контроля (надзора)
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Уровень контроля (надзора).
        /// </summary>
        public virtual ControlLevel? Level { get; set; }

        /// <summary>
        /// Идентификатор вида контроля из ЕРВК	
        /// </summary>
        public virtual string ErvkId { get; set; }
    }
}