namespace Bars.Gkh.Migrations._2016.Version_2016090300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016090300"), MigrationDependsOn(typeof(Version_2016081600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_ROOM", new Column("CHAMBER_NUM", DbType.String, 100, ColumnProperty.Null));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_ROOM", "CHAMBER_NUM");
        }
    }
}