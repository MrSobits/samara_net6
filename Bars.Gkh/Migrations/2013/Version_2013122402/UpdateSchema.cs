namespace Bars.Gkh.Migrations.Version_2013122402
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122402")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("OWNER_NUMBER", DbType.Int32, (object) 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "OWNER_NUMBER");
        }
    }
}