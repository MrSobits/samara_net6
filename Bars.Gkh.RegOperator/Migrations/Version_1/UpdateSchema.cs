namespace Bars.Gkh.RegOperator.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddEntityTable("REGOP_PERIOD",
                new Column("CSTART", DbType.DateTime, ColumnProperty.NotNull),
                new Column("CEND", DbType.DateTime, ColumnProperty.Null),
                new Column("CIS_CLOSED", DbType.Boolean, ColumnProperty.NotNull)
                );

            Database.AddEntityTable("REGOP_BANK_ACCOUNT",
                new Column("CACCOUNT_NUM", DbType.String, 50, ColumnProperty.Null),
                new Column("CCURR_BALANCE", DbType.Decimal, ColumnProperty.NotNull)
                );

            Database.AddEntityTable("REGOP_UNACCEPT_C_PACKET",
                new Column("CCREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("CDESCRIPTION", DbType.String, 1000, ColumnProperty.Null)
                );

            Database.AddEntityTable("REGOP_UNACCEPT_CHARGE",
                new Column("CCHARGE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CCHARGE_TARIFF", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CPENALTY", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CRECALC", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CGUID", DbType.String, 40, ColumnProperty.Null),
                new RefColumn("PACKET_ID", ColumnProperty.NotNull, "UNACC_CH_PAC", "REGOP_UNACCEPT_C_PACKET", "ID")
                );

            Database.AddEntityTable("REGOP_RO_CHARGE_ACCOUNT",
                new Column("CCURR_BALANCE", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "RO_CH_ACC_RO", "GKH_REALITY_OBJECT", "ID")
                );

            Database.AddEntityTable("REGOP_RO_PAYMENT_ACCOUNT",
                new Column("CACC_TYPE", DbType.Int16, ColumnProperty.NotNull),
                new Column("CCURR_BALANCE", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("BANK_ACC_ID", ColumnProperty.NotNull, "RO_PAY_ACC", "REGOP_BANK_ACCOUNT", "ID"),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "RO_PAY_ACC_RO", "GKH_REALITY_OBJECT", "ID")
                );

            Database.AddEntityTable("REGOP_RO_CHARGE_ACC_CHARGE",
                new Column("CCHARGED", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "RO_CH_ACC_CH_ACC", "REGOP_RO_CHARGE_ACCOUNT", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "RO_CH_ACC_PERIOD", "REGOP_PERIOD", "ID")
                );

            Database.AddEntityTable("REGOP_RO_PAYMENT_ACC_OP",
                new Column("CDATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("COPER_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("COPER_TYPE", DbType.Int16, ColumnProperty.NotNull),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "RO_PAY_ACC_PAYM_ACC", "REGOP_RO_PAYMENT_ACCOUNT", "ID")
                );

            Database.AddEntityTable("REGOP_PERS_ACC_OWNER", 
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("OWNER_TYPE", DbType.Int16, ColumnProperty.NotNull)
                );

            Database.AddTable("REGOP_INDIVIDUAL_ACC_OWN",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("FIRST_NAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("SECOND_NAME", DbType.String, 100, ColumnProperty.Null),
                new Column("SURNAME", DbType.String, 100, ColumnProperty.Null)
                );
            Database.AddForeignKey("FK_INDIV_ACC_OWN_ACC_OWN", "REGOP_INDIVIDUAL_ACC_OWN", "ID", "REGOP_PERS_ACC_OWNER", "ID");

            Database.AddTable("REGOP_LEGAL_ACC_OWN",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("CONTRAGENT_ID", DbType.Int64, ColumnProperty.NotNull)
                );
            Database.AddForeignKey("FK_LEGAL_ACC_OWN_ACC_OWN", "REGOP_LEGAL_ACC_OWN", "ID", "REGOP_PERS_ACC_OWNER", "ID");

            Database.AddEntityTable("REGOP_PERS_ACC",
                new RefColumn("ROOM_ID", ColumnProperty.NotNull, "PERS_ACC_ROOM", "GKH_ROOM", "ID"),
                new RefColumn("ACC_OWNER_ID", ColumnProperty.NotNull, "PERS_ACC_ACC_OWN", "REGOP_PERS_ACC_OWNER", "ID"),
                new Column("ACC_NUM", DbType.String, 20, ColumnProperty.NotNull),
                new Column("AREA_SHARE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CHARGED_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PAID_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PENALTY_SUM", DbType.Decimal, ColumnProperty.NotNull)
                );

            Database.AddEntityTable("REGOP_PERS_ACC_CHARGE",
                new Column("CHARGE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("GUID", DbType.String, 40, ColumnProperty.NotNull),
                new Column("CHARGE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CHARGE_TARIFF", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PENALTY", DbType.Decimal, ColumnProperty.NotNull),
                new Column("RECALC", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("PERS_ACC_ID", ColumnProperty.NotNull, "PERS_ACC_CH_ACC", "REGOP_PERS_ACC", "ID")
                );

            Database.AddEntityTable("REGOP_PERS_ACC_PAYMENT",
                new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PAYMENT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("PERS_ACC_ID", ColumnProperty.NotNull, "PERS_ACC_PAY_ACC", "REGOP_PERS_ACC", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERS_ACC_PAYMENT");
            Database.RemoveTable("REGOP_PERS_ACC_CHARGE");
            Database.RemoveTable("REGOP_PERS_ACC");
            Database.RemoveTable("REGOP_LEGAL_ACC_OWN");
            Database.RemoveTable("REGOP_INDIVIDUAL_ACC_OWN");
            Database.RemoveTable("REGOP_PERS_ACC_OWNER");
            Database.RemoveTable("REGOP_RO_PAYMENT_ACC_OP");
            Database.RemoveTable("REGOP_RO_CHARGE_ACC_CHARGE");
            Database.RemoveTable("REGOP_RO_PAYMENT_ACCOUNT");
            Database.RemoveTable("REGOP_RO_CHARGE_ACCOUNT");
            Database.RemoveTable("REGOP_UNACCEPT_CHARGE");
            Database.RemoveTable("REGOP_UNACCEPT_C_PACKET");
            Database.RemoveTable("REGOP_BANK_ACCOUNT");
            Database.RemoveTable("REGOP_PERIOD");
        }
    }
}
