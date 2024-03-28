namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014120500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014120401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_CASHPAY_PERS_ACC",
                new RefColumn("CASHPAYM_CENTER_ID", ColumnProperty.NotNull, "CSHPM_CNTR_PA_CSHPM", "REGOP_CASHPAYMENT_CENTER", "ID"),
                new RefColumn("PERS_ACC_ID", ColumnProperty.NotNull, "CSHPM_CNTR_PA_PA", "REGOP_PERS_ACC", "ID"),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_CASHPAY_PERS_ACC");
        }
    }
}
