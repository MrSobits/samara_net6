namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Пруфы об устранении
    /// </summary>
    public class AppealOrderFile : BaseEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual AppealOrder AppealOrder { get; set; }

        /// <summary>
        /// Поручитель
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }        

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Description { get; set; }
      
    }
}
