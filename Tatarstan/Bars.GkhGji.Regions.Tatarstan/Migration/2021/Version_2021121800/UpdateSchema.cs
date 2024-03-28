namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021121800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021121800")]
    [MigrationDependsOn(typeof(Version_2021121700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiActCheckExplanationActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_EXPLANATION_ACTION", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.gjiActCheckExplanationActionTable.Name,
                "GJI_ACTCHECK_ACTION",
                this.gjiActCheckExplanationActionTable.Name + "_ACTION",
                new Column("CONTR_PERS_TYPE", DbType.Int16),
                new RefColumn("CONTR_PERS_CONTRAGENT_ID",
                    this.gjiActCheckExplanationActionTable.Name + "_CONTRAGENT",
                    "GKH_CONTRAGENT",
                    "ID"),
                new Column("ATTACHMENT_NAME", DbType.String.WithSize(255)),
                new RefColumn("ATTACHMENT_FILE_ID",
                    this.gjiActCheckExplanationActionTable.Name + "_ATTACHMENT_FILE",
                    "B4_FILE_INFO",
                    "ID"),
                new Column("EXPLANATION", DbType.String.WithSize(1000)));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.gjiActCheckExplanationActionTable);
        }
    }
}