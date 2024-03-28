namespace Bars.GkhCr.Migration.Version_2015031300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migration.Version_2015031201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("CR_OBJ_PROTOCOL", new RefColumn("TYPE_DOCUMENT_CR_ID", "TYPE_DOCUMENT_CR_ID", "GKH_DICT_MULTIITEM", "ID"));

            if (Database.ColumnExists("CR_OBJ_PROTOCOL", "TYPE_DOCUMENT_CR"))
            {
                Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'Act')
where TYPE_DOCUMENT_CR = 10");

                Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ProtocolFailureCr')
where TYPE_DOCUMENT_CR = 20");

                Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ProtocolNeedCr')
where TYPE_DOCUMENT_CR = 30");

                Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ProtocolChangeCr')
where TYPE_DOCUMENT_CR = 40");

                Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ActExpluatatinAfterCr')
where TYPE_DOCUMENT_CR = 50");

                Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ProtocolCompleteCr')
where TYPE_DOCUMENT_CR = 60");

                Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ActAuditDataExpense')
where TYPE_DOCUMENT_CR = 70");

                Database.RemoveColumn("CR_OBJ_PROTOCOL", "TYPE_DOCUMENT_CR");
            }
        }

        public override void Down()
        {
            Database.AddColumn("CR_OBJ_PROTOCOL", "TYPE_DOCUMENT_CR", DbType.Int32, ColumnProperty.NotNull, 10);

            Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR = 20
where TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ProtocolFailureCr')");

            Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR = 30
where TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ProtocolNeedCr')");

            Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR = 40
where TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ProtocolChangeCr')");

            Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR = 50
where TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ActExpluatatinAfterCr')");

            Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR = 60
where TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ProtocolCompleteCr')");

            Database.ExecuteNonQuery(@"update CR_OBJ_PROTOCOL
set TYPE_DOCUMENT_CR = 70
where TYPE_DOCUMENT_CR_ID = (select min(id) from gkh_dict_multiitem
where key = 'ActAuditDataExpense')");

            Database.RemoveColumn("CR_OBJ_PROTOCOL", "TYPE_DOCUMENT_CR_ID");
        }
    }
}