namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017091100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017091100")]
    [MigrationDependsOn(typeof(Version_2017081600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_PERIOD_ROLLBACK_HISTORY", 
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "REGOP_PER_ROLLBACK_PER_ID", "REGOP_PERIOD", "ID"),
                new RefColumn("USER_ID", ColumnProperty.NotNull, "REGOP_PER_ROLLBACK_USET_ID", "B4_USER", "ID"),
                new Column("PERIOD_NAME", DbType.String, ColumnProperty.NotNull),
                new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("REASON", DbType.String, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PERIOD_ROLLBACK_HISTORY");
        }
    }
}