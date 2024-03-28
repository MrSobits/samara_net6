namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022100300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [MigrationDependsOn(typeof(Version_2022082200.UpdateSchema))]
    [Migration("2022100300")]
    public class UpdateSchema : Migration
    {
        private readonly string tableName = "DECISION_KNM_ACTION";
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.tableName,
                new RefColumn("DECISION_ID", ColumnProperty.NotNull, "DISPOSAL_KNM_ACTION__DECISION_ID", "GJI_DECISION", "ID"),
                new RefColumn("KNM_ACTION_ID", ColumnProperty.NotNull, "DISPOSAL_KNM_ACTION__KNM_ACTION_ID", "GJI_DICT_KNM_ACTION", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Schema = "public", Name = this.tableName});
        }
    }
}