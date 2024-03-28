namespace Bars.Gkh.Migrations._2016.Version_2016012800
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Migration("2016012800")
    /// </summary>
    [Migration("2016012800")]
    [MigrationDependsOn(typeof(Version_2016011200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            if (!this.Database.ColumnExists("GKH_MORG_CONTRACT", "DRAWING_PD_DATE_THIS_MONTH"))
            {
                this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("DRAWING_PD_DATE_THIS_MONTH", DbType.Boolean, ColumnProperty.NotNull, false));
            }
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            if (this.Database.ColumnExists("GKH_MORG_CONTRACT", "DRAWING_PD_DATE_THIS_MONTH"))
            {
                this.Database.RemoveColumn("GKH_MORG_CONTRACT", "DRAWING_PD_DATE_THIS_MONTH");
            }
        }
    }
}
