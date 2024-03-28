namespace Bars.GisIntegration.Base.Migrations.Version_2016062800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016062800")]
    [MigrationDependsOn(typeof(Bars.Gkh.Quartz.Scheduler.Migrations.Version_2016050100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GI_ATTACHMENT",
                new RefColumn("FILE_INFO_ID", "GI_ATTACH_FILE_INFO", "B4_FILE_INFO", "ID"),
                new Column("GUID", DbType.String, 50),
                new Column("HASH", DbType.String, 200),
                new Column("NAME", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 50));

            this.Database.AddEntityTable("GI_INTEGR_DICT",
               new Column("NAME", DbType.String, 500),
               new Column("ACTION_CODE", DbType.String, 500),
               new Column("REGISTRY_NUMBER", DbType.String, 50),
               new Column("DATE_INTEG", DbType.DateTime));

            this.Database.AddEntityTable("GI_INTEGR_REF_DICT",
                new RefColumn("DICT_ID", "GI_INTEGR_REF_DICT", "GI_INTEGR_DICT", "ID"),
                new Column("CLASS_NAME", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("GIS_REC_ID", DbType.String, 10),
                new Column("GIS_REC_NAME", DbType.String, 1000),
                new Column("GKH_REC_ID", DbType.Int64),
                new Column("GKH_REC_NAME", DbType.String, 1000),
                new Column("GIS_REC_GUID", DbType.String, 50));

            this.Database.AddEntityTable("GI_CONTRAGENT",
                new Column("GKHID", DbType.Int64),
                new Column("FULLNAME", DbType.String, 1000),
                new Column("OGRN", DbType.String, 50),
                new Column("ORGROOTENTITYGUID", DbType.String, 50),
                new Column("ORGVERSIONGUID", DbType.String, 50),
                new Column("SENDERID", DbType.String, 50),
                new Column("FACT_ADDRESS", DbType.String, 500),
                new Column("JUR_ADDRESS", DbType.String, 500),
                new Column("IS_INDIVIDUAL", DbType.Boolean));

            this.Database.AddPersistentObjectTable("GI_CONTAINER",
                new RefColumn("CONTRAGENT_ID", "GI_CONTAINER_CONTRAGENT", "GI_CONTRAGENT", "ID"),
                new Column("CONTAINER_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("UPLOAD_DATE", DbType.DateTime),
                new Column("METHOD_CODE", DbType.String, 200));

            this.Database.AddEntityTable("GI_PACKAGE",
                 new Column("NOT_SIGNED_DATA", DbType.Binary, ColumnProperty.Null),
                 new Column("SIGNED_DATA", DbType.Binary, ColumnProperty.Null),
                 new Column("TRANSPORT_GUID_DICTIONARY", DbType.Binary, ColumnProperty.Null),
                 new Column("NAME", DbType.String, 250),
                 new Column("USER_NAME", DbType.String, 200),
                 new Column("STATE", DbType.Int16),
                 new RefColumn("CONTRAGENT_ID", "GI_PACK_CONTR", "GI_CONTRAGENT", "ID"));

            this.Database.AddEntityTable("GI_PACKAGE_TRIGGER",
                new RefColumn("PACKAGE_ID", "GI_PACKAGE_TRIGGER_PACKAGE", "GI_PACKAGE", "ID"),
                new RefColumn("TRIGGER_ID", "GI_PACKAGE_TRIGGER_TRIGGER", "SCHDLR_TRIGGER", "ID"),
                new Column("MESSAGE", DbType.String, 10000),
                new Column("PROCESSING_RESULT", DbType.Binary, ColumnProperty.Null),
                new Column("ACK_MESSAGE_GUID", DbType.String, 100));

            this.Database.AddEntityTable("GI_TASK",
                  new Column("CLASS_NAME", DbType.String, 200),
                  new Column("DESCRIPTION", DbType.String, 500),
                  new Column("START_TIME", DbType.DateTime),
                  new Column("END_TIME", DbType.DateTime),
                  new Column("USER_NAME", DbType.String, 200));

            this.Database.AddEntityTable("GI_TASK_TRIGGER",
               new RefColumn("TASK_ID", "GI_TASK_TRIGGER_TASK", "GI_TASK", "ID"),
               new RefColumn("TRIGGER_ID", "GI_TASK_TRIGGER_TRIGGER", "SCHDLR_TRIGGER", "ID"),
               new Column("TRIGGER_TYPE", DbType.Int16));

            this.Database.AddEntityTable("GI_SERVICE_SETTINGS",
                new Column("INTEGRATION_SERVICE", DbType.Int32, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull),
                new Column("ADDRESS", DbType.String, 255),
                new Column("ASYNC_ADDRESS", DbType.String, 255));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GI_SERVICE_SETTINGS");
            this.Database.RemoveTable("GI_TASK_TRIGGER");
            this.Database.RemoveTable("GI_TASK");
            this.Database.RemoveTable("GI_PACKAGE_TRIGGER");
            this.Database.RemoveTable("GI_PACKAGE");
            this.Database.RemoveTable("GI_CONTAINER");
            this.Database.RemoveTable("GI_CONTRAGENT");
            this.Database.RemoveTable("GI_INTEGR_REF_DICT");
            this.Database.RemoveTable("GI_INTEGR_DICT");
            this.Database.RemoveTable("GI_ATTACHMENT");
        }
    }
}
