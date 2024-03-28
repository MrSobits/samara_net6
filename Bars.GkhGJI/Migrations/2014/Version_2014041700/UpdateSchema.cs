namespace Bars.GkhGji.Migrations.Version_2014041700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014041100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Убираю NotNull у колонок - поскольку теперь нарушения могут быть без домов
            Database.ChangeColumn("GJI_INSPECTION_VIOLATION", new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.Null));
            Database.ChangeColumn("GJI_ACTCHECK_ROBJECT", new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.Null));
        }

        public override void Down()
        {
            //Возвращать NotNull неимет смысла поскольку уже могут быть добавлены нарушения без домов (то есть с null)
        }
    }
}