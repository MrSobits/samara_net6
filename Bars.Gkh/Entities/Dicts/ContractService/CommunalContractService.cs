namespace Bars.Gkh.Entities.Dicts.ContractService
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Коммунальная услуга
    /// </summary>
    public class CommunalContractService : ManagementContractService
    {
        /// <summary>
        /// Коммунальный ресурс
        /// </summary>
        public virtual TypeCommunalResource CommunalResource { get; set; }

        /// <summary>
        /// Порядок сортировки 
        /// </summary>
        public virtual int SortOrder { get; set; }

        /// <summary>
        /// Услуги предоставляется на ОДН (общедомовые нужды)
        /// </summary>
        public virtual bool IsHouseNeeds { get; set; }
    }
}
