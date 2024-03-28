namespace Bars.Gkh.Migrations.Version_2013113000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013113000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013112700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //жилой дом организации поставщика жил. услуг
            Database.AddEntityTable(
                "GKH_SERV_ORG_REAL_OBJ",
                new Column("SERV_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SORG_RO_SORG", false, "GKH_SERV_ORG_REAL_OBJ", "SERV_ORG_ID");
            Database.AddIndex("IND_GKH_SORG_RO_RO", false, "GKH_SERV_ORG_REAL_OBJ", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GKH_SORG_RO_SORG", "GKH_SERV_ORG_REAL_OBJ", "SERV_ORG_ID", "GKH_SERVICE_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GKH_SORG_RO_RO", "GKH_SERV_ORG_REAL_OBJ", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");

            //договор организации поставщика жил. услуг
            Database.AddEntityTable(
                "GKH_SORG_CONTRACT",
                new Column("SERV_ORG_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("FILE_INFO_ID", DbType.Int64, 22),
                new Column("NOTE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SORG_CN_SORG", false, "GKH_SORG_CONTRACT", "SERV_ORG_ID");
            Database.AddIndex("IND_GKH_SORG_CN_FILE", false, "GKH_SORG_CONTRACT", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GKH_SORG_CN_SORG", "GKH_SORG_CONTRACT", "SERV_ORG_ID", "GKH_SERVICE_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GKH_SORG_CN_FILE", "GKH_SORG_CONTRACT", "FILE_INFO_ID", "B4_FILE_INFO", "ID");

            //жилой дом договора организации поставщика жил. услуг
            Database.AddEntityTable(
                "GKH_SORG_REALOBJ_CONTRACT",
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("SERV_ORG_CONTRACT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SORG_CN_RO_CON", false, "GKH_SORG_REALOBJ_CONTRACT", "SERV_ORG_CONTRACT_ID");
            Database.AddIndex("IND_GKH_SORG_CN_RO_RO", false, "GKH_SORG_REALOBJ_CONTRACT", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_GKH_SORG_CN_RO_CON", "GKH_SORG_REALOBJ_CONTRACT", "SERV_ORG_CONTRACT_ID", "GKH_SORG_CONTRACT", "ID");
            Database.AddForeignKey("FK_GKH_SORG_CN_RO_RO", "GKH_SORG_REALOBJ_CONTRACT", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");

        }

        public override void Down()
        {
            Database.RemoveConstraint("GKH_SORG_CONTRACT", "FK_GKH_SORG_CN_SORG");
            Database.RemoveConstraint("GKH_SORG_CONTRACT", "FK_GKH_SORG_CN_FILE");
            Database.RemoveConstraint("GKH_SORG_REALOBJ_CONTRACT", "FK_GKH_SORG_CN_RO_CON");
            Database.RemoveConstraint("GKH_SORG_REALOBJ_CONTRACT", "FK_GKH_SORG_CN_RO_RO");
            Database.RemoveConstraint("GKH_SERV_ORG_REAL_OBJ", "FK_GKH_SORG_RO_RO");
            Database.RemoveConstraint("GKH_SERV_ORG_REAL_OBJ", "FK_GKH_SORG_RO_SORG");

            Database.RemoveTable("GKH_SERV_ORG_REAL_OBJ");
            Database.RemoveTable("GKH_SORG_CONTRACT");
            Database.RemoveTable("GKH_SORG_REALOBJ_CONTRACT");
        }
    }
}