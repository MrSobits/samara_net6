namespace Bars.GkhGji.Migrations._2020.Version_2020022100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020022100")]
    [MigrationDependsOn(typeof(Version_2020021400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
          
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("REALITY_ADDRESSES", DbType.String, 500));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("INCOMING_SOURCES", DbType.String, 250));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("EXECUTORS", DbType.String, 800));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("TESTERS", DbType.String, 800));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "TESTERS");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "EXECUTORS");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "INCOMING_SOURCES");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "REALITY_ADDRESSES");
        }
    }
}