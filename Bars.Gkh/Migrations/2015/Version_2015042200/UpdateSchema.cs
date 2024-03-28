namespace Bars.Gkh.Migration.Version_2015042200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015042200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015041000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("GKH_DICT_NORMATIVE_DOC_ITEM", "DOC_NUMBER"))
            {
                Database.ChangeColumn("GKH_DICT_NORMATIVE_DOC_ITEM", new Column("DOC_NUMBER", DbType.String, 400));
            }
        }

        public override void Down()
        {
        }
    }
}