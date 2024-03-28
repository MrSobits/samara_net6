namespace Bars.Gkh.Migrations.Version_2013101600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013101100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CIT_SUG", new Column("HAS_ANSWER", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddColumn("GKH_CIT_SUG", new Column("ANSWER_TEXT", DbType.String, 1000, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG", "HAS_ANSWER");
            Database.RemoveColumn("GKH_CIT_SUG", "ANSWER_TEXT");
        }
    }
}