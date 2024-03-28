namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021121400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.Ecm7.Providers;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    
    [Migration("2021121400")]
    [MigrationDependsOn(typeof(Version_2021121301.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiPreventiveActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_DOCUMENT_PREVENTIVE_ACTION"};

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddRefColumn(gjiPreventiveActionTable, 
                new RefColumn("ZONAL_INSPECTION_ID", ColumnProperty.NotNull, "GJI_DOCUMENT_PREVENTIVE_ACTION_GKH_DICT_ZONAINSP", "GKH_DICT_ZONAINSP", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.gjiPreventiveActionTable, "ZONAL_INSPECTION_ID");
        }
    }
}