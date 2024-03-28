namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014121600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014111700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddColumn("GJI_NSO_ACTCHECK", new Column("DOCUMENT_PLACE", DbType.String, 1000));
			Database.AddColumn("GJI_NSO_ACTCHECK", new Column("DOCUMENT_TIME", DbType.DateTime, 25));
        }

        public override void Down()
        {
			Database.RemoveColumn("GJI_NSO_ACTCHECK", "DOCUMENT_PLACE");
			Database.RemoveColumn("GJI_NSO_ACTCHECK", "DOCUMENT_TIME");
        }
    }
}