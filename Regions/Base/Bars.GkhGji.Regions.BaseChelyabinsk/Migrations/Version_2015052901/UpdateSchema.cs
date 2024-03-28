namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015052901
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015052901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015052900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_ACTCHECK_LTEXT",
                new RefColumn("DOCUMENT_ID", "GJI_ACTCHECK_LTEXT_DOC", "GJI_DOCUMENT", "ID"));

            this.Database.ExecuteNonQuery(
                "UPDATE GJI_ACTCHECK_LTEXT SET DOCUMENT_ID = ACTCHECK_ID WHERE ACTCHECK_ID IS NOT NULL");

            this.Database.AlterColumnSetNullable("GJI_ACTCHECK_LTEXT", "DOCUMENT_ID",false);

            this.Database.RemoveColumn("GJI_ACTCHECK_LTEXT", "ACTCHECK_ID");
        }

        public override void Down()
        {
            this.Database.AddRefColumn("GJI_ACTCHECK_LTEXT",
                new RefColumn("ACTCHECK_ID", "GJI_ACTCHECKRO_LTEXT_ACT", "GJI_ACTCHECK", "ID"));

            this.Database.ExecuteNonQuery(
                "UPDATE GJI_ACTCHECK_LTEXT SET ACTCHECK_ID = DOCUMENT_ID WHERE DOCUMENT_ID IS NOT NULL");

            this.Database.AlterColumnSetNullable("GJI_ACTCHECK_LTEXT", "ACTCHECK_ID", false);

            this.Database.RemoveColumn("GJI_ACTCHECK_LTEXT", "DOCUMENT_ID");
        }
    }
}