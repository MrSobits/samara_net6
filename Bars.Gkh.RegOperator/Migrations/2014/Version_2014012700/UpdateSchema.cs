namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012505.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_COMP_PROC",
                new Column("TYPE", DbType.Int32, ColumnProperty.Null),
                new Column("STATUS", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("ISSUER", ColumnProperty.NotNull, "CPROC_ISSUER", "GKH_OPERATOR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_COMP_PROC");
        }
    }
}
