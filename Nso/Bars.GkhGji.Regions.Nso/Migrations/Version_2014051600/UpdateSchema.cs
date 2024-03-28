namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014051600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014051600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014050800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("DATE_STATEMENT", DbType.DateTime));
            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("TIME_STATEMENT", DbType.DateTime, 25));

            Database.AddRefColumn("GJI_NSO_DISPOSAL", new RefColumn("POLITIC_ID", "GJI_NSO_DISPOSAL_P", "GKH_POLITIC_AUTHORITY", "ID"));

            Database.AddEntityTable(
                "GJI_NSO_DISP_DOCCONFIRM",
                new Column("DOC_NAME", DbType.String, 300),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_NSO_DISP_DOCCONFIRM_D", "GJI_DISPOSAL", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "DATE_STATEMENT");
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "TIME_STATEMENT");

            Database.RemoveColumn("GJI_NSO_DISPOSAL", "POLITIC_ID");

            Database.RemoveTable("GJI_NSO_DISP_DOCCONFIRM");
        }
    }
}