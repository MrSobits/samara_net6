namespace Bars.GkhGji.Migrations._2015.Version_2015081400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015081400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015081300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_CONTRAGENT_AUDIT_PURP",
                new Column("LAST_INSP_DATE", DbType.DateTime),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull,  "GKH_CTRG_AUD_PUR_CTRG", "GKH_CONTRAGENT", "ID"),
                new RefColumn("AUDIT_PURPOSE_ID", ColumnProperty.NotNull, "GKH_CTRG_AUD_PUR_AP", "GJI_DICT_AUDIT_PURPOSE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_CONTRAGENT_AUDIT_PURP");
        }
    }
}