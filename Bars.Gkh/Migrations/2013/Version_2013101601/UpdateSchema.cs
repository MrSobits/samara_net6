namespace Bars.Gkh.Migrations.Version_2013101601
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013101600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_SUG_RUBRIC", new Column("CODE", DbType.Int64, ColumnProperty.NotNull, 0));
            Database.AddColumn("GKH_SUG_RUBRIC", new Column("IS_ACTUAL", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_SUG_RUBRIC", "CODE");
            Database.RemoveColumn("GKH_SUG_RUBRIC", "IS_ACTUAL");
        }
    }
}