namespace Bars.Gkh.Migrations._2015.Version_2015092800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015092101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveIndex("IND_CLW_PETITION", "CLW_DICT_PETITION_TYPE");
        }

        public override void Down()
        {
        }
    }
}
