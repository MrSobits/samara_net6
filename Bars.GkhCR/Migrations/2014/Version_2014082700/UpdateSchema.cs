using Bars.Gkh.Utils;

namespace Bars.GkhCr.Migrations.Version_2014082700
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014082700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014060300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("CR_OBJ_FIN_SOURCE_RES", "FIN_SOURCE_ID", true);
        }

        public override void Down()
        {
            // не трбуется, т.к. может не откатиться
        }
    }
}