namespace Bars.GkhDi.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using Attributes;
    using Enums;

    public class HousingService : BaseService
    {
        /// <summary>
        /// Тип оказания услуги
        /// </summary>
        public virtual TypeOfProvisionServiceDi TypeOfProvisionService { get; set; }

        /// <summary>
        /// Периодичность выполнения
        /// </summary>
        public virtual PeriodicityTemplateService Periodicity { get; set; }

        /// <summary>
        /// Оборудование
        /// </summary>
        [OptionField("Оборудование")]
        public virtual EquipmentDi Equipment { get; set; }

        /// <summary>
        /// номер протокола
        /// </summary>
        public virtual string ProtocolNumber { get; set; }

        /// <summary>
        /// Протокол от
        /// </summary>
        public virtual DateTime? ProtocolFrom { get; set; }

        /// <summary>
        /// Протокол
        /// </summary>
        public virtual FileInfo Protocol { get; set; }
    }
}
