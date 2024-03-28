namespace Bars.Gkh.Migrations._2016.Version_2016081600
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016081600")]
    [MigrationDependsOn(typeof(Version_2016080800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("CONTRACT_FOUNDATION", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            this.Database.AddRefColumn("GKH_MORG_CONTRACT_JSKTSJ", new RefColumn("TERMINATION_FILE", "TC_TERM_FILE", "B4_FILE_INFO", "ID"));
            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("TERMINATION_DATE", DbType.DateTime));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "CONTRACT_FOUNDATION");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "TERMINATION_FILE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "TERMINATION_DATE");
        }
    }
}