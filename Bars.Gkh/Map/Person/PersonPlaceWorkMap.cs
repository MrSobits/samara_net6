/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// 
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// МЕста работы физ лиц
///     /// </summary>
///     public class PersonPlaceWorkMap : BaseImportableEntityMap<PersonPlaceWork>
///     {
///         public PersonPlaceWorkMap()
///             : base("GKH_PERSON_PLACEWORK")
///         {
/// 
///             Map(x => x.StartDate, "START_DATE");
///             Map(x => x.EndDate, "END_DATE");
/// 
///             References(x => x.Person, "PERSON_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Position, "POSITION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "место работы физ лица"</summary>
    public class PersonPlaceWorkMap : BaseImportableEntityMap<PersonPlaceWork>
    {
        
        public PersonPlaceWorkMap() : 
                base("место работы физ лица", "GKH_PERSON_PLACEWORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.StartDate, "Дата начала").Column("START_DATE");
            Property(x => x.EndDate, "Дата окончания").Column("END_DATE");
            Reference(x => x.Person, "Person").Column("PERSON_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "УО, ставлю ссылку на контрагента на всякий случай если вдруг захотят нетолько по " +
                    "УО но и по подрядчикам заполнять места работы").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.FileInfo, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.Position, "Должность").Column("POSITION_ID").Fetch();
        }
    }
}
