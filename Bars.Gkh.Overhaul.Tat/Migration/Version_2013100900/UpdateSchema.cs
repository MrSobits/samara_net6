namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013100900
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013100801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Объект долгосрочной программы
            Database.AddEntityTable(
                    "OVRHL_LONGTERM_PR_OBJECT",
                    new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull)
                );
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_LONGTERM_PR_OBJECT");
        }
    }
}
