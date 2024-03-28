namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022020300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.Gkh;

    [Migration("2022020300")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022013100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhGjiTatarstan", "CreateFunctionGetDocParRoAdrByViolStage");
        }
    }
}