namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>
    /// Маппинг для "Лица, присутствующие при проверке (или свидетели) для акта без взаимодействия"
    /// </summary>
    public class ActIsolatedWitnessMap : BaseEntityMap<ActIsolatedWitness>
    {
        
        public ActIsolatedWitnessMap() : 
                base("Лица, присутствующие при проверке (или свидетели) для акта без взаимодействия", "GJI_ACTISOLATED_WITNESS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Fio, "ФИО").Column("FIO").Length(300);
            this.Property(x => x.Position, "Должность").Column("POSITION").Length(300);
            this.Property(x => x.IsFamiliar, "С актом ознакомлен").Column("IS_FAMILIAR");
            this.Reference(x => x.ActIsolated, "Акт без взаимодействия").Column("ACTISOLATED_ID").NotNull().Fetch();
        }
    }
}
