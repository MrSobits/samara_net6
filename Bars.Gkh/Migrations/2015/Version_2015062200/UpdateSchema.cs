namespace Bars.Gkh.Migrations._2015.Version_2015062200
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Collections.Generic;
    using System.Data;
	using Bars.B4.Application;
	using Bars.B4.DataAccess;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015061101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        if (ApplicationContext.Current.Configuration.DbDialect == DbDialect.PostgreSql)
	        {
		        var allSequences = new List<string>();
		        var tableSequences = new Dictionary<string, string>();

		        using (var reader = GetAllSequences())
		        {
			        while (reader.Read())
			        {
				        allSequences.Add(reader[0].ToString().ToLower());
			        }
		        }

		        using (var reader = GetTableSequences())
		        {
			        while (reader.Read())
			        {
				        tableSequences.Add(reader[0].ToString(), reader[1].ToString());
			        }
		        }

		        foreach (var pair in tableSequences)
		        {
			        var correctSequence = pair.Key.ToLower() + "_id_seq";
			        var sequence = pair.Value.ToLower();

			        if (sequence == correctSequence || allSequences.Contains(correctSequence))
			        {
				        continue;
			        }

			        if (Database.DatabaseKind == DbmsKind.PostgreSql)
			        {
			            Database.ExecuteNonQuery(string.Format("alter sequence {0} rename to {1}", sequence, correctSequence));
			        }
		        }
	        }
        }

        private IDataReader GetAllSequences()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                return Database.ExecuteQuery("select relname from pg_class where relkind='S'");
            }

            return null;
        }

        private IDataReader GetTableSequences()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                return Database.ExecuteQuery(@"SELECT 
                                 c.relname AS table_name,
                                 substring(d.adsrc FROM E'^nextval\\(''([^'']*)''(?:::text|::regclass)?\\)') AS seq_name 
                                FROM pg_class c 
                                JOIN pg_attribute a ON (c.oid = a.attrelid) 
                                JOIN pg_attrdef d ON (a.attrelid = d.adrelid AND a.attnum = d.adnum) 
                                WHERE d.adsrc ~ '^nextval'");
            }

            return null;
        }

        public override void Down()
        {

        }
    }
}