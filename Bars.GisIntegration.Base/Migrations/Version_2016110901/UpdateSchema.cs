namespace Bars.GisIntegration.Base.Migrations.Version_2016110901
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GisIntegration.Base.Extensions;

    // Миграции после 16.10.03 из остальных модулей
    [Migration("2016110901")]
    [MigrationDependsOn(typeof(Version_2016110900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //Bars.GisIntegration.RegOp.Migrations.Version_2016100400
            this.Database.AddRisEntityTable("RIS_CAPITAL_REPAIR_CHARGE",
            new RefColumn("PAYMENT_DOC_ID", "CAPITAL_REPAIR_CHARGE_PAYMENT_DOC_ID", "RIS_PAYMENT_DOC", "ID"),
            new Column("CONTRIBUTION", DbType.Decimal),
            new Column("ACCOUNTING_PERIOD_TOTAL", DbType.Decimal),
            new Column("MONEY_RECALCULATION", DbType.Decimal),
            new Column("MONEY_DISCOUNT", DbType.Decimal),
            new Column("TOTAL_PAYABLE", DbType.Decimal));

            this.Database.AddRisEntityTable("RIS_CAPITAL_REPAIR_DEBT",
               new RefColumn("PAYMENT_DOC_ID", "CAPITAL_REPAIR_DEBT_PAYMENT_DOC_ID", "RIS_PAYMENT_DOC", "ID"),
               new Column("MONTH", DbType.Decimal),
               new Column("YEAR", DbType.Decimal),
               new Column("TOTAL_PAYABLE", DbType.Decimal));

            //Bars.GisIntegration.RegOp.Migrations.Version_2016100500
            if (!this.Database.ColumnExists("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM"))
            {
                this.Database.AddColumn("RIS_PAYMENT_DOC", new Column("PAYMENT_DOC_NUM", DbType.String, 100));
            }

            //Bars.GisIntegration.RegOp.Migrations.Version_2016100600
            if (this.Database.ColumnExists("RIS_IND", "SURNAME"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("SURNAME", DbType.String, 100));
            }
            if (this.Database.ColumnExists("RIS_IND", "FIRSTNAME"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("FIRSTNAME", DbType.String, 100));
            }
            if (this.Database.ColumnExists("RIS_IND", "PATRONYMIC"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("PATRONYMIC", DbType.String, 100));
            }
            if (this.Database.ColumnExists("RIS_IND", "IDSERIES"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("IDSERIES", DbType.String, 200));
            }
            if (this.Database.ColumnExists("RIS_IND", "IDNUMBER"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("IDNUMBER", DbType.String, 200));
            }
            if (this.Database.ColumnExists("RIS_IND", "PLACEBIRTH"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("PLACEBIRTH", DbType.String, 300));
            }

            //Bars.GisIntegration.Inspection.Migrations.Version_2016101400
            this.Database.AddColumn("GI_INSPECTION_PLAN", "URI_REGISTRATION_NUMBER", DbType.Int32);
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "URI_REGISTRATION_NUMBER", DbType.Int32);
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "URI_REGISTRATION_DATE", DbType.DateTime);
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "PROSECUTOR_AGREEMENT_INFORMATION", DbType.String);
            this.Database.AddRefColumn("GI_INSPECTION_EXAMINATION", new RefColumn("GIS_CONTRAGENT", "GI_INSP_EXAM_CONTR", "GI_CONTRAGENT", "ID"));
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "ORG_ROOT_ENTITY_GUID");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "ACTIVITY_PLACE");

            //Bars.GisIntegration.Inspection.Migrations.Version_2016110200
            this.Database.AddColumn("GI_INSPECTION_EXAM_PLACE", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            this.Database.AddColumn("GI_INSPECTION_EXAM_PLACE", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull, "0"));
            this.Database.AddColumn("GI_INSPECTION_EXAM_PLACE", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            this.Database.AddRefColumn("GI_INSPECTION_EXAM_PLACE", new RefColumn("GI_CONTRAGENT_ID", "GI_EXAM_PLACE_CONTRAGENT", "GI_CONTRAGENT", "ID"));
            this.Database.AddColumn("GI_INSPECTION_EXAM_PLACE", new Column("GUID", DbType.String, 50));

            this.Database.AddColumn("GI_INSPECTION_PRECEPT_ATTACH", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            this.Database.AddColumn("GI_INSPECTION_PRECEPT_ATTACH", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull, "0"));
            this.Database.AddColumn("GI_INSPECTION_PRECEPT_ATTACH", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            this.Database.AddRefColumn("GI_INSPECTION_PRECEPT_ATTACH", new RefColumn("GI_CONTRAGENT_ID", "GI_PRECEPT_ATTACH_CONTRAGENT", "GI_CONTRAGENT", "ID"));
            this.Database.AddColumn("GI_INSPECTION_PRECEPT_ATTACH", new Column("GUID", DbType.String, 50));

            this.Database.AddColumn("GI_INSPECTION_EXAM_ATTACH", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            this.Database.AddColumn("GI_INSPECTION_EXAM_ATTACH", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull, "0"));
            this.Database.AddColumn("GI_INSPECTION_EXAM_ATTACH", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            this.Database.AddRefColumn("GI_INSPECTION_EXAM_ATTACH", new RefColumn("GI_CONTRAGENT_ID", "GI_EXAM_ATTACH_CONTRAGENT", "GI_CONTRAGENT", "ID"));
            this.Database.AddColumn("GI_INSPECTION_EXAM_ATTACH", new Column("GUID", DbType.String, 50));

            this.Database.AddColumn("GI_INSPECTION_OFFENCE_ATTACH", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            this.Database.AddColumn("GI_INSPECTION_OFFENCE_ATTACH", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull, "0"));
            this.Database.AddColumn("GI_INSPECTION_OFFENCE_ATTACH", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            this.Database.AddRefColumn("GI_INSPECTION_OFFENCE_ATTACH", new RefColumn("GI_CONTRAGENT_ID", "GI_OFFENCE_ATTACH_CONTRAGENT", "GI_CONTRAGENT", "ID"));
            this.Database.AddColumn("GI_INSPECTION_OFFENCE_ATTACH", new Column("GUID", DbType.String, 50));

            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("SHOULD_NOT_BE_REGISTERED", DbType.Boolean));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("FUNCTION_REGISTRY_NUMBER", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("AUTHORIZED_PERSONS", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("INVOLVED_EXPERTS", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("PRECEPT_GUID", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("OBJECT_CODE", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("OBJECT_GUID", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("IDENTIFIED_OFFENCES", DbType.String, 100000));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_FROM", DbType.DateTime));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_TO", DbType.DateTime));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_PLACE", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("FAMILIARIZATION_DATE", DbType.DateTime));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("IS_SIGNED", DbType.Boolean));
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("FAMILIARIZED_PERSON", DbType.String));

            this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("DEADLINE", DbType.DateTime));
            this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("IS_FULFILED_PRECEPT", DbType.Boolean));
            this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("CANCEL_REASON_GUID", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("ORG_ROOT_ENTITY_GUID", DbType.String));
            this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("IS_CANCELLED_AND_IS_FULFILED", DbType.Boolean));

            this.Database.AddColumn("GI_INSPECTION_OFFENCE", new Column("IS_FULFILED_OFFENCE", DbType.Boolean));

            this.Database.AddRisEntityTable("GI_EXAMINATION_OTHER_DOCUMENT",
                new RefColumn("EXAMINATION_ID", ColumnProperty.NotNull, "GI_EXAM_OTHER_DOC_EXAM", "GI_INSPECTION_EXAMINATION", "ID"),
                new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "GI_EXAM_OTHER_DOC_ATTACH", "GI_ATTACHMENT", "ID"));

            this.Database.AddRisEntityTable("GI_CANCEL_PRECEPT_ATTACH",
                new RefColumn("PRECEPT_ID", ColumnProperty.NotNull, "GI_CANCEL_PRECEPT_ATTACH_PRECEPT", "GI_INSPECTION_PRECEPT", "ID"),
                new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "GI_ANCEL_PRECEPT_ATTACH_ATT", "GI_ATTACHMENT", "ID"));

            //Bars.Gkh.Ris.Migrations.Version_2016100300
            this.Database.AddRisEntityTable("RIS_CAPITAL_REPAIR_CHARGE",
            new RefColumn("PAYMENT_DOC_ID", "RIS_CAPITAL_REPAIR_CHARGE_PAYMENT_DOC_ID", "RIS_PAYMENT_DOC", "ID"),
            new Column("CONTRIBUTION", DbType.Decimal),
            new Column("ACCOUNTING_PERIOD_TOTAL", DbType.Decimal),
            new Column("MONEY_RECALCULATION", DbType.Decimal),
            new Column("MONEY_DISCOUNT", DbType.Decimal),
            new Column("TOTAL_PAYABLE", DbType.Decimal));

            if (!this.Database.TableExists("RIS_CAPITAL_REPAIR_DEBT"))
            {
                this.Database.AddRisEntityTable(
                    "RIS_CAPITAL_REPAIR_DEBT",
                    new RefColumn("PAYMENT_DOC_ID", "RIS_CAPITAL_REPAIR_DEBT_PAYMENT_DOC_ID", "RIS_PAYMENT_DOC", "ID"),
                    new Column("MONTH", DbType.Decimal),
                    new Column("YEAR", DbType.Decimal),
                    new Column("TOTAL_PAYABLE", DbType.Decimal));
            }

            //Bars.Gkh.Ris.Migrations.Version_2016100400
            this.Database.AddRisEntityTable("HOME_PERIOD",
               new RefColumn("HOUSE_ID", "RIS_PERIOD_HOUSE_ID", "RIS_HOUSE", "ID"),
               new Column("FIAS_HOUSE_GUID", DbType.String),
               new Column("IS_UO", DbType.Boolean));

            //Bars.Gkh.Ris.Migrations.Version_2016100500
            if (!this.Database.ColumnExists("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM"))
            {
                this.Database.AddColumn("RIS_PAYMENT_DOC", new Column("PAYMENT_DOC_NUM", DbType.String, 100));
            }

            //Bars.Gkh.Ris.Migrations.Version_2016102000
            this.Database.AddColumn("RIS_CAPITAL_REPAIR_CHARGE", new Column("ORG_PPAGUID", DbType.String));

            this.Database.AddColumn("RIS_CAPITAL_REPAIR_DEBT", new Column("ORG_PPAGUID", DbType.String));

            this.Database.AddColumn("RIS_HOUSING_SERVICE_CHARGE_INFO", new Column("ORG_PPAGUID", DbType.String));

            this.Database.AddColumn("RIS_ADDITIONAL_SERVICE_CHARGE_INFO", new Column("ORG_PPAGUID", DbType.String));

            this.Database.AddColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", new Column("ORG_PPAGUID", DbType.String));

            this.Database.AddColumn("RIS_PAYMENT_DOC", new Column("PAYMENTS_TAKEN", DbType.Byte));

            this.Database.AddRisEntityTable(
                "RIS_INSURANCE",
                new Column("INSURANCE_PRODUCT_GUID", DbType.String),
                new Column("RATE", DbType.Decimal),
                new Column("ACCOUNTING_PERIOD_TOTAL", DbType.Decimal),
                new Column("CALC_EXPLANATION", DbType.String),
                new Column("TOTAL_PAYABLE", DbType.Decimal),
                new Column("MONEY_DISCOUNT", DbType.Decimal),
                new Column("MONEY_RECALCULATION", DbType.Decimal));

            //Bars.Gkh.Ris.Migrations.Version_2016102600
            this.Database.ChangeColumn("RIS_CHARTER", new Column("MANAGERS", DbType.String, 1000));

            //Bars.Gkh.Ris.Migrations.Version_2016102601
            this.Database.AddRisEntityTable("RIS_LIFT",
                new RefColumn("APARTAMENT_HOUSE", "RIS_LIFT_HOUSE", "RIS_HOUSE", "ID"),
                new Column("ENTRANCE_NUM", DbType.String),
                new Column("FACTORY_NUM", DbType.String),
                new Column("OPERATING_LIMIT", DbType.String),
                new Column("TERMINATION_DATE", DbType.DateTime),
                new Column("OGF_DATA_CODE", DbType.String),
                new Column("OGF_DATA_VALUE", DbType.String),
                new Column("TYPE_CODE", DbType.String),
                new Column("TYPE_GUID", DbType.String));

            this.Database.AddColumn("RIS_HOUSE", new Column("CONDITIONAL_NUMBER", DbType.String));
            this.Database.AddColumn("RIS_HOUSE", new Column("VALUE", DbType.String));
            this.Database.AddColumn("RIS_HOUSE", new Column("CODE", DbType.String));
            this.Database.AddColumn("RIS_HOUSE", new Column("TYPE", DbType.String));
            this.Database.AddColumn("RIS_HOUSE", new Column("REG_NUMBER", DbType.String));
            this.Database.AddColumn("RIS_HOUSE", new Column("REG_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveTable("RIS_LIFT");
            this.Database.RemoveColumn("RIS_HOUSE", "CONDITIONAL_NUMBER");
            this.Database.RemoveColumn("RIS_HOUSE", "VALUE");
            this.Database.RemoveColumn("RIS_HOUSE", "CODE");
            this.Database.RemoveColumn("RIS_HOUSE", "TYPE");
            this.Database.RemoveColumn("RIS_HOUSE", "REG_NUMBER");
            this.Database.RemoveColumn("RIS_HOUSE", "REG_DATE");

            this.Database.ChangeColumn("RIS_CHARTER", new Column("MANAGERS", DbType.String));

            this.Database.RemoveColumn("RIS_CAPITAL_REPAIR_CHARGE", "ORG_PPAGUID");
            this.Database.RemoveColumn("RIS_CAPITAL_REPAIR_DEBT", "ORG_PPAGUID");
            this.Database.RemoveColumn("RIS_HOUSING_SERVICE_CHARGE_INFO", "ORG_PPAGUID");
            this.Database.RemoveColumn("RIS_ADDITIONAL_SERVICE_CHARGE_INFO", "ORG_PPAGUID");
            this.Database.RemoveColumn("RIS_MUNICIPAL_SERVICE_CHARGE_INFO", "ORG_PPAGUID");
            this.Database.RemoveColumn("RIS_PAYMENT_DOC", "PAYMENTS_TAKEN");
            this.Database.RemoveTable("RIS_INSURANCE");

            if (this.Database.ColumnExists("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM"))
            {
                this.Database.RemoveColumn("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM");
            }

            this.Database.RemoveTable("HOME_PERIOD");

            this.Database.RemoveTable("RIS_CAPITAL_REPAIR_CHARGE");
            this.Database.RemoveTable("RIS_CAPITAL_REPAIR_DEBT");

            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "SHOULD_NOT_BE_REGISTERED");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "FUNCTION_REGISTRY_NUMBER");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "AUTHORIZED_PERSONS");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "INVOLVED_EXPERTS");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "PRECEPT_GUID");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "OBJECT_CODE");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "OBJECT_GUID");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "IDENTIFIED_OFFENCES");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "RESULT_FROM");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "RESULT_TO");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "FAMILIARIZATION_DATE");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "IS_SIGNED");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "FAMILIARIZED_PERSON");

            this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "DEADLINE");
            this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "IS_FULFILED_PRECEPT");
            this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "CANCEL_REASON_GUID");
            this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "ORG_ROOT_ENTITY_GUID");
            this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "IS_CANCELLED_AND_IS_FULFILED");

            this.Database.RemoveColumn("GI_INSPECTION_OFFENCE", "IS_FULFILED_OFFENCE");

            this.Database.RemoveTable("GI_EXAMINATION_OTHER_DOCUMENT");
            this.Database.RemoveTable("GI_CANCEL_PRECEPT_ATTACH");

            this.Database.RemoveColumn("GI_INSPECTION_PLAN", "URI_REGISTRATION_NUMBER");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "URI_REGISTRATION_NUMBER");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "URI_REGISTRATION_DATE");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "PROSECUTOR_AGREEMENT_INFORMATION");
            this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "GIS_CONTRAGENT");
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "ORG_ROOT_ENTITY_GUID", DbType.String, 50);
            this.Database.AddColumn("GI_INSPECTION_EXAMINATION", "ACTIVITY_PLACE", DbType.String, 500);

            this.Database.RemoveColumn("RIS_CAPITAL_REPAIR_CHARGE", "ORG_PPAGUID_REPAIRE_CHARGE");

            this.Database.RemoveColumn("RIS_CAPITAL_REPAIR_CHARGE", "ORG_PPAGUID");
            this.Database.RemoveColumn("RIS_CAPITAL_REPAIR_DEBT", "ORG_PPAGUID");
            this.Database.RemoveColumn("RIS_PAYMENT_DOC", "PAYMENTS_TAKEN");
            this.Database.RemoveTable("RIS_INSURANCE");

            if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "BANK_NAME"))
            {
                this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("BANK_NAME", DbType.String, 160));
            }
            if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_NAME"))
            {
                this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_NAME", DbType.String, 160));
            }
            if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "SUPPLIER_NAME"))
            {
                this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("SUPPLIER_NAME", DbType.String, 160));
            }

            if (this.Database.ColumnExists("RIS_IND", "SURNAME"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("SURNAME", DbType.String, 50));
            }
            if (this.Database.ColumnExists("RIS_IND", "FIRSTNAME"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("FIRSTNAME", DbType.String, 50));
            }
            if (this.Database.ColumnExists("RIS_IND", "PATRONYMIC"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("PATRONYMIC", DbType.String, 50));
            }
            if (this.Database.ColumnExists("RIS_IND", "IDSERIES"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("IDSERIES", DbType.String, 50));
            }
            if (this.Database.ColumnExists("RIS_IND", "IDNUMBER"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("IDNUMBER", DbType.String, 50));
            }
            if (this.Database.ColumnExists("RIS_IND", "PLACEBIRTH"))
            {
                this.Database.ChangeColumn("RIS_IND", new Column("PLACEBIRTH", DbType.String, 50));
            }

            if (this.Database.ColumnExists("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM"))
            {
                this.Database.RemoveColumn("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM");
            }

            this.Database.RemoveTable("RIS_CAPITAL_REPAIR_CHARGE");
            this.Database.RemoveTable("RIS_CAPITAL_REPAIR_DEBT");
        }
    }
}