namespace Bars.Gkh.Map.RealityObj
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.Gkh.Entities.RealityObj;


	/// <summary>Маппинг для "Запись фиас-адреса c HouseGuid"</summary>
	public class FiasHouseMap : PersistentObjectMap<GkhFiasHouse>
	{

		public FiasHouseMap() :
				base("Запись фиас-адреса c HouseGuid", "GKH_FIAS_HOUSE")
		{
		}

		protected override void Map()
		{
			Property(x => x.PostalCode, "POSTALCODE").Column("POSTALCODE").Length(6);
			Property(x => x.Okato, "ОКАТО").Column("OKATO").Length(11);
			Property(x => x.Oktmo, "ОКТМО").Column("OKTMO").Length(15);
			Property(x => x.BuildNum, "BUILDNUM").Column("BUILDNUM").Length(5);
			Property(x => x.StrucNum, "STRUCNUM").Column("STRUCNUM").Length(5);
			Property(x => x.HouseNum, "HOUSENUM").Column("HOUSENUM").Length(10);
			Property(x => x.HouseId, "HOUSEID").Column("HOUSEID").Length(50);
			Property(x => x.HouseGuid, "HOUSEGUID").Column("HOUSEGUID").Length(50).NotNull();
			Property(x => x.AoGuid, "AOGUID").Column("AOGUID").Length(50);
		}
	}
}