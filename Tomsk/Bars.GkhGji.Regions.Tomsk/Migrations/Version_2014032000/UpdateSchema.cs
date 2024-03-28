namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014032000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014030700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_ACTCHECK_FAMILIAR",
                new Column("FIO", DbType.String, 300),
                new RefColumn("ACTCHECK_ID", ColumnProperty.NotNull, "GJI_ACTCHECK_FAM_ACT", "GJI_ACTCHECK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTCHECK_FAMILIAR");
        }
    }
}