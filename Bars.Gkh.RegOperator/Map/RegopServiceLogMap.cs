/// <mapping-converter-backup>
/// using Bars.Gkh.RegOperator.Entities.Dict;
/// 
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
/// 
///     public class RegopServiceLogMap : BaseImportableEntityMap<RegopServiceLog>
///     {
///         public RegopServiceLogMap()
///             : base("REGOP_SERVICE_LOG")
///         {
///             Map(x => x.CashPayCenterName, "CASH_PAY_CENTER_NAME");
///             Map(x => x.DateExecute, "DATE_EXECUTE");
///             Map(x => x.FileNum, "FILE_NUM");
///             Map(x => x.FileDate, "FILE_DATE");
///             Map(x => x.MethodType, "METHOD_TYPE");
///             Map(x => x.Status, "STATUS", true, false);
/// 
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Справочник расчетов пеней"</summary>
    public class RegopServiceLogMap : BaseImportableEntityMap<RegopServiceLog>
    {
        
        public RegopServiceLogMap() : 
                base("Справочник расчетов пеней", "REGOP_SERVICE_LOG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CashPayCenterName, "Наименование РКЦ").Column("CASH_PAY_CENTER_NAME").Length(250);
            Property(x => x.DateExecute, "Время").Column("DATE_EXECUTE");
            Property(x => x.FileNum, "Номер файла").Column("FILE_NUM").Length(250);
            Property(x => x.FileDate, "Дата файла").Column("FILE_DATE");
            Property(x => x.MethodType, "Название метода").Column("METHOD_TYPE");
            Property(x => x.Status, "Успешно - если все проверки успешны, Безуспешно - если какое либо из правил не ус" +
                    "пешен").Column("STATUS").DefaultValue(false).NotNull();
            Reference(x => x.File, "Файл лога").Column("FILE_ID").NotNull().Fetch();
        }
    }
}
