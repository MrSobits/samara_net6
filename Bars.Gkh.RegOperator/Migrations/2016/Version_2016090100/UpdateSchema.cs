namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016090100
{
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016090100
    /// </summary>
    [Migration("2016090100")]
    [MigrationDependsOn(typeof(Version_2016012100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016031800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016041000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016051800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016052700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016080900.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016082700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_PAYMENT_OPERATION_BASE",
                new Column("DOC_NUM", DbType.String, 200),
                new Column("DOC_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("OP_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("FACT_OP_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("REASON", DbType.String, 200),
                new Column("USER_NAME", DbType.String, 100),
                new Column("PAYMENT_SOURCE", DbType.Int32, ColumnProperty.NotNull),
                new Column("GUID", DbType.String, 250));

            this.Database.AddTable("REGOP_PAYMENT_CORRECTION_SOURCE",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "PAYMENT_CORRECTION_PA_ID", "REGOP_PERS_ACC", "ID"));

            this.Database.AddForeignKey("FK_PAYMENT_OPERATION_BASE_ID", "REGOP_PAYMENT_CORRECTION_SOURCE", "ID", "REGOP_PAYMENT_OPERATION_BASE", "ID");

            this.Database.AddEntityTable("REGOP_PAYMENT_CORRECTION",
                new Column("PAYMENT_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("TAKE_AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("ENROLL_AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("PAYMENT_OP_ID", ColumnProperty.NotNull, "PAYMENT_OP_ID", "REGOP_PAYMENT_OPERATION_BASE", "ID"));
        }
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PAYMENT_CORRECTION");
            this.Database.RemoveTable("REGOP_PAYMENT_CORRECTION_SOURCE");
            this.Database.RemoveTable("REGOP_PAYMENT_OPERATION_BASE");
        }
    }
}
