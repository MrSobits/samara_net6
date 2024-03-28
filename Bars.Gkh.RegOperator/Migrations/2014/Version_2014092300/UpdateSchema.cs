namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;


    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014091700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("REGOP_UNACCEPT_CHARGE", new RefColumn("ACC_ID", ColumnProperty.Null, "ROP_UNACC_CH_ACC", "REGOP_PERS_ACC", "ID"));
            Database.ChangeColumn("REGOP_UNACCEPT_CHARGE", new Column("DESCR", DbType.String, 500));
        }

        public override void Down()
        {
        }
    }
}
