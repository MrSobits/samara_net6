namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг для Технических заказчиков 
    /// </summary>
    public class TechnicalCustomerMap : BaseImportableEntityMap<TechnicalCustomer>
    {
        public TechnicalCustomerMap()
            :base("Технические заказчики", "GKH_TECHNICAL_CUSTOMER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}