using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.GkhCr.Entities;
using System.ComponentModel;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023081800
{
    [Migration("2023081800")]
    [MigrationDependsOn(typeof(_2023.Version_2023062100.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "CR_OBJ_BUILD_CONTRACT_TERMINATION",
                new RefColumn("BUILD_CONTRACT_ID", "CR_OBJ_BC_TERM_BUILDCONTRACT", "CR_OBJ_BUILD_CONTRACT", "ID"),
                new RefColumn("DOCUMENT_FILE_ID", "CR_OBJ_BC_TERM_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("REASON_ID", "CR_OBJ_BC_TERM_REASON", "CR_DICT_TERMINATION_REASON", "ID"),
                new Column("REASON", DbType.String),
                new Column("DOCUMENT_NUMBER", DbType.String),
                new Column("TERMINATION_DATE", DbType.DateTime));

            Database.ExecuteNonQuery("insert into cr_obj_build_contract_termination (object_create_date, object_edit_date, object_version, build_contract_id, termination_date, reason, document_file_id, document_number, reason_id)" +
            "select current_date, current_date, 1, id, termination_date, termination_reason, termination_document_file_id, termination_document_number, termination_reason_id from cr_obj_build_contract");
        }

        public override void Down()
        {
            Database.RemoveTable("CR_OBJ_BUILD_CONTRACT_TERMINATION");        
        }
    }
}