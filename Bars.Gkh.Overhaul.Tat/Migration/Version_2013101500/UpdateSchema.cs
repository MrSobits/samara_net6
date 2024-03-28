namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013101500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013101403.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //----Операции по специальному счету
            Database.AddEntityTable(
                "OVRHL_ACCOUNT_SPEC_OPERATION",
                new RefColumn("ACCOUNT", ColumnProperty.NotNull, "OVRHL_ACCOUNT_SPEC_FK", "OVRHL_SPECIAL_ACCOUNT", "ID"),
                new RefColumn("NAME", ColumnProperty.NotNull, "OVRHL_OPERATION_SP_NAME_FK", "OVRHL_DICT_ACCTOPERATION", "ID"),
                new Column("OPERATION_DATE", DbType.Date),
                new Column("SUM", DbType.Double),
                new Column("RECEIVER", DbType.String,128),
                new Column("PAYER", DbType.String, 128),
                new Column("PURPOSE", DbType.String, 128));

            //----Операции по реальному счету
            Database.AddEntityTable(
                "OVRHL_ACCOUNT_REAL_OPERATION",
                new RefColumn("ACCOUNT", ColumnProperty.NotNull, "OVRHL_ACCOUNT_REAL_FK", "OVRHL_REAL_ACCOUNT", "ID"),
                new RefColumn("NAME", ColumnProperty.NotNull, "OVRHL_OPERATION_RL_NAME_FK", "OVRHL_DICT_ACCTOPERATION", "ID"),
                new Column("OPERATION_DATE", DbType.Date),
                new Column("SUM", DbType.Double),
                new Column("RECEIVER", DbType.String, 128),
                new Column("PAYER", DbType.String, 128),
                new Column("PURPOSE", DbType.String, 128));

            //---- Движения по счету начислений
            Database.AddEntityTable(
               "OVRHL_ACCOUNT_ACCR_OPERATION",
                new RefColumn("ACCOUNT", ColumnProperty.NotNull, "OVRHL_ACCOUNT_ACCR_FK", "OVRHL_ACCRUALS_ACCOUNT", "ID"),
                new Column("ACCRUAL_DATE", DbType.Date),
                new Column("TOTAL_DEBIT", DbType.Double),
                new Column("TOTAL_CREDIT", DbType.Double),
                new Column("OPENING_BALANCE", DbType.Double),
                new Column("CLOSING_BALANCE", DbType.Double));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_ACCOUNT_SPEC_OPERATION");
            Database.RemoveTable("OVRHL_ACCOUNT_REAL_OPERATION");
            Database.RemoveTable("OVRHL_ACCOUNT_ACCR_OPERATION");
        }
    }
}