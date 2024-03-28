namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015112600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.11.26.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015112600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015111600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "REGOP_PAYMENT_DOC_LOG",
                new RefColumn("PARENT_ID", "REGOP_PAYMENT_DOC_LOG_PARENT", "REGOP_PAYMENT_DOC_LOG", "ID"),
                new Column("UID", DbType.String, 40, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 512, ColumnProperty.NotNull),
                new Column("START_TIME", DbType.DateTime, 512, ColumnProperty.Null),
                new Column("ACC_COUNT", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ALL_ACC_COUNT", DbType.Int64, 22, ColumnProperty.NotNull));
        }

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PAYMENT_DOC_LOG");
        }
    }
}
