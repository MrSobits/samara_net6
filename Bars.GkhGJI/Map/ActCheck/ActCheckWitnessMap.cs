/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Лица, присутсвующие при проверке (они же свидетели)"
///     /// </summary>
///     public class ActCheckWitnessMap : BaseGkhEntityMap<ActCheckWitness>
///     {
///         public ActCheckWitnessMap() : base("GJI_ACTCHECK_WITNESS")
///         {
///             Map(x => x.Fio, "FIO").Length(300).Not.Nullable();
///             Map(x => x.Position, "POSITION").Length(300);
///             Map(x => x.IsFamiliar, "IS_FAMILIAR");
/// 
///             References(x => x.ActCheck, "ACTCHECK_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Лица, присутствующие при проверке (или свидетели)"</summary>
    public class ActCheckWitnessMap : BaseEntityMap<ActCheckWitness>
    {
        
        public ActCheckWitnessMap() : 
                base("Лица, присутствующие при проверке (или свидетели)", "GJI_ACTCHECK_WITNESS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Fio, "ФИО").Column("FIO").Length(300).NotNull();
            Property(x => x.Position, "Должность").Column("POSITION").Length(300);
            Property(x => x.IsFamiliar, "С актом ознакомлен").Column("IS_FAMILIAR");
            Reference(x => x.ActCheck, "Акт проверки").Column("ACTCHECK_ID").NotNull().Fetch();
            this.Property(x => x.ErpGuid, nameof(ActCheckViolation.ErpGuid)).Column("ERP_GUID");
        }
    }
}
