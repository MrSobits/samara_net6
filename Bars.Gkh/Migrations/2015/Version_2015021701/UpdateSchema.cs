namespace Bars.Gkh.Migration.Version_2015021701
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015021701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015021700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_ROOM", new Column("ADDRESS_CODE", DbType.String, 50, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_ROOM", "ADDRESS_CODE");
        }
    }
}