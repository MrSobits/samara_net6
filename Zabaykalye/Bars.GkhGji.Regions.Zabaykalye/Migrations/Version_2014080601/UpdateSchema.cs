namespace Bars.GkhGji.Regions.Zabaykalye.Migrations.Version_2014080601
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Zabaykalye.Migrations.Version_2014080600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Таблица Группа Нарушений
            Database.AddEntityTable(
                "GJI_DOC_VIOLGROUP",
                new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "GJI_DOC_VIOLGROUP_D", "GJI_DOCUMENT", "ID"),
                new RefColumn("RO_ID", ColumnProperty.Null, "GJI_DOC_VIOLGROUP_RO", "GKH_REALITY_OBJECT", "ID"), // Нарушения могут быть без дома поэтому может быть null
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("ACTION", DbType.String, 500),
                new Column("DATE_PLAN_REMOVAL", DbType.DateTime),
                new Column("DATE_FACT_REMOVAL", DbType.DateTime));

            // Таблица Пункты по группе нарушений
            Database.AddEntityTable(
                "GJI_DOC_VIOLGROUP_POINT",
                new RefColumn("VIOLSTAGE_ID", ColumnProperty.NotNull, "GJI_DOC_VIOLGROUPPOINT_VS", "GJI_INSPECTION_VIOL_STAGE", "ID"),
                new RefColumn("VIOLGROUP_ID", ColumnProperty.NotNull, "GJI_DOC_VIOLGROUPPOINT_GR", "GJI_DOC_VIOLGROUP", "ID"));

            Database.AddEntityTable(
                "GJI_DOC_VIOLGROUP_LTEXT",
                new RefColumn("VIOLGROUP_ID", ColumnProperty.NotNull, "GJI_DOC_VIOLGROUP_LTEXT_G", "GJI_DOC_VIOLGROUP", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null),
                new Column("ACTION", DbType.Binary, ColumnProperty.Null));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DOC_VIOLGROUP_POINT");
            Database.RemoveTable("GJI_DOC_VIOLGROUP_LTEXT");
            Database.RemoveTable("GJI_DOC_VIOLGROUP");
        }
    }
}