namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [MigrationDependsOn(typeof(Version_2022042605.UpdateSchema))]
    [Migration("2022042700")]
    public class UpdateSchema: Migration
    {
        public const string TableName = "GJI_DECISION_CONTROL_OBJECT_INFO";
        
        private readonly Column[] columns =
        {
            new RefColumn("DECISION_ID", ColumnProperty.NotNull, "GJI_CTR_OI_DECISION", "GJI_DECISION", "ID"),
            new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GJI_CTR_OI_REALITY_OBJ", "GKH_REALITY_OBJECT", "ID"),
            new RefColumn("CONTROL_OBJECT_KIND_ID", ColumnProperty.NotNull, "GJI_CTR_OI_CTR_OBJ_KIND", "GJI_DICT_CONTROL_OBJECT_KIND", "ID")
        }; 
        
        public override void Up()
        {
            this.Database.AddEntityTable(TableName, this.columns);
        }

        public override void Down()
        {
            this.Database.RemoveTable(TableName);
        }
    }
}