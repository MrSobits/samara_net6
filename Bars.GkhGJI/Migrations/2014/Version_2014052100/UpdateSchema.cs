namespace Bars.GkhGji.Migrations.Version_2014052100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014052000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("SPECIAL_CONTROL", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "SPECIAL_CONTROL");
        }
    }
}