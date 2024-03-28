namespace Bars.Gkh.Map.Dict.ContractService
{
    using Bars.Gkh.Entities.Dicts.ContractService;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для "Услуги по договорам управления"
    /// </summary>
    public class ManagementContractServiceMap : BaseImportableEntityMap<ManagementContractService>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ManagementContractServiceMap()
            : base("GKH_DICT_MAN_CONTRACT_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ExportId, "ExportId").Column("EXPORT_ID").NotNull();
            this.Property(x => x.Code, "Код").Column("CODE").NotNull().Length(255);
            this.Property(x => x.ServiceType, "Вид услуги").Column("SERVICE_TYPE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull().Length(255);
            this.Reference(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE_ID").NotNull().Fetch();
        }
    }

    /// <summary>ReadOnly ExportId</summary>
    public class ManagementContractServiceNhMapping : BaseHaveExportIdMapping<ManagementContractService>
    {
    }
}
