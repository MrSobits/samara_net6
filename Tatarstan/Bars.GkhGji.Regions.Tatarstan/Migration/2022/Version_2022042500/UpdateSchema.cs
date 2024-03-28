namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022042500")]
    [MigrationDependsOn(typeof(Version_2022042201.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName
        {
            Name = "GJI_DICT_CONTROL_OBJECT_TYPE"
        };
        
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable
                (
                    this.table.Name, 
                    new Column("NAME", DbType.String.WithSize(1000), ColumnProperty.NotNull),
                    new Column("ERVK_ID", DbType.String.WithSize(36), ColumnProperty.NotNull)
                );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.table);
        }
    }
}
