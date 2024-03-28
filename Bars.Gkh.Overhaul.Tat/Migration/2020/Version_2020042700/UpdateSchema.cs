namespace Bars.Gkh.Overhaul.Tat.Migration._2020.Version_2020042700
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020042700")]
    [MigrationDependsOn(typeof(Version_2019022200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            var query = @"drop table if exists tmp_acc_notice_row_number;
                           create temp table tmp_acc_notice_row_number as
                             select t1.id, t1.notice_number, t1.notice_num, t4.code, row_number() over(partition by t4.id order by t1.object_create_date)
                             from OVRHL_DEC_SPEC_ACC_NOTICE t1
                               join OVRHL_PROP_OWN_DECISION_BASE t2 on t1.spec_acc_dec_id = t2.id
                               join GKH_REALITY_OBJECT t3 on t2.reality_object_id = t3.id
                               join GKH_DICT_MUNICIPALITY t4 on t3.municipality_id = t4.id;
                           
                           --обновление номера
                           update tmp_acc_notice_row_number set notice_num = row_number;
                           update tmp_acc_notice_row_number set notice_number = code||'-'||notice_num;
                           
                           --обновление основной таблицы
                           update OVRHL_DEC_SPEC_ACC_NOTICE t1 set (notice_number,notice_num,object_edit_date) = (t2.notice_number,t2.notice_num,now())
                           from tmp_acc_notice_row_number t2
                           where t1.id = t2.id;";

            this.Database.ExecuteNonQuery(query);
        }
    }
}
