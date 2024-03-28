namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014091700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014091400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PRIVILEGED_CATEGORY",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String),
                new Column("PERCENT", DbType.Decimal),
                new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_TO", DbType.DateTime));

            Database.AddRefColumn("REGOP_PERS_ACC_OWNER", new RefColumn("PRIVILEGED_CATEGORY", "PERS_ACC_PRIV_CAT", "REGOP_PRIVILEGED_CATEGORY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PRIVILEGED_CATEGORY");
            Database.RemoveColumn("REGOP_PERS_ACC_OWNER", "PRIVILEGED_CATEGORY");
        }
    }
}
