/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// 
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Контрагент"
///     /// </summary>
///     public class PersonDisqualificationInfoMap : BaseImportableEntityMap<PersonDisqualificationInfo>
///     {
///         public PersonDisqualificationInfoMap()
///             : base("GKH_PERSON_DISQUAL")
///         {
///             Map(x => x.TypeDisqualification, "DISQ_TYPE").Not.Nullable().CustomType<TypePersonDisqualification>();
/// 
///             Map(x => x.DisqDate, "DISQ_DATE");
///             Map(x => x.EndDisqDate, "DISQ_END_DATE");
///             Map(x => x.PetitionDate, "PETITION_DATE");
///             Map(x => x.PetitionNumber, "PETITION_NUMBER").Length(100);
/// 
///             References(x => x.Person, "PERSON_ID").Not.Nullable().Fetch.Join();
///             References(x => x.PetitionFile, "PETITION_FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Сведения о дисквалификации"</summary>
    public class PersonDisqualificationInfoMap : BaseImportableEntityMap<PersonDisqualificationInfo>
    {
        
        public PersonDisqualificationInfoMap() : 
                base("Сведения о дисквалификации", "GKH_PERSON_DISQUAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeDisqualification, "Основание дисвалификации").Column("DISQ_TYPE").NotNull();
            Property(x => x.DisqDate, "Дата дисквалификации").Column("DISQ_DATE");
            Property(x => x.EndDisqDate, "Дата окончания").Column("DISQ_END_DATE");
            Property(x => x.PetitionDate, "Ходотайство - дата").Column("PETITION_DATE");
            Property(x => x.PetitionNumber, "Ходотайство - номер").Column("PETITION_NUMBER").Length(100);
            Property(x => x.NameOfCourt, "Наименование суда").Column("NAME_OF_COURT");
            Reference(x => x.Person, "Person").Column("PERSON_ID").NotNull().Fetch();
            Reference(x => x.PetitionFile, "Ходотайство - файл").Column("PETITION_FILE_ID").Fetch();
            
        }
    }
}
