namespace Bars.GisIntegration.Inspection.Migrations.Version_2016080900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GisIntegration.Base.Extensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016080900")]
    [MigrationDependsOn(typeof(Version_2016062900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("EXAMFORM_CODE", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("EXAMFORM_GUID", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("ORDER_NUMBER", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("ORG_ROOT_ENTITY_GUID", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("ACTIVITY_PLACE", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("FIRSTNAME", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("LASTNAME", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("MIDDLENAME", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("OVERSIGHT_ACT_CODE", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("OVERSIGHT_ACT_GUID", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("BASE_CODE", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("BASE_GUID", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_DOC_TYPE_CODE", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_DOC_TYPE_GUID", DbType.String));
            //this.Database.ChangeColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_DOC_NUMBER", DbType.String));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }
    }
}
