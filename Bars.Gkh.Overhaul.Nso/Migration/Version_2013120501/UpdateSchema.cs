namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013120501
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013120500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.TableExists("OVRHL_DICT_ACCTOPERATION"))
            {
                Database.AddEntityTable(
                    "OVRHL_DICT_ACCTOPERATION",
                    new Column("NAME", DbType.String, ColumnProperty.NotNull),
                    new Column("CODE", DbType.Int64, ColumnProperty.NotNull),
                    new Column("TYPE", DbType.Int64, ColumnProperty.NotNull));
            }

            //Операции по счету начислений
            Database.RemoveColumn("OVRHL_ACCOUNT_ACCR_OPERATION", "ACCOUNT");
            Database.AddRefColumn("OVRHL_ACCOUNT_ACCR_OPERATION", new RefColumn("ACCOUNT_ID", "OVRHL_ACCACCROPER_ACC", "OVRHL_ACCRUALS_ACCOUNT", "ID"));

            //Счет
            Database.RemoveColumn("OVRHL_REAL_ACCOUNT", "REG_OPER_ID");
            Database.AddRefColumn("OVRHL_REAL_ACCOUNT", new RefColumn("OWNER_ID", "OVRHL_REALACC_OWN", "GKH_CONTRAGENT", "ID"));

            //Операции по счету
            Database.RemoveColumn("OVRHL_ACCOUNT_REAL_OPERATION", "ACCOUNT");
            Database.AddRefColumn("OVRHL_ACCOUNT_REAL_OPERATION", new RefColumn("ACCOUNT_ID", "OVRHL_ACCREALOPER_ACC", "OVRHL_REAL_ACCOUNT", "ID"));

            Database.RemoveColumn("OVRHL_ACCOUNT_REAL_OPERATION", "NAME");
            Database.AddRefColumn("OVRHL_ACCOUNT_REAL_OPERATION", new RefColumn("OPERATION_ID", "OVRHL_ACCREALROPER_OPR", "OVRHL_DICT_ACCTOPERATION", "ID"));

            //Спецсчет
            Database.RemoveColumn("OVRHL_SPECIAL_ACCOUNT", "ACCOUNT_OWNER_ID");
            Database.AddRefColumn("OVRHL_SPECIAL_ACCOUNT", new RefColumn("OWNER_ID", "OVRHL_SPECACC_OWN", "GKH_CONTRAGENT", "ID"));

            //Операции по спецсчету
            Database.RemoveColumn("OVRHL_ACCOUNT_SPEC_OPERATION", "ACCOUNT");
            Database.AddRefColumn("OVRHL_ACCOUNT_SPEC_OPERATION", new RefColumn("ACCOUNT_ID", "OVRHL_ACCSPECOPER_ACC", "OVRHL_SPECIAL_ACCOUNT", "ID"));

            Database.RemoveColumn("OVRHL_ACCOUNT_SPEC_OPERATION", "NAME");
            Database.AddRefColumn("OVRHL_ACCOUNT_SPEC_OPERATION", new RefColumn("OPERATION_ID", "OVRHL_ACCSPECROPER_OPR", "OVRHL_DICT_ACCTOPERATION", "ID"));
        }

        public override void Down()
        {
            //Операции по счету начислений
            Database.RemoveColumn("OVRHL_ACCOUNT_ACCR_OPERATION", "ACCOUNT_ID");
            Database.AddRefColumn("OVRHL_ACCOUNT_ACCR_OPERATION", new RefColumn("ACCOUNT", "OVRHL_ACCACCROPER_ACC", "OVRHL_ACCRUALS_ACCOUNT", "ID"));

            //Счет
            Database.RemoveColumn("OVRHL_REAL_ACCOUNT", "OWNER_ID");
            Database.AddRefColumn("OVRHL_REAL_ACCOUNT", new RefColumn("REG_OPER_ID", "OVRHL_REALACC_OWN", "GKH_CONTRAGENT", "ID"));

            //Операции по счету
            Database.RemoveColumn("OVRHL_ACCOUNT_REAL_OPERATION", "ACCOUNT_ID");
            Database.AddRefColumn("OVRHL_ACCOUNT_REAL_OPERATION", new RefColumn("ACCOUNT", "OVRHL_ACCREALOPER_ACC", "OVRHL_REAL_ACCOUNT", "ID"));

            Database.RemoveColumn("OVRHL_ACCOUNT_REAL_OPERATION", "OPERATION_ID");
            Database.AddRefColumn("OVRHL_ACCOUNT_REAL_OPERATION", new RefColumn("NAME", "OVRHL_ACCREALROPER_OPR", "OVRHL_DICT_ACCTOPERATION", "ID"));

            //Спецсчет
            Database.RemoveColumn("OVRHL_SPECIAL_ACCOUNT", "ACCOUNT_OWNER_ID");
            Database.AddRefColumn("OVRHL_SPECIAL_ACCOUNT", new RefColumn("OWNER_ID", "OVRHL_SPECACC_OWN", "GKH_CONTRAGENT", "ID"));

            //Операции по спецсчету
            Database.RemoveColumn("OVRHL_ACCOUNT_SPEC_OPERATION", "ACCOUNT");
            Database.AddRefColumn("OVRHL_ACCOUNT_SPEC_OPERATION", new RefColumn("ACCOUNT_ID", "OVRHL_ACCSPECOPER_ACC", "OVRHL_SPECIAL_ACCOUNT", "ID"));

            Database.RemoveColumn("OVRHL_ACCOUNT_SPEC_OPERATION", "NAME");
            Database.AddRefColumn("OVRHL_ACCOUNT_SPEC_OPERATION", new RefColumn("OPERATION_ID", "OVRHL_ACCSPECROPER_OPR", "OVRHL_DICT_ACCTOPERATION", "ID"));
        }
    }
}