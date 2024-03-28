namespace Bars.Gkh.Migrations.Version_2013111500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013111500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013111200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("HCS_HOUSE_ACCOUNT_CHARGE", "SUPPLIER", DbType.String, 300, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT_CHARGE", "SUPPLIER");
        }
    }
}