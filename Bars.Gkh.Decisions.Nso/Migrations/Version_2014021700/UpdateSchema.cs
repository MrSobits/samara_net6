namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014021700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014021202.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_DEC_NOTIF",
                new Column("NOTIF_NUMBER", DbType.String, 200, ColumnProperty.Null),
                new Column("NOTIF_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("ACCOUNT_NUM", DbType.String, 200, ColumnProperty.Null),
                new Column("OPEN_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("CLOSE_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("INCOME_NUM", DbType.String, 200, ColumnProperty.Null),
                new Column("REG_DATE", DbType.DateTime, ColumnProperty.Null),

                new Column("ORIG_INCOME", DbType.Boolean, ColumnProperty.Null),
                new Column("COPY_INCOME", DbType.Boolean, ColumnProperty.Null),
                new Column("COPY_PROTO_INCOME", DbType.Boolean, ColumnProperty.Null),

                new RefColumn("DOC_FILE_ID", "REGOP_NOT_DOCFILE", "B4_FILE_INFO", "ID"),
                new RefColumn("PROTO_FILE_ID", "REGOP_NOT_PROTOF", "B4_FILE_INFO", "ID"),
                new RefColumn("BANKDOC_FILE_ID", "REGOP_NOT_BANKF", "B4_FILE_INFO", "ID"),

                new RefColumn("RO_DEC_PROTO_ID", "REGOP_NOT_DP_FK", "GKH_OBJ_D_PROTOCOL", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_DEC_NOTIF");
        }
    }
}