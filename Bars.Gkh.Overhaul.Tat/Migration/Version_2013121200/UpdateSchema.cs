namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013121200
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013121100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                if (Database.ColumnExists("OVRHL_ACCOUNT", "NUMBER"))
                {
                    Database.RenameColumn("OVRHL_ACCOUNT", "NUMBER", "ACC_NUMBER");
                }
            }
        }

        public override void Down()
        {

        }
    }
}