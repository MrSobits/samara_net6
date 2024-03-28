namespace Bars.GkhCr.Regions.Tatarstan.Migrations._2019.Version_2019122000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019122000")]
    [MigrationDependsOn(typeof(Version_2019121700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string DictElementOutdoorTableName = "CR_DICT_ELEMENT_OUTDOOR";
        private const string WorkElementOutdoorTableName = "CR_WORK_ELEMENT_OUTDOOR";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.DictElementOutdoorTableName,
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("ELEMENT_GROUP", DbType.Int32),
                new RefColumn("UNIT_MEASURE_ID", "CR_DICT_ELEMENT_OUTDOOR_UM",
                    "GKH_DICT_UNITMEASURE", "ID"));

            this.Database.AddEntityTable(UpdateSchema.WorkElementOutdoorTableName,
                new RefColumn("ELEMENT_OUTDOOR_ID", ColumnProperty.NotNull,
                    "CR_DICT_ELEMENT_OUTDOOR_EO",
                    "CR_DICT_ELEMENT_OUTDOOR", "ID"),
                new RefColumn("WORK_OUTDOOR_ID", ColumnProperty.NotNull,
                    "CR_DICT_WORK_OUTDOOR_WO",
                    "CR_DICT_WORK_OUTDOOR", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.WorkElementOutdoorTableName);
            this.Database.RemoveTable(UpdateSchema.DictElementOutdoorTableName); ;
        }
    }
}