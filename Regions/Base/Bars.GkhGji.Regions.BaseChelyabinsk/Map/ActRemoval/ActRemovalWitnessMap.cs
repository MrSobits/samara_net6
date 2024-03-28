namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActRemoval
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    /// <summary>Маппинг для "Лица, присутствующие при проверке (или свидетели)"</summary>
    public class ActRemovalWitnessMap : BaseEntityMap<ActRemovalWitness>
    {
        
        public ActRemovalWitnessMap() : 
                base("Лица, присутствующие при проверке (или свидетели)", "GJI_NSO_ACTREMOVAL_WITNESS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Fio, "ФИО").Column("FIO").Length(300).NotNull();
            this.Property(x => x.Position, "Должность").Column("POSITION").Length(300);
            this.Property(x => x.IsFamiliar, "С актом ознакомлен").Column("IS_FAMILIAR");
            this.Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
        }
    }
}
