namespace Sobits.GisGkh.Migrations._2020.Version_2020091100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2020091100")]
    [MigrationDependsOn(typeof(Version_2020060200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GIS_GKH_REQUESTS", "dictionarynumber");
        }

        public override void Down()
        {
            Database.AddColumn("GIS_GKH_REQUESTS", new Column("dictionarynumber", DbType.String));
        }
    }
}