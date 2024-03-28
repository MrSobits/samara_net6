namespace Bars.Gkh.Migrations._2016.Version_2016040800
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция конвертации запросов в дизайнера
    /// </summary>
    [Migration("2016040800")]
    [MigrationDependsOn(typeof(Version_2016033000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("OBJECT_CONSTRUCTIJN", DbType.Int32, 30));
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("BUILT_ON_RESETTLEMENT_PROGRAM", DbType.Int32, 30));
            this.Database.ExecuteNonQuery("update GKH_REALITY_OBJECT set OBJECT_CONSTRUCTIJN = 30, BUILT_ON_RESETTLEMENT_PROGRAM =30");
        }


        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "OBJECT_CONSTRUCTIJN");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "BUILT_ON_RESETTLEMENT_PROGRAM");
        }
    }
}