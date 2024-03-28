namespace Bars.Gkh.Migrations.Version_2013103001
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013103001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013103000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_NORMATIVE_DOC", new Column("EXTERNAL_ID", DbType.String, 36));
        }
        
        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_NORMATIVE_DOC", "EXTERNAL_ID");
        }
    }
}