namespace Bars.GkhEdoInteg.Migrations.Version_2013061700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013061700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhEdoInteg.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("INTGEDO_APPCITS", new Column("IS_DOC_EDO", DbType.Boolean, false));
            Database.AddColumn("INTGEDO_APPCITS", new Column("DATE_LOAD_DOC", DbType.DateTime));

            Database.AddColumn("INTGEDO_APPCITS", new Column("COUNT_LOAD_DOC", DbType.Int32, 0));
            Database.AddColumn("INTGEDO_APPCITS", new Column("MSG_LOAD_DOC", DbType.String, 250));
        }

        public override void Down()
        {
            Database.RemoveColumn("INTGEDO_APPCITS", "IS_DOC_EDO");
            Database.RemoveColumn("INTGEDO_APPCITS", "DATE_LOAD_DOC");

            Database.RemoveColumn("INTGEDO_APPCITS", "COUNT_LOAD_DOC");
            Database.RemoveColumn("INTGEDO_APPCITS", "MSG_LOAD_DOC");
        }
    }
}