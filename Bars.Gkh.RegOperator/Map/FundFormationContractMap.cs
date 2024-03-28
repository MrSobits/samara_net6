/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class FundFormationContractMap : BaseImportableEntityMap<FundFormationContract>
///     {
///         public FundFormationContractMap()
///             : base("OVRHL_FUND_FORMAT_CONTR")
///         {
///             Map(x => x.ContractNumber, "CONTRACT_NUMBER", false, 100);
///             Map(x => x.ContractDate, "CONTRACT_DATE");
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.TypeContract, "TYPE_CONTRACT", true);
///             References(x => x.LongTermPrObject, "LONG_TERM_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RegOperator, "REG_OPER_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Реестр договоров на формирование фонда капитального ремонта"</summary>
    public class FundFormationContractMap : BaseImportableEntityMap<FundFormationContract>
    {
        
        public FundFormationContractMap() : 
                base("Реестр договоров на формирование фонда капитального ремонта", "OVRHL_FUND_FORMAT_CONTR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LongTermPrObject, "Объект капитального ремонта").Column("LONG_TERM_OBJ_ID").NotNull().Fetch();
            Reference(x => x.RegOperator, "Региональный оператор").Column("REG_OPER_ID").NotNull().Fetch();
            Property(x => x.TypeContract, "Тип договора").Column("TYPE_CONTRACT").NotNull();
            Property(x => x.ContractNumber, "Номер").Column("CONTRACT_NUMBER").Length(100);
            Property(x => x.ContractDate, "Дата договора").Column("CONTRACT_DATE");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
