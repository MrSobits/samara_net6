namespace Bars.GkhGji.Migrations._2024.Version_2024030127
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using System.Data;

    [Migration("2024030127")]
    [MigrationDependsOn(typeof(Version_2024030126.UpdateSchema))]
    /// Является Version_2022051701 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName actCheckTable = new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK" };
        private SchemaQualifiedObjectName actCheckPeriodTable = new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_PERIOD" };

        private Column[] actCheckColumns =
        {
            new Column("SIGNER_ID", DbType.Int32),
            new Column("ACQUAINTED_PERSON_TITLE", DbType.String, 100)
        };

        private Column[] actCheckPeriodColumns =
        {
            new Column("DURATION_DAYS", DbType.Int32),
            new Column("DURATION_HOURS", DbType.Int32)
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.actCheckColumns.ForEach(column => this.Database.AddColumn(this.actCheckTable, column));
            this.actCheckPeriodColumns.ForEach(column => this.Database.AddColumn(this.actCheckPeriodTable, column));

            this.Database.AddForeignKey("FK_ACTCHECK_DICT_INSPECTOR_ID", this.actCheckTable, "SIGNER_ID", "GKH_DICT_INSPECTOR", "ID");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.actCheckColumns.ForEach(column => this.Database.RemoveColumn(this.actCheckTable, column.Name));
            this.actCheckPeriodColumns.ForEach(column => this.Database.RemoveColumn(this.actCheckPeriodTable, column.Name));

            this.Database.RemoveConstraint(this.actCheckTable, "FK_ACTCHECK_DICT_INSPECTOR_ID");
        }
    }
}