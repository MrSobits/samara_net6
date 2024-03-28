namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014051600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014051600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014050800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("DATE_STATEMENT", DbType.DateTime));
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("TIME_STATEMENT", DbType.DateTime, 25));

            this.Database.AddRefColumn("GJI_NSO_DISPOSAL", new RefColumn("POLITIC_ID", "GJI_NSO_DISPOSAL_P", "GKH_POLITIC_AUTHORITY", "ID"));

            this.Database.AddEntityTable(
                "GJI_NSO_DISP_DOCCONFIRM",
                new Column("DOC_NAME", DbType.String, 300),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_NSO_DISP_DOCCONFIRM_D", "GJI_DISPOSAL", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "DATE_STATEMENT");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "TIME_STATEMENT");

            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "POLITIC_ID");

            this.Database.RemoveTable("GJI_NSO_DISP_DOCCONFIRM");
        }
    }
}