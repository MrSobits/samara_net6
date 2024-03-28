
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Лица, присутствующие при проверке (или свидетели)"</summary>
    public class PreventiveVisitWitnessMap : BaseEntityMap<PreventiveVisitWitness>
    {
        
        public PreventiveVisitWitnessMap() : 
                base("Лица, присутствующие при проверке (или свидетели)", "GJI_PREVENTIVE_VISIT_WITNESS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Fio, "ФИО").Column("FIO").Length(300).NotNull();
            Property(x => x.Position, "Должность").Column("POSITION").Length(300);
            Property(x => x.IsFamiliar, "С актом ознакомлен").Column("IS_FAMILIAR");
            Reference(x => x.PreventiveVisit, "Акт проф визита").Column("PREVENT_VISIT_ID").NotNull().Fetch();
        }
    }
}
