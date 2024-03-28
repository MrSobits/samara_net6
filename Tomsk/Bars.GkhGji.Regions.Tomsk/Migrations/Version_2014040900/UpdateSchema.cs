namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014040900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014032000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Увеличиваю длину поля Дополнителньые материалы
            Database.ChangeColumn("GJI_REQUIREMENT", new Column("ADD_MATERIALS", DbType.String, 2000));
        }

        public override void Down()
        {
            //обратных действий производит ьненадо потомучто при уменьшении длины поля все потрется в колонке
        }
    }
}