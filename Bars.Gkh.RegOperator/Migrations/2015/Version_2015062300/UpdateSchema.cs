namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015062300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015062200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("REGOP_INDIVIDUAL_ACC_OWN", "FIAS_FACT_ADDRESS_ID"))
            {
                Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("FIAS_FACT_ADDRESS_ID", DbType.Int64, 22));
            }
            if (!Database.ConstraintExists("REGOP_INDIVIDUAL_ACC_OWN", "FK_GKH_CONTR_FADR"))
            {
                Database.AddForeignKey("FK_GKH_CONTR_FADR", "REGOP_INDIVIDUAL_ACC_OWN", "FIAS_FACT_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            }
            if (!Database.ColumnExists("REGOP_INDIVIDUAL_ACC_OWN", "ADDRESS_OUT_SUBJECT"))
            {
                Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("ADDRESS_OUT_SUBJECT", DbType.String, 500));
            }
            if (!Database.ColumnExists("REGOP_INDIVIDUAL_ACC_OWN", "EMAIL"))
            {
                Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("EMAIL", DbType.String, 255));
            }

            if (!Database.ColumnExists("REGOP_PERS_ACC_OWNER", "BILLING_ADDRESS_TYPE"))
            {
                Database.AddColumn("REGOP_PERS_ACC_OWNER", new Column("BILLING_ADDRESS_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));
            }
        }

        public override void Down()
        {
            if (Database.ConstraintExists("REGOP_INDIVIDUAL_ACC_OWN", "FK_GKH_CONTR_FADR"))
            {
                Database.RemoveConstraint("REGOP_INDIVIDUAL_ACC_OWN", "FK_GKH_CONTR_FADR");
            }
            if (Database.ColumnExists("REGOP_INDIVIDUAL_ACC_OWN", "FIAS_FACT_ADDRESS_ID"))
            {
                Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "FIAS_FACT_ADDRESS_ID");
            }
            if (Database.ColumnExists("REGOP_INDIVIDUAL_ACC_OWN", "ADDRESS_OUT_SUBJECT"))
            {
                Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "ADDRESS_OUT_SUBJECT");
            }
            if (Database.ColumnExists("REGOP_INDIVIDUAL_ACC_OWN", "EMAIL"))
            {
                Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "EMAIL");
            }

            if (Database.ColumnExists("REGOP_PERS_ACC_OWNER", "BILLING_ADDRESS_TYPE"))
            {
                Database.RemoveColumn("REGOP_PERS_ACC_OWNER", "BILLING_ADDRESS_TYPE");
            }
        }
    }
}
