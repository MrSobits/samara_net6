namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020092200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020092200")]
    [MigrationDependsOn(typeof(Version_2020031700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //-----Справочник кодов регионов
            Database.AddEntityTable(
                "GJI_CH_DICT_FLDOC_TYPE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));

            Database.AddEntityTable(
             "GJI_CH_DICT_PAYER_STATUS",
             new Column("NAME", DbType.String, 5000, ColumnProperty.NotNull),
             new Column("CODE", DbType.String, 4, ColumnProperty.NotNull));

            InsertPayerStatus();

            Database.AddEntityTable("GJI_CH_LICENSE_REISSUANCE",
                 new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_REISSUANCE_C", "GKH_CONTRAGENT", "ID"),
                 new RefColumn("LICENSE_ID", ColumnProperty.Null, "REISS_GKH_MANORG_LIC", "GKH_MANORG_LICENSE", "ID"),
                 new RefColumn("STATE_ID", ColumnProperty.Null, "GJI_LICENSE_REISSUANCE", "B4_STATE", "ID"),
                 new Column("REISSUANCE_DATE", DbType.DateTime),
                 new Column("REG_NUMBER", DbType.String, 100),
                 new Column("REG_NUM", DbType.Int64, 22),
                 new Column("CONF_DUTY", DbType.String, 1000));

            Database.AddEntityTable("GJI_CH_LICENSE_REISSUANCE_PERSON",
                    new RefColumn("LIC_REISSUANCE_ID", ColumnProperty.NotNull, "PERSON_REISSUANCE_GJI_LR", "GJI_CH_LICENSE_REISSUANCE", "ID"),
                    new RefColumn("PERSON_ID", ColumnProperty.NotNull, "PERSON_REISSUANCE_PERSON_P", "GKH_PERSON", "ID"));

            Database.AddEntityTable("GJI_CH_LICENSE_REISSUANCE_PROVDOC",
                    new RefColumn("LIC_REISSUANCE_ID", ColumnProperty.NotNull, "REISSUANCE_PROVDOC_GJI_LR", "GJI_CH_LICENSE_REISSUANCE", "ID"),
                    new RefColumn("LIC_PROVDOC_ID", ColumnProperty.NotNull, "REISSUANCE_PROVDOC_PROVDOC_LP", "GKH_DICT_LIC_PROVDOC", "ID"),
                    new RefColumn("LIC_PROVDOC_FILE_ID", ColumnProperty.None, "REISSUANCE_PROVDOC_FILE_INFO", "B4_FILE_INFO", "ID"),
                    new Column("LIC_PROVDOC_NUMBER", DbType.String, 100),
                    new Column("LIC_PROVDOC_DATE", DbType.DateTime));

            Database.AddEntityTable(
                     "GJI_CH_GIS_GMP",
                     new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
                     new Column("BILL_DATE", DbType.DateTime, ColumnProperty.NotNull),
                     new Column("BILL_FOR", DbType.String, ColumnProperty.NotNull),
                     new Column("DOCUMENT_NUMBER", DbType.String, 500),
                     new Column("DOCUMENT_SERIAL", DbType.String, 500),
                     new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                     new Column("IDENT_TYPE", DbType.Int32, 4, ColumnProperty.None),
                     new Column("PAYER_TYPE", DbType.Int32, 4, ColumnProperty.None),
                     new Column("ANSWER", DbType.String, 500),
                     new Column("INN", DbType.String, 20),
                     new Column("CITIZENSHIP", DbType.Boolean),
                     new Column("KBK", DbType.String, 500),
                     new Column("KIO", DbType.String, 5),
                     new Column("RECONCILE_ANSWER", DbType.String, 100),
                     new Column("KPP", DbType.String, 20),
                     new Column("OKTMO", DbType.String, 20),
                     new Column("PAYMENT_TYPE", DbType.String, 500),
                     new Column("STATE", DbType.String, 500),
                     new Column("TAX_DATE", DbType.String),
                     new Column("CHARGE_ID", DbType.String, 100),
                     new Column("TAX_NUM", DbType.String),
                     new Column("TAX_PERIOD", DbType.String),
                     new Column("PAY_KBK", DbType.String, ColumnProperty.None),
                     new Column("ALT_PAYER_IDENTIFIER_SENT", DbType.String, 25),
                     new Column("BILL_DATE_SENT", DbType.DateTime),
                     new Column("BILL_FOR_SENT", DbType.String, 255),
                     new Column("DOC_NUM_DATE", DbType.String, ColumnProperty.None),
                     new Column("KBK_SENT", DbType.String, 500),
                     new Column("OKTMO_SENT", DbType.String, 20),
                     new Column("LEGAL_ACT", DbType.String, 255),
                     new Column("LEGAL_ACT_SENT", DbType.String, 255),
                     new Column("PAYER_NAME", DbType.String, 255),
                     new Column("PAYER_NAME_SENT", DbType.String, 255),
                     new Column("PAYT_REASON_SENT", DbType.String, 2),
                     new Column("STATE_SENT", DbType.String, 500),
                     new Column("TAX_DATE_SENT", DbType.String, 255),
                     new Column("TAX_NUM_SENT", DbType.String, 255),
                     new Column("TAX_PERIOD_SENT", DbType.String, 255),
                     new Column("TOTAL_AMMOUNT_SENT", DbType.Decimal),
                     new Column("REASON", DbType.String, 512),
                     new Column("PAY_END_DATE", DbType.DateTime, ColumnProperty.None),
                     new Column("PAY_START_DATE", DbType.DateTime, ColumnProperty.None),
                     new Column("CHARGE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 1),
                     new Column("UIN", DbType.String, 27, ColumnProperty.None),
                     new Column("RESOLUTION_ID", DbType.Int32, ColumnProperty.None),
                     new Column("PROTOCOL_ID", DbType.Int32, ColumnProperty.None),
                     new Column("TOTAL_AMMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                     new Column("MESSAGEID", DbType.String, 36, ColumnProperty.Null),
                     new Column("PAYT_REASON", DbType.String, 2, ColumnProperty.Null),
                     new Column("ALT_PAYER_IDENTIFIER", DbType.String, 25),
                     new Column("MANORG_LIC_REQUEST_ID", DbType.Int32, ColumnProperty.None),
                     new Column("REISSUANCE_ID", DbType.Int32, ColumnProperty.None),
                     new Column("LIC_REQ_TYPE", DbType.Int32, 4, ColumnProperty.None),
                     new Column("GIS_GMP_PAYMENTS_KIND", DbType.Int32, 4, ColumnProperty.None),
                     new Column("GIS_GMP_PAYMENTS_TYPE", DbType.Int32, 4, ColumnProperty.None, 1),
                     new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_GIS_GMP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
                     new RefColumn("PAYER_STATUS_ID", ColumnProperty.None, "FK_GIS_GMP_PAYER_STATUS", "GJI_CH_DICT_PAYER_STATUS", "ID"),
                     new RefColumn("PHYSICALPERSON_DOCTYPE_ID", ColumnProperty.None, "GJI_CH_GIS_GMP_PHYSICALPERSON_DOCTYPE_ID", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"),
                     new RefColumn("FLDOCTYPE_ID", ColumnProperty.None, "GJI_CH_GIS_GMP_FLDOCTYPE_ID", "GJI_CH_DICT_FLDOC_TYPE", "ID"));

            Database.AddEntityTable(
                    "GJI_CH_GIS_GMP_FILE",
                    new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
                    new RefColumn("GIS_GMP_ID", ColumnProperty.None, "GJI_CH_GIS_GMP_ID", "GJI_CH_GIS_GMP", "ID"),
                    new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_GIS_GMP_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
           "GJI_CH_GIS_GMP_PAYMENTS",
           new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
           new Column("KBK", DbType.String),
           new Column("OKTMO", DbType.String),
           new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.None),
           new Column("PAYMENT_ID", DbType.String),
           new Column("PRPOSE", DbType.String),
           new Column("RECONSILE", DbType.Int32, 4, ColumnProperty.NotNull, 20),
           new Column("UIN", DbType.String),
           new RefColumn("GIS_GMP_ID", ColumnProperty.None, "PAYM_GJI_CH_GIS_GMP_ID", "GJI_CH_GIS_GMP", "ID"),
           new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "PAYM_GJI_CH_GIS_GMP_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
        "GJI_CH_PAY_REG_REQUESTS",
        new Column("MESSAGEID", DbType.String, 36, ColumnProperty.Null),
        new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_PAY_REG_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
        new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
        new Column("ANSWER", DbType.String, 500),
        new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
        new Column("PAY_REG_PAYMENTS_KIND", DbType.Int32, 4, ColumnProperty.None),
        new Column("PAY_REG_PAYMENTS_TYPE", DbType.Int32, 4, ColumnProperty.None, 1),
        new Column("PAY_START_DATE", DbType.DateTime, ColumnProperty.None),
        new Column("PAY_END_DATE", DbType.DateTime, ColumnProperty.None));

            Database.AddEntityTable(
          "GJI_CH_PAY_REG_FILE",
          new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
          new RefColumn("PAY_REG_REQUESTS_ID", ColumnProperty.None, "GJI_CH_PAY_REG_REQUESTS_ID", "GJI_CH_PAY_REG_REQUESTS", "ID"),
          new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_PAY_REG_FILE_INFO_ID", "B4_FILE_INFO", "ID"));


            Database.AddEntityTable(
                "GJI_CH_PAY_REG",
                new RefColumn("GIS_GMP_ID", ColumnProperty.Null, "GJI_CH_PAY_REG_GIS_GMP_ID", "GJI_CH_GIS_GMP", "ID"),
                new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("KBK", DbType.String),
                new Column("OKTMO", DbType.String),
                new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.None),
                new Column("PAYMENT_ID", DbType.String),
                new Column("PURPOSE", DbType.String),
                new Column("UIN", DbType.String),
                new Column("PAYMENT_ORG", DbType.String),
                new Column("PAYMENT_ORG_DESCR", DbType.String),
                new Column("IS_PAYFINE_ADDED", DbType.Boolean, false),
                new Column("PAYER_ID", DbType.String),
                new Column("PAYER_ACCOUNT", DbType.String),
                new Column("PAYER_NAME", DbType.String),
                new Column("BDI_STATUS", DbType.String),
                new Column("BDI_PAYT_REASON", DbType.String),
                new Column("BDI_TAXPERIOD", DbType.String),
                new Column("BDI_TAXDOCNUMBER", DbType.String),
                new Column("BDI_TAXDOCDATE", DbType.String),
                new Column("ACCDOCDATE", DbType.DateTime),
                new Column("ACCDOCNO", DbType.String),
                new Column("RECONSILE", DbType.Int32, 4, ColumnProperty.NotNull, 20),
                new Column("STATUS", DbType.Byte));

            CopyGISGMPPaymentsToPayReg();

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_PAY_REG");
            Database.RemoveTable("GJI_CH_PAY_REG_FILE");
            Database.RemoveTable("GJI_CH_PAY_REG_REQUESTS");
            Database.RemoveTable("GJI_CH_GIS_GMP_PAYMENTS");
            Database.RemoveTable("GJI_CH_GIS_GMP_FILE");
            Database.RemoveTable("GJI_CH_GIS_GMP");
            Database.RemoveTable("GJI_CH_LICENSE_REISSUANCE_PROVDOC");
            Database.RemoveTable("GJI_CH_LICENSE_REISSUANCE_PERSON");
            Database.RemoveTable("GJI_CH_LICENSE_REISSUANCE");        
            Database.RemoveTable("GJI_CH_DICT_PAYER_STATUS");
            Database.RemoveTable("GJI_CH_DICT_FLDOC_TYPE");
        }

        private void InsertPayerStatus()
        {
            var sql = @"INSERT INTO GJI_CH_DICT_PAYER_STATUS
                (object_version, object_create_date, object_edit_date, code, name) VALUES
                (0, now(), now(), '01', 'Юридическое лицо - налогоплательщик (плательщик сборов)'),
                (0, now(), now(), '02', 'Налоговый агент'),
                (0, now(), now(), '03', 'Организация федеральной почтовой связи, составившая распоряжение о переводе денежных средств по каждому платежу физического лица.'),
                (0, now(), now(), '04', 'Налоговый орган'),
                (0, now(), now(), '05', 'Территориальные органы Федеральной службы судебных приставов'),
                (0, now(), now(), '06', 'Участник внешнеэкономической деятельности- юридическое лицо'),
                (0, now(), now(), '07', 'Таможенный орган'),
                (0, now(), now(), '08', 'Плательщик - юридическое лицо (индивидуальный предприниматель), осуществляющее перевод денежных средств в уплату страховых взносов и иных платежей в бюджетную систему Российской Федерации.'),
                (0, now(), now(), '09', 'Налогоплательщик (плательщик сборов) - индивидуальный предприниматель'),
                (0, now(), now(), '10', 'Налогоплательщик (плательщик сборов) - нотариус, занимающийся частной практикой'),
                (0, now(), now(), '11', 'Налогоплательщик (плательщик сборов) - адвокат, учредивший адвокатский кабинет'),
                (0, now(), now(), '12', 'Налогоплательщик (плательщик сборов) - глава крестьянского (фермерского) хозяйства'),
                (0, now(), now(), '13', 'Налогоплательщик (плательщик сборов) - иное физическое лицо - клиент банка (владелец счета)'),
                (0, now(), now(), '14', 'Налогоплательщики, производящие выплаты физическим лицам'),
                (0, now(), now(), '15', 'Кредитная организация (филиал кредитной организации), платёжный агент, организация федеральной почтовой связи, составившие платёжное поручение на общую сумму с реестром на перевод денежных средств, принятых от плательщиков - физических лиц.'),
                (0, now(), now(), '16', 'Участник внешнеэкономической деятельности - физическое лицо'),
                (0, now(), now(), '17', 'Участник внешнеэкономической деятельности - индивидуальный предприниматель'),
                (0, now(), now(), '18', 'Плательщик таможенных платежей, не являющийся декларантом, на которого законодательством Российской Федерации возложена обязанность по уплате таможенных платежей.'),
                (0, now(), now(), '19', 'Организации и их филиалы (далее - организации), составившие распоряжение о переводе денежных средств, удержанных из заработной платы (дохода) должника - физического лица в счёт погашения задолженности по платежам в бюджетную систему Российской Федерации на основании исполнительного документа, направленного в организацию в установленном порядке.'),
                (0, now(), now(), '20', 'Кредитная организация (филиал кредитной организации), платёжный агент, составившие распоряжение о переводе денежных средств по каждому платежу физического лица.'),
                (0, now(), now(), '21', 'Ответственный участник консолидированной группы налогоплательщиков.'),
                (0, now(), now(), '22', 'Участник консолидированной группы налогоплательщиков.'),
                (0, now(), now(), '23', 'Органы контроля за уплатой страховых взносов.'),
                (0, now(), now(), '24', 'Плательщик - физическое лицо, осуществляющее перевод денежных средств в уплату страховых взносов и иных платежей в бюджетную систему Российской Федерации'),
                (0, now(), now(), '25', 'Банки - гаранты, составившие распоряжение о переводе денежных средств в бюджетную систему Российской Федерации при возврате налога на добавленную стоимость, излишне полученной налогоплательщиком (зачтенной ему) в заявительном порядке, а также при уплате акцизов, исчисленных по операциям реализации подакцизных товаров за пределы территории Российской Федерации, и акцизов по алкогольной и (или) подакцизной спиртосодержащей продукции.'),
                (0, now(), now(), '26', 'Учредители (участники) должника, собственники имущества должника - унитарного предприятия или третьи лица, составившие распоряжение о переводе денежных средств на погашение требований к должнику по уплате обязательных платежей, включённых в реестр требований кредиторов, в ходе процедур, применяемых в деле о банкротстве.');";

            this.Database.ExecuteNonQuery(sql);
        }

        private void CopyGISGMPPaymentsToPayReg()
        {
            var sql = @"INSERT INTO GJI_CH_PAY_REG
                (object_version, object_create_date, object_edit_date, amount, kbk, oktmo, payment_date, payment_id, purpose, uin, gis_gmp_id, reconsile)
                SELECT object_version, object_create_date, object_edit_date, amount, kbk, oktmo, payment_date, payment_id, prpose, uin, gis_gmp_id, reconsile
                FROM GJI_CH_GIS_GMP_PAYMENTS;";

            this.Database.ExecuteNonQuery(sql);

        }
    }
}


