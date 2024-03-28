namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017071900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017071900")]
    [MigrationDependsOn(typeof(Version_2017071200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "REGOP_DEBTOR_UPDATE_CONFIG",
                new Column("PERIOD_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("DAY_OF_WEEK", DbType.Int32),
                new Column("DAY_OF_MOUNTH", DbType.Int32),
                new Column("Hours", DbType.Int32, ColumnProperty.NotNull),
                new Column("Minutes", DbType.Int32, ColumnProperty.NotNull));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_DEBTOR_UPDATE_CONFIG");
        }
    }
}