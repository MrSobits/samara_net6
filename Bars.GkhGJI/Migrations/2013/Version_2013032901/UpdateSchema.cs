using System.Data;

namespace Bars.GkhGji.Migrations.Version_2013032901
{
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013032800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_INSPECTION_DISPHEAD", "IS_INSPECTION_SURVEY");
        }

        public override void Down()
        {
            Database.AddColumn("GJI_INSPECTION_DISPHEAD", new Column("IS_INSPECTION_SURVEY", DbType.Boolean, ColumnProperty.NotNull, false));
        }
    }
}