namespace Bars.Gkh.Regions.Tatarstan.Migrations._2019.Version_2019060500
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019060500")]
    [MigrationDependsOn(typeof(_2018.Version_2018122000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly List<Tuple<string, string, string>> listTable = new List<Tuple<string, string, string>>
        {
            Tuple.Create("GJI_NSO_DISP_VERIFSUBJ", "survey_subject_id", "gji_dict_survey_subj"),
            Tuple.Create("GJI_NSO_DISPOSAL_SURVEY_PURP", "survey_purp_id", "gji_dict_survey_purp"),
            Tuple.Create("GJI_NSO_DISPOSAL_SURVEY_OBJ", "survey_obj_id", "gji_dict_survey_obj"),
            Tuple.Create("GJI_NSO_DISPOSAL_INSPFOUNDCHECK", "inspfound_id", "gkh_dict_normative_doc")
        };

        public override void Up()
        {
            var sb = new StringBuilder();
            foreach (var list in this.listTable)
            {
                sb.Append(
                    $@"insert into {list.Item1}(object_version, object_create_date, object_edit_date, disposal_id, {list.Item2})
                        select 19092, now()::timestamp(0), now()::timestamp(0), d.id, (select id from {list.Item3} where name = 'Отсутствует') 
                            from gji_inspection i
                            join gji_document doc on doc.inspection_id = i.id
                            join gji_disposal d on d.id = doc.id
                        where type_base in (30, 50, 40, 20) 
                            and not exists(select 1 from {list.Item1} s where s.disposal_id = d.id) 
                            and (select id from {list.Item3} where name = 'Отсутствует') is not null;");
            }

            this.Database.ExecuteNonQuery(sb.ToString());
        }

        public override void Down()
        {
            var sb = new StringBuilder();
            foreach (var list in this.listTable)
            {
                sb.Append($@"DELETE from {list.Item1} WHERE object_version=19092");
            }

            this.Database.ExecuteNonQuery(sb.ToString());
        }
    }
}