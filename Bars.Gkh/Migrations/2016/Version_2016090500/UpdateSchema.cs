namespace Bars.Gkh.Migrations._2016.Version_2016090500
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;
    using Gkh;

    /// <summary>
    /// Миграция 2016090500
    /// </summary>
    [Migration("2016090500")]
    [MigrationDependsOn(typeof(Version_2016090100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_DICT_MUNICIPALITY", new Column("OKTMO", DbType.Int64));

            ViewManager.Drop(this.Database, "Gkh");
            ViewManager.Create(this.Database, "Gkh");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }
    }
}
