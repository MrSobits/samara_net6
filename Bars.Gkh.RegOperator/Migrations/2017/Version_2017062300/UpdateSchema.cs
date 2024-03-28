namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017062300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    using ForeignKeyConstraint = Bars.B4.Modules.Ecm7.Framework.ForeignKeyConstraint;

    [Migration("2017062300")]
    [MigrationDependsOn(typeof(Version_2017060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName TableName => new SchemaQualifiedObjectName
        {
            Name = "CHES_NOT_MATCH_ADDRESS",
            Schema = "IMPORT"
        };

        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_CHES_IMPORT",
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "CHES_IMPORT_PERIOD_ID", "REGOP_PERIOD", "ID"),
                new Column("STATE", DbType.Int32, ColumnProperty.NotNull),
                new Column("ANALYSIS_STATE", DbType.Int32, ColumnProperty.NotNull),
                new Column("LOADED_FILES", DbType.String, 500),
                new Column("USER_ID", DbType.Int64),
                new Column("TASK_ID", DbType.Int64));

            // создаем ключи и индексы вручную, т.к. при удалении пользователя или задачи нужна каскадность
            this.Database.AddForeignKey("FK_CHES_IMPORT_USER_ID", "REGOP_CHES_IMPORT", "USER_ID", "B4_USER", "ID", ForeignKeyConstraint.SetNull);
            this.Database.AddForeignKey("FK_CHES_IMPORT_TASK_ID", "REGOP_CHES_IMPORT", "TASK_ID", "B4_TASK_ENTRY", "ID", ForeignKeyConstraint.SetNull);

            this.Database.AddIndex("IND_CHES_IMPORT_USER_ID", false, "REGOP_CHES_IMPORT", "USER_ID");
            this.Database.AddIndex("IND_CHES_IMPORT_TASK_ID", false, "REGOP_CHES_IMPORT", "TASK_ID");

          	this.Database.ExecuteNonQuery(@"create schema if not exists IMPORT");
          
            this.Database.AddTable(this.TableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("EXTERNAL_ADDRESS", DbType.String, 300, ColumnProperty.NotNull),
                new Column("HOUSE_GUID", DbType.String, 36));

            this.Database.AddRefColumn(this.TableName, new RefColumn("PERIOD_ID", "NOT_MATCH_ADDRESS_PERIOD_ID", "REGOP_PERIOD", "ID"));

            this.Database.ExecuteNonQuery(@"
                        CREATE UNIQUE INDEX on import.ches_not_match_address(external_address, period_id);
                        CREATE INDEX on import.ches_not_match_address(house_guid);
                        CREATE INDEX on import.ches_not_match_address(period_id);
                        CREATE INDEX on import.ches_not_match_address(external_address);");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_CHES_IMPORT");
            this.Database.RemoveTable(this.TableName);
        }
    }
}