namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2021122400")]
    [MigrationDependsOn(typeof(Version_2021122301.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_DICT_PREVENTIVE_ITEMS";
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddGkhDictTable(TableName);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(TableName);
        }
    }
}