using System.Data;
using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.GkhRf.Migrations.Version_2013031600
{
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013031600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhRf.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            ViewManager.Drop(Database, "GkhRf");
            Database.RemoveColumn("RF_REQUEST_TRANSFER", "DOCUMENT_NUM");
            Database.AddColumn("RF_REQUEST_TRANSFER", "DOCUMENT_NUM", DbType.Int32);

            //-----таблица проверки лимита по заявке
            Database.AddEntityTable("RF_LIMIT_CHECK",
                new Column("TYPE_PROGRAM", DbType.Int32, 4, ColumnProperty.NotNull));
            //-----

            //-----таблица связи проверки лимита по заявке и источника финансирования
            Database.AddEntityTable("RF_LIMIT_CHECK_FINSOURCE",
                new RefColumn("LIMIT_CHECK_ID", ColumnProperty.NotNull, "RF_LIMITCH_FS_L", "RF_LIMIT_CHECK", "ID"),
                new RefColumn("FINANCE_SOURCE_ID", ColumnProperty.NotNull, "RF_LIMITCH_FS_FS_F", "CR_DICT_FIN_SOURCE", "ID"));
            //-----
            ViewManager.Create(Database, "GkhRf");
        }

        public override void Down()
        {
            Database.RemoveColumn("RF_REQUEST_TRANSFER", "DOCUMENT_NUM");
            Database.AddColumn("RF_REQUEST_TRANSFER", "DOCUMENT_NUM", DbType.String, 50);

            Database.RemoveTable("RF_LIMIT_CHECK_FINSOURCE");
            Database.RemoveTable("RF_LIMIT_CHECK");
            ViewManager.Drop(Database, "GkhRf");
        }
    }
}