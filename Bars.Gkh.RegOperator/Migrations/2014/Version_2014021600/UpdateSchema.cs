namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_PAYMENT_ACC_OP", new Column("COPER_STATUS", DbType.Int16, ColumnProperty.NotNull, 0));

            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "BANK_ACC_ID");
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", 
                new RefColumn("BANK_ACC_ID", ColumnProperty.Null, "RO_PAY_ACC", "REGOP_BANK_ACCOUNT", "ID")
              );

            Database.AddEntityTable("REG_OP_SSP_ACC_PA_P",
                new RefColumn("PAY_ID", ColumnProperty.NotNull, "RO_SA_PA_P", "REGOP_PERS_ACC_PAYMENT", "ID"),
                new RefColumn("SA_ID", ColumnProperty.NotNull, "RO_SA_PA_SA", "REG_OP_SUSPEN_ACCOUNT", "ID")
                );


            Database.AddEntityTable("REG_OP_SSP_ACC_RO_PA",
                new RefColumn("OP_ID", ColumnProperty.NotNull, "RO_SA_RO_OP", "REGOP_RO_PAYMENT_ACC_OP", "ID"),
                new RefColumn("SA_ID", ColumnProperty.NotNull, "RO_SA_RO_SA", "REG_OP_SUSPEN_ACCOUNT", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACC_OP", "COPER_STATUS");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "BANK_ACC_ID");
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT",
                new RefColumn("BANK_ACC_ID", ColumnProperty.Null, "RO_PAY_ACC", "REGOP_BANK_ACCOUNT", "ID")
              );

            Database.RemoveTable("REG_OP_SSP_ACC_RO_PA");
            Database.RemoveTable("REG_OP_SSP_ACC_PA_P");
        }
    }
}
