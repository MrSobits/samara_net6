namespace Bars.GkhGji.Migrations._2020.Version_2020042000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020042000")]
    [MigrationDependsOn(typeof(Version_2020041600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_EDS_INSPECTION",
                new Column("INSPECTION_NUMBER", DbType.String),
                new Column("INSPECTION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("IS_NOT_READ", DbType.Boolean, true),
                new Column("TYPE_BASE", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("INSPECTION_ID", "FK_EDS_INSPECTION", "GJI_INSPECTION", "ID"),
                new RefColumn("CONTRAGENT_ID", "FK_EDS_CONTRAGENT", "GKH_CONTRAGENT", "ID"));

            Database.AddColumn("GJI_ACTCHECK_ANNEX", new Column("ANNEX_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_DISPOSAL_ANNEX", new Column("ANNEX_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_PRESCRIPTION_ANNEX", new Column("ANNEX_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_PROTOCOL_ANNEX", new Column("ANNEX_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));  
            Database.AddColumn("GJI_RESOLUTION_ANNEX", new Column("ANNEX_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION_ANNEX", "ANNEX_TYPE");         
            Database.RemoveColumn("GJI_PROTOCOL_ANNEX", "ANNEX_TYPE");
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "ANNEX_TYPE");
            Database.RemoveColumn("GJI_DISPOSAL_ANNEX", "ANNEX_TYPE");
            Database.RemoveColumn("GJI_ACTCHECK_ANNEX", "ANNEX_TYPE");
            this.Database.RemoveTable("GJI_EDS_INSPECTION");
        }
    }
}