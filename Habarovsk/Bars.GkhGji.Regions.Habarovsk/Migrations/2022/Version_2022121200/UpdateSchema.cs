namespace Bars.GkhGji.Regions.Habarovsk.Migrations.Version_2022121200
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2022121200")]
    [MigrationDependsOn(typeof(Version_2022111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_PROTOCOL_OSP_REQUEST",
                new Column("FIO", DbType.String),
                new Column("ADDRESS", DbType.String),
                new Column("MUNICIPALITY", DbType.String),
                new RefColumn("RO_ID", "GJI_PROTOCOL_OSP_REQ_RO_ID", "GKH_REALITY_OBJECT", "ID"),
                new Column("RO_FIAS_GUID", DbType.String),
                new Column("USER_ESIA_GUID", DbType.String),
                new Column("DATE", DbType.DateTime),
                new Column("GJI_ID", DbType.String),
                new Column("APPROVED", DbType.Int16, 30),
                new Column("EMAIL", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PROTOCOL_OSP_REQUEST");
        }
    }
}