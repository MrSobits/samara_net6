namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014073000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014072103.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("TOMSK_GJI_ARTICLELAW", new Column("PHYS_PENALTY", DbType.String, 300));
            Database.ChangeColumn("TOMSK_GJI_ARTICLELAW", new Column("JUR_PENALTY", DbType.String, 300));
        }

        public override void Down()
        {
        }
    }
}