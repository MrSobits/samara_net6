namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014021101
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014021100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("DEC_CREDIT_ORG_NOTIF",
                new Column("BANK_ACC_NUM", DbType.String, 40, ColumnProperty.Null),
                new Column("NOTIF_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("HAS_ORIG_NOTIF", DbType.Boolean, ColumnProperty.NotNull),
                new Column("HAS_PROT_COPY", DbType.Boolean, ColumnProperty.NotNull),
                new Column("HAS_REF_COPY", DbType.Boolean, ColumnProperty.NotNull),
                new Column("GJI_NUM", DbType.String, 40, ColumnProperty.Null),
                new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("REG_DATE", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("BANK_FILE_ID", ColumnProperty.Null, "CR_ORD_NOTIF_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("CR_ORG_ID", ColumnProperty.Null, "CR_ORD_NOTIF_CR_ORG", "OVRHL_CREDIT_ORG", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.Null, "CR_ORD_NOTIF_CR_ORG", "B4_STATE", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("DEC_CREDIT_ORG_NOTIF");
        }
    }
}