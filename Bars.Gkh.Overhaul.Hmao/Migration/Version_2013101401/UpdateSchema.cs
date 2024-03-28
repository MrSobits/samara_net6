namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101401
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "OVRHL_PROP_OWN_DECISION_BASE",
                new Column("MONTHLY_PAYMENT", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("PROP_OWNER_PROTOCOL_ID", ColumnProperty.NotNull, "OVRHL_DECISION_PROTOCOL", "OVRHL_PROP_OWN_PROTOCOLS", "ID"));

            Database.AddTable(
                "OVRHL_PROP_OWN_DECISION",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TYPE_ORGANIZATION", DbType.Int64, ColumnProperty.NotNull),
                new Column("MINIMAL_FUND_VOLUME", DbType.Decimal),
                new Column("ACCOUNT_NUMBER", DbType.String),
                new Column("ACCOUNT_CREATE_DATE", DbType.DateTime),
               // new RefColumn("REG_OPERATOR_ID", "OVRHL_DECISION_REG_OPER", "OVRHL_REG_OPERATOR", "ID"),
                new RefColumn("MANAGING_ORG_ID", "OVRHL_DECISION_MANORG", "GKH_MANAGING_ORGANIZATION", "ID"),
                new RefColumn("CREDIT_ORG_ID", "OVRHL_DECISION_CREDIT_ORG", "OVRHL_CREDIT_ORG", "ID"));

            Database.AddForeignKey("FK_OVRHL_PROP_OWN_DECISION", "OVRHL_PROP_OWN_DECISION", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");

            Database.AddTable(
                "OVRHL_PROP_OWN_DEC_REGOP",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique)
               // new RefColumn("REG_OPERATOR_ID", ColumnProperty.NotNull, "OVRHL_DEC_REGOP_REG_OPER", "OVRHL_REG_OPERATOR", "ID")
               );

            Database.AddForeignKey("FK_OVRHL_PROP_OWN_DEC_REGOP", "OVRHL_PROP_OWN_DEC_REGOP", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");

            Database.AddEntityTable(
                "OVRHL_PROP_OWN_DECISION_WORK",
                new RefColumn("PROP_OWN_DECISION_ID", "OVRHL_DECISION_WORK", "OVRHL_PROP_OWN_DECISION_BASE", "ID"),
                new RefColumn("WORK_ID", "OVRHL_DECISION_WORK_WORK", "GKH_DICT_WORK", "ID"));

            Database.AddUniqueConstraint("UNQ_PROP_OWN_DECISION_WORK", "OVRHL_PROP_OWN_DECISION_WORK", "PROP_OWN_DECISION_ID", "WORK_ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("OVRHL_PROP_OWN_DECISION_WORK", "UNQ_PROP_OWN_DECISION_WORK");
            Database.RemoveTable("OVRHL_PROP_OWN_DECISION_WORK");
            Database.RemoveTable("OVRHL_PROP_OWN_DECISION");
            Database.RemoveTable("OVRHL_PROP_OWN_DEC_REGOP");
            Database.RemoveTable("OVRHL_PROP_OWN_DECISION_BASE");
        }
    }
}