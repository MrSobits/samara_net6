namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014062000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014061900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddColumn("GJI_TOMSK_RESOLUTION", "RESOLUTION_TEXT", DbType.String, 2000, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_TOMSK_RESOLUTION", "RESOLUTION_TEXT");
        }
    }
}