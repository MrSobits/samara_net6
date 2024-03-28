namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021120200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021120200")]
    [MigrationDependsOn(typeof(Version_2021112400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiActCheckActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ACTION", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.gjiActCheckActionTable.Name, new []
            {
                new RefColumn("ACTCHECK_ID", ColumnProperty.NotNull, "GJI_ACTCHECK_ACTION_ACTCHECK", "GJI_ACTCHECK", "ID"),
                new Column("ACTION_TYPE", DbType.Int16, ColumnProperty.NotNull),
                new Column("DATE", DbType.Date, ColumnProperty.NotNull),
                new RefColumn("CREATION_PLACE_ID", ColumnProperty.NotNull, "GJI_ACTCHECK_ACTION_CREATION_PLACE", "B4_FIAS_ADDRESS", "ID")
            });
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.gjiActCheckActionTable);
        }
    }
}