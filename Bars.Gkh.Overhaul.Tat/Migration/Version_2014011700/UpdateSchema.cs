namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014011700
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014011700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014011601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
           // AddColumn("OVRHL_ACCOUNT", "OVRHL_ACC_ROBJ", "LONG_TERM_OBJ_ID");

            AddColumn("OVRHL_PROP_OWN_DECISION_BASE", "OVRHL_OWNDEC_BASE_ROBJ", "OBJECT_ID");

            AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "OVRHL_OWNPROT_ROBJ", "OBJECT_ID");
        }

        public override void Down()
        {
           // RemoveColumn("OVRHL_ACCOUNT", "OVRHL_ACC_ROBJ", "LONG_TERM_OBJ_ID");

            RemoveColumn("OVRHL_PROP_OWN_DECISION_BASE", "OVRHL_OWNDEC_BASE_LTO", "OBJECT_ID");

            RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "OVRHL_OWNPROT_LTO", "OBJECT_ID");
        }

        private void AddColumn(string tableName, string indexFkName, string joinColumn)
        {
            var columnName = "REALITY_OBJECT_ID";

            Database.AddRefColumn(tableName, new RefColumn(columnName, indexFkName, "GKH_REALITY_OBJECT", "ID"));

            //перевешиваем существующие записи на жилые дома
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(string.Format("update {0} as t1 set REALITY_OBJECT_ID = t2.REALITY_OBJ_ID from OVRHL_LONGTERM_PR_OBJECT t2 where t2.id=t1.{1}", tableName, joinColumn));
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(string.Format("update {0} t1 set t1.REALITY_OBJECT_ID = (select t2.REALITY_OBJ_ID from OVRHL_LONGTERM_PR_OBJECT t2 where t2.id=t1.{1})", tableName, joinColumn));
            }

            //удаляем те записи, для которых не нашелся жилой дом
            DeleteNullRecords(tableName, columnName);

            AddNotNull(tableName, columnName);

            Database.RemoveColumn(tableName, joinColumn);
        }

        private void RemoveColumn(string tableName, string indexFkName, string joinColumn)
        {
            Database.AddRefColumn(tableName, new RefColumn(joinColumn, indexFkName, "OVRHL_LONGTERM_PR_OBJECT", "ID"));

            //перевешиваем существующие записи на одп
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(string.Format("update {0} as t1 set {1} = t2.id from OVRHL_LONGTERM_PR_OBJECT t2 where t2.REALITY_OBJ_ID=t1.REALITY_OBJECT_ID", tableName, joinColumn));
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(string.Format("update {0} t1 set t1.{1} = (select t2.id from OVRHL_LONGTERM_PR_OBJECT t2 where t2.REALITY_OBJ_ID=t1.REALITY_OBJECT_ID)", tableName, joinColumn));
            }

            //удаляем те записи, для которых не нашелся одп
            DeleteNullRecords(tableName, joinColumn);

            AddNotNull(tableName, joinColumn);

            Database.RemoveColumn(tableName, "REALITY_OBJECT_ID");
        }

        private void DeleteNullRecords(string tableName, string column)
        {
            Database.ExecuteNonQuery(string.Format("delete from {0} where {1} is null", tableName, column));
        }

        private void AddNotNull(string tableName, string column)
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(string.Format("alter table {0} alter column {1} set not null", tableName, column));
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(string.Format("alter table {0} modify {1} not null", tableName, column));
            }
        }
    }
}