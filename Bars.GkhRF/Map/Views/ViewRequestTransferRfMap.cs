/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhRf.Entities;
/// 
///     public class ViewRequestTransferRfMap : PersistentObjectMap<ViewRequestTransferRf>
///     {
///         public ViewRequestTransferRfMap()
///             : base("VIEW_RF_REQUEST_TRANSFER")
///         {
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.TypeProgramRequest, "TYPE_PROGRAM_REQUEST");
///             Map(x => x.TransferFundsCount, "TRANSFER_FUNDS_COUNT");
///             Map(x => x.TransferFundsSum, "TRANSFER_FUNDS_SUM");
///             Map(x => x.ManagingOrganizationName, "MANORG_NAME");
///             Map(x => x.MunicipalityName, "MUNICIPALITY_NAME");
///             Map(x => x.MunicipalityId, "MUNICIPALITY_ID");
///             Map(x => x.ContragentId, "CONTRAGENT_ID");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на Заявку на перечисление средств"</summary>
    public class ViewRequestTransferRfMap : PersistentObjectMap<ViewRequestTransferRf>
    {
        
        public ViewRequestTransferRfMap() : 
                base("Вьюха на Заявку на перечисление средств", "VIEW_RF_REQUEST_TRANSFER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM");
            Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            Property(x => x.TypeProgramRequest, "Тип программы заявки перечисления рег.фонда").Column("TYPE_PROGRAM_REQUEST");
            Property(x => x.TransferFundsCount, "Количество объектов").Column("TRANSFER_FUNDS_COUNT");
            Property(x => x.TransferFundsSum, "Итого сумма").Column("TRANSFER_FUNDS_SUM");
            Property(x => x.ManagingOrganizationName, "Управляющая организация").Column("MANORG_NAME");
            Property(x => x.MunicipalityName, "Муниципальное образование").Column("MUNICIPALITY_NAME");
            Property(x => x.MunicipalityId, "Идентификатор муниципального образования").Column("MUNICIPALITY_ID");
            Property(x => x.ContragentId, "Идентификатор контрагента").Column("CONTRAGENT_ID");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
