namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017080300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017080300")]
    [MigrationDependsOn(typeof(Version_2017071900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.RemoveIndex("ind_payment_doc_snap_type_holder", "REGOP_PAYMENT_DOC_SNAPSHOT");
            this.Database.RemoveIndex("ind_pay_doc_snap_per", "REGOP_PAYMENT_DOC_SNAPSHOT");
            this.Database.RemoveIndex("idx_regop_paydoc_snap_h", "REGOP_PAYMENT_DOC_SNAPSHOT");
            this.Database.AddColumn("REGOP_PAYMENT_DOC_SNAPSHOT", new Column("PAYMENT_STATE", DbType.Int32, ColumnProperty.NotNull, 0));

            this.Database.ExecuteNonQuery(@"
                        CREATE INDEX ind_payment_doc_snap_type_holder ON regop_payment_doc_snapshot USING btree (owner_type, holder_id);
                        CREATE INDEX ind_pay_doc_snap_per ON regop_payment_doc_snapshot USING btree (period_id);
                        CREATE INDEX idx_regop_paydoc_snap_h ON regop_payment_doc_snapshot USING btree (holder_type);");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "PAYMENT_STATE");
        }
    }
}