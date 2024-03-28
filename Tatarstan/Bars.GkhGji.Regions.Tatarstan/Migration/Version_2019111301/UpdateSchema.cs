namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019111301
{
	using System.Data;

	using Bars.B4.Modules.Ecm7.Framework;

	[Migration("2019111301")]
	[MigrationDependsOn(typeof(Version_2019111300.UpdateSchema))]
	public class UpdateSchema : Migration
	{
		public override void Up()
		{
			this.Database.RenameColumn("GJI_DICT_CONTROL_TYPES", "EXTERNAL_ID", "TOR_ID");
			this.Database.ExecuteNonQuery("ALTER TABLE GJI_DICT_CONTROL_TYPES ALTER COLUMN TOR_ID TYPE UUID USING TOR_ID::UUID");

			this.Database.AddColumn("GJI_TAT_DISPOSAL", new Column("TOR_ID", DbType.Guid));
			this.Database.AddColumn("GJI_TAT_DISPOSAL", new Column("CONTROL_CARD_TOR_ID", DbType.Guid));
		}

		public override void Down()
		{
			this.Database.RenameColumn("GJI_DICT_CONTROL_TYPES", "TOR_ID", "EXTERNAL_ID");
			this.Database.ChangeColumn("GJI_DICT_CONTROL_TYPES", new Column("EXTERNAL_ID", DbType.String.WithSize(300)));
			
			this.Database.RemoveColumn("GJI_TAT_DISPOSAL", "TOR_ID");
			this.Database.RemoveColumn("GJI_TAT_DISPOSAL", "CONTROL_CARD_TOR_ID");
		}
	}
}
