/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CompetitionLotMap : BaseImportableEntityMap<CompetitionLot>
///     {
///         public CompetitionLotMap() : base("CR_COMPETITION_LOT")
///         {
///             References(x => x.Competition, "COMPETITION_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.LotNumber, "LOT_NUMBER", true);
///             Map(x => x.StartingPrice, "STARTING_PRICE", true);
///             Map(x => x.Subject, "SUBJECT", false, 500);
/// 
///             //вкладка "договор"
///             Map(x => x.ContractNumber, "CONTRACT_NUMBER", false, 100);
///             Map(x => x.ContractDate, "CONTRACT_DATE", false);
///             Map(x => x.ContractFactPrice, "CONTRACT_FACT_PRICE", false);
///             References(x => x.ContractFile, "CONTRACT_FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Лот конкурса"</summary>
    public class CompetitionLotMap : BaseImportableEntityMap<CompetitionLot>
    {
        
        public CompetitionLotMap() : 
                base("Лот конкурса", "CR_COMPETITION_LOT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Competition, "Конкурс").Column("COMPETITION_ID").NotNull().Fetch();
            Property(x => x.LotNumber, "Номер лота").Column("LOT_NUMBER").NotNull();
            Property(x => x.StartingPrice, "Начальная цена").Column("STARTING_PRICE").NotNull();
            Property(x => x.Subject, "Предмет договора").Column("SUBJECT").Length(500);
            Property(x => x.ContractNumber, "Номер договора").Column("CONTRACT_NUMBER").Length(100);
            Property(x => x.ContractDate, "Дата договора").Column("CONTRACT_DATE");
            Property(x => x.ContractFactPrice, "Фактическая цена договора").Column("CONTRACT_FACT_PRICE");
            Reference(x => x.ContractFile, "Файд договора").Column("CONTRACT_FILE_ID").Fetch();
        }
    }
}
