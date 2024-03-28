namespace Bars.Gkh.Regions.Samara.Migrations.Version_1
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
                "GKH_SAM_RO_OWNERCODE",
                new Column("OWNER_CODE", DbType.String, 100, ColumnProperty.Null),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_SAM_RO_OC_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_SAM_RO_OWNERCODE");
        }
    }
}