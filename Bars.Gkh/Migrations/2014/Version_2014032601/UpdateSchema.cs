namespace Bars.Gkh.Migrations.Version_2014032601
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014032600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("B4_FIAS_ADDRESS_UID", "UID"))
            {
                Database.RenameColumn("B4_FIAS_ADDRESS_UID", "UID", "ICS_UID");
            }
        }

        public override void Down()
        {
            if (Database.ColumnExists("B4_FIAS_ADDRESS_UID", "ICS_UID"))
            {
                Database.RenameColumn("B4_FIAS_ADDRESS_UID", "ICS_UID", "UID");
            }
        }
    }
}