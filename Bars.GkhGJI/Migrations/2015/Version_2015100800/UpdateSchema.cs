namespace Bars.GkhGji.Migrations._2015.Version_2015100800
{
	using System.Data;
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
	using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015100200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
				"GJI_DICT_ANNEX_APPEAL_LIC_ISS",
                new Column("NAME", DbType.String, 500),
                new Column("CODE", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_ANNEX_APPEAL_LIC_ISS");
        }
    }
}