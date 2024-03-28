namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013112700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013112601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            
            // удаляем колонку из записи Краткосрочной программы
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "TYPE_ACTUALITY");

            // Добавляем колонку в Дом Краткосрочки
            Database.AddColumn("OVRHL_SHORT_PROG_OBJ", new Column("TYPE_ACTUALITY", DbType.Int16, 4, ColumnProperty.NotNull, 10));
            
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SHORT_PROG_OBJ", "TYPE_ACTUALITY");

            Database.AddColumn("OVRHL_SHORT_PROG_REC", new Column("TYPE_ACTUALITY", DbType.Int16, 4, ColumnProperty.NotNull, 10));
        }
    }
}