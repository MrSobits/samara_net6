namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2022092800
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2022092800")]
    [MigrationDependsOn(typeof(Version_2022060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_VDGO_VIOLATORS",
                 new RefColumn("GJI_CONTRAGENT_ID", "GJI_VDGO_VIOLATORS_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                 new RefColumn("GJI_MIN_ORG_CONTRAGENT_ID", "GJI_VDGO_VIOLATORS_MIN_ORG_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                 new RefColumn("GJI_RO_ID", "GJI_VDGO_VIOLATORS_RO", "GKH_REALITY_OBJECT", "ID"),
                 new RefColumn("GJI_FILE_ID", "GJI_VDGO_VIOLATORS_FILE", "B4_FILE_INFO", "ID"),
                 new Column("NOTIFICATION_NUMBER", DbType.Int32),
                 new Column("NOTIFICATION_DATE", DbType.DateTime),
                 new Column("DATE_EXECUTION", DbType.DateTime),
                 new Column("FIO", DbType.String),
                 new Column("PHONE_NUMBER", DbType.String),
                 new Column("EMAIL", DbType.String),
                 new Column("DESCRIPTION", DbType.String),
                 new Column("MARK_OF_EXECUTION", DbType.Boolean));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_VDGO_VIOLATORS");
        }
    }
}