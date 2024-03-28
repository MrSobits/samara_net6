namespace Bars.GkhCr.Regions.Tatarstan.Migrations._2019.Version_2019121600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019121600")]
    [MigrationDependsOn(typeof(Version_2019121400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string DictWorkOutdoorTableName = "CR_DICT_WORK_OUTDOOR";
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.DictWorkOutdoorTableName,
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("TYPE_WORK", DbType.Int32, ColumnProperty.NotNull, 10),
                new RefColumn("UNIT_MEASURE_ID", "CR_DICT_WORK_OUTDOOR_UM",
                    "GKH_DICT_UNITMEASURE", "ID"),
                new Column("IS_ACTUAL", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DESCRIPTION", DbType.String));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.DictWorkOutdoorTableName); ;
        }
    }
}