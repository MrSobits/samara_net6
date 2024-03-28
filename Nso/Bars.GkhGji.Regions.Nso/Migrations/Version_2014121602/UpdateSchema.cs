namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014121602
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014121601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Направление деятельности
            Database.AddEntityTable("GJI_ACTIVITY_DIRECTION",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));

            //вид документа-основания
            Database.AddEntityTable("GJI_KIND_BASE_DOC",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTIVITY_DIRECTION");
            Database.RemoveTable("GJI_KIND_BASE_DOC");
        }
    }
}