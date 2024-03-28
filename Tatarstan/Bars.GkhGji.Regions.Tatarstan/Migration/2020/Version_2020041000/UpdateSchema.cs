namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020041000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using DatabaseExtensions = Bars.Gkh.Utils.DatabaseExtensions;

    [Migration("2020041000")]
    [MigrationDependsOn(typeof(Version_2019122401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TatarstanProtocolGjiTableName = "GJI_TATARSTAN_PROTOCOL_GJI";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(UpdateSchema.TatarstanProtocolGjiTableName,
                "GJI_DOCUMENT",
                "TATARSTAN_PROTOCOL_GJI",
                new Column("DATE_SUPPLY", DbType.DateTime),
                new Column("DATE_OFFENSE", DbType.DateTime),
                new Column("TIME_OFFENSE", DbType.DateTime),
                new Column("ANNUL_REASON", DbType.String.WithSize(255)),
                new Column("UPDATE_REASON", DbType.String.WithSize(255)),
                new Column("PAIDED", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("PENALTY_AMOUNT", DbType.Decimal),
                new Column("DATE_TRANSFER_SSP", DbType.DateTime),
                new Column("TERMINATION_BASEMENT", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("TERMINATION_DOC_NUM", DbType.String.WithSize(255)),
                new RefColumn("SANCTION_ID", "GJI_TATARSTAN_PROTOCOL_GJI_SANCTION", "GJI_DICT_SANCTION", "ID"),
                new RefColumn("GIS_GMP_PATTERN_ID", "GJI_TATARSTAN_PROTOCOL_GJI_PATTERN", "GJI_TAT_GIS_GMP_PATTERN_DICT", "ID"),
                new Column("TYPE_EXECUTANT", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("MUNICIPALITY_ID", "GJI_TATARSTAN_PROTOCOL_GJI_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("ZONAL_INSPECTION_ID", "GJI_TATARSTAN_PROTOCOL_GJI_INSPECTION", "GKH_DICT_ZONAINSP", "ID"),
                new Column("SUR_NAME", DbType.String.WithSize(255)),
                new Column("NAME", DbType.String.WithSize(255)),
                new Column("PATRONYMIC", DbType.String.WithSize(255)),
                new Column("BIRTH_DATE", DbType.DateTime),
                new Column("BIRTH_PLACE", DbType.String.WithSize(255)),
                new Column("FACT_ADDRESS", DbType.String.WithSize(255)),
                new Column("CITIZENSHIP_TYPE", DbType.Int32),
                new RefColumn("CITIZENSHIP_ID", "GJI_TATARSTAN_PROTOCOL_GJI_CITIZENSHIP", "GJI_DICT_CITIZENSHIP", "ID"),
                new Column("SERIAL_AND_NUMBER", DbType.String.WithSize(255)),
                new Column("ISSUE_DATE", DbType.DateTime),
                new Column("ISSUING_AUTHORITY", DbType.String),
                new Column("COMPANY", DbType.String.WithSize(255)),
                new Column("MARITAL_STATUS", DbType.String),
                new Column("DEPENDENT_COUNT", DbType.Int32),
                new Column("REGISTRATION_ADDRESS", DbType.String),
                new Column("SALARY", DbType.Decimal),
                new Column("RESPONSIBILITY_PUNISHMENT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("PROTOCOL_EXPLANATION", DbType.String),
                new Column("IS_IN_TRIBUNAL", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("TRIBUNAL_NAME", DbType.String),
                new Column("OFFENCE_ADDRESS", DbType.String),
                new Column("ACCUSED_EXPLANATION", DbType.String),
                new Column("REJECTION_SIGNATURE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("RESIDENCE_PETITION", DbType.Boolean, ColumnProperty.NotNull, false)
            );

            //протокол гжи контрагент
            this.Database.AddJoinedSubclassTable("GJI_TATARSTAN_PROTOCOL_GJI_CONTRAGENT",
                UpdateSchema.TatarstanProtocolGjiTableName,
                "TATARSTAN_PROTOCOL_GJI_CONTRAGENT",
                new RefColumn("CONTRAGENT_ID", "GJI_TATARSTAN_PROTOCOL_GJI_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                new Column("DELEGATE_FIO", DbType.String),
                new Column("DELEGATE_COMPANY", DbType.String),
                new Column("PROCURATION_NUMBER", DbType.String),
                new Column("PROCURATION_DATE", DbType.DateTime),
                new Column("DELEGATE_RESPONSIBILITY_PUNISHMENT", DbType.Boolean, ColumnProperty.NotNull, false));

            //инспекторы
            this.Database.AddEntityTable("GJI_TATARSTAN_PROTOCOL_GJI_INSPECTOR",
                new RefColumn("TATARSTAN_PROTOCOL_GJI_ID",
                    "GJI_TATARSTAN_PROTOCOL_GJI_INPECTOR_PROTOCOL",
                    UpdateSchema.TatarstanProtocolGjiTableName,
                    "ID"),
                new RefColumn("INSPECTOR_ID", "GJI_TATARSTAN_PROTOCOL_GJI_INPECTOR_INSPECTOR", "GKH_DICT_INSPECTOR", "ID")
            );

            //статьи закона
            this.Database.AddEntityTable("GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LOW",
                new RefColumn("TATARSTAN_PROTOCOL_GJI_ID",
                    "GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LOW_PROTOCOL",
                    UpdateSchema.TatarstanProtocolGjiTableName,
                    "ID"),
                new RefColumn("ARTICLE_LOW_ID", "GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LOW_ARTICLE", "GJI_DICT_ARTICLELAW", "ID")
            );

            //адреса
            this.Database.AddEntityTable("GJI_TATARSTAN_PROTOCOL_GJI_REALITY_OBJECT",
                new RefColumn("TATARSTAN_PROTOCOL_GJI_ID",
                    "GJI_TATARSTAN_PROTOCOL_GJI_REALITY_OBJECT_PROTOCOL",
                    UpdateSchema.TatarstanProtocolGjiTableName,
                    "ID"),
                new RefColumn("REALITY_OBJECT_ID", "GJI_TATARSTAN_PROTOCOL_GJI_REALITY_OBJECT_RO", "GKH_REALITY_OBJECT", "ID")
            );

            //приложения
            this.Database.AddEntityTable("GJI_TATARSTAN_PROTOCOL_GJI_ANNEX",
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String),
                new Column("DESCRIPTION", DbType.String),
                new RefColumn("FILE_ID", "GJI_TATARSTAN_PROTOCOL_GJI_ANNEX_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("TATARSTAN_PROTOCOL_GJI_ID", "GJI_TATARSTAN_PROTOCOL_GJI_ANNEX_PROTOCOL", UpdateSchema.TatarstanProtocolGjiTableName, "ID")
            );

            //инспекции
            DatabaseExtensions.AddJoinedSubclassTable(this.Database,
                "GJI_INSPECTION_PROTGJI",
                "GJI_INSPECTION",
                "GJI_INSPECTION_PROTGJI");

            //нарушения
            this.Database.AddEntityTable("GJI_TATARSTAN_PROTOCOL_GJI_VIOLATION",
                new RefColumn("TATARSTAN_PROTOCOL_GJI_ID",
                    "GJI_TATARSTAN_PROTOCOL_GJI_VIOLATION_PROTOCOL",
                    UpdateSchema.TatarstanProtocolGjiTableName,
                    "ID"),
                new RefColumn("VIOLATION_GJI_ID", "GJI_TATARSTAN_PROTOCOL_GJI_VIOLATION_VIOLATION", "GJI_DICT_VIOLATION", "ID")
            );

            //Сведения о свидетелях и потерпевших
            this.Database.AddEntityTable("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS",
                new Column("EYEWITNESS_TYPE", DbType.Int32),
                new Column("FIO", DbType.String),
                new Column("FACT_ADDRESS", DbType.String),
                new Column("PHONE", DbType.String),
                new RefColumn("TATARSTAN_PROTOCOL_GJI_ID", "GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS", "GJI_TATARSTAN_PROTOCOL_GJI", "ID")
            );

            this.Database.RemoveConstraint("GJI_TAT_GIS_CHARGE", "FK_GJI_TAT_GIS_CHARGE_RES");
            this.Database.RenameColumn("GJI_TAT_GIS_CHARGE", "RESOL_ID", "DOC_ID");
            this.Database.AddForeignKey("FK_GJI_TAT_GIS_CHARGE_GJI_DOC", "GJI_TAT_GIS_CHARGE", "DOC_ID", "GJI_DOCUMENT", "ID");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveConstraint("GJI_TAT_GIS_CHARGE", "FK_GJI_TAT_GIS_CHARGE_GJI_DOC");
            this.Database.RenameColumn("GJI_TAT_GIS_CHARGE", "DOC_ID", "RESOL_ID");
            this.Database.AddForeignKey("FK_GJI_TAT_GIS_CHARGE_RES", "GJI_TAT_GIS_CHARGE", "RESOL_ID", "GJI_RESOLUTION", "ID");

            this.Database.RemoveTable("GJI_INSPECTION_PROTGJI");
            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_GJI_INSPECTOR");
            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LOW");
            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_GJI_REALITY_OBJECT");
            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_GJI_ANNEX");
            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_GJI_CONTRAGENT");
            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS");
            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_GJI_VIOLATION");
            this.Database.RemoveTable(UpdateSchema.TatarstanProtocolGjiTableName);
        }
    }
}
