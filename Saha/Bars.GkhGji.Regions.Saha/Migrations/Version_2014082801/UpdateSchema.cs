namespace Bars.GkhGji.Regions.Saha.Migrations.Version_2014082800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014082800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Saha.Migrations.Version_2014080601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Определение протокола для САХИ как subclass
            Database.AddTable(
                "SAHA_GJI_PROTOCOLDEF",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TIME_END", DbType.DateTime),
                new Column("TIME_START", DbType.DateTime),
                new Column("FILE_DESC_ID", DbType.Int64, 22));
            Database.AddIndex("IND_SAHA_GJI_PROTOCOLDEF_ID", false, "SAHA_GJI_PROTOCOLDEF", "ID");
            Database.AddForeignKey("FK_SAHA_GJI_PROTOCOLDEF_ID", "SAHA_GJI_PROTOCOLDEF", "ID", "GJI_PROTOCOL_DEFINITION", "ID");

            Database.AddIndex("IND_SAHA_GJI_PROTOCOLDEF_FD", false, "SAHA_GJI_PROTOCOLDEF", "FILE_DESC_ID");
            Database.AddForeignKey("FK_SAHA_GJI_PROTOCOLDEF_FD", "SAHA_GJI_PROTOCOLDEF", "FILE_DESC_ID", "B4_FILE_INFO", "ID");

            // делаем sql-скрипт чтобы сразу создать в новой таблицы записи для тех которые уже имеются в БД 
            Database.ExecuteNonQuery(@"insert into SAHA_GJI_PROTOCOLDEF (id)
                                     select id from GJI_PROTOCOL_DEFINITION");

            // Определение постановления для САХИ как subclass
            Database.AddTable(
                "SAHA_GJI_RESOLUTIONDEF",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TIME_END", DbType.DateTime),
                new Column("TIME_START", DbType.DateTime),
                new Column("FILE_DESC_ID", DbType.Int64, 22));
            Database.AddIndex("IND_SAHA_GJI_RESOLUTIONDEF_ID", false, "SAHA_GJI_RESOLUTIONDEF", "ID");
            Database.AddForeignKey("FK_SAHA_GJI_RESOLUTIONDEF_ID", "SAHA_GJI_RESOLUTIONDEF", "ID", "GJI_RESOLUTION_DEFINITION", "ID");

            Database.AddIndex("IND_SAHA_GJI_RESOLUTIONDEF_FD", false, "SAHA_GJI_RESOLUTIONDEF", "FILE_DESC_ID");
            Database.AddForeignKey("FK_SAHA_GJI_RESOLUTIONDEF_FD", "SAHA_GJI_RESOLUTIONDEF", "FILE_DESC_ID", "B4_FILE_INFO", "ID");

            // делаем sql-скрипт чтобы сразу создать в новой таблицы записи для тех которые уже имеются в БД 
            Database.ExecuteNonQuery(@"insert into SAHA_GJI_RESOLUTIONDEF (id)
                                     select id from GJI_RESOLUTION_DEFINITION");
        }

        public override void Down()
        {
            Database.RemoveConstraint("SAHA_GJI_PROTOCOLDEF", "FK_SAHA_GJI_PROTOCOLDEF_ID");
            Database.RemoveConstraint("SAHA_GJI_PROTOCOLDEF", "FK_SAHA_GJI_PROTOCOLDEF_FD");
            Database.RemoveTable("SAHA_GJI_PROTOCOLDEF");

            Database.RemoveConstraint("SAHA_GJI_RESOLUTIONDEF", "FK_SAHA_GJI_RESOLUTIONDEF_ID");
            Database.RemoveConstraint("SAHA_GJI_RESOLUTIONDEF", "FK_SAHA_GJI_RESOLUTIONDEF_FD");
            Database.RemoveTable("SAHA_GJI_RESOLUTIONDEF");
        }
    }
}