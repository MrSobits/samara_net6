namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020120700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020120700")]
    [MigrationDependsOn(typeof(Version_2020120600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GJI_SMEV_PREMISES",
            new RefColumn("ANS_FILE_ID", ColumnProperty.Null, "GJI_PREM_FILE_ID", "B4_FILE_INFO", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("ACT_NAME", DbType.String, 500),
            new Column("ACT_DEPARTMENT", DbType.String, 500),
            new Column("ACT_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("ACT_NUMBER", DbType.String, 50),
            new Column("MESSAGE_ID", DbType.String, 500),
            new Column("ANSWER", DbType.String, 500),
            new Column("OKTMO", DbType.String, 50),
            new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_SMEV_PREM_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
            "GJI_SMEV_PREMISES_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_PREMISES_ID", ColumnProperty.None, "GJI_SMEV_PREMISES_ID", "GJI_SMEV_PREMISES", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_SMEV_PREMISES_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_SMEV_PREMISES_FILE");
            Database.RemoveTable("GJI_SMEV_PREMISES");
        }
    }
}
