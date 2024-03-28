namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021012100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021012100")]
    [MigrationDependsOn(typeof(_2020.Version_2020101300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string GisChargeTable = "GJI_TAT_GIS_CHARGE";
        private const string SendLogColumn = "SEND_LOG";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.GisChargeTable,
                new Column(UpdateSchema.SendLogColumn, DbType.String));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.GisChargeTable, UpdateSchema.SendLogColumn);
        }
    }
}