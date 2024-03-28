namespace Bars.GisIntegration.Inspection.Migrations.Version_2016101400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016101400")]
    [MigrationDependsOn(typeof(Version_2016080900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddColumn("GI_INSPECTION_PLAN", "URI_REGISTRATION_NUMBER", DbType.Int32);
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "URI_REGISTRATION_NUMBER", DbType.Int32);
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "URI_REGISTRATION_DATE", DbType.DateTime);
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "PROSECUTOR_AGREEMENT_INFORMATION", DbType.String);
            //this.Database.AddRefColumn("GI_INSPECTION_EXAMINATION", new RefColumn("GIS_CONTRAGENT", "GI_INSP_EXAM_CONTR", "GI_CONTRAGENT", "ID"));
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "ORG_ROOT_ENTITY_GUID");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "ACTIVITY_PLACE");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("GI_INSPECTION_PLAN", "URI_REGISTRATION_NUMBER");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "URI_REGISTRATION_NUMBER");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "URI_REGISTRATION_DATE");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "PROSECUTOR_AGREEMENT_INFORMATION");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "GIS_CONTRAGENT");
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "ORG_ROOT_ENTITY_GUID", DbType.String, 50);
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "ACTIVITY_PLACE", DbType.String, 500);
        }
    }
}
