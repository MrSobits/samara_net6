namespace Bars.Gkh.Migrations._2016.Version_2016061300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016.06.13.00
    /// </summary>
    [Migration("2016061300")]

    [MigrationDependsOn(typeof(_2015.Version_2015120500.UpdateSchema))]
    [MigrationDependsOn(typeof(_2015.Version_2015121400.UpdateSchema))]
    [MigrationDependsOn(typeof(_2015.Version_2015121600.UpdateSchema))]
    [MigrationDependsOn(typeof(_2015.Version_2015123000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016011500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016012800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016031500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016040800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016051900.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016060300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("CLW_LAWSUIT", new Column("FACT_INITIATED_NOTE", DbType.String, 255));

            this.Database.AddEntityTable("CLW_LAWSUIT_DOCUMENTATION",
                new RefColumn("LAWSUIT_ID", "LAWSUIT_ID", "CLW_LAWSUIT", "ID"),
                new RefColumn("FILE_ID", "FILE_ID", "B4_FILE_INFO", "ID"),
                new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 50, ColumnProperty.NotNull),
                new Column("NOTE", DbType.String, 255),
                new Column("NUMBER", DbType.Int32, ColumnProperty.NotNull));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT", "FACT_INITIATED_NOTE");
            this.Database.RemoveTable("CLW_LAWSUIT_DOCUMENTATION");
        }
    }
}
