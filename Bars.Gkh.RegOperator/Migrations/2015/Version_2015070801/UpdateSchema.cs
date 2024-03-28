namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015070801
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015070801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015070800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_RO_SUPP_ACC_OP",
                new RefColumn("WORK_ID", "REGOP_RO_SUPP_ACC_OP_WORK_ID", "GKH_DICT_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_SUPP_ACC_OP", "WORK_ID");
        }
    }
}
