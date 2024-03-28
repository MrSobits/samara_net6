namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022103100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022103100")]
    [MigrationDependsOn(typeof(Version_2022102000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName appealTable = new SchemaQualifiedObjectName { Name = "GJI_RAPID_RESPONSE_SYSTEM_APPEAL" };
        private readonly SchemaQualifiedObjectName appealDetailsTable = new SchemaQualifiedObjectName { Name = "GJI_RAPID_RESPONSE_SYSTEM_APPEAL_DETAILS" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.appealTable.Name, 
                new RefColumn("APPEAL_CITS_ID", ColumnProperty.NotNull,$"{appealTable.Name}_APPEAL_CITS", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("CONTRAGENT_ID", $"{appealTable.Name}_CONTRAGENT", "GKH_CONTRAGENT", "ID"));

            this.Database.AddEntityTable(this.appealDetailsTable.Name,
                new RefColumn("STATE_ID", $"{appealDetailsTable.Name}_STATE", "B4_STATE", "ID"),
                new RefColumn("USER_ID", $"{appealDetailsTable.Name}_USER", "B4_USER", "ID"), 
                new RefColumn("APPEAL_CITS_REALITY_OBJECT_ID", $"{appealDetailsTable.Name}_APPEAL_CITS_REALITY_OBJECT", "GJI_APPCIT_RO", "ID"),
                new RefColumn("RAPID_RESPONSE_SYSTEM_APPEAL_ID", ColumnProperty.NotNull,  $"{appealDetailsTable.Name}_{appealTable.Name}", appealTable.Name, "ID"),
                new Column("RECEIPT_DATE", DbType.Date),
                new Column("CONTROL_PERIOD", DbType.Date));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.appealTable);
            this.Database.RemoveTable(this.appealDetailsTable);
        }
    }
}