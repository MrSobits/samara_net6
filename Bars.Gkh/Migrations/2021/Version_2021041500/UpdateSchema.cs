namespace Bars.Gkh.Migrations._2021.Version_2021041500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021041500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021030400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("IS_CLUTTERED", DbType.Boolean, ColumnProperty.NotNull, false));       
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "IS_CLUTTERED");
        }
    }
}