namespace Bars.Gkh.Migrations.Version_2013111100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013111100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013110801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", "OWNER_CODE", DbType.String, 100, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "OWNER_CODE");
        }
    }
}