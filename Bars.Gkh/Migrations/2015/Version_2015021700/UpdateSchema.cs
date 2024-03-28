namespace Bars.Gkh.Migrations.Version_2015021700
{
	using System.Data;

	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

	using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015021700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015021600.UpdateSchema))]
	public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
	{
		public override void Up()
		{
			Database.AddColumn("GKH_CONTRAGENT", new Column("TAX_REGISTRATION_SERIES", DbType.String, 100, ColumnProperty.Null));
			Database.AddColumn("GKH_CONTRAGENT", new Column("TAX_REGISTRATION_NUMBER", DbType.String, 300, ColumnProperty.Null));
			Database.AddColumn("GKH_CONTRAGENT", new Column("TAX_REGISTRATION_ISSUED_BY", DbType.String, 300, ColumnProperty.Null));
			Database.AddColumn("GKH_CONTRAGENT", new Column("TAX_REGISTRATION_DATE", DbType.Date, ColumnProperty.Null));
		}

		public override void Down()
		{
			Database.RemoveColumn("GKH_CONTRAGENT", "TAX_REGISTRATION_SERIES");
			Database.RemoveColumn("GKH_CONTRAGENT", "TAX_REGISTRATION_NUMBER");
			Database.RemoveColumn("GKH_CONTRAGENT", "TAX_REGISTRATION_ISSUED_BY");
			Database.RemoveColumn("GKH_CONTRAGENT", "TAX_REGISTRATION_DATE");
		}
	}
}