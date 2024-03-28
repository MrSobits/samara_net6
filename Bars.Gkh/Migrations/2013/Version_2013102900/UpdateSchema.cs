namespace Bars.Gkh.Migrations.Version_2013102900
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013102401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("HCS_HOUSE_INFO_OVERVIEW",
                    new Column("INDIV_ACCOUNTS_COUNT", DbType.Int16),
                    new Column("INDIV_OWNER_ACCOUNTS_COUNT", DbType.Int16),
                    new Column("INDIV_TENANT_ACCOUNTS_COUNT", DbType.Int16),
                    new Column("LEGAL_ACCOUNTS_COUNT", DbType.Int16),
                    new Column("LEGAL_OWNER_ACCOUNTS_COUNT", DbType.Int16),
                    new Column("LEGAL_TENANT_ACCOUNTS_COUNT", DbType.Int16),
                    new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "HCS_HOUSE_INFO_RO", "GKH_REALITY_OBJECT", "ID"));


            Database.AddEntityTable("HCS_HOUSE_ACCOUNT",
                    new Column("PAYMENT_CODE", DbType.Int16),
                    new Column("APARTMENT", DbType.Int16),
                    new Column("LIVING", DbType.Boolean),
                    new Column("RESIDENTS_COUNT", DbType.Int16),
                    new Column("HOUSE_STATUS", DbType.String),
                    new Column("APARTMENT_AREA", DbType.Decimal),
                    new Column("LIVING_AREA", DbType.Decimal),
                    new Column("ROOMS_COUNT", DbType.Int16),
                    new Column("ACCOUNT_STATE", DbType.String),
                    new Column("PRIVATIZED", DbType.Boolean),
                    new Column("TEMPORARY_GONE_COUNT", DbType.Int16),

                    new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "HCS_HOUSE_ACCOUNT_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable("HCS_METER_READING",
                    new Column("SERVICE", DbType.String),
                    new Column("METER_SERIAL", DbType.String),
                    new Column("METER_TYPE", DbType.String),
                    new Column("CURRENT_READ_DATE", DbType.DateTime),
                    new Column("PREV_READ_DATE", DbType.DateTime),
                    new Column("CURRENTE_READ", DbType.Decimal),
                    new Column("PREV_READ", DbType.Decimal),
                    new Column("EXPENSE", DbType.Decimal),
                    new Column("PLANNED_EXPENSE", DbType.Decimal),

                    new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "HCS_METER_READ_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable("HCS_HOUSE_OVERALL_BALANCE",
                    new Column("SERVICE", DbType.String),
                    new Column("INNER_BALANCE", DbType.Decimal),
                    new Column("MOUNTH_CHARGE", DbType.Decimal),
                    new Column("PAYMENT", DbType.Decimal),
                    new Column("PAID", DbType.Decimal),
                    new Column("OUTER_BALANCE", DbType.Decimal),
                    new Column("CORRECTION_COEF", DbType.Decimal),
                    new Column("HOUSE_EXPENSE", DbType.Decimal),
                    new Column("ACCOUNTS_EXPENSE", DbType.Decimal),

                    new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "HCS_HOUSE_BALANCE_RO", "GKH_REALITY_OBJECT", "ID"));


            Database.AddEntityTable("HCS_HOUSE_METER_READING",
                    new Column("SERVICE", DbType.String),
                    new Column("METER_TYPE", DbType.String),
                    new Column("CURRENT_READ_DATE", DbType.DateTime),
                    new Column("PREV_READ_DATE", DbType.DateTime),
                    new Column("CURRENTE_READ", DbType.Decimal),
                    new Column("PREV_READ", DbType.Decimal),
                    new Column("EXPENSE", DbType.Decimal),
                    new Column("NON_LIVING_EXPENSE", DbType.Decimal),

                    new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "HCS_HOUSE_METER_READ_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable("HCS_HOUSE_ACCOUNT_CHARGE",
                    new Column("SERVICE", DbType.String),
                    new Column("TARIFF", DbType.Decimal),
                    new Column("EXPENSE", DbType.Decimal),
                    new Column("COMPLETE_CALC", DbType.Decimal),
                    new Column("UNDERDELIVERY", DbType.Decimal),
                    new Column("CHARGED", DbType.Decimal),
                    new Column("RECALC", DbType.Decimal),
                    new Column("CHANGED", DbType.Decimal),
                    new Column("PAYMENT", DbType.Decimal),
                    new Column("CHARGED_PAYMENT", DbType.Decimal),
                    new Column("OUTER_BALANCE", DbType.Decimal),
                    new Column("INNER_BALANCE", DbType.Decimal),

                    new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "HCS_HOUSE_ACC_CHARGE_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("HCS_HOUSE_ACCOUNT_CHARGE");
            Database.RemoveTable("HCS_HOUSE_METER_READING");
            Database.RemoveTable("HCS_HOUSE_OVERALL_BALANCE");
            Database.RemoveTable("HCS_METER_READING");
            Database.RemoveTable("HCS_HOUSE_ACCOUNT");
            Database.RemoveTable("HCS_HOUSE_INFO_OVERVIEW");
        }
    }
}