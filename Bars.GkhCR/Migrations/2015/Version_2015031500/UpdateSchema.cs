namespace Bars.GkhCr.Migrations.Version_2015031500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migration.Version_2015031300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("CR_EST_CALC_ESTIMATE", "DOCUMENT_NAME"))
            {
                // Откатывать в методе Down не надо. Ранее не угадали с размером.
                Database.ChangeColumn("CR_EST_CALC_ESTIMATE", new Column("DOCUMENT_NAME", DbType.String, 2000));
            }
        }

        public override void Down()
        {
        }
    }
}