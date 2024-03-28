namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020041400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020041400")]
    [MigrationDependsOn(typeof(Version_2020041000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        private const string TatarstanResolutionGjiTableName = "GJI_TATARSTAN_RESOLUTION_GJI";

        /// <inheritdoc />
        public override void Up()
        {
            #region resolution
            this.Database.AddJoinedSubclassTable(UpdateSchema.TatarstanResolutionGjiTableName,
                "GJI_RESOLUTION",
                "TATARSTAN_RESOLUTION_GJI",
                new RefColumn("CITIZENSHIP_ID", "GJI_TATARSTAN_RESOLUTION_GJI_CITIZENSHIP", "GJI_DICT_CITIZENSHIP", "ID"),
                new Column("TERMINATION_DOC_NUM", DbType.String),
                new Column("TYPE_EXECUTANT", DbType.Int32, ColumnProperty.NotNull),
                new Column("SUR_NAME", DbType.String.WithSize(255)),
                new Column("NAME", DbType.String.WithSize(255)),
                new Column("PATRONYMIC", DbType.String.WithSize(255)),
                new Column("BIRTH_DATE", DbType.DateTime),
                new Column("BIRTH_PLACE", DbType.String.WithSize(255)),
                new Column("FACT_ADDRESS", DbType.String.WithSize(255)),
                new Column("CITIZENSHIP_TYPE", DbType.Int32),
                new Column("SERIAL_AND_NUMBER", DbType.String.WithSize(255)),
                new Column("ISSUE_DATE", DbType.DateTime),
                new Column("ISSUING_AUTHORITY", DbType.String),
                new Column("COMPANY", DbType.String.WithSize(255)),
                new Column("MARITAL_STATUS", DbType.String),
                new Column("DEPENDENT_COUNT", DbType.Int32),
                new Column("REGISTRATION_ADDRESS", DbType.String),
                new Column("SALARY", DbType.Decimal),
                new Column("RESPONSIBILITY_PUNISHMENT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DELEGATE_FIO", DbType.String),
                new Column("DELEGATE_COMPANY", DbType.String),
                new Column("PROCURATION_NUMBER", DbType.String),
                new Column("PROCURATION_DATE", DbType.DateTime),
                new Column("DELEGATE_RESPONSIBILITY_PUNISHMENT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IMPROVING_FACT", DbType.String),
                new Column("DISIMPROVING_FACT", DbType.String));
            #endregion
            
            this.Database.RemoveConstraint("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS",
                "FK_GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS");

            this.Database.RemoveConstraint("GJI_TATARSTAN_PROTOCOL_GJI_ANNEX",
                "FK_GJI_TATARSTAN_PROTOCOL_GJI_ANNEX_PROTOCOL");

            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS",
                "TATARSTAN_PROTOCOL_GJI_ID", "DOCUMENT_GJI_ID");

            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_ANNEX",
                "TATARSTAN_PROTOCOL_GJI_ID", "DOCUMENT_GJI_ID");

            this.Database.AddForeignKey("GJI_DOCUMENT_GJI_GJI_EYEWITNESS", "GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS",
                "DOCUMENT_GJI_ID", "GJI_DOCUMENT", "ID");
            this.Database.AddForeignKey("GJI_DOCUMENT_GJI_ANNEX_PROTOCOL", "GJI_TATARSTAN_PROTOCOL_GJI_ANNEX",
                "DOCUMENT_GJI_ID", "GJI_DOCUMENT", "ID");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS",
                "DOCUMENT_GJI_ID", "TATARSTAN_PROTOCOL_GJI_ID");

            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_ANNEX",
                "DOCUMENT_GJI_ID", "TATARSTAN_PROTOCOL_GJI_ID");

            this.Database.RemoveConstraint("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS",
                "FK_GJI_DOCUMENT_GJI_GJI_EYEWITNESS");

            this.Database.RemoveConstraint("GJI_TATARSTAN_PROTOCOL_GJI_ANNEX",
                "FK_GJI_DOCUMENT_GJI_ANNEX_PROTOCOL");

            this.Database.AddForeignKey("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS", "GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS",
                "TATARSTAN_PROTOCOL_GJI_ID", "GJI_TATARSTAN_PROTOCOL_GJI", "ID");
            this.Database.AddForeignKey("GJI_TATARSTAN_PROTOCOL_GJI_ANNEX_PROTOCOL", "GJI_TATARSTAN_PROTOCOL_GJI_ANNEX",
                "TATARSTAN_PROTOCOL_GJI_ID", "GJI_TATARSTAN_PROTOCOL_GJI", "ID");

            this.Database.RemoveTable(UpdateSchema.TatarstanResolutionGjiTableName);
        }
    }
}
