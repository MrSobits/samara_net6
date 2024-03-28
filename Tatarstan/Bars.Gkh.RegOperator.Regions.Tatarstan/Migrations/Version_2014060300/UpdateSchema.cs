namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2014060300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060300")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_CONFCONTRIB",
                new RefColumn("MANAGORG_ID", ColumnProperty.NotNull, "CONFCONTR_MANAG", "GKH_MANAGING_ORGANIZATION", "ID"));

            Database.AddEntityTable("REGOP_CONFCONTRIB_DOC",
                new RefColumn("CONFIRMCONTRIB_ID", ColumnProperty.NotNull, "CONF_DOC_CONTRIB", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("REALOBJ_ID", ColumnProperty.NotNull, "CONF_DOC_REALOBJ", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("SCAN_ID", "CONF_DOC_FILE", "B4_FILE_INFO", "ID"),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DATE_FROM", DbType.DateTime),
                new Column("TRANSFER_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_CONFCONTRIB");
            Database.RemoveTable("REGOP_CONFCONTRIB_DOC");
        }
    }
}