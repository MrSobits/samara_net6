namespace Bars.Gkh.Migrations._2021.Version_2021052600
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021052600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021051100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("DECLINE_REASON", DbType.String, ColumnProperty.None, 4000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "DECLINE_REASON");
        }
    }
}