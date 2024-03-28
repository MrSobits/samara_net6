namespace Bars.Gkh.Migrations.Version_2015070900
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015070900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015070600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Down()
        {
            Database.ExecuteNonQuery("DELETE FROM TABLE_LOCK WHERE 1 = 1");
            Database.RemoveColumn("TABLE_LOCK", "ACTION");

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {

                var triggers = Database.ExecuteQuery("SELECT event_object_table, trigger_name FROM information_schema.triggers WHERE trigger_name like '%_tl'");

                while (triggers.Read())
                {
                    Database.ExecuteQuery(string.Format("DROP TRIGGER IF EXISTS {1} ON {0}", triggers.GetString(0), triggers.GetString(1)));
                }
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                var triggers = Database.ExecuteQuery("SELECT OBJECT_NAME FROM USER_OBJECTS WHERE OBJECT_TYPE = 'TRIGGER' AND OBJECT_NAME LIKE '%_TL'");

                while (triggers.Read())
                {
                    Database.ExecuteQuery(string.Format("DROP TRIGGER {0}", triggers.GetString(0)));
                }
            }

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

        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {

                var triggers = Database.ExecuteQuery("SELECT event_object_table FROM information_schema.triggers WHERE trigger_name = 'table_locker'");
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
                var triggers = Database.ExecuteQuery("SELECT OBJECT_NAME FROM USER_OBJECTS WHERE OBJECT_TYPE = 'TRIGGER' AND OBJECT_NAME LIKE '%_TL'");

                while (triggers.Read())
                {
                    var triggerName = triggers.GetString(0);
                    Database.ExecuteQuery(string.Format("DROP TRIGGER {0}", triggerName));
                }
            }

            Database.ExecuteNonQuery("DELETE FROM TABLE_LOCK WHERE 1 = 1");
            Database.AddColumn("TABLE_LOCK", "ACTION", DbType.String, 6, ColumnProperty.NotNull);
            Database.RemoveIndex("TBL_LOCK_TBL_IND", "TABLE_LOCK");
            Database.AddIndex("TBL_LOCK_TBL_IND", true, "TABLE_LOCK", "TABLE_NAME", "ACTION");

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"CREATE OR REPLACE FUNCTION table_locker() RETURNS TRIGGER AS $table_locker$
  BEGIN
    IF EXISTS(SELECT * FROM TABLE_LOCK WHERE LOWER(TABLE_NAME) = LOWER(TG_TABLE_NAME) AND LOWER(ACTION) = LOWER(TG_OP)) THEN
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
    TNAME IN VARCHAR2
    ACT IN VARCHAR2)
AS
  cnt              NUMBER(38, 0);
  LOCKED_EXCEPTION EXCEPTION;
BEGIN
  SELECT COUNT(ID)
  INTO cnt
  FROM TABLE_LOCK
  WHERE LOWER(TABLE_NAME) = LOWER(TNAME) AND LOWER(ACTION) = LOWER(ACT);
  IF cnt                  > 0 THEN
    RAISE LOCKED_EXCEPTION;
  END IF;
END TABLE_LOCKER;
");
            }
        }
    }
}