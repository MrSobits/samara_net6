namespace Bars.Gkh.Gis.Map.ManOrg.Contract
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg.Contract;

    /// <summary>
    /// Маппинг для TransferContractWorkService
    /// </summary>
    public class TransferContractWorkServiceMap : BaseEntityMap<TransferContractWorkService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public TransferContractWorkServiceMap()
            : base("Работы и услуги договора передачи управления УК", "TRANSFER_CONTRACT_WORK_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Contract, "Договор управления").Column("CONTRACT_ID").NotNull().Fetch();
            this.Reference(x => x.WorkService, "Работы и услуги").Column("WORK_SERVICE_ID").NotNull().Fetch();
            this.Property(x => x.PaymentAmount, "Размер платы").Column("PAYMENT_AMOUNT");
        }
    }
}
