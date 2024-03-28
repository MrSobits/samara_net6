namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013121002
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121002")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013121000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PROP_OWN_DECISION_BASE", new Column("DECISION_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddColumn("OVRHL_PROP_OWN_DECISION_BASE", new Column("METHOD_FORM_FUND", DbType.Int32, 4, ColumnProperty.Null, 0));
            Database.AddColumn("OVRHL_PROP_OWN_DECISION_BASE", new Column("MO_ORG_FORM", DbType.Int32, 4, ColumnProperty.NotNull, 10));

            Database.RenameTable("OVRHL_PROP_OWN_DEC_REGOP", "OVRHL_PR_DEC_REGOP_ACC");

            Database.ExecuteNonQuery("ALTER TABLE ovrhl_prop_own_decision_base ALTER COLUMN prop_owner_protocol_id DROP NOT NULL;"
                         + "ALTER TABLE ovrhl_pr_dec_regop_acc ALTER COLUMN reg_operator_id DROP NOT NULL;");

            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION", "MINIMAL_FUND_VOLUME");
            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION", "ACCOUNT_ID");

            Database.RenameTable("OVRHL_PROP_OWN_DECISION", "OVRHL_PR_DEC_SPEC_ACC");

            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("ACC_NUMBER", DbType.String, 250));
            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("OPEN_DATE", DbType.DateTime));
            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("CLOSE_DATE", DbType.DateTime));
            Database.AddRefColumn("OVRHL_PR_DEC_SPEC_ACC", new RefColumn("HELP_FILE_ID", "OV_DEC_SPEC_ACC_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("OVRHL_PR_DEC_SPEC_ACC", new RefColumn("CREDIT_ORG_ID", "OV_DEC_SPEC_AC_CR_ORG", "OVRHL_CREDIT_ORG", "ID"));

            Database.AddTable("OVRHL_PR_DEC_MIN_AMOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("PAY_SIZE", DbType.Decimal),
                new Column("PAY_DATE_START", DbType.DateTime),
                new Column("PAY_DATE_END", DbType.DateTime));

            Database.AddForeignKey("FK_OVRHL_PR_DEC_MIN_AMOUNT", "OVRHL_PR_DEC_MIN_AMOUNT", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");
  
            Database.AddEntityTable(
                "OVRHL_DEC_SPEC_ACC_NOTICE",
                new Column("NOTICE_NUMBER", DbType.String, 250),
                 new Column("NOTICE_DATE", DbType.DateTime),
                 new Column("REG_DATE", DbType.DateTime),
                 new Column("GJI_NUMBER", DbType.String, 250),
                 new Column("HAS_ORIG", DbType.Boolean, ColumnProperty.NotNull, false),
                 new Column("HAS_COPY_PROT", DbType.Boolean, ColumnProperty.NotNull, false),
                 new Column("HAS_COPY_CERT", DbType.Boolean, ColumnProperty.NotNull, false),

                new RefColumn("STATE_ID", "OV_DEC_NOTICE_STAT", "B4_STATE", "ID"),
                new RefColumn("SPEC_ACC_DEC_ID", "OV_DEC_NOTICE_SP_ACC", "OVRHL_PR_DEC_SPEC_ACC", "ID"),
                new RefColumn("FILE_ID", "OV_DEC_NOTICE_FILE", "B4_FILE_INFO", "ID"));

            Database.AddUniqueConstraint("UNQ_OV_DEC_NOTICE_SP_ACC", "OVRHL_DEC_SPEC_ACC_NOTICE", "SPEC_ACC_DEC_ID");

            //добавляю Foreign Key и Index потому что RefColumn добавляли в AddTable
            Database.AddIndex("IND_OV_DEC_SPEC_REG_OP", false, "OVRHL_PR_DEC_SPEC_ACC", "REG_OPERATOR_ID");
            Database.AddForeignKey("FK_OV_DEC_SPEC_REG_OP", "OVRHL_PR_DEC_SPEC_ACC", "REG_OPERATOR_ID", "OVRHL_REG_OPERATOR", "ID");
            Database.AddIndex("IND_OV_DEC_SPEC_MAN_ORG", false, "OVRHL_PR_DEC_SPEC_ACC", "MANAGING_ORG_ID");
            Database.AddForeignKey("FK_OV_DEC_SPEC_MAN_ORG", "OVRHL_PR_DEC_SPEC_ACC", "MANAGING_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddIndex("IND_OV_DEC_SPEC_CR_ORG", false, "OVRHL_PR_DEC_SPEC_ACC", "CREDIT_ORG_ID");
            Database.AddForeignKey("FK_OV_DEC_SPEC_CR_ORG", "OVRHL_PR_DEC_SPEC_ACC", "CREDIT_ORG_ID", "OVRHL_CREDIT_ORG", "ID");
            Database.AddIndex("IND_OV_DEC_REGOP_REGOP", false, "OVRHL_PR_DEC_REGOP_ACC", "REG_OPERATOR_ID");
            Database.AddForeignKey("FK_OV_DEC_REGOP_REGOP", "OVRHL_PR_DEC_REGOP_ACC", "REG_OPERATOR_ID", "OVRHL_REG_OPERATOR", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("OVRHL_PR_DEC_SPEC_ACC", "FK_OV_DEC_SPEC_REG_OP");
            Database.RemoveConstraint("OVRHL_PR_DEC_SPEC_ACC", "FK_OV_DEC_SPEC_MAN_ORG");
            Database.RemoveConstraint("OVRHL_PR_DEC_SPEC_ACC", "FK_OV_DEC_SPEC_CR_ORG");
            Database.RemoveConstraint("OVRHL_PR_DEC_REGOP_ACC", "FK_OV_DEC_REGOP_REGOP");
            Database.RemoveIndex("OVRHL_PR_DEC_SPEC_ACC", "IND_OV_DEC_SPEC_REG_OP");
            Database.RemoveIndex("OVRHL_PR_DEC_SPEC_ACC", "IND_OV_DEC_SPEC_MAN_ORG");
            Database.RemoveIndex("OVRHL_PR_DEC_SPEC_ACC", "IND_OV_DEC_SPEC_CR_ORG");
            Database.RemoveIndex("OVRHL_PR_DEC_REGOP_ACC", "IND_OV_DEC_REGOP_REGOP");

            Database.RemoveConstraint("OVRHL_DEC_SPEC_ACC_NOTICE", "UNQ_OV_DEC_NOTICE_SP_ACC");

            Database.RemoveTable("OVRHL_DEC_SPEC_ACC_NOTICE");
            Database.RemoveConstraint("OVRHL_PR_DEC_MIN_AMOUNT", "FK_OVRHL_PR_DEC_MIN_AMOUNT");
            Database.RemoveTable("OVRHL_PR_DEC_MIN_AMOUNT");

            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "CREDIT_ORG_ID");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "HELP_FILE_ID");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "ACC_NUMBER");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "OPEN_DATE");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "CLOSE_DATE");

            Database.RenameTable("OVRHL_PR_DEC_SPEC_ACC", "OVRHL_PROP_OWN_DECISION");

            Database.AddTable("OVRHL_PROP_OWN_DECISION", new Column("MINIMAL_FUND_VOLUME", DbType.Decimal));

            Database.AddRefColumn("OVRHL_PROP_OWN_DECISION",
                new RefColumn("ACCOUNT_ID", "OVRHL_PROPOWNDEC_ACCOUNT", "OVRHL_ACCOUNT", "ID"));

            Database.RenameTable( "OVRHL_PR_DEC_REGOP_ACC", "OVRHL_PROP_OWN_DEC_REGOP");

            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION_BASE", "DECISION_TYPE");
            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION_BASE", "METHOD_FORM_FUND");
            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION_BASE", "MO_ORG_FORM");
        }
    }
}