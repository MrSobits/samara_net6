namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2014060600
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2014060300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("REGOP_CONFCONTRIB_DOC", "DOCUMENT_DATE"))
            {
                Database.RenameColumn("REGOP_CONFCONTRIB_DOC", "DOCUMENT_DATE", "TRANSFER_DATE");
            }
        }

        public override void Down()
        {
            if (Database.ColumnExists("REGOP_CONFCONTRIB_DOC", "TRANSFER_DATE"))
            {
                Database.RenameColumn("REGOP_CONFCONTRIB_DOC", "TRANSFER_DATE", "DOCUMENT_DATE");
            }
        }
    }
}