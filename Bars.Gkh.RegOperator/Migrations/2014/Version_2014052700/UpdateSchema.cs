namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014052700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014050800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_CHANGE", new Column("ACTUAL_FROM", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_CHANGE", "ACTUAL_FROM");
        }
    }
}
