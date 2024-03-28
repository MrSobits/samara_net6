namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014060800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014042500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("DEC_GOV_DECISION",
                new Column("AUTH_PERSON", DbType.String, 200, ColumnProperty.Null),
                new Column("AUTH_PERSON_PHONE", DbType.String, 30, ColumnProperty.Null),
                new Column("DESTROY", DbType.Boolean, ColumnProperty.Null),
                new Column("DESTROY_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("FUND_BY_REGOP", DbType.Boolean, ColumnProperty.Null),
                new Column("MAX_FUND", DbType.Decimal, ColumnProperty.Null),
                new Column("PROTOCOL_NUM", DbType.String, 200, ColumnProperty.Null),
                new Column("PROTOCOL_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("REALTY_MANAG", DbType.String, ColumnProperty.Null),
                new Column("RECONSTRUCT", DbType.Boolean, ColumnProperty.Null),
                new Column("RECONSTR_START", DbType.DateTime, ColumnProperty.Null),
                new Column("RECONSTR_END", DbType.DateTime, ColumnProperty.Null),
                new Column("TAKE_APARTS", DbType.Boolean, ColumnProperty.Null),
                new Column("TAKE_APARTS_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("TAKE_LAND", DbType.Boolean, ColumnProperty.Null),
                new Column("TAKE_LAND_DATE", DbType.DateTime, ColumnProperty.Null),

                new RefColumn("STATE_ID", "GKH_GOV_D_PROT_STATE", "B4_STATE", "ID"),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_GOV_D_PROT_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("PROT_FILE_ID", ColumnProperty.Null, "GKH_GOV_D_PROT_FILE", "B4_FILE_INFO", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("DEC_GOV_DECISION");
        }
    }
}
