namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2020071300
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2020071300")]
    [MigrationDependsOn(typeof(Version_202031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {    
            Database.AddEntityTable(
           "GJI_CH_COURT_PRACTICE_GJIDOC",
           new RefColumn("CP_ID", ColumnProperty.NotNull, "FK_CPDOC_COURT_PRACTICE", "GJI_CH_COURT_PRACTICE", "ID"),
           new RefColumn("DOCUMENT_ID", ColumnProperty.None, "FK_CPDOC_DOCUMENT_ID", "GJI_DOCUMENT", "ID"));

           InsertDocumentsIntoCourtPractice();
            UpdateRemindersInspector();

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_COURT_PRACTICE_GJIDOC");         
        }


        private void InsertDocumentsIntoCourtPractice()
        {
            var sql = @"insert into GJI_CH_COURT_PRACTICE_GJIDOC (CP_ID, DOCUMENT_ID, object_create_date, object_edit_date, object_version)
                        select id CP_ID, DOCUMENT_ID, object_create_date, object_edit_date, object_version from GJI_CH_COURT_PRACTICE where document_id >0";

            this.Database.ExecuteNonQuery(sql);
        }
        private void UpdateRemindersInspector()
        {
            var sql = @"drop table if exists tmp_executants;
            create table tmp_executants as(
            select chr.id, ex.executant_id from GJI_APPCIT_EXECUTANT ex
            join CHELYABINSK_GJI_REMINDER chr on chr.APPEAL_CITS_EXECUTANT_ID = ex.id);
            update gji_reminder set inspector_id = ex.executant_id
            from tmp_executants ex where gji_reminder.id = ex.id and inspector_id is null";

            this.Database.ExecuteNonQuery(sql);
        }
    }
}