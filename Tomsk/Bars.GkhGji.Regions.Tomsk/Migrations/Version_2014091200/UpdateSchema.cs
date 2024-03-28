namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014091200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014073000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("TOMSK_GJI_ARTICLELAW", new Column("OFF_PENALTY", DbType.String, 300, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("TOMSK_GJI_ARTICLELAW", "OFF_PENALTY");
        }
    }
}