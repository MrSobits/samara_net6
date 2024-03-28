namespace Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Роль контрагента ВДГО (внутридомовое газовое оборудование)
    /// </summary>
    public class GasEquipmentOrg : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
        
        /// <summary>
        /// Контакт организации
        /// </summary>
        public virtual ContragentContact Contact { get; set; }
    }
}