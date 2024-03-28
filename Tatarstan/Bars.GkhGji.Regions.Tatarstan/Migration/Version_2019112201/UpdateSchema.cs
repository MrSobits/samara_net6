namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019112201
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019112201")]
    [MigrationDependsOn(typeof(Version_2019112200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GJI_DICT_EFFEC_PERF_INDEX", new Column("CODE", DbType.String.WithSize(300), ColumnProperty.Null));
            this.Database.ChangeColumn("GJI_DICT_EFFEC_PERF_INDEX", new Column("NAME", DbType.String.WithSize(500), ColumnProperty.Null));
            this.Database.ChangeColumn("GJI_DICT_EFFEC_PERF_INDEX", new Column("PARAM_NAME", DbType.String.WithSize(500), ColumnProperty.Null));
            this.Database.ChangeColumn("GJI_DICT_EFFEC_PERF_INDEX", new Column("UNIT_MEASURE", DbType.String.WithSize(300), ColumnProperty.Null));
        }
    }
}
