namespace Bars.GkhGji.Migrations._2014.Version_2014121603
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121603")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014121602.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                    "GJI_PARAMETER",
                    new Column("KEY", DbType.String, ColumnProperty.NotNull),
                    new Column("VALUE", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PARAMETER");
        }
    }
}