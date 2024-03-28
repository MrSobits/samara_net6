namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014030300
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014022800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("STATE_ID", "REGOP_RO_LOAN_STATE", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_LOAN", "STATE_ID");
        }
    }
}
