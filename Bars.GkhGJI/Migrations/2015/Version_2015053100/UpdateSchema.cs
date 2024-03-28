namespace Bars.GkhGji.Migrations.Version_2015053100
{
    using B4.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;
	using Bars.B4.Application;
	using Bars.B4.DataAccess;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015053100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015052500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
	    public override void Up()
	    {
		    if (Database.DatabaseKind == DbmsKind.PostgreSql)
		    {
			    bool oldSequenceExists = false;
			    bool newSequenceExists = false;

			    using (var reader = GetReaderOldSequence())
			    {
				    if (reader.Read())
				    {
					    oldSequenceExists = reader[0].ToBool();
				    }
			    }

			    using (var reader = GetReaderNewSequence())
			    {
				    if (reader.Read())
				    {
					    newSequenceExists = reader[0].ToBool();
				    }
			    }

			    if (oldSequenceExists && !newSequenceExists)
			    {
				    Database.ExecuteNonQuery(
					    "alter sequence gji_dict_violation_normativedocitem_id_seq rename to gji_dict_viol_normditem_id_seq");
			    }
		    }

		    Database.AddColumn("GJI_DICT_VIOL_NORMDITEM", new Column("VIOL_STRUCT", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_VIOL_NORMDITEM", "VIOL_STRUCT");
        }

        private IDataReader GetReaderOldSequence()
        {
            return Database
                .ExecuteQuery("SELECT 1 as name FROM pg_class where relname = 'gji_dict_violation_normativedocitem_id_seq'");
        }

        private IDataReader GetReaderNewSequence()
        {
            return Database
                .ExecuteQuery("SELECT 1 as name FROM pg_class where relname = 'gji_dict_viol_normditem_id_seq'");
        }
    }
}