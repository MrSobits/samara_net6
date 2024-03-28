namespace Bars.Gkh.Gis.Map.ManOrg.Contract
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg.Contract;

    /// <summary>
    /// Маппинг для JskTsjContractWorkService
    /// </summary>
    public class JskTsjContractWorkServiceMap : BaseEntityMap<JskTsjContractWorkService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public JskTsjContractWorkServiceMap()
            : base("Работы и услуги договора управления ТСЖ/ЖСК", "JSKTSJ_CONTRACT_WORK_SERVICE")
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
