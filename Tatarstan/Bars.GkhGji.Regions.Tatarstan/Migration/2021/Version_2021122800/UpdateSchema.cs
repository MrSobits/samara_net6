namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122800
{
    using Bars.B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021122800")]
    [MigrationDependsOn(typeof(Version_2021122701.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiMotivatedPresentationActionIsolatedTable =
            new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiMotivatedPresentationActionIsolatedRealityObjectTable =
            new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED_ROBJECT", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiMotivatedPresentationActionIsolatedViolationTable =
            new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED_VIOLATION", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.gjiMotivatedPresentationActionIsolatedTable.Name,
                "GJI_DOCUMENT",
                this.gjiMotivatedPresentationActionIsolatedTable.Name + "_DOCUMENT",
                new RefColumn("CREATION_PLACE_ID",
                    this.gjiMotivatedPresentationActionIsolatedTable.Name + "_CREATION_PLACE",
                    "B4_FIAS_ADDRESS",
                    "ID"),
                new RefColumn("ISSUED_MOTIVATED_PRESENTATION_ID",
                    this.gjiMotivatedPresentationActionIsolatedTable.Name + "_ISSUED_MOTIVATED_PRESENTATION",
                    "GKH_DICT_INSPECTOR",
                    "ID"),
                new RefColumn("RESPONSIBLE_EXECUTION_ID",
                    this.gjiMotivatedPresentationActionIsolatedTable.Name + "_RESPONSIBLE_EXECUTION",
                    "GKH_DICT_INSPECTOR",
                    "ID"));

            this.Database.AddEntityTable(this.gjiMotivatedPresentationActionIsolatedRealityObjectTable.Name,
                new RefColumn("MOTIVATED_PRESENTATION_ID",
                    this.gjiMotivatedPresentationActionIsolatedRealityObjectTable.Name + "_MOTIVATED_PRESENTATION",
                    this.gjiMotivatedPresentationActionIsolatedTable.Name,
                    "ID"),
                new RefColumn("REALITY_OBJECT_ID",
                    this.gjiMotivatedPresentationActionIsolatedRealityObjectTable.Name + "_REALITY_OBJECT",
                    "GKH_REALITY_OBJECT",
                    "ID"));

            this.Database.AddEntityTable(this.gjiMotivatedPresentationActionIsolatedViolationTable.Name,
                new RefColumn("MOTIVATED_PRESENTATION_ROBJECT_ID",
                    this.gjiMotivatedPresentationActionIsolatedViolationTable.Name + "_MOTIVATED_PRESENTATION_ROBJECT",
                    this.gjiMotivatedPresentationActionIsolatedRealityObjectTable.Name,
                    "ID"),
                new RefColumn("VIOLATION_ID",
                    this.gjiMotivatedPresentationActionIsolatedViolationTable.Name + "_VIOLATION",
                    "GJI_DICT_VIOLATION",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.gjiMotivatedPresentationActionIsolatedViolationTable);
            this.Database.RemoveTable(this.gjiMotivatedPresentationActionIsolatedRealityObjectTable);
            this.Database.RemoveTable(this.gjiMotivatedPresentationActionIsolatedTable);
        }
    }
}