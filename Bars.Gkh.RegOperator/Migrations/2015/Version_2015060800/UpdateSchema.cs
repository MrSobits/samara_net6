namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015060800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015060800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations.V_2015060300))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("REGOP_INDIVIDUAL_ACC_OWN", "BIRTH_PLACE"))
            {
                Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", "BIRTH_PLACE", DbType.String, 300, ColumnProperty.Null);
            }
        }

        public override void Down()
        {
            if (Database.ColumnExists("REGOP_INDIVIDUAL_ACC_OWN", "BIRTH_PLACE"))
            {
                Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "BIRTH_PLACE");
            }
        }
    }
}
