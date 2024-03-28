namespace Bars.Gkh.Migrations._2020.Version_2020122100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020122100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020120400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("TYPE_PROBLEM", "FK_CIT_SUG_TYPE_PROBLEM", "GKH_DICT_TYPE_PROBLEM", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG", "TYPE_PROBLEM");
        }
    }
}