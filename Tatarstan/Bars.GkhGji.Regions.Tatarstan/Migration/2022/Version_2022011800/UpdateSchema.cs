namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022011800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022011800")]
    [MigrationDependsOn(typeof(Version_2022011300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table =
            new SchemaQualifiedObjectName { Name = "GJI_ACT_ACTIONISOLATED_DEFINITION" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable
            (
                this.table.Name,
                new Column("NUMBER", DbType.String.WithSize(25)),
                new Column("DATE", DbType.Date),
                new Column("EXECUTION_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String.WithSize(500)),
                new Column("DEFINITION_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("OFFICIAL_ID", ColumnProperty.NotNull, this.table.Name + "_OFFICIAL", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, this.table.Name + "_REALITY_OBJECT", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("ACT_ID", ColumnProperty.NotNull, this.table.Name + "_ACT", "GJI_ACT_ACTIONISOLATED", "ID")
            );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(table);
        }
    }
}