namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014111700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014111700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014111601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NOTICE_DATE_PROTOCOL", DbType.DateTime));
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NOTICE_TIME_PROTOCOL", DbType.DateTime));
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NOTICE_PLACE_CREATION", DbType.String, 500));
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NOTICE_DESCRIPTION", DbType.String, 500));

            this.Database.RemoveTable("GJI_NSO_DISP_NOTICE_LTEXT");
            this.Database.RemoveTable("GJI_NSO_DISP_NOTICE");

            this.Database.AddEntityTable(
                "GJI_NSO_DISP_LTEXT",
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_NSO_DISP_LTEXT_D", "GJI_NSO_DISPOSAL", "ID"),
                new Column("NOTICE_DESCRIPTION", DbType.Binary, ColumnProperty.Null));

        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NOTICE_DATE_PROTOCOL");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NOTICE_TIME_PROTOCOL");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NOTICE_PLACE_CREATION");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NOTICE_DESCRIPTION");

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

            this.Database.RemoveTable("GJI_NSO_DISP_LTEXT");
        }
    }
}