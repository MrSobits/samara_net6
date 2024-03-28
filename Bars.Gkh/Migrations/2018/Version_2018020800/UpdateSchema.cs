namespace Bars.Gkh.Migrations._2018.Version_2018020800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018020800")]
    [MigrationDependsOn(typeof(Version_2018011900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("AREA_FEDERAL_OWNED", DbType.Decimal));
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("AREA_COMMERCIAL_OWNED", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "AREA_FEDERAL_OWNED");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "AREA_COMMERCIAL_OWNED");
        }
    }
}