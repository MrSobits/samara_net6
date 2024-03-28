namespace Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Договор между поставщиком газового оборудрования (ВДГО) и жилым домом
    /// </summary>
    public class GasEquipmentOrgRealityObj : BaseGkhEntity
    {
        /// <summary>
        /// Роль контрагента ВДГО (внутридомовое газовое оборудование)
        /// </summary>
        public virtual GasEquipmentOrg GasEquipmentOrg { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дата начала предоставления услуги
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания предоставления услуги 
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}