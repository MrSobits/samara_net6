namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014121603
{
    using System.Collections.Generic;
    using System.Text;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using B4.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121603")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014121602.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn(
                "GJI_NSO_DISP_VERIFSUBJ",
                new RefColumn(
                    "SURVEY_SUBJECT_ID",
                    ColumnProperty.Null,
                    "GJI_NSO_DISP_VERIFSUBJ_SSD",
                    "GJI_DICT_SURVEY_SUBJ",
                    "ID"));


            MigrateData();
        }

        private void MigrateData()
        {
            var maxId = GetNextId("GJI_DICT_SURVEY_SUBJ");

            var insert = new StringBuilder();
            var codeName = new Dictionary<string, string>();
            using (var reader = Database.ExecuteQuery(@"select distinct(TYPE_VERIF_SUBJ) as type,
                     case when TYPE_VERIF_SUBJ = 10 then 'соблюдение обязательных требований'
                      when TYPE_VERIF_SUBJ = 20 then 'соответствие сведений, содержащихся в уведомлении о начале осуществления отдельных видов предпринимательской деятельности, обязательным требованиям'
                      when TYPE_VERIF_SUBJ = 30 then 'выполнение предписаний органов государственного контроля (надзора)'
                     end as name
                  from GJI_NSO_DISP_VERIFSUBJ"))
            {
                while (reader.Read())
                {
                    codeName[reader["type"].ToString()] = reader["name"].ToString();
                }
            }

            foreach (var kv in codeName)
            {
                var id = maxId++;
                insert.AppendFormat(
                    "insert into GJI_DICT_SURVEY_SUBJ (id, code, name, object_version, object_create_date, object_edit_date) values ({0}, '{1}', '{2}', {3}, {4}, {5});\r\n",
                    id,
                    kv.Key,
                    kv.Value,
                    0,
                    "now()",
                    "now()");

                insert.AppendFormat(
                    "update GJI_NSO_DISP_VERIFSUBJ set SURVEY_SUBJECT_ID = {0} where TYPE_VERIF_SUBJ = {1};\r\n",
                    id,
                    kv.Key.ToInt());
            }

            if (insert.Length > 0)
            {
                Database.ExecuteNonQuery(insert.ToString());
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_NSO_DISP_VERIFSUBJ", "SURVEY_SUBJECT_ID");
        }

        private long GetNextId(string tableName)
        {
            var maxId = 0L;
            using (var idReader = Database.ExecuteQuery(string.Format("SELECT MAX(ID) FROM {0}", tableName)))
            {
                if (idReader.Read())
                {
                    maxId = idReader[0].ToLong();
                }
            }

            return maxId + 1;
        }
    }
}