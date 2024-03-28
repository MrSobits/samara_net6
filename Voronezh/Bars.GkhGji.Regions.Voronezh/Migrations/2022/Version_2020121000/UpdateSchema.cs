namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020121000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020121000")]
    [MigrationDependsOn(typeof(Version_2020120700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GJI_CH_SMEV_DISKVLIC",
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("BIRTH_DATE", DbType.DateTime),
            new Column("BIRTH_PLACE", DbType.String, 500),
            new Column("DISQ_DAYS", DbType.String, 10),
            new Column("DISQ_MONTHS", DbType.String, 10),
            new Column("DISQ_YEARS", DbType.String, 10),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("END_DISQ_DATE", DbType.DateTime),
            new Column("MESSAGE_ID", DbType.String, 500),
            new Column("ANSWER", DbType.String, 500),
            new Column("FAMILY_NAME", DbType.String, 50),
            new Column("FIRST_NAME", DbType.String, 50),
            new Column("PATRONYMIC", DbType.String, 50),
            new Column("REQUEST_ID", DbType.String, 50),
            new Column("FORM_DATE", DbType.DateTime),
            new Column("REG_NUMBER", DbType.String, 50),
            new Column("ARTICLE", DbType.String, 500),
            new Column("LAW_DATE", DbType.DateTime),
            new Column("CASE_NUMBER", DbType.String, 50),
            new Column("LAW_NAME", DbType.String, 50),
            new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_SMEV_DISKVLIC_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
            "GJI_CH_SMEV_DISKVLIC_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_DISKVLIC_ID", ColumnProperty.None, "GJI_SMEV_DISKVLIC_ID", "GJI_CH_SMEV_DISKVLIC", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_SMEV_DISKVLIC_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_DISKVLIC_FILE");
            Database.RemoveTable("GJI_CH_SMEV_DISKVLIC");
        }
    }
}
