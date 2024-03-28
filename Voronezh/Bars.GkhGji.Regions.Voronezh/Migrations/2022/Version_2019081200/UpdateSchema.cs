namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019081200
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019081200")]
    [MigrationDependsOn(typeof(Version_2019080500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("ACT_DATE_CREATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("DURATION_HOURS", DbType.Int16, ColumnProperty.None));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("HAS_VIOLATIONS", DbType.Int32, 4, ColumnProperty.NotNull, 30));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("NEED_TO_UPDATE", DbType.Int32, 4, ColumnProperty.NotNull, 20));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("REPRESENTATIVE_FULL_NAME", DbType.String));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("REPRESENTATIVE_POSITION", DbType.String));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("START_DATE", DbType.DateTime, ColumnProperty.None));

            Database.AddEntityTable("GJI_CH_GIS_ERP_VIOLATION",
               new Column("CODE", DbType.String, ColumnProperty.None),
               new Column("DATE_APPOINTMENT", DbType.DateTime, ColumnProperty.None),
               new Column("EXECUTION_DEADLINE", DbType.DateTime, ColumnProperty.None),
               new Column("EXECUTION_NOTE", DbType.String, ColumnProperty.None),
               new Column("NUM_GUID", DbType.String, ColumnProperty.None),
               new Column("TEXT", DbType.String, ColumnProperty.None),
               new Column("VIOLATION_ACT", DbType.String, ColumnProperty.None),
               new Column("VIOLATION_NOTE", DbType.String, ColumnProperty.None),
               new Column("VLAWSUIT_TYPE_ID", DbType.Int32, 4, ColumnProperty.NotNull, 30),
               new RefColumn("GIS_ERP_ID", ColumnProperty.None, "GJI_CH_GIS_ERP_VIOLAT_ERP_ID", "GJI_CH_GIS_ERP", "ID"));
        }

        public override void Down()
        {

            Database.RemoveTable("GJI_CH_GIS_ERP_VIOLATION");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "START_DATE");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "REPRESENTATIVE_POSITION");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "REPRESENTATIVE_FULL_NAME");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "NEED_TO_UPDATE");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "HAS_VIOLATIONS");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "DURATION_HOURS");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "ACT_DATE_CREATE");
        }
    }
}