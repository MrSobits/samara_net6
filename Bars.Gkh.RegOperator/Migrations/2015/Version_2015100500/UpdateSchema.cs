namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015100500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015093002.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Метод выполняется во время наката миграции
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("STATE", DbType.Int16, ColumnProperty.NotNull, 20));
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "uuid_generate_v4()::text"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "RAWTOHEX(sys_guid())"));
            }
            Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("ACCEPTED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("PAD_STATE", DbType.Int32, ColumnProperty.NotNull, 10));
            Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("PC_STATE", DbType.Int32, ColumnProperty.NotNull, 10));
        }

        /// <summary>
        /// Метод выполняется во время отката миграции
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "STATE");
            Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "TRANSFER_GUID");
            Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "ACCEPTED");
            Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "PAD_STATE");
            Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "PC_STATE");
        }
    }
}
