namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022040700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2022040700")]
    [MigrationDependsOn(typeof(Version_2022040600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string InspectorPositionsTable = "GJI_DICT_INSPECTOR_POSITIONS";
        
        public override void Up()
        {
            this.Database.AddGkhDictTable(InspectorPositionsTable);
        }

        public override void Down()
        {
            this.Database.RemoveTable(InspectorPositionsTable);
        }
    }
}