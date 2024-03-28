namespace Bars.Gkh.Entities.ManOrg
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class ManagingOrgRegistry: BaseImportableEntity
    {

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Дата предоставления сведений
        /// </summary>
        public virtual DateTime InfoDate { get; set; }

        /// <summary>
        /// Тип сведений
        /// </summary>
        public virtual TsjInfoType InfoType { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        public virtual string RegNumber { get; set; }

        /// <summary>
        /// Дата внесения записи в ЕГРЮЛ
        /// </summary>
        public virtual DateTime? EgrulDate { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo Doc { get; set; }
    }
}
