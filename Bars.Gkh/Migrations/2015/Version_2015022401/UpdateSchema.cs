namespace Bars.Gkh.Migration.Version_2015022401
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015022401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015022400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_SUG_RUBRIC", new Column("EXPIRE_SUGGESTION_TERM", DbType.Int32, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_SUG_RUBRIC", "EXPIRE_SUGGESTION_TERM");
        }
    }
}