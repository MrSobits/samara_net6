namespace Bars.Gkh.Migrations._2019.Version_2019072300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019072300")]
    [MigrationDependsOn(typeof(Version_2019071100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("NUMBER_GIS_GKH_MATCHED_APARTMENTS", DbType.Int32));
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("NUMBER_GIS_GKH_MATCHED_NON_RESIDENTAL", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "NUMBER_GIS_GKH_MATCHED_NON_RESIDENTAL");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "NUMBER_GIS_GKH_MATCHED_APARTMENTS");
        }
    }
}