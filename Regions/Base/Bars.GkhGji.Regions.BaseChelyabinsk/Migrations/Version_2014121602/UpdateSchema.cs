namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121602
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Направление деятельности
            this.Database.AddEntityTable("GJI_ACTIVITY_DIRECTION",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));

            //вид документа-основания
            this.Database.AddEntityTable("GJI_KIND_BASE_DOC",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_ACTIVITY_DIRECTION");
            this.Database.RemoveTable("GJI_KIND_BASE_DOC");
        }
    }
}