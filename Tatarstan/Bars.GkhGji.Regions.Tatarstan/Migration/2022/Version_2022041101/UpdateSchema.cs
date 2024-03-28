namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041101
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022041101")]
    [MigrationDependsOn(typeof(Version_2022041100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName
        {
            Name = "GJI_DECISION"
        };

        public override void Up()
        {
            this.Database.AddRefColumn(
                this.table,
                new RefColumn("DECISION_PLACE_ID", $"{this.table.Name}_DECISION_PLACE_ID", "B4_FIAS_ADDRESS", "ID")
            );
        }

        public override void Down()
        {
            this.Database.RemoveColumn(this.table, "DECISION_PLACE_ID");
        }
    }
}