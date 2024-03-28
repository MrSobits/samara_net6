namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016041000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016.04.10.00
    /// </summary>
    [Migration("2016041000")]
    [MigrationDependsOn(typeof(Version_2016032200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_PA_GROUP", 
                new Column("NAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("IS_SYSTEM", DbType.Int32, 4, ColumnProperty.NotNull, 20));

            this.Database.AddEntityTable(
                "REGOP_PA_GROUP_RELATION",
                new RefColumn("GROUP_ID", "REGOP_PA_GROUP_RELATION", "REGOP_PA_GROUP", "ID"),
                new RefColumn("PA_ID", "REGOP_PA_GROUP_RELATION", "REGOP_PERS_ACC", "ID"));

        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.Delete("REGOP_PA_GROUP_RELATION");
            this.Database.Delete("REGOP_PA_GROUP");
        }
    }
}
