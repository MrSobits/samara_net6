namespace Bars.GkhGji.Regions.Samara.Migrations.Version_1
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_SAM_APPCITS_TESTER",
                new RefColumn("APPEALCIT_ID", ColumnProperty.NotNull, "SAM_GJI_APPCITS_TESTER_A", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("TESTER_ID", ColumnProperty.NotNull, "SAM_GJI_APPCITS_TESTER_T", "GKH_DICT_INSPECTOR", "ID"));

            Database.ExecuteNonQuery(@"insert into GJI_SAM_APPCITS_TESTER (ID, APPEALCIT_ID, TESTER_ID, object_version, object_create_date, object_edit_date)
                                     select id, id, TESTER_ID, object_version, object_create_date, object_edit_date from GJI_APPEAL_CITIZENS where TESTER_ID is not null");
            
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_SAM_APPCITS_TESTER");
        }
    }
}