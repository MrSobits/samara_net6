namespace Bars.Gkh.Migrations._2021.Version_2021061100
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021061100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021061000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OPERATOR", new Column("EXPORT_FORMAT", DbType.Int32, defaultValue: 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OPERATOR", "EXPORT_FORMAT");
        }
    }
}