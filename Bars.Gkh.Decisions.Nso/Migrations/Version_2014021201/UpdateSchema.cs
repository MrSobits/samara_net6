namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014021201
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014021200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("GKH_OBJ_D_PROTOCOL", "FILE_ID", true);

            Database.AlterColumnSetNullable("GKH_OBJ_D_PROTOCOL", "DOCUMENT_NAME", true);

            Database.AlterColumnSetNullable("GKH_OBJ_D_PROTOCOL", "DOCUMENT_NUM", true);

            Database.AlterColumnSetNullable("GKH_OBJ_D_PROTOCOL", "DESCR", true);
        }

        public override void Down()
        {
        }
    }
}