namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022102000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022102000")]
    [MigrationDependsOn(typeof(Version_2022101400.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2022101700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GJI_INSPECTION_PREVENTIVEACTION" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.table.Name,
                "GJI_INSPECTION",
                "GJI_INSPECTION_GJI_INSPECTION_PREVENTIVEACTION",
                new Column("TYPE_FORM", DbType.Int32),
                new RefColumn("PREVENTIVE_ACTION_ID", ColumnProperty.NotNull,  "GJI_INSPECTION_PREVENTIVEACTION_PREVENTIVE_ACTION", "GJI_INSPECTION", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.table);
        }
    }
}