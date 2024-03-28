namespace Bars.GkhDi.Migrations.Version_2013092500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013072300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DISINFO_FINACT_DOCYEAR", new Column("DOCUMENT_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DISINFO_FINACT_DOCYEAR", "DOCUMENT_DATE");
        }
    }
}