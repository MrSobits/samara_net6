namespace Bars.Gkh.Migrations._2016.Version_2016052000
{
    using System.Data;
    using System.Linq;

    using B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция '2016.05.20.00'
    /// </summary>
    [Migration("2016052000")]
    [MigrationDependsOn(typeof(Version_2016050500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_COMM_RESOURCE",
               new Column("NAME", DbType.String, 255),
               new Column("CODE", DbType.String, 255));

            this.Database.AddEntityTable("GKH_DICT_STOP_REASON",
               new Column("NAME", DbType.String, 255),
               new Column("CODE", DbType.String, 255));

            if (!this.Database.TableExists("GKH_RO_PUB_SERVORG"))
            {
                this.Database.AddEntityTable(
                 "GKH_RO_PUB_SERVORG",
                 new RefColumn("PUB_SERVORG_ID", "GKH_RO_PUB_SERVORG", "GKH_PUBLIC_SERVORG", "ID"),
                 new RefColumn("REAL_OBJ_ID", "GKH_RO_PUB_S_ORG_RO", "GKH_REALITY_OBJECT", "ID"),
                 new RefColumn("FILE_INFO_ID", "GKH_PUBSERV_FILE", "B4_FILE_INFO", "ID"),
                 new Column("DATE_START", DbType.DateTime),
                 new Column("DATE_END", DbType.DateTime),
                 new Column("CONTRACT_NUMBER", DbType.String),
                 new Column("CONTRACT_DATE", DbType.DateTime),
                 new Column("NOTE", DbType.String, 300));
            }

            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("TERM_BILLING_PYMNT_LATER", DbType.Int32));
            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("TERM_PYMNT_LATER", DbType.Int32));
            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("DEADLINE_INFO_DEBT", DbType.Int32));

            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("REASON", DbType.Int32));
            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("START_DAY", DbType.Int32));
            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("END_DAY", DbType.Int32));
            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("START_PERIOD", DbType.Int32));
            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("END_PERIOD", DbType.Int32));

            this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("DATE_STOP", DbType.DateTime));
            this.Database.AddRefColumn("GKH_RO_PUB_SERVORG", new RefColumn("STOP_REASON_ID", "FK_STOP_REASON_ID", "GKH_DICT_STOP_REASON", "ID"));

            if (!this.Database.TableExists("GIS_SERVICE_DICTIONARY"))
            {
                this.Database.AddEntityTable("GIS_SERVICE_DICTIONARY",
                new Column("CODE", DbType.Int32),
                new Column("NAME", DbType.String, 200),
                new Column("MEASURE", DbType.String, 200),
                new Column("TYPE_SERVICE", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("TYPE_COMM_RESOURCE", DbType.Int32),
                new Column("FOR_ALL_HOUSE_NEEDS", DbType.Boolean),
                new RefColumn("UNIT_MEASURE_ID", "GIS_SERVICE_UNIT_MEASURE", "GKH_DICT_UNITMEASURE", "ID")
                );
            }

            this.Database.AddEntityTable("GKH_RO_PUBRESORG_SERVICE",
                new RefColumn("PUB_RESORG_ID", "FK_PUB_RESORG_ID", "GKH_RO_PUB_SERVORG", "ID"),
                new RefColumn("SRV_ID", "FK_SRV_ID", "GIS_SERVICE_DICTIONARY", "ID"),
                new RefColumn("COMMUN_RES_ID", "FK_COMMUN_RES_ID", "GKH_DICT_COMM_RESOURCE", "ID"),
                new RefColumn("UNIT_MEASURE_ID", "FK_UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID"),
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime),
                new Column("HEATING_SYSTEM_TYPE", DbType.Int32),
                new Column("SCHEME_CONN_TYPE", DbType.Int32),
                new Column("PLAN_VOLUME", DbType.String, 255),
                new Column("SERVICE_PERIOD", DbType.Decimal));

            this.Database.AddEntityTable("GKH_QUALITY_LEVEL",
                new RefColumn("UNIT_MEASURE_ID", "FK_UNIT_MEASURE", "GKH_DICT_UNITMEASURE", "ID"),
                new RefColumn("PUB_SERVORG_ID", "FK_PUB_SERVORG", "GKH_RO_PUBRESORG_SERVICE", "ID"),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull),
                new Column("VALUE", DbType.Decimal, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_TEMP_GRAPH_INFO",
                new RefColumn("CONTRACT_ID", "FK_CONTRACT_ID", "GKH_RO_PUB_SERVORG", "ID"),
                new Column("ORG_STATE_ROLE", DbType.Int32, ColumnProperty.NotNull),
                new Column("COOLANT_TEMP_SUPPLY", DbType.Int32, ColumnProperty.NotNull),
                new Column("COOLANT_TEMP_RETURN", DbType.Int32, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_RSOCONTRACT_BASE_PARTY",
                new RefColumn("SERV_ORG_CONTRACT_ID", "FK_SERV_ORG_CONTRACT_ID", "GKH_RO_PUB_SERVORG", "ID"),
                new Column("TYPE_CONTRACT", DbType.Int32, ColumnProperty.NotNull));

            // Стороны договора: РСО и исполнитель коммунальных услуг
            this.Database.AddTable(
                "GKH_RSOCONTRACT_SERVICE_PERFORMER",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("RESOURCE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("MANORG_ID", "FK_MANORG_ID", "GKH_MANAGING_ORGANIZATION", "ID"));
            this.Database.AddForeignKey("FK_GKH_RSO_SERVICE_PERFORMER", "GKH_RSOCONTRACT_SERVICE_PERFORMER", "ID", "GKH_RSOCONTRACT_BASE_PARTY", "ID");

            // Сторона договора "Физическое лицо"
            this.Database.AddTable(
                "GKH_RSOCONTRACT_INDIV_OWNER",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TYPE_PERSON", DbType.Int32, ColumnProperty.NotNull),
                new Column("FIRST_NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("LAST_NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("MIDDLE_NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("GENDER", DbType.Int32, ColumnProperty.NotNull),
                new Column("DOCUMENT_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("ISSUE_DATE", DbType.DateTime),
                new Column("DOCUMENT_SERIES", DbType.String, 100, ColumnProperty.NotNull),
                new Column("DOCUMENT_NUMBER", DbType.String, 100, ColumnProperty.NotNull),
                new Column("BIRTH_PLACE", DbType.String, 500, ColumnProperty.NotNull),
                new Column("BIRTH_DATE", DbType.DateTime, ColumnProperty.NotNull));
            this.Database.AddForeignKey("FK_GKH_RSOCONTRACT_INDIV_OWNER", "GKH_RSOCONTRACT_INDIV_OWNER", "ID", "GKH_RSOCONTRACT_BASE_PARTY", "ID");

            // Сторона договора "Юридическое лицо"
            this.Database.AddTable(
               "GKH_RSOCONTRACT_JUR_PERSON",
               new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
               new Column("TYPE_PERSON", DbType.Int32, ColumnProperty.NotNull),
               new RefColumn("CONTRAGENT_ID", "FK_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));
            this.Database.AddForeignKey("FK_GKH_RSOCONTRACT_JUR_PERSON", "GKH_RSOCONTRACT_JUR_PERSON", "ID", "GKH_RSOCONTRACT_BASE_PARTY", "ID");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            var gkh1468Migrations = this.Database.GetAppliedMigrations().Where(x => x.ModuleId == "Bars.Gkh1468").ToList();

            this.Database.Delete("GKH_RSOCONTRACT_JUR_PERSON");
            this.Database.Delete("GKH_RSOCONTRACT_INDIV_OWNER");
            this.Database.Delete("GKH_RSOCONTRACT_SERVICE_PERFORMER");
            this.Database.Delete("GKH_RSOCONTRACT_BASE_PARTY");


            this.Database.Delete("GKH_TEMP_GRAPH_INFO");
            this.Database.Delete("GKH_QUALITY_LEVEL");
            this.Database.Delete("GKH_RO_PUBRESORG_SERVICE");
            
            if (this.Database.TableExists("GIS_SERVICE_DICTIONARY"))
            {
                this.Database.Delete("GIS_SERVICE_DICTIONARY");
            }

            if (this.Database.TableExists("GKH_RO_PUB_SERVORG"))
            {
                if (gkh1468Migrations.All(x => x.Version != "2013091200" && x.Version != "2016061600"))
                {
                    this.Database.Delete("GKH_RO_PUB_SERVORG");
                }
                else
                {
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "DATE_STOP");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG","STOP_REASON_ID");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "REASON");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "START_DATE");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "END_DATE");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "START_PERIOD");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "END_PERIOD");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "TERM_BILLING_PYMNT_LATER");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "TERM_PYMNT_LATER");
                    this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "DEADLINE_INFO_DEBT");
                }
            }

            this.Database.Delete("GKH_DICT_COMM_RESOURCE");
            this.Database.Delete("GKH_DICT_STOP_REASON");
        }
    }
}
