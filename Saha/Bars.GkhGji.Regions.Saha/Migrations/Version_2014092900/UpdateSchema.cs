namespace Bars.GkhGji.Regions.Saha.Migrations.Version_2014092900
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Saha.Migrations.Version_2014090902.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_DOC_VIOLGROUP_POINT",
                new RefColumn("VIOLGROUP_ID", ColumnProperty.Null, "GJI_DOC_VIOLGROUPPOINT_GR", "GJI_DOC_VIOLGROUP", "ID"));
            Database.ChangeColumn("GJI_DOC_VIOLGROUP_POINT",
                new RefColumn("VIOLSTAGE_ID", ColumnProperty.Null, "GJI_DOC_VIOLGROUPPOINT_VS", "GJI_INSPECTION_VIOL_STAGE", "ID"));
            Database.ChangeColumn("GJI_DOC_VIOLGROUP_LTEXT",
                new RefColumn("VIOLGROUP_ID", ColumnProperty.Null, "GJI_DOC_VIOLGROUP_LTEXT_G", "GJI_DOC_VIOLGROUP", "ID"));
        }

        public override void Down()
        {
        }
    }
}