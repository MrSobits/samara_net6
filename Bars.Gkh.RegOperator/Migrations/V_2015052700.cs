namespace Bars.Gkh.RegOperator.Migrations
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015060300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015052900.UpdateSchema))]
    public class V_2015060300 : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.AddEntityTable(
                "regop_payment_doc_snapshot",
                new Column("holder_id", DbType.Int64, ColumnProperty.NotNull),
                new Column("holder_type", DbType.String, 100, ColumnProperty.Null),
                new Column("raw_data", DbType.String, 65535, ColumnProperty.Null),
                new Column("doc_date", DbType.DateTime, ColumnProperty.Null),
                new Column("doc_num", DbType.String, 100, ColumnProperty.Null),
                new Column("municipality", DbType.String, 200, ColumnProperty.Null),
                new Column("owner_address", DbType.String, 200, ColumnProperty.Null),
                new Column("owner_type", DbType.Int32, ColumnProperty.Null),
                new Column("payer", DbType.String, 200, ColumnProperty.Null),
                new Column("receiver_account", DbType.String, 200, ColumnProperty.Null),
                new Column("settlement", DbType.String, 200, ColumnProperty.Null),
                new Column("total_payment", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("period_id", ColumnProperty.NotNull, "pay_doc_snap_per", "regop_period", "id")
                );
            Database.AddIndex("idx_regop_paydoc_snap_h", false, "regop_payment_doc_snapshot", "holder_type");

            Database.AddEntityTable(
                "regop_pers_paydoc_snap",
                new Column("account_id", DbType.Int64, ColumnProperty.NotNull),
                new Column("acc_num", DbType.String, 20, ColumnProperty.NotNull),
                new Column("charge_sum", DbType.Decimal, ColumnProperty.NotNull),
                new Column("raw_data", DbType.String, 65535, ColumnProperty.Null),
                new Column("room_address", DbType.String, 200, ColumnProperty.Null),
                new Column("room_type", DbType.Int32, ColumnProperty.Null),
                new Column("services", DbType.String, 3000, ColumnProperty.Null),
                new Column("tariff", DbType.Decimal, ColumnProperty.NotNull),
                new Column("area", DbType.Single, ColumnProperty.NotNull),
                new RefColumn("snapshot_id", ColumnProperty.NotNull, "regop_perspay_snap", "regop_payment_doc_snapshot", "id")
                );

            Database.AddEntityTable(
                "regop_payment_doc_templ",
                new Column("template_bytes", DbType.Binary, ColumnProperty.NotNull),
                new Column("template_code", DbType.String, 100, ColumnProperty.NotNull),
                new RefColumn("period_id", ColumnProperty.NotNull, "pay_doc_templ_per", "regop_period", "id")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("regop_payment_doc_templ");
            Database.RemoveTable("regop_pers_paydoc_snap");

            Database.RemoveIndex("idx_regop_paydoc_snap_h", "regop_payment_doc_snapshot");
            Database.RemoveTable("regop_payment_doc_snapshot");
        }

        #endregion
    }
}