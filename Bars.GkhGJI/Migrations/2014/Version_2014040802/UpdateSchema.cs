namespace Bars.GkhGji.Migrations.Version_2014040802
{
    using System.Data;
    using Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040802")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014040801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL_PROVDOC", new Column("DESCRIPTION", DbType.String, 2000, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL_PROVDOC", "DESCRIPTION");
        }
    }
}