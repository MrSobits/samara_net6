namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016120200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2016120200
    /// </summary>
    [Migration("2016120200")]
    [MigrationDependsOn(typeof(Version_2016110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_BUILDER_CONFIG",
                new Column("PATH", DbType.String, 500, ColumnProperty.NotNull),
                new Column("OWNER_TYPE", DbType.Int16, ColumnProperty.NotNull),
                new Column("ENABLED", DbType.Boolean, ColumnProperty.NotNull));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_BUILDER_CONFIG");
        }
    }
}
