namespace Bars.GkhCr.Migrations.Version_2014100701
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014100700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Сделано таким образом, так как соответствующая сущность была перенесена из регоператора.
            if (Database.TableExists("REGOP_ACTPAYMENT_DETAILS"))
            {
                Database.RenameTable("REGOP_ACTPAYMENT_DETAILS", "CR_ACTPAYMENT_DETAILS");
            }
            else
            {
                Database.AddEntityTable("CR_ACTPAYMENT_DETAILS",
                    new RefColumn("ACTPAYMENT_ID", ColumnProperty.NotNull, "REGOP_ACTP_DET_AP", "CR_OBJ_PER_ACT_PAYMENT", "ID"),
                    new Column("SRC_FIN_TYPE", DbType.Int16, ColumnProperty.NotNull, 10),
                    new Column("BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                    new Column("PAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0));
            }
        }

        public override void Down()
        {
            Database.RemoveTable("CR_ACTPAYMENT_DETAILS");
        }
    }
}