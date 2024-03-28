namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014012800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014012700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_TOMSK_ACTCHECK_TIME",
                new Column("HOUR", DbType.Int32),
                new Column("MINUTE", DbType.Int32),
                new RefColumn("ACT_CHECK_ID", ColumnProperty.NotNull, "ACT_CHECK_TIME", "GJI_ACTCHECK", "ID"));

            Database.AddEntityTable(
                "GJI_TOMSK_PROVDOC_DATE",
                new Column("DATE_TIME_DOCUMENT", DbType.DateTime),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "PROVIDE_DOC_DATE", "GJI_DISPOSAL", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TOMSK_ACTCHECK_TIME");
            Database.RemoveTable("GJI_TOMSK_PROVDOC_DATE");
        }
    }
}
