namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021031700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021031700")]
    [MigrationDependsOn(typeof(Version_2021012100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GJI_TAT_GIS_GMP_PATTERN_DICT",
                new Column("relevance", DbType.Boolean, false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_TAT_GIS_GMP_PATTERN_DICT", "relevance");
        }
    }
}