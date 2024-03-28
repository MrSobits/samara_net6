using System.Data;

namespace Bars.GkhRf.Migrations.Version_2013041000
{
    using Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhRf.Migrations.Version_2013031600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("RF_TRANSFER_REC_OBJ", "EXTERNAL_ID"))
            {
                Database.AddColumn("RF_TRANSFER_REC_OBJ", new Column("EXTERNAL_ID", DbType.String, 36));
            }
        }

        public override void Down()
        {
        }
    }
}