namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014032800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014032700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Database.RemoveColumn("OVRHL_CREDIT_ORG", "OKTMO");            
        }

        public override void Down()
        {
           // Database.AddColumn("OVRHL_CREDIT_ORG", new Column("OKTMO", DbType.String));
        }
    }
}