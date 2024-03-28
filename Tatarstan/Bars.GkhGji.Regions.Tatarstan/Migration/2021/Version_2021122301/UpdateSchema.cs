namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122301
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2021122301")]
    [MigrationDependsOn(typeof(Version_2021122300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddGkhDictTable("GJI_DICT_OBJECTIVES_PREVENTIVE_MEASURES");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_OBJECTIVES_PREVENTIVE_MEASURES");
        }
    }
}
