namespace Bars.GkhGji.Regions.Stavropol.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_STAVROPOL_ACTCHECK_TIME",
                new Column("HOUR", DbType.Int32),
                new Column("MINUTE", DbType.Int32),
                new RefColumn("ACT_CHECK_ID", ColumnProperty.NotNull, "ACT_CHECK_TIME", "GJI_ACTCHECK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_STAVROPOL_ACTCHECK_TIME");
        }
    }
}
