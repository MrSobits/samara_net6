namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021501
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("REGOP_RO_LOAN", "DOC_ID", true);
        }

        public override void Down()
        {
        }
    }
}
