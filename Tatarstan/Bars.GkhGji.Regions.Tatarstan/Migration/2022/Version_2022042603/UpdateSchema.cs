namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042603
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [MigrationDependsOn(typeof(Version_2022042602.UpdateSchema))]
    [Migration("2022042603")]
    public class UpdateSchema: Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName
        {
            Name = "GJI_DICT_CONTROL_OBJECT_KIND"
        };
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable
                (
                    this.table.Name,
                    new Column("NAME", DbType.String.WithSize(1000), ColumnProperty.NotNull),
                    new Column("ERVK_ID", DbType.String.WithSize(36)),
                    new RefColumn("CONTROL_TYPE_ID", ColumnProperty.NotNull, "GJI_DICT_OBJECT_KIND_CONTROL_TYPE", "GJI_DICT_CONTROL_TYPES", "Id"),
                    new RefColumn("CONTROL_OBJECT_TYPE_ID", ColumnProperty.NotNull, "GJI_DICT_OBJECT_TYPE_OBJECT_TYPE", "GJI_DICT_CONTROL_OBJECT_TYPE", "Id")
                );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.table);
        }
    }
}