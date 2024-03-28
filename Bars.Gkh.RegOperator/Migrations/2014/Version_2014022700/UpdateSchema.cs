namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014022700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014022601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_CALC_PARAM_TRACE",
                new Column("CALC_GUID", DbType.String, 50, ColumnProperty.NotNull),
                new Column("CALC_TYPE", DbType.Int32),
                new Column("USED_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PARAM_VALUES", DbType.String, 1000));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_CALC_PARAM_TRACE");
        }
    }
}
