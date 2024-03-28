namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122300
{
    using Bars.B4.Modules.Ecm7.Framework;

    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021122300")]
    [MigrationDependsOn(typeof(Version_2021122200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiActCheckDocRequestActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_DOC_REQUEST_ACTION", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiActCheckDocRequestActionRequestInfoTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_DOC_REQUEST_ACTION_REQUEST_INFO", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.gjiActCheckDocRequestActionTable.Name,
                "GJI_ACTCHECK_ACTION",
                this.gjiActCheckDocRequestActionTable.Name + "_ACTION",
                new Column("CONTR_PERS_TYPE", DbType.Int16),
                new RefColumn("CONTR_PERS_CONTRAGENT_ID",
                    this.gjiActCheckDocRequestActionTable.Name + "_CONTRAGENT",
                    "GKH_CONTRAGENT",
                    "ID"),
                new Column("DOC_PROVIDING_PERIOD", DbType.Int64, ColumnProperty.NotNull),
                new RefColumn("DOC_PROVIDING_ADDRESS_ID",
                    gjiActCheckDocRequestActionTable.Name + "_DOC_PROVIDING_ADDRESS",
                    "B4_FIAS_ADDRESS",
                    "ID"),
                new Column("CONTR_PERS_EMAIL_ADDRESS", DbType.String.WithSize(100)),
                new Column("POSTAL_OFFICE_NUMBER", DbType.String.WithSize(50)),
                new Column("EMAIL_ADDRESS", DbType.String.WithSize(50)),
                new Column("COPY_DETERMINATION_DATE", DbType.Date),
                new Column("LETTER_NUMBER", DbType.String.WithSize(50))
            );

            this.Database.AddEntityTable(this.gjiActCheckDocRequestActionRequestInfoTable.Name,
                new RefColumn("DOC_REQUEST_ACTION_ID",
                    this.gjiActCheckDocRequestActionRequestInfoTable.Name + "_DOC_REQUEST_ACTION",
                    this.gjiActCheckDocRequestActionTable.Name,
                    "ID"),
                new Column("REQUEST_INFO_TYPE", DbType.Int16),
                new Column("NAME", DbType.String.WithSize(255)),
                new RefColumn("FILE_ID",
                    this.gjiActCheckDocRequestActionRequestInfoTable.Name + "_FILE",
                    "B4_FILE_INFO",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.gjiActCheckDocRequestActionRequestInfoTable);
            this.Database.RemoveTable(this.gjiActCheckDocRequestActionTable);
        }
    }
}