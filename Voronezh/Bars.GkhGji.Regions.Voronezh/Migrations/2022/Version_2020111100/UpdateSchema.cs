namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020111100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020111100")]
    [MigrationDependsOn(typeof(Version_2020102200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_APPCIT_RESOLUTION",
              new Column("RESOLUTION_TEXT", DbType.String, 200),
              new Column("RESOLUTION_TERM", DbType.DateTime),
              new Column("RESOLUTION_AUTHOR", DbType.String, 200),
              new Column("RESOLUTION_DATE", DbType.DateTime),
              new Column("CONTENT", DbType.String),
              new Column("IMPORT_ID", DbType.String, 20),
              new Column("PARENT_ID", DbType.String, 20),
            new RefColumn("APPCIT_ID", ColumnProperty.NotNull, "GJI_CH_APPEAL_CIT_ID", "GJI_APPEAL_CITIZENS", "ID"));

            Database.AddEntityTable(
                "GJI_APPCIT_RESOLUTION_EXECUTOR",
                new Column("NAME", DbType.String, 50),
                new Column("SURNAME", DbType.String, 50),
                new Column("PATRONYMIC", DbType.String, 50),
                new Column("PERSONAL_TERM", DbType.DateTime),
                new Column("COMMENT", DbType.String),
                new Column("IS_RESPONSIBLE", DbType.Int32),
                new RefColumn("RESOLUTION_ID", ColumnProperty.None, "GJI_CH_APPCIT_RESOLUTION_ID", "GJI_APPCIT_RESOLUTION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_APPCIT_RESOLUTION_EXECUTOR");
            Database.RemoveTable("GJI_APPCIT_RESOLUTION"); 
        }
    }
}


