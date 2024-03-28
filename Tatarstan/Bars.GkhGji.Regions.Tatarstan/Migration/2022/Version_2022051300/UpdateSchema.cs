namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022051300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [MigrationDependsOn(typeof(Version_2022042700.UpdateSchema))]
    [Migration("2022051300")]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_DECISION_CONTROL_OBJECT_INFO";
        
        public override void Up()
        {
            this.Database.RemoveColumn(TableName, "REALITY_OBJECT_ID");
            this.Database.AddRefColumn(TableName, new RefColumn("INSPECTION_ROBJECT_ID", ColumnProperty.NotNull, "GJI_CTR_OI_REALITY_OBJ", "GJI_INSPECTION_ROBJECT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn(TableName, "INSPECTION_ROBJECT_ID");
            this.Database.AddRefColumn(TableName, new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GJI_CTR_OI_REALITY_OBJ", "GKH_REALITY_OBJECT", "ID"));
        }
    }
}