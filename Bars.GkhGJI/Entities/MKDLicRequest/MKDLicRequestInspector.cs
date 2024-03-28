
namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Enums;
    using System;

    /// <summary>
    /// Прилагаемые документы
    /// </summary>
    public class MKDLicRequestInspector : BaseEntity
    {
        /// <summary>
        /// MKDLicRequest
        /// </summary>
        public virtual MKDLicRequest MKDLicRequest { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Inspector { get; set; }
       
    }
}
