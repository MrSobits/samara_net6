namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Regions.Nso.Entities;

	/// <summary>Маппинг для "Лица, присутствующие при проверке (или свидетели)"</summary>
    public class ActRemovalWitnessMap : BaseEntityMap<ActRemovalWitness>
    {
        
        public ActRemovalWitnessMap() : 
                base("Лица, присутствующие при проверке (или свидетели)", "GJI_NSO_ACTREMOVAL_WITNESS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Fio, "ФИО").Column("FIO").Length(300).NotNull();
            Property(x => x.Position, "Должность").Column("POSITION").Length(300);
            Property(x => x.IsFamiliar, "С актом ознакомлен").Column("IS_FAMILIAR");
            Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
        }
    }
}
