namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014111600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014111600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014102900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("PERIOD_CORRECT", DbType.String, 500));

            this.Database.AddEntityTable(
                "GJI_ACTCHECK_LTEXT",
                new RefColumn("ACTCHECK_ID", ColumnProperty.NotNull, "GJI_ACTCHECK_LTEXT_A", "GJI_NSO_ACTCHECK", "ID"),
                new Column("NOT_HAVE_VIOL", DbType.Binary, ColumnProperty.Null));

            this.Database.AddEntityTable(
                "GJI_PROTOCOL_LTEXT",
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GJI_PROTOCOL_LTEXT_P", "GJI_PROTOCOL", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));

            
            this.Database.AddEntityTable(
                "GJI_NSO_DISP_NOTICE",
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_NSO_DISP_NOTICE_D", "GJI_DISPOSAL", "ID"),
                new Column("DATE_PROTOCOL", DbType.DateTime),
                new Column("TIME_PROTOCOL", DbType.DateTime),
                new Column("PLACE_CREATION", DbType.String, 500),
                new Column("DESCRIPTION", DbType.String, 500));

            this.Database.AddEntityTable(
                "GJI_NSO_DISP_NOTICE_LTEXT",
                new RefColumn("NOTICE_ID", ColumnProperty.NotNull, "GJI_PROTOCOL_LTEXT_N", "GJI_NSO_DISP_NOTICE", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "PERIOD_CORRECT");

            this.Database.RemoveTable("GJI_ACTCHECK_LTEXT");
            this.Database.RemoveTable("GJI_PROTOCOL_LTEXT");
            
            this.Database.RemoveTable("GJI_NSO_DISP_NOTICE_LTEXT");
            this.Database.RemoveTable("GJI_NSO_DISP_NOTICE");
        }
    }
}