namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012701
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_COMP_PROC", "ISSUER");
            Database.AddRefColumn("REGOP_COMP_PROC", new RefColumn("ISSUER", ColumnProperty.NotNull, "CPROC_ISSUER", "B4_USER", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_COMP_PROC", "ISSUER");
            Database.AddRefColumn("REGOP_COMP_PROC", new RefColumn("ISSUER", ColumnProperty.NotNull, "CPROC_ISSUER", "GKH_OPERATOR", "ID"));
        }
    }
}
