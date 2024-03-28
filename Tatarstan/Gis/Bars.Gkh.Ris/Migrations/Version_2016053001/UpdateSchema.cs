namespace Bars.Gkh.Ris.Migrations.Version_2016053001
{
    using System.Data;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using B4.Modules.Ecm7.Framework;

    [Migration("2016053001")]
    [MigrationDependsOn(typeof(Version_2016053000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisEntityTable("SUPPLY_RESOURCE_CONTRACT",
            //    new Column("COMPTETION_DATE", DbType.DateTime),
            //    new Column("START_DATE", DbType.Byte),
            //    new Column("START_DATE_NEXT_MONTH", DbType.Boolean),
            //    new Column("END_DATE", DbType.Byte),
            //    new Column("END_DATE_NEXT_MONTH", DbType.Boolean),
            //    new Column("CONTRACT_BASE_CODE", DbType.String),
            //    new Column("CONTRACT_BASE_GUID", DbType.String),
            //    new Column("CONTRACT_TYPE", DbType.Int16),
            //    new Column("PERSON_TYPE", DbType.Int16),
            //    new Column("PERSON_TYPE_ORGANIZATION", DbType.Int16),
            //    new RefColumn("CONTRAGENT_ID", "SR_CONTRACT_CONTRAGENT", "RIS_CONTRAGENT", "ID"),
            //    new Column("IND_SURNAME", DbType.String),
            //    new Column("IND_FIRSTNAME", DbType.String),
            //    new Column("IND_PATRONYMIC", DbType.String),
            //    new Column("IND_SEX", DbType.Int16),
            //    new Column("IND_DATE_OF_BIRTH", DbType.DateTime),
            //    new Column("IND_SNILS", DbType.String),
            //    new Column("IND_IDENTITY_TYPE_CODE", DbType.String),
            //    new Column("IND_IDENTITY_TYPE_GUID", DbType.String),
            //    new Column("IND_IDENTITY_SERIES", DbType.String),
            //    new Column("IND_IDENTITY_NUMBER", DbType.String),
            //    new Column("IND_IDENTITY_ISSUE_DATE", DbType.DateTime),
            //    new Column("IND_PLACE_BIRTH", DbType.String),
            //    new Column("COMM_METERING_RES_TYPE", DbType.Int16),
            //    new Column("FIAS_HOUSE_GUID", DbType.String),
            //    new Column("CONTRACT_NUMBER", DbType.String),
            //    new Column("SIGNING_DATE", DbType.DateTime),
            //    new Column("EFFECTIVE_DATE", DbType.DateTime),
            //    new Column("BILLING_DATE", DbType.Byte),
            //    new Column("PAYMENT_DATE", DbType.Byte),
            //    new Column("PROVIDING_INFORMATION_DATE", DbType.Byte),
            //    new Column("TERMINATE_REASON_CODE", DbType.String),
            //    new Column("TERMINATE_REASON_GUID", DbType.String),
            //    new Column("TERMINATE_DATE", DbType.DateTime),
            //    new Column("ROLLOVER_DATE", DbType.DateTime));

            //this.Database.AddRisEntityTable("SUP_RES_CONTRACT_TEMPERATURE_CHART",
            //    new RefColumn("CONTRACT_ID", "SR_CONTRACT_TEMP_CONTRACT", "SUPPLY_RESOURCE_CONTRACT", "ID"),
            //    new Column("OUTSIDE_TEMPERATURE", DbType.Int32),
            //    new Column("FLOWLINE_TEMPERATURE", DbType.String),
            //    new Column("OPPOSITELINE_TEMPERATURE", DbType.String));

            //this.Database.AddRisEntityTable("SUP_RES_CONTRACT_SERVICE_RESOURCE",
            //    new RefColumn("CONTRACT_ID", "SR_CONTRACT_SERV_RES_CONTRACT", "SUPPLY_RESOURCE_CONTRACT", "ID"),
            //    new Column("SERVICE_TYPE_CODE", DbType.String),
            //    new Column("SERVICE_TYPE_GUID", DbType.String),
            //    new Column("MUNICIPAL_RESOURCE_CODE", DbType.String),
            //    new Column("MUNICIPAL_RESOURCE_GUID", DbType.String),
            //    new Column("START_SUPPLY_DATE", DbType.DateTime),
            //    new Column("END_SUPPLY_DATE", DbType.DateTime));

            //this.Database.AddRisEntityTable("SUP_RES_CONTRACT_ATTACHMENT",
            //   new RefColumn("CONTRACT_ID", "SR_CONTRACT_ATTACH_CONTRACT", "SUPPLY_RESOURCE_CONTRACT", "ID"),
            //   new RefColumn("ATTACHMENT_ID", "SR_CONTRACT_ATTACH_ATTACH", "RIS_ATTACHMENT", "ID"));

            //this.Database.AddRisEntityTable("SUP_RES_CONTRACT_SUBJECT",
            //    new RefColumn("CONTRACT_ID", "SR_CONTRACT_SUBJ_CONTRACT", "SUPPLY_RESOURCE_CONTRACT", "ID"),
            //    new Column("SERVICE_TYPE_CODE", DbType.String),
            //    new Column("SERVICE_TYPE_GUID", DbType.String),
            //    new Column("MUNICIPAL_RESOURCE_CODE", DbType.String),
            //    new Column("MUNICIPAL_RESOURCE_GUID", DbType.String),
            //    new Column("HEATING_SYSTEM_TYPE", DbType.Int16),
            //    new Column("CONNECTION_SCHEME_TYPE", DbType.Int16),
            //    new Column("START_SUPPLY_DATE", DbType.DateTime),
            //    new Column("END_SUPPLY_DATE", DbType.DateTime),
            //    new Column("PLANNED_VOLUME", DbType.Decimal),
            //    new Column("UNIT", DbType.String),
            //    new Column("FEEDING_MODE", DbType.String));

            //this.Database.AddRisEntityTable("SUP_RES_CONTRACT_SUBJECT_OTHER_QUALITY",
            //   new RefColumn("CONTRACT_SUBJECT_ID", "SR_CONTR_SUBJ_Q_CS", "SUP_RES_CONTRACT_SUBJECT", "ID"),
            //   new Column("INDICATOR_NAME", DbType.String),
            //   new Column("NUMBER", DbType.Decimal),
            //   new Column("OKEI", DbType.String));
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("SUP_RES_CONTRACT_SUBJECT_OTHER_QUALITY");
            //this.Database.RemoveTable("SUP_RES_CONTRACT_SUBJECT");
            //this.Database.RemoveTable("SUP_RES_CONTRACT_ATTACHMENT");
            //this.Database.RemoveTable("SUP_RES_CONTRACT_SERVICE_RESOURCE");
            //this.Database.RemoveTable("SUP_RES_CONTRACT_TEMPERATURE_CHART");
            //this.Database.RemoveTable("SUPPLY_RESOURCE_CONTRACT");
        }
    }
}