namespace Bars.Gkh.Regions.Tatarstan.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_TAT_ROLE_TYPEHOUSE",
                new Column("TYPE_HOUSE", DbType.Int64, ColumnProperty.NotNull, 0),
                new RefColumn("ROLE_ID", ColumnProperty.NotNull, "GKHTAT_ROLETYPEHOUSE_ROLE", "B4_ROLE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_TAT_ROLE_TYPEHOUSE");    
        }
    }
}