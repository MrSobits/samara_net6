namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015121600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015121100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_TOMSK_DISP_VERIFSUBJ_LIC",
				new RefColumn("SURVEY_SUBJECT_LIC_ID", "GJI_TOMSK_DISP_VERIFSUBJ_LIC_SSD", "GJI_DICT_SURVEY_SUBJ_LICENSING", "ID"),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "DISP_VERIFSUBJLIC", "GJI_DISPOSAL", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_TOMSK_DISP_VERIFSUBJ_LIC");
        }
    }
}
