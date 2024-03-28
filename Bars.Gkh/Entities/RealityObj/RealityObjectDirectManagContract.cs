namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Договор непосредственного управления жилым домом
    /// </summary>
    public class RealityObjectDirectManagContract : ManOrgBaseContract
    {
        /// <summary>
        /// Нехранимое поле, идентификатор жилого дома
        /// </summary>
        public virtual long RealityObjectId { get; set; }

        /// <summary>
        /// Договор оказания услуг
        /// </summary>
        public virtual bool IsServiceContract { get; set; }

        /// <summary>
        /// Дата начала оказания услуг
        /// </summary>
        public virtual DateTime? DateStartService { get; set; }

        /// <summary>
        /// Дата окончания оказания услуг
        /// </summary>
        public virtual DateTime? DateEndService { get; set; }

        /// <summary>
        /// Файл договора
        /// </summary>
        public virtual FileInfo ServContractFile { get; set; }
    }
}