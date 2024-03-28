namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014102700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014102300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddColumn("REGOP_TRANSFER", new Column("IS_LOAN", DbType.Boolean, ColumnProperty.NotNull, "FALSE"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("REGOP_TRANSFER", new Column("IS_LOAN", DbType.Boolean, ColumnProperty.NotNull, "f"));
            }

            Database.AddColumn("REGOP_RO_LOAN", new Column("OP_DATE", DbType.DateTime, ColumnProperty.Null));

            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("RO_ACC_SUBJ_ID", ColumnProperty.Null, "LOAN_RO_SUBJ_P_ACC", "REGOP_RO_PAYMENT_ACCOUNT", "ID"));
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("RO_ACC_ID", ColumnProperty.Null, "LOAN_RO_P_ACC", "REGOP_RO_PAYMENT_ACCOUNT", "ID"));

            Database.ExecuteNonQuery(
                @"UPDATE regop_ro_loan l
SET RO_ACC_SUBJ_ID = (SELECT
	ID
FROM
	REGOP_RO_PAYMENT_ACCOUNT A
WHERE
	A .ro_id = l.ro_subj_id),
	RO_ACC_ID = (SELECT
		ID
	FROM
		REGOP_RO_PAYMENT_ACCOUNT A
	WHERE
		A .ro_id = l.ro_id)");

            Database.RemoveColumn("REGOP_RO_LOAN", "RO_SUBJ_ID");
            Database.RemoveColumn("REGOP_RO_LOAN", "RO_ID");
        }

        public override void Down()
        {
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("RO_SUBJ_ID", ColumnProperty.Null, "LOAN_RO_SUBJ", "GKH_REALITY_OBJECT", "ID"));
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("RO_ID", ColumnProperty.Null, "LOAN_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.RemoveColumn("REGOP_RO_LOAN", "RO_ACC_SUBJ_ID");
            Database.RemoveColumn("REGOP_RO_LOAN", "RO_ACC_ID");

            Database.RemoveColumn("REGOP_TRANSFER", "IS_LOAN");
            Database.RemoveColumn("REGOP_RO_LOAN", "OP_DATE");
        }
    }
}
