namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014092600
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014062200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.TableExists("OVRHL_ACCOUNT_REAL_OPERATION"))
            {
                Database.RemoveTable("OVRHL_ACCOUNT_REAL_OPERATION");
            }

            if (Database.TableExists("OVRHL_REAL_ACCOUNT"))
            {
                Database.RemoveTable("OVRHL_REAL_ACCOUNT");
            }

            if (Database.TableExists("OVRHL_ACCOUNT_SPEC_OPERATION"))
            {
                Database.RemoveTable("OVRHL_ACCOUNT_SPEC_OPERATION");
            }

            if (Database.TableExists("OVRHL_SPECIAL_ACCOUNT"))
            {
                Database.RemoveTable("OVRHL_SPECIAL_ACCOUNT");
            }

            if (Database.TableExists("OVRHL_ACCOUNT_ACCR_OPERATION"))
            {
                Database.RemoveTable("OVRHL_ACCOUNT_ACCR_OPERATION");
            }

            if (Database.TableExists("OVRHL_ACCRUALS_ACCOUNT"))
            {
                Database.RemoveTable("OVRHL_SPECIAL_ACCOUNT");
            }
        }

        public override void Down()
        {
            // не требуется
        }
    }
}