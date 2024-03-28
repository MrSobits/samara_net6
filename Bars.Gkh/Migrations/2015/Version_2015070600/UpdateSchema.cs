namespace Bars.Gkh.Migrations.Version_2015070600
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015070600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015070202.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Down()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                var triggers =
                            Database.ExecuteQuery(
                                "SELECT event_object_table FROM information_schema.triggers WHERE trigger_name = 'table_locker'");
                var tableNames = new List<string>();
                while (triggers.Read())
                {
                    tableNames.Add(triggers.GetString(0));
                }

                foreach (var tableName in tableNames.Distinct())
                {
                    Database.ExecuteQuery(string.Format("DROP TRIGGER IF EXISTS table_locker ON {0}", tableName));
                }
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                var triggers =
                            Database.ExecuteQuery(
                                "SELECT OBJECT_NAME FROM USER_OBJECTS WHERE OBJECT_TYPE = 'TRIGGER' AND OBJECT_NAME LIKE '%_TL'");

                while (triggers.Read())
                {
                    var triggerName = triggers.GetString(0);
                    Database.ExecuteQuery(string.Format("DROP TRIGGER {0}", triggerName));
                }
            }

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("DROP FUNCTION IF EXISTS table_locker()");
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("DROP PROCEDURE TABLE_LOCKER");
            }

            Database.RemoveTable("TABLE_LOCK");
        }

        public override void Up()
        {
            Database.AddTable("TABLE_LOCK", new Column("TABLE_NAME", DbType.String, 30, ColumnProperty.NotNull), new Column("LOCK_START", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddIndex("TBL_LOCK_TBL_IND", true, "TABLE_LOCK", "TABLE_NAME");

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"CREATE OR REPLACE FUNCTION table_locker() RETURNS TRIGGER AS $table_locker$
  BEGIN
    IF EXISTS(SELECT * FROM TABLE_LOCK WHERE LOWER(TABLE_NAME) = LOWER(TG_TABLE_NAME)) THEN
      RAISE EXCEPTION 'В настоящий момент изменение некоторых данных заблокировано. Повторите попытку позже';
    END IF;
    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
      RETURN NEW;
    END IF;
    IF TG_OP = 'DELETE' THEN
      RETURN OLD;
    END IF;
  END;
$table_locker$ LANGUAGE plpgsql");
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"CREATE OR REPLACE PROCEDURE TABLE_LOCKER(
    TNAME IN VARCHAR2)
AS
  cnt              NUMBER(38, 0);
  LOCKED_EXCEPTION EXCEPTION;
BEGIN
  SELECT COUNT(ID)
  INTO cnt
  FROM TABLE_LOCK
  WHERE LOWER(TABLE_NAME) = LOWER(TNAME);
  IF cnt                  > 0 THEN
    RAISE LOCKED_EXCEPTION;
  END IF;
END TABLE_LOCKER;
");
            }
        }
    }
}