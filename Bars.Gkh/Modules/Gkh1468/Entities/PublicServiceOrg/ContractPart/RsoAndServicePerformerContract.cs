namespace Bars.Gkh.Modules.Gkh1468.Entities.ContractPart
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Стороны договора: РСО и исполнитель коммунальных услуг
    /// </summary>
    public class RsoAndServicePerformerContract : BaseContractPart
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Коммерческий учет ресурса осуществляет
        /// </summary>
        public virtual CommercialMeteringResourceType CommercialMeteringResourceType { get; set; }
    }
}
