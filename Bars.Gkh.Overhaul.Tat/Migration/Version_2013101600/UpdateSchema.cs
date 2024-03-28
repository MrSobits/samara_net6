namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013101600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013101500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            //Database.AddColumn("OVRHL_STRUCT_EL", new Column("MUT_EXCLUS_GROUP", DbType.String, 300));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_STRUCT_EL", "MUT_EXCLUS_GROUP");
        }
    }
}