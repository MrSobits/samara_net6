namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014121200
{
    using Bars.B4.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014120800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Добавляем дефолтные значения Настройки параметров РФ для полей
        /// Archiving, GroupingOption, Format
        /// </summary>
        public override void Up()
        {
            Database.ExecuteNonQuery("delete from REGOP_PARAMETER where key = 'Archiving'");
            Database.ExecuteNonQuery("delete from REGOP_PARAMETER where key = 'GroupingOption'");
            Database.ExecuteNonQuery("delete from REGOP_PARAMETER where key = 'Format'");

            Database.ExecuteNonQuery("INSERT INTO REGOP_PARAMETER (id, object_version, object_create_date, object_edit_date, key, value) VALUES ({0}, 0, now(), now(), 'Archiving', '0')"
                .FormatUsing(GetNextId("REGOP_PARAMETER")));
            Database.ExecuteNonQuery("INSERT INTO REGOP_PARAMETER (id, object_version, object_create_date, object_edit_date, key, value) VALUES ({0}, 0, now(), now(), 'GroupingOption', '0')"
                .FormatUsing(GetNextId("REGOP_PARAMETER")));
            Database.ExecuteNonQuery("INSERT INTO REGOP_PARAMETER (id, object_version, object_create_date, object_edit_date, key, value) VALUES ({0}, 0, now(), now(), 'Format', '0')"
                .FormatUsing(GetNextId("REGOP_PARAMETER")));
        }

        /// <summary>
        /// Удаляем значения Настройки параметров РФ для полей
        /// Archiving, GroupingOption, Format (Их не было до этого времени)
        /// </summary>
        public override void Down()
        {
            Database.ExecuteNonQuery("delete from REGOP_PARAMETER where key = 'Archiving'");
            Database.ExecuteNonQuery("delete from REGOP_PARAMETER where key = 'GroupingOption'");
            Database.ExecuteNonQuery("delete from REGOP_PARAMETER where key = 'Format'");
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
