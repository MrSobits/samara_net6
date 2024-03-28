namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021031900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021031900")]
    [MigrationDependsOn(typeof(Version_2021031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            Database.ChangeColumn("GJI_TAT_GIS_GMP_PATTERN_DICT", new Column("PATTERN_NAME", DbType.String, 500));
        }

        /// <inheritdoc />
        public override void Down()
        {
            Database.ChangeColumn("GJI_TAT_GIS_GMP_PATTERN_DICT", new Column("PATTERN_NAME", DbType.String, 255));
        }
    }
}