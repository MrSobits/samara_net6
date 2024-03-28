namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014073000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014071500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_CASHPAYMENT_CENTER",
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "CASHPAYM_CNTR_CTRG", "GKH_CONTRAGENT", "ID"),
                new Column("IDENTIFIER", DbType.Int64),
                new Column("CONDUCTSACCRUAL", DbType.Boolean));

            Database.AddEntityTable("REGOP_CASHPAYM_CENTER_MU",
                new RefColumn("CASHPAYM_CENTER_ID", ColumnProperty.NotNull, "CSHPM_CNTR_MU_CSHPM", "REGOP_CASHPAYMENT_CENTER", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "CSHPM_CNTR_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddEntityTable("REGOP_CASHPAYM_CENTER_REAL_OBJ",
                new RefColumn("CASHPAYM_CENTER_ID", ColumnProperty.NotNull, "CSHPM_CNTR_RO_CSHPM", "REGOP_CASHPAYMENT_CENTER", "ID"),
                new RefColumn("REAL_OBJ_ID", ColumnProperty.NotNull, "CSHPM_CNTR_RO_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_CASHPAYMENT_CENTER");
            Database.RemoveTable("REGOP_CASHPAYM_CENTER_MU");
            Database.RemoveTable("REGOP_CASHPAYM_CENTER_REAL_OBJ");
        }
    }
}
