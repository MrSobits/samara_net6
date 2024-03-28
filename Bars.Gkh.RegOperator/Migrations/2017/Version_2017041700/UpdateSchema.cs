namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017041700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2017041700
    /// </summary>
    [Migration("2017041700")]
    [MigrationDependsOn(typeof(Version_2017032100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017032201.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017032400.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017040800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017041100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017041300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_PERS_ACC_OWNERSHIP_HISTORY",
                new Column("ACTUAL_FROM", DbType.DateTime),
                new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "FK_REGOP_OWNERSHIP_ACCOUNT", "REGOP_PERS_ACC", "ID"),
                new RefColumn("OWNER_ID", ColumnProperty.NotNull, "FK_REGOP_OWNERSHIP_OWNER", "REGOP_PERS_ACC_OWNER", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PERS_ACC_OWNERSHIP_HISTORY");
        }
    }
}