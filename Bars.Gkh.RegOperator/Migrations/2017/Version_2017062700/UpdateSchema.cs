namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017062700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2017062700
    /// </summary>
    [Migration("2017062700")]
    [MigrationDependsOn(typeof(Version_2017060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LOC_CODE", new Column("AOGUID", DbType.String));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LOC_CODE", "AOGUID");
        }
    }
}
