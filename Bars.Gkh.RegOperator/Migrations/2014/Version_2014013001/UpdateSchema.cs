namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014013001
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014013001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014013000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_CHARGE_ACC_CHARGE", new Column("CDATE", DbType.DateTime, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_CHARGE_ACC_CHARGE", "CDATE");
        }
    }
}
