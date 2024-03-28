namespace Bars.Gkh.Migrations.Version_2013072401
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Изменил поля справочника "Группы конструктивного элемента"
            Database.RemoveColumn("GKH_DICT_CONEL_GROUP", "CODE");
            Database.RemoveColumn("GKH_DICT_CONEL_GROUP", "CLASS");

            Database.AddColumn("GKH_DICT_CONEL_GROUP", new Column("NECESSARILY", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
        }
    }
}