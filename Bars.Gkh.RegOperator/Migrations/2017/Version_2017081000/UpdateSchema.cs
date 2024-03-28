namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017081000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017081000")]
    [MigrationDependsOn(typeof(Version_2017080400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("DEBTOR_TYPE", DbType.Int32, ColumnProperty.Null));
            this.Database.AddRefColumn("CLW_DEBTOR_CLAIM_WORK", 
                new RefColumn("OWNER_ID", ColumnProperty.Null, "CLW_DEBT_CLW_OWNER", "REGOP_PERS_ACC_OWNER", "ID"));

            this.Database.ChangeColumnNotNullable("REGOP_DEBTOR", "START_DATE", false);
            this.Database.ChangeColumnNotNullable("REGOP_DEBTOR", "MONTH_COUNT", false);

            //добавляем дочерние таблицы
            this.Database.AddTable("CLW_LEGAL_CLAIM_WORK",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new RefColumn("JURISDICTION_RO_ID", ColumnProperty.Null, "LEGAL_CLAIM_WORK_JUR_RO", "GKH_REALITY_OBJECT", "ID"));
            this.Database.AddForeignKey("FK_LEGAL_CLAIM_WORK", "CLW_LEGAL_CLAIM_WORK", "ID", "CLW_DEBTOR_CLAIM_WORK", "ID");

            this.Database.AddTable("CLW_INDIVIDUAL_CLAIM_WORK",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));
            this.Database.AddForeignKey("FK_INDIVIDUAL_CLAIM_WORK", "CLW_INDIVIDUAL_CLAIM_WORK", "ID", "CLW_DEBTOR_CLAIM_WORK", "ID");

            //таблица с детализацией
            this.Database.AddEntityTable(
                "CLW_CLAIM_WORK_ACC_DETAIL",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "CLAIM_WORK_ACC_DETAIL_ACC", "REGOP_PERS_ACC", "ID"),
                new RefColumn("CLAIM_WORK_ID", ColumnProperty.NotNull, "CLAIM_WORK_ACC_DETAIL_CLW", "CLW_CLAIM_WORK", "ID"),
                new Column("CUR_CHARGE_DEBT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CUR_PENALTY_DEBT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("ORIG_CHARGE_DEBT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("ORIG_PENALTY_DEBT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("COUNT_DAYS_DELAY", DbType.Int32, ColumnProperty.NotNull),
                new Column("COUNT_MONTH_DELAY", DbType.Int32, ColumnProperty.NotNull));

            //мигрируем данные
            this.Database.ExecuteNonQuery(this.query);

            this.Database.AlterColumnSetNullable("CLW_DEBTOR_CLAIM_WORK", "DEBTOR_TYPE", false);
            this.Database.AlterColumnSetNullable("CLW_DEBTOR_CLAIM_WORK", "OWNER_ID", false);

            //ViewManager.Drop(this.Database, "Regop", "DeleteViewDebtor");
            //ViewManager.Create(this.Database, "Regop", "CreateViewDebtor");

            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "ACCOUNT_ID");
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "COUNT_MONTH_DELAY");
        }

        /// не откатываю специально, тк мы потом заново не сможем накатить
        public override void Down()
        {
        }

        private string query = @"
            update CLW_DEBTOR_CLAIM_WORK clw
            set DEBTOR_TYPE = o.OWNER_TYPE,
	            OWNER_ID = o.ID
            from regop_pers_acc_owner o 
            join regop_pers_acc pa on o.id=ACC_OWNER_ID
            where clw.ACCOUNT_ID=pa.ID;

            --дочерние ПИР
            insert into CLW_LEGAL_CLAIM_WORK (id)
            select id 
            from CLW_DEBTOR_CLAIM_WORK
            where DEBTOR_TYPE = 1;

            insert into CLW_INDIVIDUAL_CLAIM_WORK (id)
            select id 
            from CLW_DEBTOR_CLAIM_WORK
            where DEBTOR_TYPE = 0;

            --добавление детализации
            insert into CLW_CLAIM_WORK_ACC_DETAIL 
            (
	             object_version, object_create_date, object_edit_date,
	             ACCOUNT_ID, CLAIM_WORK_ID, 
	             CUR_CHARGE_DEBT, CUR_PENALTY_DEBT, 
	             ORIG_CHARGE_DEBT, ORIG_PENALTY_DEBT,
	             START_DATE, COUNT_DAYS_DELAY, COUNT_MONTH_DELAY
            )
            select 
	            0, now()::date, now()::date,
	            ACCOUNT_ID, clw.id,
	            CUR_CHARGE_DEBT, CUR_PENALTY_DEBT,
	            ORIG_CHARGE_DEBT, ORIG_PENALTY_DEBT,
	            START_DATE, COUNT_DAYS_DELAY, COUNT_MONTH_DELAY
            from CLW_DEBTOR_CLAIM_WORK d
            join CLW_CLAIM_WORK clw on d.id=clw.id;";
    }
}