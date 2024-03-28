namespace Bars.Gkh.Migrations.Version_2014121902
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121902")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014121901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddColumn("GKH_PERSON", new Column("ADDRESS_BIRTH", DbType.String, 2000, ColumnProperty.Null));
			Database.AddColumn("GKH_PERSON", new Column("BIRTHDATE", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
			Database.RemoveColumn("GKH_PERSON", "ADDRESS_BIRTH");
			Database.RemoveColumn("GKH_PERSON", "BIRTHDATE");
        }
    }
}