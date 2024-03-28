namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021120800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    [Migration("2021120800")]
    [MigrationDependsOn(typeof(Version_2021120700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiPreventiveActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_DOCUMENT_PREVENTIVE_ACTION"};

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable
            (
                this.gjiPreventiveActionTable.Name,
                "GJI_DOCUMENT",
                "GJI_PREVENTIVE_ACTION_GJI_DOCUMENT",
                new Column("ACTION_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("ACTION_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("VISIT_TYPE", DbType.Int32),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GJI_DOCUMENT_PREVENTIVE_ACTION_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("CONTROLLED_ORGANIZATION_ID", "GJI_DOCUMENT_PREVENTIVE_ACTION_CONTROLLED_ORGANIZATION", "GKH_CONTRAGENT", "ID"),
                new RefColumn("PLAN_ID", "GJI_DOCUMENT_PREVENTIVE_ACTION_GJI_DICT_PLANACTION", "GJI_DICT_PLANACTION", "ID")
            );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.gjiPreventiveActionTable);
        }
    }
}