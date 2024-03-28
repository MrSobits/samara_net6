namespace Bars.B4.Modules.FIAS.Migrations.Version_2016072100
{
	using System.Data;
	using global::Bars.B4.Modules.Ecm7.Framework;
    using NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016072100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.FIAS.Migrations.Version_2014121500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddPersistentObjectTable("B4_FIAS_HOUSE",
				new Column("HOUSE_ID", DbType.Guid),
				new Column("HOUSE_GUID", DbType.Guid),
				new Column("AO_GUID", DbType.Guid, ColumnProperty.NotNull),
				new Column("POSTAL_CODE", DbType.String, 6),
				new Column("OKATO", DbType.String, 11),
				new Column("OKTMO", DbType.String, 11),
				new Column("HOUSE_NUM", DbType.String, 20),
				new Column("BUILD_NUM", DbType.String, 10),
				new Column("STRUC_NUM", DbType.String, 10),
				new Column("STAT_STATUS", DbType.Int32, 4),
				new Column("UPDATE_DATE", DbType.DateTime),
				new Column("TYPE_RECORD", DbType.Int32, 4, ColumnProperty.NotNull, 10)
                );
			
			Database.AddIndex("B4_FIAS_HOUSE_HOUSEID", false, "B4_FIAS_HOUSE", "HOUSE_ID");
			Database.AddIndex("B4_FIAS_HOUSE_HOUSEGUID", false, "B4_FIAS_HOUSE", "HOUSE_GUID");
			Database.AddIndex("B4_FIAS_HOUSE_AOGUID", false, "B4_FIAS_HOUSE", "AO_GUID");
			Database.AddIndex("B4_FIAS_HOUSE_POSTALCODE", false, "B4_FIAS_HOUSE", "POSTAL_CODE");

			Database.AddColumn("B4_FIAS_ADDRESS", "HOUSE_GUID", DbType.Guid);
			Database.AddIndex("B4_FIAS_ADDRESS_HOUSEGUID", false, "B4_FIAS_ADDRESS", "HOUSE_GUID");
		}

        public override void Down()
        {
			Database.RemoveTable("B4_FIAS_HOUSE");
			Database.RemoveColumn("B4_FIAS_ADDRESS", "HOUSE_GUID");
		}
    }
}