namespace Bars.Gkh.Migrations.Version_2013122400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_Version_2013122300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("PERS_ACC_NUM", DbType.String, 30));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "PERS_ACCOUNT");
        }
    }
}