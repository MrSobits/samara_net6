namespace Bars.Gkh.Ris.Migrations.Version_2016061000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016061000")]
    [MigrationDependsOn(typeof(Version_2016060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("FULLNAME", DbType.String, 300));
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("SHORTNAME", DbType.String, 300));
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("OGRN", DbType.String, 250));
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("INN", DbType.String, 20));
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("KPP", DbType.String, 20));
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("OKOPF", DbType.String, 50));
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("ADDRESS", DbType.String, 500));
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("FIASHOUSEGUID", DbType.String, 1000));
            //this.Database.ChangeColumn("RIS_SUBSIDARY", new Column("SOURCENAME", DbType.String, 255));

            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("ORG_ROOT_ENTITY_GUID", DbType.String, 50));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("ACTIVITY_PLACE", DbType.String, 500));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("EVENT_DESC", DbType.String, 2000));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("RESULT_DOC_TYPE_CODE", DbType.String, 10));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("RESULT_DOC_TYPE_GUID", DbType.String, 50));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("RESULT_DOC_NUMBER", DbType.String, 300));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("BASE_CODE", DbType.String, 10));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("BASE_GUID", DbType.String, 50));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("EXAMFORM_CODE", DbType.String, 10));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("EXAMFORM_GUID", DbType.String, 50));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("LASTNAME", DbType.String, 100));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("FIRSTNAME", DbType.String, 100));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("MIDDLENAME", DbType.String, 100));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("OVERSIGHT_ACT_CODE", DbType.String, 10));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("OVERSIGHT_ACT_GUID", DbType.String, 50));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("OBJECTIVE", DbType.String, 2000));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("TASKS", DbType.String, 2000));
            //this.Database.ChangeColumn("RIS_INSPECTION_EXAMINATION", new Column("ORDER_NUMBER", DbType.String, 300));

            //this.Database.ChangeColumn("RIS_INSPECTION_EXAM_PLACE", new Column("FIAS_HOUSE_GUID", DbType.String, 50));
            
            //this.Database.ChangeColumn("RIS_INSPECTION_PRECEPT", new Column("NUMBER", DbType.String, 300));
            //this.Database.ChangeColumn("RIS_INSPECTION_PRECEPT", new Column("FIAS_HOUSE_GUID", DbType.String, 50));

            //this.Database.ChangeColumn("RIS_INSPECTION_OFFENCE", new Column("NUMBER", DbType.String, 300));
            //this.Database.ChangeColumn("RIS_INSPECTION_OFFENCE", new Column("CANCEL_DECISION_NUM", DbType.String, 300));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }
    }
}
