namespace Bars.Gkh.Reforma.Migrations.Version_2014112200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2014112100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "RFRM_REPORTING_PERIOD",
                new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.NotNull),
                new Column("STATE", DbType.Int16, ColumnProperty.NotNull),
                new RefColumn("PERIOD_DI_ID", ColumnProperty.Null, "RFRM_REP_PERIOD_DI", "DI_DICT_PERIOD", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("RFRM_REPORTING_PERIOD");
        }
    }
}