namespace Bars.Gkh.Migrations._2017.Version_2017032800
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <inheritdoc />
    [Migration("2017032800")]
    [MigrationDependsOn(typeof(Version_2017032400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            if (this.Database.ColumnExists("CLW_PRETENSION", "SUM_PENALTY_CALC"))
            {
                this.Database.ExecuteNonQuery("ALTER TABLE PUBLIC.CLW_PRETENSION ALTER COLUMN SUM_PENALTY_CALC TYPE VARCHAR(2000)");
            }
        }
    }
}
