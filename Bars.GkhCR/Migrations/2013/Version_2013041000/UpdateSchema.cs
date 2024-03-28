namespace Bars.GkhCr.Migrations.Version_2013041000
{
    using Bars.Gkh;
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013040900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("cr_dict_fin_source", new Column("code", DbType.String, 200));

            Database.ChangeColumn("cr_dict_program", new Column("code", DbType.String, 200));
            Database.ChangeColumn("cr_dict_program", new Column("description", DbType.String, 2000));

            Database.ChangeColumn("cr_object", new Column("gji_num", DbType.String, 300));
            Database.ChangeColumn("cr_object", new Column("program_num", DbType.String, 300));
            Database.ChangeColumn("cr_object", new Column("federal_num", DbType.String, 300));

            Database.ChangeColumn("cr_obj_protocol", new Column("document_num", DbType.String, 300));
            Database.ChangeColumn("cr_obj_protocol", new Column("description", DbType.String, 2000));

            Database.ChangeColumn("cr_obj_contract", new Column("document_num", DbType.String, 300));
            Database.ChangeColumn("cr_obj_contract", new Column("description", DbType.String, 2000));

            Database.ChangeColumn("cr_obj_type_work", new Column("manufacturer_name", DbType.String, 2000));
            Database.ChangeColumn("cr_obj_type_work", new Column("description", DbType.String, 2000));

            Database.ChangeColumn("cr_obj_perfomed_work_act", new Column("document_num", DbType.String, 300));

            Database.ChangeColumn("cr_obj_perfomed_wact_rec", new Column("reason", DbType.String, 1000));
            Database.ChangeColumn("cr_obj_perfomed_wact_rec", new Column("document_num", DbType.String, 250));
            Database.ChangeColumn("cr_obj_perfomed_wact_rec", new Column("document_name", DbType.String, 1000));

            Database.ChangeColumn("cr_voice_qual_member", new Column("reason", DbType.String, 2000));

            Database.ChangeColumn("cr_obj_estimate_calc", new Column("res_stat_doc_num", DbType.String, 300));
            Database.ChangeColumn("cr_obj_estimate_calc", new Column("estimate_doc_num", DbType.String, 300));
            Database.ChangeColumn("cr_obj_estimate_calc", new Column("estimate_file_doc_num", DbType.String, 300));

            Database.ChangeColumn("cr_est_calc_estimate", new Column("reason", DbType.String, 1000));
            Database.ChangeColumn("cr_est_calc_estimate", new Column("document_num", DbType.String, 300));
            Database.ChangeColumn("cr_est_calc_estimate", new Column("document_name", DbType.String, 1000));

            Database.ChangeColumn("cr_est_calc_res_statem", new Column("reason", DbType.String, 1000));
            Database.ChangeColumn("cr_est_calc_res_statem", new Column("document_num", DbType.String, 300));
            Database.ChangeColumn("cr_est_calc_res_statem", new Column("document_name", DbType.String, 1000));

            Database.ChangeColumn("cr_obj_bank_statement", new Column("personal_account", DbType.String, 300));
            Database.ChangeColumn("cr_obj_bank_statement", new Column("document_num", DbType.String, 300));

            Database.ChangeColumn("cr_payment_order", new Column("bid_num", DbType.String, 300));
            Database.ChangeColumn("cr_payment_order", new Column("document_num", DbType.String, 300));

            Database.ChangeColumn("cr_obj_cmp_archive", new Column("manufacturer_name", DbType.String, 2000));
        }

        public override void Down()
        {
        }
    }
}