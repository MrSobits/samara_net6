namespace Bars.GkhCr.Migrations.Version_2013032300
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013032102.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_PAYMENT_ORDER", new Column("REPEAT_SEND", DbType.Boolean, false));
            Database.AddColumn("CR_PAYMENT_ORDER", new Column("IDDOC", DbType.String, 250));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_PAYMENT_ORDER", "REPEAT_SEND");
            Database.RemoveColumn("CR_PAYMENT_ORDER", "IDDOC");
        }
    }
}